using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ludoux.LrcHelper.FileWriter
{
    class LogFileWriter
    {
        string folderPath;
        string fileName;
        string fileEncoding;
        const string HEAD_DESCRIPTION = "";//如果需要填写，记得加\r\n
        List<string> log = new List<string> ();
        const string Bar = "SongNum|SongID      |SongName                                          |LrcSts         |ErrorInfo";//string.Format("{0,-7}|{1,-12}|{2,-50}|{3,-15}|ErrorInfo", "SongNum", "SongID", "SongName", "LrcSts");
        public LogFileWriter(string folderPath,string fileName = "Log.txt", string fileEncoding = "UTF-8")
        {
            if (!folderPath.EndsWith(@"\"))
                folderPath += @"\";
            this.folderPath = folderPath;
            fileName = FormatFileName.CleanInvalidFileName(fileName);
            this.fileName = fileName;
            this.fileEncoding = fileEncoding;
            log.Add(HEAD_DESCRIPTION);
            for (int i = 0; i < 2; i++)
                log.Add("");
        }
        public string GetFilePath()
        { return folderPath + fileName; }
        public void AppendHeadInformation(string type, long id, string name,int count, string status)
        {
            log.Add("");
            log[1] = string.Format("{0}ID:{1}\r\n{0}Name:{2}\r\nCount:{3}\r\nTask Status:{4}\r\n\r\n", type, id.ToString(), name, count.ToString(), status);
        }
        public void AppendLyricsDownloadTaskDetail()
        {
            log.Add("");
            log[2] = Bar;
        }
        public void AppendLyricsDownloadTaskDetail(int count)
        {
            for (int i = 0; i < count; i++)
                log.Add("");//先写空白，后面并行直接写[i]
        }
        public void AppendLyricsDownloadTaskDetail(int songNum, long songid, string songName, string lyricsStatus, string errorInfo)
        { //前面的头信息和bar占了2个list元素
            log[songNum + 2] = string.Format("{0,-7}|{1,-12}|{2,-50}|{3,-15}|{4}", songNum, songid, songName, lyricsStatus, errorInfo);
        }
        public void AppendBottomInformation(double usedTime)
        {
            log.Add(string.Format("{0}   Used Time:{1}sec", DateTime.Now.ToString(), usedTime.ToString()));
            log.Add(string.Format("[re: Made by LrcHelper @ https://github.com/ludoux/lrchelper ]\r\n[ve:{0}]\r\nEnjoy music with lyrics now!(*^_^*)", FileVersionInfo.GetVersionInfo(Application.ExecutablePath).FileVersion));
        }
        public void WriteFIle()
        {
            StringBuilder text = new StringBuilder();
            foreach (string t in log)
            {
                text.Append(t + "\r\n");
            }
            System.IO.File.WriteAllText(GetFilePath(), text.ToString(), Encoding.GetEncoding(fileEncoding));
        }
    }
    class LyricsFileWriter
    {
        string folderPath;
        string fileName;
        string fileEncoding;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="folderPath">".\\"或"C:\sss\"</param>
        /// <param name="filenamePatern">"[title].lrc"支援[track number][title][artist][album]</param>
        public LyricsFileWriter(string folderPath, string filenamePatern, NeteaseMusic.Music music, string fileEncoding = "UTF-8",int totalWidth = 2)
        {
            if (!folderPath.EndsWith(@"\"))
                folderPath += @"\";
            this.folderPath = folderPath;
            if (!System.IO.Directory.Exists(folderPath))
                System.IO.Directory.CreateDirectory(folderPath);
            this.fileEncoding = fileEncoding;
            fileName = filenamePatern;//结尾应该为.lrc
            fileName = fileName.Replace("[track number]", music.Index.ToString().PadLeft(totalWidth,'0'));
            fileName = fileName.Replace("[title]", music.Title);
            fileName = fileName.Replace("[artist]", music.Artist);
            fileName = fileName.Replace("[album]", music.Album);
            fileName = FormatFileName.CleanInvalidFileName(fileName);
        }
        public string GetFilePath()
        { return folderPath + fileName; }
        public void WriteFile(string lrc)
        {
            System.IO.File.WriteAllText(folderPath + fileName, lrc + "\r\n[re:Made by LrcHelper @https://github.com/ludoux/lrchelper]\r\n[ve:" + FileVersionInfo.GetVersionInfo(Application.ExecutablePath).FileVersion + "]", Encoding.GetEncoding(fileEncoding));
        }
    }
    static class FormatFileName
    {
        //非法字符列表
        private static readonly char[] InvalidFileNameChars = new[]
        {

            '"','<','>','|','\0','\u0001','\u0002','\u0003','\u0004','\u0005','\u0006','\a','\b','\t','\n','\v','\f',
            '\r','\u000e','\u000f','\u0010','\u0011','\u0012','\u0013','\u0014','\u0015','\u0016','\u0017',
            '\u0018','\u0019','\u001a','\u001b','\u001c','\u001d','\u001e','\u001f',':','*','?','\\','/'
        };

        //过滤方法

        public static string CleanInvalidFileName(string fileName)

        {
            fileName = fileName + "";
            fileName = InvalidFileNameChars.Aggregate(fileName, (current, c) => current.Replace(c + "", ""));
            if (fileName.Length > 1)
                if (fileName[0] == '.')
                    fileName = "dot" + fileName.TrimStart('.');
            return fileName;
        }
    }
}
