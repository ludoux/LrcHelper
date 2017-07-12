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
            int ModelIndex = Convert.ToInt32(LyricsStylenumericUpDown.Value);
            int DelayMsec = Convert.ToInt32(DelayMsecnumericUpDown.Value);
            if (MusicradioButton.Checked)
            {
                Music m = new Music(ID);
                string Log = DownloadLrc(ID, ModelIndex, DelayMsec, ".\\" + m.GetFileName() + ".lrc", out LyricsStatus status);
                sw.Stop();
                if (Log == "")
                    StatusInfolabel.Text = "Done Status:" + status + "\r\nUsed Time:" + Math.Round(sw.Elapsed.TotalSeconds, 3) + "sec";
                else
                    StatusInfolabel.Text = Log + " Status:" + status + "\r\nUsed Time:" + Math.Round(sw.Elapsed.TotalSeconds, 3) + "sec";
                GETbutton.Enabled = true;
                IDtextBox.Clear();
            }
            else if (PlaylistradioButton.Checked)
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
                    List<long> iDList = pl.SongIDInPlaylist;
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
                        Parallel.For(0, iDList.Count, parOpts, i =>

                         {
                             parOpts.CancellationToken.ThrowIfCancellationRequested();
                             string ErrorLog = "";
                             LyricsStatus status = LyricsStatus.Unsured;
                             Music m = new Music(iDList[i]);
                             try
                             {
                                 ErrorLog = DownloadLrc(iDList[i], ModelIndex, DelayMsec, ".\\" + folderName + @"\" + m.GetFileName() + ".lrc", out status);
                             }
                             catch(Exception ex)
                             {
                                 ErrorLog += "<" + ex.Message + ">";
                             }
                             finally
                             {
                                 if (System.IO.File.Exists(".\\" + folderName + @"\" + m.GetFileName() + ".lrc"))
                                     Log[i] = string.Format("{0,-7}|{1,-12}|{2,-50}|{3,-15}|√ ", i + 1, iDList[i], m.Name, status) + ErrorLog;
                                 else
                                     Log[i] = string.Format("{0,-7}|{1,-12}|{2,-50}|{3,-15}|× ", i + 1, iDList[i], m.Name, status) + ErrorLog;
                                 this.Invoke((Action)delegate
                                 {
                                     StatusPDFinishedCountlabel.Text = (Convert.ToInt32(StatusPDFinishedCountlabel.Text) + 1).ToString();
                                 });
                                 m = null;
                             }
                         });
                    }
                    catch (OperationCanceledException)
                    {
                        //
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
                    OutLog.Append(string.Format("\r\n\r\n{0,-7}|{1,-12}|{2,-50}|{3,-15}|ErrorInfo", "SongNum", "SongID", "SongName", "LrcSts"));
                    for (int i = 0; i < Log.Count; i++)
                        OutLog.Append("\r\n" + Log[i]);
                    OutLog.Append("\r\n\r\n" + DateTime.Now.ToString() + "  Used Time:" + Math.Round(sw.Elapsed.TotalSeconds, 3) + "sec\r\n[re:Made by LrcHelper @https://github.com/ludoux/lrchelper]\r\n[ve:" + FileVersionInfo.GetVersionInfo(Application.ExecutablePath).FileVersion + "]\r\nEnjoy music with lyrics now!(*^_^*)");
                    System.IO.File.WriteAllText(".\\" + folderName + @"\Log.txt", OutLog.ToString(), Encoding.UTF8);
                    this.Invoke((Action)delegate
                    {
                        StatusInfolabel.Text = (cancelToken.IsCancellationRequested == true ? "Canceled" : "Finished") + "\r\nRead Log.txt to learn more.";
                    });
                    cancelToken.Dispose();
                    Log.Clear();
                    OutLog.Clear();
                    pl = null;
                });
            }
            else if (AlbumradioButton.Checked)
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
                    Album a = new Album(ID);
                    List<long> iDList = a.SongIDInAlbum;
                    string folderName = a.GetFolderName();
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
                        Parallel.For(0, iDList.Count, parOpts, i =>
                        {
                            parOpts.CancellationToken.ThrowIfCancellationRequested();
                            Music m = new Music(iDList[i]);
                            string ErrorLog = DownloadLrc(iDList[i], ModelIndex, DelayMsec, ".\\" + folderName + @"\" + m.GetFileName() + ".lrc", out LyricsStatus status);
                            if (System.IO.File.Exists(".\\" + folderName + @"\" + m.GetFileName() + ".lrc"))
                                Log[i] = string.Format("{0,-7}|{1,-12}|{2,-50}|{3,-15}|√ ", i + 1, iDList[i], m.Name, status) + ErrorLog;
                            else
                                Log[i] = string.Format("{0,-7}|{1,-12}|{2,-50}|{3,-15}|× ", i + 1, iDList[i], m.Name, status) + ErrorLog;
                            this.Invoke((Action)delegate
                            {
                                StatusPDFinishedCountlabel.Text = (Convert.ToInt32(StatusPDFinishedCountlabel.Text) + 1).ToString();
                            });
                            m = null;
                        });
                    }
                    catch (OperationCanceledException)
                    {
                        //
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
                    OutLog.Append("PlaylistID:" + ID + "\r\nPlaylistName:" + a.Name + "\r\nTotalCount:" + iDList.Count + "\r\nTask Status:");
                    OutLog.Append(cancelToken.IsCancellationRequested == true ? "Canceled" : "Finished");
                    OutLog.Append(string.Format("\r\n\r\n{0,-7}|{1,-12}|{2,-50}|{3,-15}|ErrorInfo", "SongNum", "SongID", "SongName", "LrcSts"));
                    for (int i = 0; i < Log.Count; i++)
                        OutLog.Append("\r\n" + Log[i]);
                    OutLog.Append("\r\n\r\n" + DateTime.Now.ToString() + "  Used Time:" + Math.Round(sw.Elapsed.TotalSeconds, 3) + "sec\r\n[re:Made by LrcHelper @https://github.com/ludoux/lrchelper]\r\n[ve:" + FileVersionInfo.GetVersionInfo(Application.ExecutablePath).FileVersion + "]\r\nEnjoy music with lyrics now!(*^_^*)");
                    System.IO.File.WriteAllText(".\\" + folderName + @"\Log.txt", OutLog.ToString(), Encoding.UTF8);
                    this.Invoke((Action)delegate
                    {
                        StatusInfolabel.Text = (cancelToken.IsCancellationRequested == true ? "Canceled" : "Finished") + "\r\nRead Log.txt to learn more.";
                    });
                    cancelToken.Dispose();
                    Log.Clear();
                    OutLog.Clear();
                    a = null;
                });
            }
        }
        private string DownloadLrc(long MusicID, int ModeIIndex, int DelayMsc, string File, out LyricsStatus status)
        {
            ExtendedLyrics l = new ExtendedLyrics(MusicID);
            l.FetchOnlineLyrics();
            string lyricText = l.GetCustomLyric(ModeIIndex, DelayMsc);
            if (lyricText != "")
            {
                System.IO.File.WriteAllText(File, lyricText + "\r\n[re:Made by LrcHelper @https://github.com/ludoux/lrchelper]\r\n[ve:" + FileVersionInfo.GetVersionInfo(Application.ExecutablePath).FileVersion + "]", Encoding.UTF8);
                status = l.Status;
            }
            else
                status = l.Status;
            return l.ErrorLog;
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
                else if (System.Text.RegularExpressions.Regex.IsMatch(cbt, @"album\?id\="))
                {
                    string iD = System.Text.RegularExpressions.Regex.Match(cbt, @"(?<=album\?id\=)\d+(?=\D*)").Value;
                    AlbumradioButton.Checked = true;
                    IDtextBox.Text = iD;
                }
            }
        }

        private void AdvancedSettingscheckBox_CheckedChanged(object sender, EventArgs e)
        {
            LyricsStylenumericUpDown.Value = 0;
            DelayMsecnumericUpDown.Value = 100;//恢复默认
            AdvancedSettingsgroupBox.Visible = AdvancedSettingscheckBox.Checked;
        }
    }
}
