using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LRCHelper
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
    class OnlineLyric
    {//目前仅针对网易云音乐 http://moonlib.com/606.html
        /// <summary>
        /// 从网易云音乐只抓取歌词或翻译(唯一)
        /// </summary>
        /// <param name="ID">歌曲ID</param>
        /// <param name="translation">是否只抓取翻译?默认false</param>
        /// <returns>=无歌词或翻译为空,无法获取为空</returns>
        internal string GetOnlineLyricFrom163(int ID, bool translation = false)
        {
            string sLRC = "";
            if (translation == false) //歌词 若无则返回空
            {
                string sContent;
                HttpRequest hr = new HttpRequest();
                sContent = hr.getContent("http://music.163.com/api/song/media?id=" + ID);
                if (sContent.Substring(0, 4).Equals("ERR!"))
                    return "";
                //反序列化JSON数据  
                JObject o = (JObject)JsonConvert.DeserializeObject(sContent);
                if (Regex.IsMatch(o.Root.ToString(), @"""lyric""") == false)
                    return "";
                sLRC = o["lyric"].ToString();

            }
            else//翻译 若无则返回空
            {
                string sContent;
                HttpRequest hr = new HttpRequest();
                sContent = hr.getContent("http://music.163.com/api/song/lyric?os=pc&id=" + ID + "&tv=-1");
                if (sContent.Substring(0, 4).Equals("ERR!"))
                    return "";
                //反序列化JSON数据  
                JObject o = (JObject)JsonConvert.DeserializeObject(sContent);
                sLRC = o["tlyric"].ToString();
                o = (JObject)JsonConvert.DeserializeObject(sLRC);
                sLRC = o["lyric"].ToString();
            }
            sLRC = sLRC.Replace(@"\r", "");
            sLRC = sLRC.Replace(@"\n", "");
            sLRC = Regex.Replace(sLRC, "\n", "\r\n");
            sLRC = Regex.Replace(sLRC, "\r\n$", "");
            return sLRC;
        }
        /// <summary>
        /// 获取歌词状态
        /// </summary>
        /// <param name="ID">歌曲ID</param>
        /// <returns>0为无人上传歌词,1为有词,2为纯音乐,-1错误</returns>
        internal int GetLyricStatusFrom163(int ID)
        {
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
            if(Regex.IsMatch(o.First.ToString(),@"""nolyric"": true") ==true)
                return 2;
            else
            {
                if (GetOnlineLyricFrom163(ID) != "")
                    return 1;
            }
            if (o.First.ToString() == o.Last.ToString() && o.First.ToString() == @"""code"": 200")
                return 0;
            return -1;
            
        }
        internal string  GetSongNameFrom163(int ID)
        {
            string sContent;
            string FinalText="";
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
        /// <summary>
        /// 获取指定歌单内的所有歌曲ID
        /// </summary>
        /// <param name="ID">歌单ID</param>
        /// <returns></returns>
        internal List<int> GetSongIDInPlaylistFrom163(int ID)
        {
            List<int> SongIDInPlaylist = new List<int>();
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
                SongIDInPlaylist.Add(Convert.ToInt32(mc[i].Value.ToString()));
            return SongIDInPlaylist;
        }
        /// <summary>
        /// 获取指定歌单的名字
        /// </summary>
        /// <param name="ID">歌单ID</param>
        /// <returns></returns>
        internal string GetPlaylistNameFrom163(int ID)
        {
            string sContent="";
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

}
