using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Net;
using System.IO;
using Ludoux.LrcHelper.SharedFramework;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Text;

namespace Ludoux.LrcHelper.NeteaseMusic
{
    class HttpRequest
    {
        public string getContent(string sURL)
        {

            string sContent = ""; //Content
            string sLine = "";

            try
            {
                WebRequest wrGETURL = WebRequest.Create(sURL);
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
    class Lyric
	{
		int ID;
		byte Status;
		bool HasOriLyric;
		bool HasTransLyric;
        string ErrorLog="";
        Lyrics MixedLyric = new Lyrics();//翻译作为trans来保存
        public Lyric(int ID)
		{
			this.ID=ID;
		}
        internal void GetOnlineLyric()
		{
            try
            {
                HasOriLyric = false;
                HasTransLyric = false;
                Lyrics tempOriLyric = new Lyrics();
                Lyrics tempTransLyric = new Lyrics();
                string sLRC = "";
                string sContent;
                HttpRequest hr = new HttpRequest();
                sContent = hr.getContent("http://music.163.com/api/song/media?id=" + ID);
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
                HasOriLyric = true;
                MixedLyric.ArrangeLyrics(sLRC);//此处当作为tempOriLyric的深拷贝
                //===========
                sContent = hr.getContent("http://music.163.com/api/song/lyric?os=pc&id=" + ID + "&tv=-1");
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
                if (tempOriLyric.Count == tempTransLyric.Count)//之下是检验count是否一样，timeline是否一样，否则错误
                {
                    int count = tempTransLyric.Count;
                    for (int i = 0; i < count; i++)
                    {
                        if (tempOriLyric[i].Timeline != tempTransLyric[i].Timeline)
                            return;
                        MixedLyric[i].SetTransLyrics("#", tempTransLyric[i].OriLyrics);
                    }
                }
                else
                {
                    ErrorLog = ErrorLog + "<COUNT ERROR>";//也有可能是无翻译
                    return;
                }

                HasTransLyric = true;

                
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
            return MixedLyric.ToString();
        }
        public string GetDelayedLyric(int DelayMsec)//1等于10ms，注意进制。应该在GetOnlineLyric()后使用,若无翻译将直接返回ori
        {
            try
            {
                StringBuilder returnString = new StringBuilder("");
                if (MixedLyric.Count != 0 && MixedLyric.HasTransLyrics())//有原文有翻译
                {
                    for (int i = 0; i < MixedLyric.Count; i++)
                    {
                        string tl = MixedLyric[i].Timeline;
                        tl = tl.Replace(Regex.Match(tl, @"(?<=\.)\d\d$").Value, (Convert.ToInt32(Regex.Match(tl, @"(?<=\.)\d\d$").Value) + DelayMsec).ToString());
                        if (returnString.ToString() != "")
                            returnString.Append("\r\n[" + MixedLyric[i].Timeline + "]" + MixedLyric[i].OriLyrics + "\r\n[" + tl + "]" + MixedLyric[i].TransLyrics);
                        else
                            returnString.Append("["+MixedLyric[i].Timeline + "]"+MixedLyric[i].OriLyrics + "\r\n[" + tl+"]"+MixedLyric[i].TransLyrics);
                    }
                }
                else if (MixedLyric.Count == 0)
                {
                    ErrorLog = ErrorLog + "<OriLyric TransLyric COUNT ERROR>";
                    return "";
                }
                else if (MixedLyric.Count != 0 && MixedLyric.HasTransLyrics()==false)//无翻译
                {
                    for (int i = 0; i < MixedLyric.Count; i++)
                    {
                        if (returnString.ToString() != "")
                            returnString.Append("\r\n[" + MixedLyric[i].Timeline + "]" + MixedLyric[i].ToString());
                        else
                            returnString.Append("[" + MixedLyric[i].Timeline + "]" + MixedLyric[i].ToString());
                    }
                }
                else
                {
                    ErrorLog = ErrorLog + "<OriLyric TransLyric COUNT ERROR>";
                    return "";
                }
                returnString = new StringBuilder(Lyrics.formatTineline(returnString.ToString(), true));
                return returnString.ToString();
            }
            catch (System.ArgumentNullException)
            {
                ErrorLog = ErrorLog + "<ArgumentNullException ERROR!>";
                return "";
            }
            catch (System.NullReferenceException)
            {
                ErrorLog = ErrorLog + "<NullReferenceException ERROR!>";
                return "";
            }

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
            sContent = hr.getContent("http://music.163.com/api/song/detail/?id=" + ID + "&ids=[" + ID + "]");
            o = (JObject)JsonConvert.DeserializeObject(sContent);
            if (o.First.ToString() == @"""songs"": []" || o.First.ToString() == @"""code"": 400")
                return -1;
            sContent = hr.getContent("http://music.163.com/api/song/media?id=" + ID);
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
		int ID;
		internal string Name
        {
            get
            {
                string sContent;
                string FinalText = "";
                HttpRequest hr = new HttpRequest();
                JObject o = new JObject();
                sContent = hr.getContent("http://music.163.com/api/song/detail/?id=" + ID + "&ids=[" + ID + "]");
                o = (JObject)JsonConvert.DeserializeObject(sContent);
                FinalText = o["songs"].ToString();
                FinalText = Regex.Replace(FinalText, @"^\[", "");
                FinalText = Regex.Replace(FinalText, @"\]$", "");
                o = (JObject)JsonConvert.DeserializeObject(FinalText);
                FinalText = o["name"].ToString();
                return FinalText;
            }
        }
		string Artist;
		string Album;
		byte Status;
		public Music(int ID)
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
        int ID;
        internal List<int> SongIDInPlaylist
        {
            get
            {
                List<int> SIPL = new List<int>(); ;//TODO:用后备！！！！
                string sContent;
                HttpRequest hr = new HttpRequest();
                JObject o = new JObject();
                sContent = hr.getContent("http://music.163.com/api/playlist/detail?id=" + ID);
                o = (JObject)JsonConvert.DeserializeObject(sContent);
                sContent = o["result"].ToString();
                o = (JObject)JsonConvert.DeserializeObject(sContent);
                sContent = o["tracks"].ToString();
                MatchCollection mc = new Regex(@"(?<=\r\n    ""id"": ).*(?=,)").Matches(sContent);//正则匹配歌曲的ID
                for (int i = 0; i < mc.Count; i++)
                    SIPL.Add(Convert.ToInt32(mc[i].Value.ToString()));
                return SIPL;
            }
        }
        internal string Name
        {
            get
            {
                string sContent = "";
                HttpRequest hr = new HttpRequest();
                JObject o = new JObject();
                sContent = hr.getContent("http://music.163.com/api/playlist/detail?id=" + ID);
                o = (JObject)JsonConvert.DeserializeObject(sContent);
                sContent = o["result"].ToString();
                o = (JObject)JsonConvert.DeserializeObject(sContent);
                sContent = o["name"].ToString();
                return sContent;
            }
        }
        public Playlist(int ID)
        {
            this.ID = ID;
        }
        internal string GetFolderName()
        {
            return FormatFileName.CleanInvalidFileName(Name);
        }
    }
}