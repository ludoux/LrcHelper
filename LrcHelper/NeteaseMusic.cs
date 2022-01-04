using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Net;
using System.IO;
using ludoux.LrcHelper.SharedFramework;
using System.ComponentModel;
using Newtonsoft.Json;

namespace ludoux.LrcHelper.NeteaseMusic
{
    class NeedLoginException : Exception
    {
        public NeedLoginException(string message) : base(message)
        {

        }
    }
    class HttpRequest
    {
        public string GetContent(string sURL, string cookie ="",string UserAgent = "Mozilla/5.0 (Linux; Android 6.0; Nexus 5 Build/MRA58N) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/96.0.4664.110 Mobile Safari/537.36 Edg/96.0.1054.62")
        {
            
            string sContent = ""; //Content
            string sLine = "";
            try
            {
                HttpWebRequest wrGETURL = WebRequest.CreateHttp(sURL);

                wrGETURL.Referer= "https://music.163.com";
                wrGETURL.Headers.Set(HttpRequestHeader.Cookie, cookie);
                wrGETURL.UserAgent = UserAgent;
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
        /// <param name="userReviseFunc">用户是否介入修改 sContent，当软件自动下载歌词报错后，才应该在启用此选项</param>
        internal void FetchOnlineLyrics(string revisedsContentOriLyricsForUserReviseFunc, string revisedsContentTransLyricsForUserReviseFunc)
        {
            bool userReviseFunc = false;
            if (revisedsContentOriLyricsForUserReviseFunc != null)
                userReviseFunc = true;
            hasOriLyrics = false;
            hasTransLyrics = false;
            Lyrics tempOriLyric = new Lyrics();
            Lyrics tempTransLyric = new Lyrics();
            string sLRC = "";
            string sContent;
            HttpRequest hr = new HttpRequest();

            try
            {
                sContent = hr.GetContent("https://music.163.com/api/song/detail/?id=" + id + "&ids=[" + id + "]");//这个是仅对确定歌词状态有用的
                if (Regex.IsMatch(sContent, @"^{""songs"":\[]") || Regex.IsMatch(sContent, @"^{""code"":400"))
                { ErrorLog += "<STATUS_ERR>"; Status = LyricsStatus.ERROR; return; }

                sContent = hr.GetContent("https://music.163.com/api/song/media?id=" + id);
                if (sContent.Substring(0, 4).Equals("ERR!"))
                { ErrorLog += "<STATUS_ERR>"; Status = LyricsStatus.ERROR; return; }
                
                if (sContent == @"{""code"":200}")//分析歌词状态
                    Status = LyricsStatus.NOTSUPPLIED;
                if (Regex.IsMatch(sContent, @"^{""nolyric"":true"))
                    Status = LyricsStatus.NOLYRICS;
                else
                    Status = LyricsStatus.EXISTED;
                
                Status = LyricsStatus.UNMATCHED;//都没踩中

                //分析原文歌词
                if (userReviseFunc)
                    sContent = revisedsContentOriLyricsForUserReviseFunc;

                if (!Regex.IsMatch(sContent, @"""lyric"""))
                { ErrorLog += "<NO_LYRIC_LABEL>"; return; }
                sLRC = Regex.Match(sContent, @"(?<=""lyric"":"").*?(?="",""code)").Value;
                tempOriLyric.ArrangeLyrics(sLRC);
                hasOriLyrics = true;
                mixedLyrics.ArrangeLyrics(sLRC);

                //===========翻译
                if (userReviseFunc)
                    sContent = revisedsContentTransLyricsForUserReviseFunc;
                else
                    sContent = hr.GetContent("https://music.163.com/api/song/lyric?os=pc&id=" + id + "&tv=-1");

                if (sContent.Substring(0, 4).Equals("ERR!"))
                {
                    ErrorLog += "<STATUS_ERR>";
                    return;
                }

                tempTransLyric.ArrangeLyrics(Regex.Match(sContent, @"(?<=,""tlyric"":{""version"":.*?,""lyric"":"").*?(?=""})").Value);
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
                ErrorLog += "<ArgumentNullException_ERR>";
            }
            catch(System.NullReferenceException)
            {
                ErrorLog += "<NullReferenceException_ERR>";
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
        int _index;
        public int Index { get => _index; set => _index = (value > 1 ? value : 1); }
        long _id;
        public long ID { get => _id; set => _id = value; }
        string _title;//由于titile和artist是一个api，所以获取任意一个都会给两个值进行获取（倘若为空才获取）
		public string Title
        {
            get
            {
                if (_title == null || _title == "")
                    fetchInfo();
                return _title;
            }
        }
        string _artist;
        public string Artist
        {
            get
            {
                if (_artist == null || _artist == "")
                    fetchInfo();
                return _artist;
            }
        }
        string _album;
        public string Album
        {
            get
            {
                if (_album == null || _album == "")
                    fetchInfo();
                return _album;
            }
        }
        private void fetchInfo()
        {
            string sContent;

            HttpRequest hr = new HttpRequest();
            sContent = hr.GetContent("https://music.163.com/api/v3/song/detail?id=" + ID + @"&c=[{""id"":""" + ID + @"""}]");
            _title = Regex.Match(sContent, @"(?<={""songs"":\[{""name"":"").*?(?="","")").Value;

            MatchCollection mc = Regex.Matches(sContent, @"(?<=""id"":\d+,""name"":"")[^/]+?(?="",""tns"")");
            //暂时这样避免直接的越界错误....
            if (mc.Count > 0)
            {
                for (int i = 0; i < (mc.Count - 1); i++)
                    _artist += mc[i].Value + ",";
                _artist += mc[mc.Count - 1].Value;  //mc.Count = 0时可能产生越界
            }
            
            _album = Regex.Match(sContent, @"(?<=,""al"":{""id"":\d+?,""name"":"").+?(?="",""picUrl)").Value;
        }
        

		public Music(long id, int index = 1)
		{
			this.ID = id;
            this.Index = index;
		}
	}
    class Playlist
    {
        long id;
        string cookie = "";
        List<long> _songidInPlaylist = new List<long>();
        internal List<long> SongidInPlaylist
        {
            get
            {
                if (_songidInPlaylist.Count == 0)
                    fetchInfo();                    
                return _songidInPlaylist;
            }
        }
        string _name;
        internal string Name
        {
            get
            {
                if (_name == null || _name == "")
                    fetchInfo();
                return _name;

            }
        }
        private void fetchInfo()
        {
            PreCheck();
        }
        public void PreCheck()
        {
            string sContent;
            HttpRequest hr = new HttpRequest();
            sContent = hr.GetContent("https://music.163.com/api/v3/playlist/detail?id=" + id + @"&c=[{""id"":""" + id + @"""}]", cookie);
            var apiData = JsonConvert.DeserializeObject<dynamic>(sContent);
            if (apiData.code != 200)
                throw new NeedLoginException(Convert.ToString(apiData.message));

            _songidInPlaylist.Clear();
            foreach (var item in apiData.playlist.trackIds)
            {
                _songidInPlaylist.Add(Convert.ToInt64(item.id));
            }
            _name = apiData.playlist.name;
            
        }
        public Playlist(long id,string cookie = "")
        {
            this.id = id;
            this.cookie = cookie;
        }
    }
    class Album
    {
        long id;
        string cookie;
        public Album(long id, string cookie = "")
        {
            this.id = id;
            this.cookie = cookie;
        }
        List<long> _songidInAlbum = new List<long>();
        internal List<long> SongidInAlbum
        {
            get
            {
                if (_songidInAlbum.Count == 0)
                    fetchInfo();
                return _songidInAlbum;
            }
        }
        string _name;
        internal string Name
        {
            get
            {
                if (_name == null || _name == "")
                    fetchInfo();
                return _name;
            }
        }
        private void fetchInfo()
        {
            PreCheck();
        }
        public void PreCheck()
        {
            string sContent;
            HttpRequest hr = new HttpRequest();
            sContent = hr.GetContent("https://music.163.com/api/album/" + id, cookie);
            var apiData = JsonConvert.DeserializeObject<dynamic>(sContent);
            if (apiData.code != 200)
                throw new NeedLoginException(Convert.ToString(apiData.message));

            _songidInAlbum.Clear();
            foreach (var item in apiData.album.songs)
            {
                _songidInAlbum.Add(Convert.ToInt64(item.id));
            }
            _name = apiData.album.name;
        }
    }
}