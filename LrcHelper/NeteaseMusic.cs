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
        private long ID;
        public enum LyricsStatus {[Description("未命中")] Unmatch = -2, [Description("错误")] Error = -1, [Description("无人上传歌词")] NotSupplied = 0, [Description("有词")] Existed = 1, [Description("纯音乐")] NoLyrics = 2, [Description("初始值（以防状态被多次更改）")] Unsured = 3 }
        private LyricsStatus _status = LyricsStatus.Unsured;
        internal LyricsStatus Status { get => _status; private set { if (_status == LyricsStatus.Unsured) { _status = value; } } }//_status 仅可供修改一次，设计是不可以对外更改的
        

        private bool hasOriLyrics;
        private bool hasTransLyrics;
        private string _errorLog = "";
        internal string ErrorLog { get => _errorLog; private set => _errorLog = value; }
        private Lyrics mixedLyrics = new Lyrics();//翻译作为trans来保存

        public ExtendedLyrics(long ID)
		{
			this.ID=ID;
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
                sContent = hr.GetContent("https://music.163.com/api/song/detail/?id=" + ID + "&ids=[" + ID + "]");//这个是仅对确定歌词状态有用的
                o = (JObject)JsonConvert.DeserializeObject(sContent);
                if (o.First.ToString() == @"""songs"": []" || o.First.ToString() == @"""code"": 400")
                { ErrorLog += "<RETURN ERR!>"; Status = LyricsStatus.Error; return; }

                sContent = hr.GetContent("https://music.163.com/api/song/media?id=" + ID);
                if (sContent.Substring(0, 4).Equals("ERR!"))
                { ErrorLog += "<RETURN ERR!>"; Status = LyricsStatus.Error; return; }
                o = (JObject)JsonConvert.DeserializeObject(sContent);//解析返回值，分析歌词状态和原文歌词

                if (o.First.ToString() == o.Last.ToString() && o.First.ToString() == @"""code"": 200")//分析歌词状态
                    Status = 0;
                if (Regex.IsMatch(o.First.ToString(), @"""nolyric"": true"))
                    Status = LyricsStatus.NoLyrics;
                else
                    Status = LyricsStatus.Existed;
                
                Status = LyricsStatus.Unmatch;//都没踩中

                //分析原文歌词
                if (Regex.IsMatch(o.Root.ToString(), @"""lyric""") == false)
                { ErrorLog += "<CAN NOT FIND LYRIC LABEL>"; return; }
                sLRC = o["lyric"].ToString();
                tempOriLyric.ArrangeLyrics(sLRC);
                hasOriLyrics = true;
                mixedLyrics.ArrangeLyrics(sLRC);

                //===========翻译
                sContent = hr.GetContent("https://music.163.com/api/song/lyric?os=pc&id=" + ID + "&tv=-1");
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
                    int j = 0;//j为外文歌词的index
                    for (int i = 0; i < tempTransLyric.Count && j < tempOriLyric.Count; j++)
                    {
                        if (tempOriLyric[j].Timeline != tempTransLyric[i].Timeline)
                            continue;
                        if(tempTransLyric[i].OriLyrics != null && tempTransLyric[i].OriLyrics != "")
                            mixedLyrics[j].SetTransLyrics("#", tempTransLyric[i].OriLyrics);//Mix是以外文歌词的j来充填，当没有trans的时候留空
                        i++;
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
        long ID;
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
                    sContent = hr.GetContent("https://music.163.com/api/song/detail/?id=" + ID + "&ids=[" + ID + "]");
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
        string Artist;
        string Album;
		byte Status;
		public Music(long ID)
		{
			this.ID = ID;
		}
        internal string GetFileName()
        {
            return FormatFileName.CleanInvalidFileName(Name);
        }
	}
    class Playlist
    {
        long ID;
        internal List<long> SongIDInPlaylist
        {
            get
            {
                List<long> SongIDInPlaylist = new List<long>(); ;//TODO:用后备！！！！
                string sContent;
                HttpRequest hr = new HttpRequest();
                JObject o = new JObject();
                sContent = hr.GetContent("https://music.163.com/api/playlist/detail?id=" + ID);
                o = (JObject)JsonConvert.DeserializeObject(sContent);
                sContent = o["result"].ToString();
                o = (JObject)JsonConvert.DeserializeObject(sContent);
                sContent = o["tracks"].ToString();
                MatchCollection mc = Regex.Matches(sContent, @"(?<=\r\n    ""id"": ).*(?=,)");//正则匹配歌曲的ID
                for (int i = 0; i < mc.Count; i++)
                    SongIDInPlaylist.Add(Convert.ToInt64(mc[i].Value.ToString()));
                return SongIDInPlaylist;
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
                    sContent = hr.GetContent("https://music.163.com/api/playlist/detail?id=" + ID);
                    o = (JObject)JsonConvert.DeserializeObject(sContent);
                    sContent = o["result"].ToString();
                    o = (JObject)JsonConvert.DeserializeObject(sContent);
                    sContent = o["name"].ToString();
                    _name = sContent;
                }
                return _name;

            }
        }
        public Playlist(long ID)
        {
            this.ID = ID;
        }
        internal string GetFolderName()
        {
            return FormatFileName.CleanInvalidFileName(Name);
        }
    }
    class Album
    {
        long ID;
        public Album(long ID)
        {
            this.ID = ID;
        }
        internal List<long> SongIDInAlbum
        {
            get
            {
                List<long> SongIDInAlbum = new List<long>(); ;//TODO:用后备！！！！
                string sContent;
                HttpRequest hr = new HttpRequest();
                JObject o = new JObject();
                sContent = hr.GetContent("https://music.163.com/api/album/" + ID);
                o = (JObject)JsonConvert.DeserializeObject(sContent);
                sContent = o["album"].ToString();
                o = (JObject)JsonConvert.DeserializeObject(sContent);
                sContent = o["songs"].ToString();
                
                MatchCollection mc = Regex.Matches(sContent, @"(?<=\r\n    ""id"": ).*?(?=\,{0,1}\r\n)");//正则匹配歌曲的ID
                for (int i = 0; i < mc.Count; i++)
                    SongIDInAlbum.Add(Convert.ToInt64(mc[i].Value.ToString()));
                return SongIDInAlbum;
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
                    sContent = hr.GetContent("https://music.163.com/api/album/" + ID);
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