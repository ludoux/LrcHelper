using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Net;
using System.IO;
using Ludoux.LrcHelper.SharedFramework;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

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
		long ID;
		byte Status;
		bool HasOriLyrics;
		bool HasTransLyrics;
        string ErrorLog = "";
        Lyrics MixedLyrics = new Lyrics();//翻译作为trans来保存
        public ExtendedLyrics(long ID)
		{
			this.ID=ID;
		}
        internal void GetOnlineLyric()
		{
            try
            {
                HasOriLyrics = false;
                HasTransLyrics = false;
                Lyrics tempOriLyric = new Lyrics();
                Lyrics tempTransLyric = new Lyrics();
                string sLRC = "";
                string sContent;
                HttpRequest hr = new HttpRequest();
                
                sContent = hr.GetContent("http://music.163.com/api/song/media?id=" + ID);
                if (sContent.Substring(0, 4).Equals("ERR!"))
                {
                    ErrorLog = ErrorLog + "<RETURN ERR!>";
                    return;
                }

                //反序列化JSON数据  
                JObject o = (JObject)JsonConvert.DeserializeObject(sContent);
                if (Regex.IsMatch(o.Root.ToString(), @"""lyric""") == false)
                {
                    ErrorLog = ErrorLog + "<CAN NOT FIND LYRIC LABEL>";
                    return;
                }
                sLRC = o["lyric"].ToString();
                tempOriLyric.ArrangeLyrics(sLRC);
                HasOriLyrics = true;
                MixedLyrics.ArrangeLyrics(sLRC);
                //===========翻译
                sContent = hr.GetContent("http://music.163.com/api/song/lyric?os=pc&id=" + ID + "&tv=-1");
                if (sContent.Substring(0, 4).Equals("ERR!"))
                {
                    ErrorLog = ErrorLog + "<RETURN ERR!>";
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
                            MixedLyrics[j].SetTransLyrics("#", tempTransLyric[i].OriLyrics);//Mix是以外文歌词的j来充填，当没有trans的时候留空
                        i++;
                    }
                    HasTransLyrics = true;
                }
                MixedLyrics.Sort();
                tempOriLyric = null;
                tempTransLyric = null;
            }
            catch(System.ArgumentNullException)
            {
                ErrorLog = ErrorLog + "<ArgumentNullException ERROR!>";
            }
            catch(System.NullReferenceException)
            {
                ErrorLog = ErrorLog + "<NullReferenceException ERROR!>";
            }
            
        }
        public override string ToString()
        {
            return MixedLyrics.ToString();
        }
        public string GetDelayedLyric(int DelayMsec)//1等于10ms，注意进制。应该在GetOnlineLyric()后使用,若无翻译将直接返回ori
        {
            string[]  result = MixedLyrics.GetWalkmanStyleLyrics(0, new object[] { DelayMsec });
            ErrorLog += result[1];
            return result[0].ToString();
        }
        public string GetErrorLog()
        {
            return ErrorLog;
        }
        public int GetLyricStatus()//在GetOnlineLyric后使用
        {
            //0为无人上传歌词,1为有词,2为纯音乐,-1错误,-2未命中
            string sContent;
            HttpRequest hr = new HttpRequest();
            JObject o = new JObject();
            sContent = hr.GetContent("http://music.163.com/api/song/detail/?id=" + ID + "&ids=[" + ID + "]");
            o = (JObject)JsonConvert.DeserializeObject(sContent);
            if (o.First.ToString() == @"""songs"": []" || o.First.ToString() == @"""code"": 400")
                return -1;
            sContent = hr.GetContent("http://music.163.com/api/song/media?id=" + ID);
            if (sContent.Substring(0, 4).Equals("ERR!"))
                return -1;

            //反序列化JSON数据  
            o = (JObject)JsonConvert.DeserializeObject(sContent);
            if (Regex.IsMatch(o.First.ToString(), @"""nolyric"": true") == true)
                return 2;
            else
            {
                if (ErrorLog == "")
                    return 1;
            }
            if (o.First.ToString() == o.Last.ToString() && o.First.ToString() == @"""code"": 200")
                return 0;
            return -2;
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
                    sContent = hr.GetContent("http://music.163.com/api/song/detail/?id=" + ID + "&ids=[" + ID + "]");
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
                List<long> SIPL = new List<long>(); ;//TODO:用后备！！！！
                string sContent;
                HttpRequest hr = new HttpRequest();
                JObject o = new JObject();
                sContent = hr.GetContent("http://music.163.com/api/playlist/detail?id=" + ID);
                o = (JObject)JsonConvert.DeserializeObject(sContent);
                sContent = o["result"].ToString();
                o = (JObject)JsonConvert.DeserializeObject(sContent);
                sContent = o["tracks"].ToString();
                MatchCollection mc = new Regex(@"(?<=\r\n    ""id"": ).*(?=,)").Matches(sContent);//正则匹配歌曲的ID
                for (int i = 0; i < mc.Count; i++)
                    SIPL.Add(Convert.ToInt64(mc[i].Value.ToString()));
                return SIPL;
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
                    sContent = hr.GetContent("http://music.163.com/api/playlist/detail?id=" + ID);
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
                List<long> SIA = new List<long>(); ;//TODO:用后备！！！！
                string sContent;
                HttpRequest hr = new HttpRequest();
                JObject o = new JObject();
                sContent = hr.GetContent("https://music.163.com/api/album/" + ID);
                o = (JObject)JsonConvert.DeserializeObject(sContent);
                sContent = o["album"].ToString();
                o = (JObject)JsonConvert.DeserializeObject(sContent);
                sContent = o["songs"].ToString();
                
                MatchCollection mc = new Regex(@"(?<=\r\n    ""id"": ).*(?=\,{0,1}\r\n)").Matches(sContent);//正则匹配歌曲的ID
                for (int i = 0; i < mc.Count; i++)//有个逗号问题
                    SIA.Add(Convert.ToInt64(mc[i].Value.ToString()));
                return SIA;
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