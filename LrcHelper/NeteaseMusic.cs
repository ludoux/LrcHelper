using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Net;
using System.IO;
using Ludoux.LrcHelper.SharedFramework;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.ComponentModel;

namespace Ludoux.LrcHelper.NeteaseMusic
{
    class HttpRequest
    {
        public string GetContent(string sURL)
        {

            string sContent = ""; //Content
            string sLine = "";
            try
            {
                HttpWebRequest wrGETURL = WebRequest.CreateHttp(sURL);
                
                wrGETURL.Referer= "https://music.163.com";
                wrGETURL.Headers.Set(HttpRequestHeader.Cookie, "appver=1.4.0; os=uwp; osver=10.0.15063.296");
                Stream objStream = wrGETURL.GetResponse().GetResponseStream();
                StreamReader objReader = new StreamReader(objStream);
                while (sLine != null)
                {
                    sLine = objReader.ReadLine();
                    if (sLine != null)
                        sContent += sLine;
                }
            }
            catch (Exception e)
            {
                sContent = "ERR!" + e.ToString();
            }
            return sContent;
        }
    }
    
    class ExtendedLyrics
    {
        private long id;
        public enum LyricsStatus {[Description("未命中")] UNMATCHED = -2, [Description("错误")] ERROR = -1, [Description("无人上传歌词")] NOTSUPPLIED = 0, [Description("有词")] EXISTED = 1, [Description("纯音乐")] NOLYRICS = 2, [Description("初始值（以防状态被多次更改）")] UNSURED = 3 }
        private LyricsStatus _status = LyricsStatus.UNSURED;
        internal LyricsStatus Status { get => _status; private set { if (_status == LyricsStatus.UNSURED) { _status = value; } } }//_status 仅可供修改一次，设计是不可以对外更改的
        

        private bool hasOriLyrics;
        private bool hasTransLyrics;
        private string _errorLog = "";
        internal string ErrorLog { get => _errorLog; private set => _errorLog = value; }
        private Lyrics mixedLyrics = new Lyrics();//翻译作为trans来保存

        public ExtendedLyrics(long ID)
		{
			this.id=ID;
		}
        /// <summary>
        /// 从云端取得该对象的歌词信息并记录
        /// </summary>
        internal void FetchOnlineLyrics()
		{
            hasOriLyrics = false;
            hasTransLyrics = false;
            Lyrics tempOriLyric = new Lyrics();
            Lyrics tempTransLyric = new Lyrics();
            string sLRC = "";
            string sContent;
            HttpRequest hr = new HttpRequest();
            JObject o = new JObject();
            try
            {
                sContent = hr.GetContent("https://music.163.com/api/song/detail/?id=" + id + "&ids=[" + id + "]");//这个是仅对确定歌词状态有用的
                o = (JObject)JsonConvert.DeserializeObject(sContent);
                if (o.First.ToString() == @"""songs"": []" || o.First.ToString() == @"""code"": 400")
                { ErrorLog += "<RETURN ERR!>"; Status = LyricsStatus.ERROR; return; }

                sContent = hr.GetContent("https://music.163.com/api/song/media?id=" + id);
                if (sContent.Substring(0, 4).Equals("ERR!"))
                { ErrorLog += "<RETURN ERR!>"; Status = LyricsStatus.ERROR; return; }
                o = (JObject)JsonConvert.DeserializeObject(sContent);//解析返回值，分析歌词状态和原文歌词

                if (o.First.ToString() == o.Last.ToString() && o.First.ToString() == @"""code"": 200")//分析歌词状态
                    Status = LyricsStatus.NOTSUPPLIED;
                if (Regex.IsMatch(o.First.ToString(), @"""nolyric"": true"))
                    Status = LyricsStatus.NOLYRICS;
                else
                    Status = LyricsStatus.EXISTED;
                
                Status = LyricsStatus.UNMATCHED;//都没踩中

                //分析原文歌词
                if (Regex.IsMatch(o.Root.ToString(), @"""lyric""") == false)
                { ErrorLog += "<CAN NOT FIND LYRIC LABEL>"; return; }
                sLRC = o["lyric"].ToString();
                tempOriLyric.ArrangeLyrics(sLRC);
                hasOriLyrics = true;
                mixedLyrics.ArrangeLyrics(sLRC);

                //===========翻译
                sContent = hr.GetContent("https://music.163.com/api/song/lyric?os=pc&id=" + id + "&tv=-1");
                if (sContent.Substring(0, 4).Equals("ERR!"))
                {
                    ErrorLog += "<RETURN ERR!>";
                    return;
                }
                //反序列化JSON数据  
                o = (JObject)JsonConvert.DeserializeObject(sContent);
                sLRC = o["tlyric"].ToString();
                o = (JObject)JsonConvert.DeserializeObject(sLRC);
                sLRC = o["lyric"].ToString();
                tempTransLyric.ArrangeLyrics(sLRC);
                if (tempOriLyric.Count >= tempTransLyric.Count && tempTransLyric.Count != 0)//翻译可能比外文歌词少，下面会对时间轴来判断配对
                {
                    int j = 0;//j为外文歌词的index 下面的循环是将外文歌词下移
                    for (int i = 0; i < tempTransLyric.Count && j < tempOriLyric.Count; j++)
                    {
                        if (tempOriLyric[j].Timeline.CompareTo(tempTransLyric[i].Timeline) < 0)//此外文歌词可能为空格之类，没有翻译，所以continue只将外文歌词下移
                            continue;
                        if (tempOriLyric[j].Timeline.CompareTo(tempTransLyric[i].Timeline) > 0)//正常情况下应该不会出现这种情况，（特例参见song?id=27901389），将翻译下移
                            i++;
                        if (tempOriLyric[j].Timeline.CompareTo(tempTransLyric[i].Timeline) == 0 && tempTransLyric[i].OriLyrics != null && tempTransLyric[i].OriLyrics != "")
                            mixedLyrics[j].SetTransLyrics("#", tempTransLyric[i].OriLyrics);//Mix是以外文歌词的j来充填，当没有trans的时候留空
                        i++;//将翻译下移
                    }
                    hasTransLyrics = true;
                }
                mixedLyrics.Sort();
                tempOriLyric = null;
                tempTransLyric = null;
            }
            catch(System.ArgumentNullException)
            {
                ErrorLog += "<ArgumentNullException ERROR!>";
            }
            catch(System.NullReferenceException)
            {
                ErrorLog += "<NullReferenceException ERROR!>";
            }
        }
        public override string ToString()
        {
            return mixedLyrics.ToString();
        }
        /// <summary>
        /// 应该在GetOnlineLyric()后使用,若无翻译将直接返回ori
        /// </summary>
        /// <param name="ModelIndex">指定的模式</param>
        /// <param name="DelayMsec">1等于10ms，注意进制。</param>
        /// <returns>返回的lrc文本</returns>
        public string GetCustomLyric(int ModelIndex, int DelayMsec)
        {
            string[] result = mixedLyrics.GetWalkmanStyleLyrics(ModelIndex, new object[] { DelayMsec });
            ErrorLog += result[1];
            return result[0];
        }
    }
	class Music
	{
        long id;
        string _name;
		internal string Name
        {
            get
            {
                if (_name != null && _name != "")
                    return _name;
                else
                {
                    string sContent;
                    string finalText = "";
                    HttpRequest hr = new HttpRequest();
                    JObject o = new JObject();
                    sContent = hr.GetContent("https://music.163.com/api/song/detail/?id=" + id + "&ids=[" + id + "]");
                    o = (JObject)JsonConvert.DeserializeObject(sContent);
                    finalText = o["songs"].ToString();
                    finalText = Regex.Replace(finalText, @"^\[", "");
                    finalText = Regex.Replace(finalText, @"\]$", "");
                    o = (JObject)JsonConvert.DeserializeObject(finalText);
                    finalText = o["name"].ToString();
                    _name = finalText;
                }
                return _name;
            }
        }
        string artist;
        string album;
		byte status;
		public Music(long ID)
		{
			this.id = ID;
		}
        internal string GetFileName()
        {
            return FormatFileName.CleanInvalidFileName(Name);
        }
	}
    class Playlist
    {
        long id;
        internal List<long> SongidInPlaylist
        {
            get
            {
                List<long> songidInPlaylist = new List<long>(); ;//TODO:用后备！！！！
                string sContent;
                HttpRequest hr = new HttpRequest();
                JObject o = new JObject();
                sContent = hr.GetContent("https://music.163.com/api/playlist/detail?id=" + id);
                o = (JObject)JsonConvert.DeserializeObject(sContent);
                sContent = o["result"].ToString();
                o = (JObject)JsonConvert.DeserializeObject(sContent);
                sContent = o["tracks"].ToString();
                MatchCollection mc = Regex.Matches(sContent, @"(?<=\r\n    ""id"": ).*(?=,)");//正则匹配歌曲的ID
                for (int i = 0; i < mc.Count; i++)
                    songidInPlaylist.Add(Convert.ToInt64(mc[i].Value.ToString()));
                return songidInPlaylist;
            }
        }
        string _name;
        internal string Name
        {
            get
            {
                if (_name != null && _name != "")
                    return _name;
                else
                {
                    string sContent = "";
                    HttpRequest hr = new HttpRequest();
                    JObject o = new JObject();
                    sContent = hr.GetContent("https://music.163.com/api/playlist/detail?id=" + id);
                    o = (JObject)JsonConvert.DeserializeObject(sContent);
                    sContent = o["result"].ToString();
                    o = (JObject)JsonConvert.DeserializeObject(sContent);
                    sContent = o["name"].ToString();
                    _name = sContent;
                }
                return _name;

            }
        }
        public Playlist(long id)
        {
            this.id = id;
        }
        internal string GetFolderName()
        {
            return FormatFileName.CleanInvalidFileName(Name);
        }
    }
    class Album
    {
        long id;
        public Album(long id)
        {
            this.id = id;
        }
        internal List<long> SongidInAlbum
        {
            get
            {
                List<long> songidInAlbum = new List<long>(); ;//TODO:用后备！！！！
                string sContent;
                HttpRequest hr = new HttpRequest();
                JObject o = new JObject();
                sContent = hr.GetContent("https://music.163.com/api/album/" + id);
                o = (JObject)JsonConvert.DeserializeObject(sContent);
                sContent = o["album"].ToString();
                o = (JObject)JsonConvert.DeserializeObject(sContent);
                sContent = o["songs"].ToString();
                
                MatchCollection mc = Regex.Matches(sContent, @"(?<=\r\n    ""id"": ).*?(?=\,{0,1}\r\n)");//正则匹配歌曲的ID
                for (int i = 0; i < mc.Count; i++)
                    SongidInAlbum.Add(Convert.ToInt64(mc[i].Value.ToString()));
                return SongidInAlbum;
            }
        }
        string _name;
        internal string Name
        {
            get
            {
                if (_name != null && _name != "")
                    return _name;
                else
                {
                    string sContent = "";
                    HttpRequest hr = new HttpRequest();
                    JObject o = new JObject();
                    sContent = hr.GetContent("https://music.163.com/api/album/" + id);
                    o = (JObject)JsonConvert.DeserializeObject(sContent);
                    sContent = o["album"].ToString();
                    o = (JObject)JsonConvert.DeserializeObject(sContent);
                    sContent = o["name"].ToString();
                    _name = sContent;
                }
                return _name;
            }
        }
        internal string GetFolderName()
        {
            return FormatFileName.CleanInvalidFileName(Name);
        }
    }
}