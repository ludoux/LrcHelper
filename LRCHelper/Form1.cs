using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LRCHelper
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        string tb1Default;
        string tb2Default;
        private void button1_Click(object sender, EventArgs e)
        {
            LyricWithTranslation LWT = new LyricWithTranslation();
            textBox2.Text = LWT.ConnectTranslationToLyric(textBox2.Text,textBox1.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            LyricFormat LF = new LyricFormat();
            textBox2.Text = LF.NewlineFormat(textBox2.Text);
            textBox2.Text = LF.BlankLyricFormat(textBox2.Text);
            textBox2.Text = LF.EngChiFormat(textBox2.Text);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            LyricWithTranslation LWT = new LyricWithTranslation();
            textBox2.Text = LWT.ArrangeLyricAndTranslation(textBox2.Text);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(textBox2.Text);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            textBox2.Text =Clipboard.GetText();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(textBox1.Text);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            textBox1.Text = Clipboard.GetText();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            tb1Default = textBox1.Text;
            tb2Default = textBox2.Text;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            textBox1.Text = tb1Default;
            textBox2.Text = tb2Default;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            LyricFormat LF = new LyricFormat();
            textBox1.Text = LF.NewlineFormat(textBox1.Text);
            textBox1.Text = LF.BlankLyricFormat(textBox1.Text);
            textBox1.Text = LF.EngChiFormat(textBox1.Text);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)//音乐ID
            {
                if(checkBox1.Checked == true)//半自动
                {
                    textBox2.Text =AutoAction(Convert.ToInt32(textBox3.Text), true, true);
                    return;
                }
                string ReturnText = AutoAction(Convert.ToInt32(textBox3.Text), true);
                if (ReturnText == "")
                    MessageBox.Show("Done Successfully");
                else
                    MessageBox.Show("Read Log About Error(s)");
            }
            if (radioButton2.Checked)//歌单ID
            {// TODO: 使用更加先进的子线程多线程处理
                string ReturnText = AutoAction(Convert.ToInt32(textBox3.Text), false);
                if (ReturnText == "")
                    MessageBox.Show("Done Successfully");
                else
                    MessageBox.Show("Read Log About Error(s)");
            }
        }
        /// <summary>
        /// 自动/半自动操作
        /// </summary>
        /// <param name="ID">歌曲ID或歌单ID(下一个参数)</param>
        /// <param name="IsSongID">歌曲ID指定T,歌单ID指定</param>
        /// <param name="SemiAuto">半自动处理?默认false</param>
        /// <returns>若为半自动处理返回带时轴无翻译的歌词,若全自动返回空,若全自动有ERROR返回ERROR</returns>
        private string AutoAction(int ID, bool IsSongID,bool SemiAuto = false)
        {    
            string FinalText = "";
            bool IsError = false;
            OnlineLyric OL = new OnlineLyric();
            LyricFormat LF = new LyricFormat();
            if (IsSongID == true)//为歌曲ID
            {
                if (OL.GetLyricStatusFrom163(ID) == 1)//有歌词存在
                {
                    try
                    {
                        if (SemiAuto == true)
                        {
                            FinalText = OL.GetOnlineLyricFrom163(Convert.ToInt32(textBox3.Text));
                            return FinalText;
                        }
                        //以下为全自动操作,即写出lrc文件
                        string OLyric = OL.GetOnlineLyricFrom163(ID);
                        string OTranslation = OL.GetOnlineLyricFrom163(ID, true);
                        OLyric = LF.EngChiFormat(OLyric);
                        OTranslation = LF.EngChiFormat(OTranslation);
                        OLyric = LF.BlankLyricFormat(OLyric);
                        OTranslation = LF.BlankLyricFormat(OTranslation);
                        LyricWithTranslation LWT = new LyricWithTranslation();
                        FinalText = LWT.ConnectTranslationToLyricOnlineVer(OLyric, OTranslation);
                        FinalText = LWT.ArrangeLyricAndTranslation(FinalText);
                        File.WriteAllText(".\\" + @"\" + FileNameFormat.CleanInvalidFileName(OL.GetSongNameFrom163(ID)) + ".lrc", FinalText, Encoding.UTF8);
                    }
                    catch (Exception ex)
                    {
                        IsError = true;
                        File.AppendAllText(".\\" + @"\" + "Log.txt", "[ERROR]SongID:" + ID + " ErrMsg:" + ex.Message + "<Please Use SemiAuto>\r\n", Encoding.UTF8);
                    }
                    
                }
            }
            else if (IsSongID == false)//获取歌单内所有歌曲ID>对个个歌曲配歌词>保存文件   会写出log
            {
                string PlaylistName = OL.GetPlaylistNameFrom163(ID);
                string CleanedPlayListName = FileNameFormat.CleanInvalidFileName(PlaylistName);//无非法字符的文件夹名
                string CleanedSongName = "";//无非法字符的文件名
                List <int> ll = OL.GetSongIDInPlaylistFrom163(ID);
                if (!Directory.Exists(".\\" + CleanedPlayListName)) //创建以歌单命名的文件夹
                    Directory.CreateDirectory(".\\" + CleanedPlayListName);
                File.AppendAllText(".\\" + CleanedPlayListName + @"\" + "Log.txt", "PlaylistID:" + ID + "PlayListName:" + PlaylistName + "SongCount:" + ll.Count + "\r\n说明:0为无人上传歌词,1为有词,2为纯音乐,-1错误\r\n======================\r\n" + string.Format("{0,-7}|{1,-12}|{2,-50}|{3,-6}|ErrorInfo", "SongNum","SongID","SongName","LrcSts"), Encoding.UTF8);
                for (int i = 0; i < ll.Count; i++) //ll[i]为操作的歌曲ID
                {
                    try
                    {
                        int LyricStatus = OL.GetLyricStatusFrom163(ll[i]);//歌词状态
                        string SongName = OL.GetSongNameFrom163(ll[i]);//歌曲名
                        CleanedSongName = FileNameFormat.CleanInvalidFileName(SongName);
                        File.AppendAllText(".\\" + CleanedPlayListName + @"\" + "Log.txt", "\r\n" + string.Format("{0,-7}|{1,-12}|{2,-50}|{3,-6}", i, ll[i].ToString(), SongName, LyricStatus), Encoding.UTF8);
                        //File.AppendAllText(".\\" + PlaylistName + @"\" + "Log.txt", "Song[" + i + "] " + ll[i].ToString() + " " + SongName + " " + LyricStatus + "\r\n", Encoding.UTF8);
                        if (LyricStatus == 1)//有歌词存在
                        {
                            string OLyric = OL.GetOnlineLyricFrom163(ll[i]);
                            string OTranslation = OL.GetOnlineLyricFrom163(ll[i], true);
                            OLyric = LF.EngChiFormat(OLyric);
                            OTranslation = LF.EngChiFormat(OTranslation);
                            OLyric = LF.BlankLyricFormat(OLyric);
                            OTranslation = LF.BlankLyricFormat(OTranslation);
                            LyricWithTranslation LWT = new LyricWithTranslation();
                            FinalText = LWT.ConnectTranslationToLyricOnlineVer(OLyric, OTranslation);
                            FinalText = LWT.ArrangeLyricAndTranslation(FinalText);
                            File.WriteAllText(".\\" + CleanedPlayListName + @"\" + CleanedSongName + ".lrc", FinalText, Encoding.UTF8);
                        }
                    }
                    catch (Exception ex)
                    {
                        IsError = true;
                        File.AppendAllText(".\\" + CleanedPlayListName + @"\" + "Log.txt", "[ERROR]"+ ex.Message + "<Please Use SemiAuto>", Encoding.UTF8);
                    }
                    
                }
            }
            if (IsError == true)
                return "ERROR";//有错误
            return "";//无错误
        }
    }
    }

