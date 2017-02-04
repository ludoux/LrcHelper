using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Ludoux.LrcHelper.NeteaseMusic;

namespace LrcHelper
{
    public partial class LrcDownloader : Form
    {
        public LrcDownloader()
        {
            InitializeComponent();
        }

        private void GETbutton_Click(object sender, EventArgs e)
        {
            int ID = Convert.ToInt32(IDtextBox.Text);
            if (MusicradioButton.Checked)
            {
                Music m = new Music(ID);
                int status;
                string Log = DownloadLrc(ID, 100, ".\\" + m.GetFileName() + ".lrc",out status);
                if (Log == "")
                    MessageBox.Show("Done Status:"+status);
                else
                    MessageBox.Show(Log + " Status:" + status);
            }
            else if(PlaylistradioButton.Checked)
            {
                List<string> Log = new List<string>();
                Playlist pl = new Playlist(ID);
                List<int> iDList = pl.SongIDInPlaylist;
                string folderName = pl.GetFolderName();
                if (!System.IO.Directory.Exists(".\\" + folderName))
                    System.IO.Directory.CreateDirectory(".\\"+folderName);
                
                for (int i = 0; i < iDList.Count; i++)
                {
                    Music m = new Music(iDList[i]);
                    int status;
                    string ErrorLog = DownloadLrc(iDList[i], 100, ".\\" + folderName +@"\"+ m.GetFileName() + ".lrc",out status);
                    if(System.IO.File.Exists(".\\" + folderName + @"\" + m.GetFileName() + ".lrc"))
                        Log.Add(string.Format("{0,-7}|{1,-12}|{2,-50}|{3,-6}|√" + ErrorLog, i, iDList[i], m.Name, status));
                    else
                        Log.Add(string.Format("{0,-7}|{1,-12}|{2,-50}|{3,-6}|×" + ErrorLog, i, iDList[i], m.Name, status));
                }
                StringBuilder OutLog = new StringBuilder();
                OutLog.Append(DateTime.Now.ToString() + " PlaylistID:" + ID + " PlaylistName:" + pl.Name + " TotalCount:" + iDList.Count + "\r\n0为无人上传歌词,1为有词,2为纯音乐,-1错误,-2未命中");
                OutLog.Append(string.Format("\r\n{0,-7}|{1,-12}|{2,-50}|{3,-6}|ErrorInfo", "SongNum", "SongID", "SongName", "LrcSts"));
                for (int i = 0; i < Log.Count; i++)
                    OutLog.Append("\r\n" + Log[i]);
                System.IO.File.WriteAllText(".\\" + folderName + @"\Log.txt", OutLog.ToString(), Encoding.UTF8);
            }
        }
        private string DownloadLrc(int MusicID,int DelayMsc, string File, out int status)
        {
            Lyric l = new Lyric(MusicID);
            
            l.GetOnlineLyric();
            string lyricText = l.GetDelayedLyric(DelayMsc);
            if (lyricText != "")
            {
                System.IO.File.WriteAllText(File, lyricText+ "\r\n[re:Made by LrcHelper @https://github.com/ludoux/lrchelper]\r\n[ve:"+System.Diagnostics.FileVersionInfo.GetVersionInfo(Application.ExecutablePath).FileVersion+ "]", Encoding.UTF8);
                status = l.GetLyricStatus();
                return "";
            }
            else
            {
                status = l.GetLyricStatus();
                return l.GetErrorLog();
            }
        }
        
    }
}
