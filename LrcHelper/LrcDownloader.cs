using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Threading;
using ludoux.LrcHelper.NeteaseMusic;
using System.Diagnostics;
using static ludoux.LrcHelper.NeteaseMusic.ExtendedLyrics;
using ludoux.LrcHelper.FileWriter;
using System.Text.RegularExpressions;
using System.IO;

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
            string filenamePattern = FilenamePatterncomboBox.Text;
            GETbutton.Enabled = false;
            StatusInfolabel.Text = "StatusInfo";
            StatusPDFinishedCountlabel.Text = "0";
            StatusPDTotalCountlabel.Text = "0";
            Stopwatch sw = new Stopwatch();
            sw.Start();
            int id = Convert.ToInt32(IDtextBox.Text);
            int modelIndex = Convert.ToInt32(LyricsStylenumericUpDown.Value);
            int delayMsec = Convert.ToInt32(DelayMsecnumericUpDown.Value);
            if (MusicradioButton.Checked)
            {
                Music m = new Music(id);
                string Log = DownloadLrc(".\\", filenamePattern, m, modelIndex, delayMsec, out LyricsStatus status, out string filePath);
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
                    Playlist pl = new Playlist(id);
                    LogFileWriter logWriter = new LogFileWriter(@".\\" + FormatFileName.CleanInvalidFileName(pl.Name) + @"\");
                    List<long> idList = pl.SongidInPlaylist;
                    logWriter.AppendLyricsDownloadTaskDetail();
                    logWriter.AppendLyricsDownloadTaskDetail(pl.SongidInPlaylist.Count);
                    this.Invoke((Action)delegate
                    {
                        StatusPDTotalCountlabel.Text = idList.Count.ToString();//写Status
                        Cancelbutton.Enabled = true;//下面使用TPL类库，可以取消
                    });
                    try
                    {
                        Parallel.For(0, idList.Count, parOpts, i =>

                         {
                             parOpts.CancellationToken.ThrowIfCancellationRequested();
                             string ErrorLog = "";
                             LyricsStatus status = LyricsStatus.UNSURED;
                             string filePath = "";
                             Music m = new Music(idList[i], i + 1);
                             try
                             {
                                 ErrorLog = DownloadLrc(@".\\" + FormatFileName.CleanInvalidFileName(pl.Name) + @"\", filenamePattern, m, modelIndex, delayMsec, out status,out filePath);
                             }
                             catch(Exception ex)
                             {
                                 ErrorLog += "<" + ex.Message + ">";
                             }
                             finally
                             {
                                 if (System.IO.File.Exists(filePath))
                                     logWriter.AppendLyricsDownloadTaskDetail(i + 1, idList[i], m.Title, status.ToString(), "√" + ErrorLog);
                                 else
                                     logWriter.AppendLyricsDownloadTaskDetail(i + 1, idList[i], m.Title, status.ToString(), "×" + ErrorLog);
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
                    logWriter.AppendHeadInformation("Playlist", id, pl.Name, idList.Count, cancelToken.IsCancellationRequested == true ? "Canceled" : "Finished");
                    logWriter.AppendBottomInformation(Math.Round(sw.Elapsed.TotalSeconds, 3));
                    logWriter.WriteFIle();
                    this.Invoke((Action)delegate
                    {
                        StatusInfolabel.Text = (cancelToken.IsCancellationRequested == true ? "Canceled" : "Finished") + "\r\nRead Log.txt to learn more.";
                    });
                    cancelToken.Dispose();
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
                    Album a = new Album(id);
                    LogFileWriter logWriter = new LogFileWriter(@".\\" + FormatFileName.CleanInvalidFileName(a.Name) + @"\");
                    List<long> idList = a.SongidInAlbum;
                    logWriter.AppendLyricsDownloadTaskDetail();
                    logWriter.AppendLyricsDownloadTaskDetail(a.SongidInAlbum.Count);
                    this.Invoke((Action)delegate
                    {
                        StatusPDTotalCountlabel.Text = idList.Count.ToString();//写Status
                        Cancelbutton.Enabled = true;//下面使用TPL类库，可以取消
                    });
                    try
                    {
                        Parallel.For(0, idList.Count, parOpts, i =>
                        {
                            parOpts.CancellationToken.ThrowIfCancellationRequested();
                            Music m = new Music(idList[i], i + 1);
                            string ErrorLog = "";
                            LyricsStatus status = LyricsStatus.UNSURED;
                            string filePath = "";
                            try
                            {
                                ErrorLog = DownloadLrc(@".\\" + FormatFileName.CleanInvalidFileName(a.Name) + @"\", filenamePattern, m, modelIndex, delayMsec, out status, out filePath);
                            }
                            catch (Exception ex)
                            {
                                ErrorLog += "<" + ex.Message + ">";
                            }
                            finally
                            {
                                if (System.IO.File.Exists(filePath))
                                    logWriter.AppendLyricsDownloadTaskDetail(i + 1, idList[i], m.Title, status.ToString(), "√" + ErrorLog);
                                else
                                    logWriter.AppendLyricsDownloadTaskDetail(i + 1, idList[i], m.Title, status.ToString(), "×" + ErrorLog);
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
                    logWriter.AppendHeadInformation("Album", id, a.Name, idList.Count, cancelToken.IsCancellationRequested == true ? "Canceled" : "Finished");
                    logWriter.AppendBottomInformation(Math.Round(sw.Elapsed.TotalSeconds, 3));
                    logWriter.WriteFIle();
                    this.Invoke((Action)delegate
                    {
                        StatusInfolabel.Text = (cancelToken.IsCancellationRequested == true ? "Canceled" : "Finished") + "\r\nRead Log.txt to learn more.";
                    });
                    cancelToken.Dispose();
                    a = null;
                });
            }
        }
        private string DownloadLrc(string folderPath, string filenamePatern, Music music, int ModeIIndex, int DelayMsc, out LyricsStatus status,out string filePath, string fileEncoding = "UTF-8")
        {
            ExtendedLyrics l = new ExtendedLyrics(music.ID);
            l.FetchOnlineLyrics();
            string lyricText = l.GetCustomLyric(ModeIIndex, DelayMsc);
            filePath = "";
            if (lyricText != "")
            {
                LyricsFileWriter writer = new LyricsFileWriter(folderPath, filenamePatern, music, fileEncoding);
                writer.WriteFile(lyricText);
                filePath = writer.GetFilePath();
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
            FilenamePatterncomboBox.Text = "[title].lrc";
        }

        private void needhelplabel_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/ludoux/LrcHelper/wiki");
        }

        private void Savelabel_Click(object sender, EventArgs e)
        {
            try
            {
                File.WriteAllText(".\\AdvancedSettings.txt", string.Format("Version:{0}\r\nTime:{1}\r\nLyricsStyle:{2}\r\nDelayMsec:{3}\r\nFilenamePattern:{4}\r\n***DO NOT CHANGE ANY TEXT AND/OR ENCODING***\r\n***If you don't want to use these settings ever, just delete this file.***", FileVersionInfo.GetVersionInfo(Application.ExecutablePath).FileVersion, DateTime.Now.ToString(), LyricsStylenumericUpDown.Value.ToString(), DelayMsecnumericUpDown.Value.ToString(), FilenamePatterncomboBox.Text), Encoding.UTF8);
            }
            catch (Exception)
            {
                MessageBox.Show("Failed to save file.");
                return;
            }
            MessageBox.Show("These AdvancedSettings will be used in following loading.");
        }

        private void LrcDownloader_Load(object sender, EventArgs e)
        {
            if(File.Exists(".\\AdvancedSettings.txt"))
            {
                string settings = File.ReadAllText(".\\AdvancedSettings.txt", Encoding.UTF8);
                AdvancedSettingscheckBox.Checked = true;
                LyricsStylenumericUpDown.Value = Convert.ToDecimal(Regex.Match(settings, @"(?<=LyricsStyle:)\d+?(?=\r\n)", RegexOptions.IgnoreCase).Value.ToString());
                DelayMsecnumericUpDown.Value = Convert.ToDecimal(Regex.Match(settings, @"(?<=DelayMsec:)\d+?(?=\r\n)", RegexOptions.IgnoreCase).Value.ToString());
                FilenamePatterncomboBox.Text = Regex.Match(settings, @"(?<=FilenamePattern:).+?(?=\r\n)", RegexOptions.IgnoreCase).Value.ToString();

            }
        }
    }
}
