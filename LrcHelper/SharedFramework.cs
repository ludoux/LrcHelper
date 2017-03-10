using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
namespace Ludoux.LrcHelper.SharedFramework
{
    
     public class LyricsLine
    {
        string _tineline;//含[]
        internal string Timeline//返回不带[]
        {
            get
            {
                MatchCollection mc = new Regex(@"(?<=\[).+(?=\])").Matches(_tineline);//不含 [ ]
                if (mc.Count != 1)//有误
                    return null;
                return mc[0].Value;
            }
            set
            {
                _tineline = "[" + value + "]";
            }
        }
        string _oriLyrics;
        internal string OriLyrics
        {
            get
            {
                return _oriLyrics;
            }
            set
            {
                _oriLyrics = value;
            }
        }
        string _break;// 用是否为空来判断是否有翻译
        internal string Break
        {
            get
            {
                return _break;
            }
            private set
            {
                _break = value;
            }
        }
        string _transLyrics;
        internal string TransLyrics
        {
            get
            {
                return _transLyrics;
            }
            private set
            {
                _transLyrics = value;
            }
        }
        public override string ToString()
        {
            if(Break==null)
            {
                return OriLyrics;
            }
            else
            {
                
                return OriLyrics + Break+TransLyrics;
            }
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
        
    }
     public class Lyrics
    {
        List<LyricsLine> LyricsLineText=new List<LyricsLine>();
        string _tagAr, _tagTi, _tagAl, _tagBy;//艺人名，曲名，专辑名，编者
        string TagAr
        {
            get
            {
                return _tagAr;
            }
            set
            {
                _tagAr = value;
            }
        }
        string TagTi
        {
            get
            {
                return _tagTi;
            }
            set
            {
                _tagTi = value;
            }
        }
        string TagAl
        {
            get
            {
                return _tagAl;
            }
            set
            {
                _tagAl = value;
            }
        }
        string TagBy
        {
            get
            {
                return _tagBy;
            }
            set
            {
                _tagBy = value;
            }
        }
        public string GetAllTags()
        {
            string allTags = "";
            if(TagAr != null && TagAl != "")
               allTags = "[Al:" + TagAr + "]";
            if(TagTi != null && TagTi != "")
               allTags += "\r\n[Ti:" + TagTi +"]";
            if(TagAl != null && TagAl != "")
               allTags += "\r\n[Al:" + TagAl + "]";
            if(TagBy != null && TagBy != "")
               allTags += "\r\n[By:" + TagBy + "]";
            return allTags;
        }
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
                    ReturnString += "\r\n" + ll.ToString();
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
        public Lyrics()
        {
        }
        public Lyrics(string Text, string Break = null)
        {
            ArrangeLyrics(Text, Break);
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
            while (Regex.IsMatch(tmp.ToString(), @"^\[.+\]\s*\r\n"))
                tmp.Replace(Regex.Match(tmp.ToString(), @"^\[.+\]\s*\r\n").Value, "");
            while (Regex.IsMatch(tmp.ToString(), @"\r\n\[.+\]\s*$"))
                tmp.Replace(Regex.Match(tmp.ToString(), @"\r\n\[.+\]\s*$").Value, "");
            while (Regex.IsMatch(tmp.ToString(), @"\r\n\s+?\r\n")) 
            {
                MatchCollection mc = new Regex(@"\r\n\s+?\r\n").Matches(tmp.ToString());                
                for (int i = 0; i<mc.Count; i++)
                    tmp.Replace(mc[i].Value, "\r\n");
            }
            while (Regex.IsMatch(tmp.ToString(), @"\r\n\[[^a-zA-Z]+\]\s*\r\n")) 
            {
                MatchCollection mc = new Regex(@"\r\n\[[^a-zA-Z]+\]\s*\r\n").Matches(tmp.ToString());
                for (int i = 0; i<mc.Count; i++)
                    tmp.Replace(mc[i].Value, "\r\n");
            }
            return tmp.ToString();
        }
        public static string formatTineline(string RowText,bool OnlyToFixDelay=false)
        {//TODO: 完善！！
            StringBuilder tmp = new StringBuilder(RowText);
            StringBuilder changedText;
            if (Regex.IsMatch(tmp.ToString(), @"\[.+\]") == false)
                return "";
            MatchCollection mc = new Regex(@"\[[^a-zA-Z]+\]").Matches(tmp.ToString());//出来的是时间轴含[]，已经除去了tags
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
                    int mSec = Convert.ToInt32(Regex.Match(mc[i].Value, @"(?<=\.).+(?=\])").Value);
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
                        changedText = new StringBuilder("[" + string.Format("{0:D2}", Convert.ToInt32(Regex.Match(changedText.ToString(), @"(?<=^\[).+(?=:)").Value) + 1) +
                                      ":" + string.Format("{0:D2}", Convert.ToInt32(Regex.Match(changedText.ToString(), @"(?<=:).+(?=\.)").Value) - 60) +
                                      "." + string.Format("{0:D2}", Convert.ToInt32(Regex.Match(changedText.ToString(), @"(?<=\.).+(?=\])").Value)) +
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
            int n = -1;//n只在歌词行时跳
            for(int i=0;i<totalCount;i++)
            {
                if(Regex.IsMatch(textList[i], @"^\[Ar:.+\]$", RegexOptions.IgnoreCase))//命中则说明为tags Ar
                {
                    string tagText = Regex.Match(textList[i], @"(?<=\[Ar:).+(?=\])", RegexOptions.IgnoreCase).Value;
                    TagAr = tagText;
                    continue;
                }
                if(Regex.IsMatch(textList[i], @"^\[Ti:.+\]$", RegexOptions.IgnoreCase))//命中则说明为tags Ti
                {
                    string tagText = Regex.Match(textList[i], @"(?<=\[Ti:).+(?=\])", RegexOptions.IgnoreCase).Value;
                    TagTi = tagText;
                    continue;
                }
                if(Regex.IsMatch(textList[i], @"^\[Al:.+\]$", RegexOptions.IgnoreCase))//命中则说明为tags Al
                {
                    string tagText = Regex.Match(textList[i], @"(?<=\[Al:).+(?=\])", RegexOptions.IgnoreCase).Value;
                    TagAl = tagText;
                    continue;
                }
                if(Regex.IsMatch(textList[i], @"^\[By:.+\]$", RegexOptions.IgnoreCase))//命中则说明为tags By
                {
                    string tagText = Regex.Match(textList[i], @"(?<=\[By:).+(?=\])", RegexOptions.IgnoreCase).Value;
                    TagBy = tagText;
                    continue;
                }
                n++;
                LyricsLineText.Add(new LyricsLine());
                LyricsLineText[n].Timeline = Regex.Match(textList[i], @"(?<=\[).+(?=\])").Value;
                if(Break!=null)//有翻译
                {
                    LyricsLineText[n].OriLyrics = Regex.Match(textList[i], @"(?<=\[" + LyricsLineText[n].Timeline + @"\]).+(?=" + Break + @")").Value;
                    LyricsLineText[n].SetTransLyrics(Break,Regex.Match(textList[i],@"(?<=" + Break + @").+$").Value);
                }
                else
                    LyricsLineText[n].OriLyrics = Regex.Match(textList[i], @"(?<=\[" + LyricsLineText[n].Timeline + @"\]).+$").Value;
            }
        }
        public bool HasTransLyrics(int Line=3)
        {
            if(LyricsLineText[Line].Break!=null)
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
