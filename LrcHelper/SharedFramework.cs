using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
namespace Ludoux.LrcHelper.SharedFramework
{
     public class LyricsLine
    {
        string Tineline;//含[]
        string OriLyrics;
        string Break;// 用是否为空来判断是否有翻译
        string TransLyrics;
        public override string ToString()
        {
            if(TransLyrics==null)
            {
                return OriLyrics;
            }
            else
            {
                
                return OriLyrics + Break+TransLyrics;
            }
        }
        internal void SetTineline(string TinelineText)//不含[ ]
        {
            Tineline="["+TinelineText+"]";
        }
        internal void SetOriLyrics(string OriLyricsText)
        {
            OriLyrics=OriLyricsText;
        }
        internal void SetTransLyrics(string BreakText,string TransLyricsText,bool ClaerIt=false)//ClearIt清除
        {
            if(ClaerIt)
            {
                Break=null;
                TransLyrics=null;
                return;
            }
            Break=BreakText;
            TransLyrics=TransLyricsText;
        }
        internal string GetTineline()
        {
            MatchCollection mc = new Regex(@"(?<=\[).+?(?=\])").Matches(Tineline);//不含 [ ]
                if(mc.Count!=1)//有误
                    return null;
                return mc[0].Value;
        }
        internal string GetOriLyrics()
        {
            return OriLyrics;
        }
        internal string GetBreak()
        {
            return Break;
        }        
        internal string GetTransLyrics()
        {
            return TransLyrics;
        }
        internal void FixTimeline()//用来处理timeline数字错误，例如毫秒数超99
        {//注意的是 lrc 中第三位若为99即代表999毫秒，为1即代表10毫秒。以下的mSec均按100进制来算
         //而实际生活中1000ms=1s
         //最后都规范成00:00.00的样子
         string handingTimeline=Console.ReadLine();
         int mSec=Convert.ToInt32(Regex.Match(handingTimeline, @"(?<=\.).+?$").Value);
         int sec=Convert.ToInt32(Regex.Match(handingTimeline, @"(?<=:).+?(?=\.)").Value);
         int min=Convert.ToInt32(Regex.Match(handingTimeline, @"^.+?(?=:)").Value);//以上三个为对handingTimeline取三个元素的结果
         handingTimeline=null;
         int upSec=0,upMin=0;//需要增加的Sec数,由于MSec超99;需要增加的Min数，由于Sec超59
         int finalMSec=0,finalSec,finalMin;//最终的MSec,Sec,Min
         if(mSec>99)//命中则不规范
         {
             finalMSec=Convert.ToInt32(Regex.Match(mSec.ToString(),@"\d\d$").Value);
             upSec=(mSec-finalMSec)/100;
         }
         else
         {
             finalMSec=mSec;
         }
         if((sec+upSec)>59)//命中则不规范
         {
             finalSec=(sec+upSec)%60;
             upMin=(sec+upSec)/60;
         }
         else
         {
             finalSec=sec+upSec;
         }
         finalMin=min+upMin;
         SetTineline(string.Format("{0:D2}",finalMin)+":"+string.Format("{0:D2}",finalSec)+"."+string.Format("{0:D2}",finalMSec));
        }
    }
     public class Lyrics
    {
        List<LyricsLine> LyricsLineText=new List<LyricsLine>();
        public int Count
        {
            get
            {
                return LyricsLineText.Count;
            }
        }
        public override string ToString()
        {
            string ReturnString=null;
            foreach (LyricsLine ll in LyricsLineText)
            {
                if (ReturnString == null)
                    ReturnString = ll.ToString();
                else
                    ReturnString = ReturnString + "\r\n" + ll.ToString();
            }
            return ReturnString;
        }
        public LyricsLine this[int index]
        {
            get
            {
                return LyricsLineText[index];
            }
        }
        public Lyrics(List<LyricsLine> Lyrics)
        {
            LyricsLineText=Lyrics;
            
        }
        public Lyrics(string Text, string Break = null)
        {
            ArrangeLyrics(Text, Break);
        }
        public Lyrics()
        {

        }
        public static string formatNewLine(string RowText)
        {
            // \r.length=1   \r\n.length=2
            StringBuilder tmp=new StringBuilder(RowText);
            
            tmp.Replace(@"\r", "\r");
            tmp.Replace(@"\n", "\n");//将明码出的\r \n 成为转义符
            
            if (Regex.IsMatch(tmp.ToString(), @"\r(?!\n)"))//存在\r单独存在的情况
            {
                MatchCollection mc = new Regex(@"\r(?!\n)").Matches(tmp.ToString());
                for (int i = 0; i < mc.Count; i++)
                {
                tmp.Remove(mc[i].Index+i, 1);//删除插入会影响sb的index
                tmp.Insert(mc[i].Index+i,"\r\n");
                }
                    
            }
            
            if (Regex.IsMatch(tmp.ToString(), @"(?<!\r)\n"))//存在\n单独存在的情况
            {
                MatchCollection mc = new Regex(@"(?<!\r)\n").Matches(tmp.ToString());
                for (int i = 0; i < mc.Count; i++)
                {
                tmp.Remove(mc[i].Index+i, 1);//删除插入会影响sb的index
                tmp.Insert(mc[i].Index+i,"\r\n");
                }
            }
            
            if (Regex.IsMatch(tmp.ToString(), @"\r\n"))//存在\r\n单独存在的情况
            {
                MatchCollection mc = new Regex(@"(?<!\r)\n").Matches(tmp.ToString());
                for (int i = 0; i < mc.Count; i++)
                {
                tmp.Remove(mc[i].Index+i, 1);//删除插入会影响sb的index
                tmp.Insert(mc[i].Index+i,"\r\n");
                }
            }
            return tmp.ToString();
        }
        public static string formatBlankLine(string RowText)
        {
            StringBuilder tmp=new StringBuilder(RowText);
            while (Regex.IsMatch(tmp.ToString(), @"\r\n\r\n")) 
                tmp.Replace("\r\n\r\n","\r\n");
            while (Regex.IsMatch(tmp.ToString(), @"^\[.+?\]\s*\r\n"))
                tmp.Replace(Regex.Match(tmp.ToString(), @"^\[.+?\]\s*\r\n").Value, "");
            while (Regex.IsMatch(tmp.ToString(), @"\r\n\[.+?\]\s*$"))
                tmp.Replace(Regex.Match(tmp.ToString(), @"\r\n\[.+?\]\s*$").Value, "");
            while (Regex.IsMatch(tmp.ToString(), @"\r\n\s+?\r\n")) 
            {
                MatchCollection mc = new Regex(@"\r\n\s+?\r\n").Matches(tmp.ToString());                
                for (int i = 0; i<mc.Count; i++)
                    tmp.Replace(mc[i].Value, "\r\n");
            }
            while (Regex.IsMatch(tmp.ToString(), @"\r\n\[.+?\]\s*\r\n")) 
            {
                MatchCollection mc = new Regex(@"\r\n\[.+?\]\s*\r\n").Matches(tmp.ToString());
                for (int i = 0; i<mc.Count; i++)
                    tmp.Replace(mc[i].Value, "\r\n");
            }
            

            return tmp.ToString();
        }
        public static string formatTineline(string RowText,bool OnlyToFixDelay=false)
        {//TODO: 完善！！
            StringBuilder tmp = new StringBuilder(RowText);
            StringBuilder changedText;
            if (Regex.IsMatch(tmp.ToString(), @"\[.+?\]") == false)
                return "";
            MatchCollection mc = new Regex(@"\[.+?\]").Matches(tmp.ToString());//出来的是时间轴含[]
            if (OnlyToFixDelay == false)
            {
                for (int i = 0; i < mc.Count; i++)
                {
                    if (!Regex.IsMatch(mc[i].Value, @"\[\d+:\d+.\d+\]"))//不能解析成时间轴
                    {
                        tmp = tmp.Replace(mc[i].Value, "");
                        continue;//进入下一次循环
                    }
                    changedText = new StringBuilder(mc[i].Value);
                    if (Regex.IsMatch(mc[i].Value, @"\[\d\:"))//为*[0:*
                    {
                        changedText = changedText.Replace("[", "[0");
                    }
                    if (Regex.IsMatch(mc[i].Value, @"\:\d\."))//为*:0.*
                    {
                        changedText = changedText.Replace(":", ":0");
                    }
                    if (Regex.IsMatch(mc[i].Value, @"\.\d\]")) //为*.0]*
                    {
                        changedText = changedText.Replace("]", "0]");
                    }
                    if (Regex.IsMatch(mc[i].Value, @"\.\d\d\d\]"))//为*.000]*
                    {
                        changedText = new StringBuilder(Regex.Replace(changedText.ToString(), @"\d\]", "]"));
                    }
                    if (Regex.IsMatch(mc[i].Value, @":\d\d\]"))//为[00:00]*
                    {
                        changedText = new StringBuilder(Regex.Replace(changedText.ToString(), @"\]", ".00]"));
                    }
                    tmp.Replace(mc[i].Value, changedText.ToString());
                }
            }
            else
            {
                for(int i=0;i<mc.Count;i++)
                {
                    changedText = new StringBuilder(mc[i].Value);
                    int mSec = Convert.ToInt32(Regex.Match(mc[i].Value, @"(?<=\.).+?(?=\])").Value);
                    if (mSec > 99)
                    {
                        changedText = new StringBuilder("[" + string.Format("{0:D2}", Convert.ToInt32(Regex.Match(changedText.ToString(), @"(?<=^\[).*?(?=:)").Value)) +
                                      ":" + string.Format("{0:D2}", Convert.ToInt32(Regex.Match(changedText.ToString(), @"(?<=:).*?(?=\.)").Value) + 1) +
                                      "." + string.Format("{0:D2}", Convert.ToInt32(Regex.Match(changedText.ToString(), @"(?<=\.).*?(?=\])").Value) - 100) +
                                      "]");
                    }

                    int sec = Convert.ToInt32(Regex.Match(mc[i].Value, @"(?<=:).+(?=\.)").Value);
                    if (sec > 59)
                    {
                        changedText = new StringBuilder("[" + string.Format("{0:D2}", Convert.ToInt32(Regex.Match(changedText.ToString(), @"(?<=^\[).+?(?=:)").Value) + 1) +
                                      ":" + string.Format("{0:D2}", Convert.ToInt32(Regex.Match(changedText.ToString(), @"(?<=:).+?(?=\.)").Value) - 60) +
                                      "." + string.Format("{0:D2}", Convert.ToInt32(Regex.Match(changedText.ToString(), @"(?<=\.).+?(?=\])").Value)) +
                                      "]");
                    }
                    tmp.Replace(mc[i].Value, changedText.ToString());
                }
            }
            return tmp.ToString();
        }
        public void ArrangeLyrics(string Text,string Break=null)
        {
            Text = formatNewLine(Text);
            Text = formatTineline(Text);
            Text = formatBlankLine(Text);            
            string[] textList = Text.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            int totalCount=textList.Count();//总行数
            for(int i=0;i<totalCount;i++)
            {
                LyricsLineText.Add(new LyricsLine());
                LyricsLineText[i].SetTineline(Regex.Match(textList[i],@"(?<=\[).+?(?=\])").Value);
                if(Break!=null)//有翻译
                {
                    LyricsLineText[i].SetOriLyrics(Regex.Match(textList[i], @"(?<=\[" + LyricsLineText[i].GetTineline() + @"\]).+?(?=" + Break + @")").Value);
                    LyricsLineText[i].SetTransLyrics(Break,Regex.Match(textList[i],@"(?<=" + Break + @").+?$").Value);
                }
                else
                    LyricsLineText[i].SetOriLyrics(Regex.Match(textList[i], @"(?<=\[" + LyricsLineText[i].GetTineline() + @"\]).+?$").Value);
            }
        }
        public bool HasTransLyrics(int Line=0)
        {
            if(LyricsLineText[Line].GetBreak()!=null)
                return true;
            else
            return false;
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
