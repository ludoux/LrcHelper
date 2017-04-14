using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Threading;
using Ludoux.LrcHelper.NeteaseMusic;
using System.Diagnostics;

namespace LrcHelper
{
    public partial class LrcDownloader : Form
    {
        public LrcDownloader()
        {
            InitializeComponent();
        }
        private CancellationTokenSource cancelToken;
        private void GETbutton_Click(object sender, EventArgs e)
        {
            GETbutton.Enabled = false;
            StatusInfolabel.Text = "StatusInfo";
            StatusPDFinishedCountlabel.Text = "0";
            StatusPDTotalCountlabel.Text = "0";
            Stopwatch sw = new Stopwatch();
            sw.Start();
            int ID = Convert.ToInt32(IDtextBox.Text);
            if (MusicradioButton.Checked)
            {
                Music m = new Music(ID);
                string Log = DownloadLrc(ID, 100, ".\\" + m.GetFileName() + ".lrc", out int status);
                sw.Stop();
                if (Log == "")
                    StatusInfolabel.Text = "Done Status:" + status + "\r\nUsed Time:" + Math.Round(sw.Elapsed.TotalSeconds, 3) + "sec";
                else
                    StatusInfolabel.Text = Log + " Status:" + status + "\r\nUsed Time:" + Math.Round(sw.Elapsed.TotalSeconds, 3) + "sec";
                GETbutton.Enabled = true;
                IDtextBox.Clear();
            }
            else if(PlaylistradioButton.Checked)
            {
                cancelToken = new CancellationTokenSource();
                ParallelOptions parOpts = new ParallelOptions()
                {
                    CancellationToken = cancelToken.Token,
                    MaxDegreeOfParallelism = System.Environment.ProcessorCount//上面三行针对TPL并行类库
                };
                Task.Factory.StartNew(() =>
                {
                    

                    List<string> Log = new List<string>();
                    Playlist pl = new Playlist(ID);
                    List<int> iDList = pl.SongIDInPlaylist;
                    string folderName = pl.GetFolderName();
                    for (int i = 0; i < iDList.Count; i++)
                        Log.Add("");//先写空白，后面并行直接写[i]
                    if (!System.IO.Directory.Exists(".\\" + folderName))
                        System.IO.Directory.CreateDirectory(".\\" + folderName);
                    this.Invoke((Action)delegate
                    {
                        StatusPDTotalCountlabel.Text = iDList.Count.ToString();//写Status
                        Cancelbutton.Enabled = true;//下面使用TPL类库，可以取消
                    });
                    try
                    {
                        Parallel.For(0, iDList.Count,parOpts, i =>
                        {
                            parOpts.CancellationToken.ThrowIfCancellationRequested();
                            Music m = new Music(iDList[i]);
                            string ErrorLog = DownloadLrc(iDList[i], 100, ".\\" + folderName + @"\" + m.GetFileName() + ".lrc", out int status);
                            if (System.IO.File.Exists(".\\" + folderName + @"\" + m.GetFileName() + ".lrc"))
                                Log[i] = string.Format("{0,-7}|{1,-12}|{2,-50}|{3,-6}|√" + ErrorLog, i + 1, iDList[i], m.Name, status);
                            else
                                Log[i] = string.Format("{0,-7}|{1,-12}|{2,-50}|{3,-6}|×" + ErrorLog, i + 1, iDList[i], m.Name, status);
                            this.Invoke((Action)delegate
                            {
                                StatusPDFinishedCountlabel.Text = (Convert.ToInt32(StatusPDFinishedCountlabel.Text) + 1).ToString();
                            });
                        });
                    }
                    catch (OperationCanceledException ex)
                    {

                    }
                    finally
                    {
                        
                        this.Invoke((Action)delegate
                        {
                            Cancelbutton.Enabled = false;
                            GETbutton.Enabled = true;
                            IDtextBox.Clear();
                        });
                    }
                    
                    sw.Stop();
                    StringBuilder OutLog = new StringBuilder();
                    OutLog.Append("PlaylistID:" + ID + "\r\nPlaylistName:" + pl.Name + "\r\nTotalCount:" + iDList.Count + "\r\nTask Status:");
                    OutLog.Append(cancelToken.IsCancellationRequested == true ? "Canceled" : "Finished");
                    OutLog.Append("\r\n0为无人上传歌词,1为有词,2为纯音乐,-1错误,-2未命中");
                    OutLog.Append(string.Format("\r\n\r\n{0,-7}|{1,-12}|{2,-50}|{3,-6}|ErrorInfo", "SongNum", "SongID", "SongName", "LrcSts"));
                    for (int i = 0; i < Log.Count; i++)
                        OutLog.Append("\r\n" + Log[i]);
                    OutLog.Append("\r\n\r\n" + DateTime.Now.ToString() + "  Used Time:" + Math.Round(sw.Elapsed.TotalSeconds, 3) + "sec\r\n[re:Made by LrcHelper @https://github.com/ludoux/lrchelper]\r\n[ve:" + FileVersionInfo.GetVersionInfo(Application.ExecutablePath).FileVersion + "]\r\nEnjoy music with lyrics now!(*^_^*)");
                    System.IO.File.WriteAllText(".\\" + folderName + @"\Log.txt", OutLog.ToString(), Encoding.UTF8);
                    this.Invoke((Action)delegate
                    {
                        StatusInfolabel.Text = (cancelToken.IsCancellationRequested == true ? "Canceled" : "Finished") + "\r\nRead Log.txt to learn more.";
                    });
                    cancelToken.Dispose();
                });
            }
            
            
        }
        private string DownloadLrc(int MusicID,int DelayMsc, string File, out int status)
        {
            ExtendedLyrics l = new ExtendedLyrics(MusicID);
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

        private void Cancelbutton_Click(object sender, EventArgs e)
        {
            cancelToken.Cancel();//停止所有工作者进程
        }

        private void LrcDownloader_Activated(object sender, EventArgs e)
        {
            if(AutoSetcheckBox.Checked && IDtextBox.Text == "")
            {
                string cbt = Clipboard.GetText();
                if(System.Text.RegularExpressions.Regex.IsMatch(cbt, @"song\?id\="))//为songID
                {
                    string iD = System.Text.RegularExpressions.Regex.Match(cbt, @"(?<=song\?id\=)\d+(?=\D*)").Value;
                    MusicradioButton.Checked = true;
                    IDtextBox.Text = iD;
                }
                else if(System.Text.RegularExpressions.Regex.IsMatch(cbt, @"playlist\?id\="))
                {
                    string iD = System.Text.RegularExpressions.Regex.Match(cbt, @"(?<=playlist\?id\=)\d+(?=\D*)").Value;
                    PlaylistradioButton.Checked = true;
                    IDtextBox.Text = iD;
                }
            }
        }
        
    }
}
