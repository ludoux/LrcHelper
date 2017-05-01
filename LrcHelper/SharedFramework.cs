using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
namespace Ludoux.LrcHelper.SharedFramework
{
    
     public class LyricsLine : IComparable<LyricsLine>
    {
        int _offset = 0;//以十毫秒数来保存，100=1000ms=1s
        int _timeline;//以十毫秒数来保存，100=1000ms=1s
        internal string Timeline//返回不带[]
        {
            get
            {
                int _tmptimeline = _timeline;//
                if (_offset != 0)
                    _tmptimeline = _tmptimeline + _offset;
                int MSec = 0, Sec = 0, Min = 0;//此处Msec为10毫秒
                if (_tmptimeline > 99)
                {
                    MSec = _tmptimeline % 100;
                    Sec = Convert.ToInt32(Math.Floor(Convert.ToDecimal(_tmptimeline / 100)));
                }
                else
                    return $"00:00.{_tmptimeline.ToString():D2}";
                
                if(Sec>59)
                {
                    Min = Convert.ToInt32(Math.Floor(Convert.ToDecimal(Sec / 60)));
                    Sec = Sec % 60;
                }
                return $"{Min:D2}:{Sec:D2}.{MSec:D2}";
            }
            set//进来不带[]
            {
                if (Regex.IsMatch(value, @"^offset:\d+$", RegexOptions.IgnoreCase))//处理offset
                {
                    _offset = Convert.ToInt32(Math.Round(Convert.ToDecimal(Regex.Match(value, @"(?<=offset:)\d+$", RegexOptions.IgnoreCase).Value) / 10));
                    return;
                }
                    
                int MSec = 0, Sec = 0, Min = 0;//此处Msec为10毫秒
                Min = Convert.ToInt32(Regex.Match(value, @"^\d+(?=:)").Value);
                Sec = Convert.ToInt32(Regex.Match(value, @"(?<=:)\d+(?=\.)").Value);
                MSec = Convert.ToInt32(Regex.Match(value, @"(?<=\.)\d+$").Value);
                if(MSec > 99)
                    MSec = Convert.ToInt32(Math.Round(Convert.ToDouble(MSec / 10)));
                int tl = MSec + Sec * 100 + Min * 100 * 60;
                if (tl > 0)
                    _timeline = tl;
                else
                    _timeline = 0;
            }
        }
        

        string _oriLyrics;
        internal string OriLyrics
        {
            get => _oriLyrics;
            set => _oriLyrics = value;
        }
        string _break;// 用是否为空来判断是否有翻译
        internal string Break
        {
            get => _break;
            private set => _break = value;
        }
        string _transLyrics;
        internal string TransLyrics
        {
            get => _transLyrics;
            private set => _transLyrics = value;
        }

        internal bool HasTrans()
        {
            if (Break != null && Break != "" && TransLyrics != null && TransLyrics != "")
                return true;
            else
                return false;
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
        internal void DelayTimeline(int MSec)//以十毫秒数来保存，100=1000ms=1s
        {
            if (_timeline + MSec > 0)
                _timeline = _timeline + MSec;
        }
        public int CompareTo(LyricsLine other)
        {
            return _timeline.CompareTo(other._timeline);
        }

    }
     public class Lyrics
    {
        List<LyricsLine> LyricsLineText=new List<LyricsLine>();
        bool HasTags
        {
            get => GetAllTags() != "";
        }
        string _tagAr, _tagTi, _tagAl, _tagBy;//艺人名，曲名，专辑名，编者
        string TagAr
        {
            get => _tagAr;
            set => _tagAr = value;
        }
        string TagTi
        {
            get => _tagTi;
            set => _tagTi = value;
        }
        string TagAl
        {
            get => _tagAl;
            set => _tagAl = value;
        }
        string TagBy
        {
            get => _tagBy;
            set => _tagBy = value;
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
            string ReturnString=GetAllTags();
            foreach (LyricsLine ll in LyricsLineText)
                ReturnString += "\r\n" + ll.ToString();
            return ReturnString;
        }

        public LyricsLine this[int index] => LyricsLineText[index];
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
        
        public void Sort()
        {
            LyricsLineText.Sort();
        }
        public void ArrangeLyrics(string Text,string Break=null)
        {
             string formatNewline(string RowText)
            {
                
                // \r.length=1   \r\n.length=2
                StringBuilder tmp = new StringBuilder(RowText);

                tmp.Replace(@"\r", "\r");
                tmp.Replace(@"\n", "\n");//将明码出的\r \n 成为转义符

                if (Regex.IsMatch(tmp.ToString(), @"\r(?!\n)"))//存在\r单独存在的情况
                {
                    MatchCollection mc = new Regex(@"\r(?!\n)").Matches(tmp.ToString());
                    for (int i = 0; i < mc.Count; i++)
                    {
                        tmp.Remove(mc[i].Index + i, 1);//删除插入会影响sb的index
                        tmp.Insert(mc[i].Index + i, "\r\n");
                    }
                }

                if (Regex.IsMatch(tmp.ToString(), @"(?<!\r)\n"))//存在\n单独存在的情况
                {
                    MatchCollection mc = new Regex(@"(?<!\r)\n").Matches(tmp.ToString());
                    for (int i = 0; i < mc.Count; i++)
                    {
                        tmp.Remove(mc[i].Index + i, 1);//删除插入会影响sb的index
                        tmp.Insert(mc[i].Index + i, "\r\n");
                    }
                }

                if (Regex.IsMatch(tmp.ToString(), @"\r\n"))//存在\r\n单独存在的情况
                {
                    MatchCollection mc = new Regex(@"(?<!\r)\n").Matches(tmp.ToString());
                    for (int i = 0; i < mc.Count; i++)
                    {
                        tmp.Remove(mc[i].Index + i, 1);//删除插入会影响sb的index
                        tmp.Insert(mc[i].Index + i, "\r\n");
                    }
                }
                return tmp.ToString();
            }
            Text = formatNewline(Text);
            string[] textList = Text.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            int totalCount=textList.Count();//总行数
            
            int j = -1;//指示List，可能有同行多个时间轴
            for (int i=0;i<totalCount;i++)//i指示原文本行，只在歌词行时跳
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
                MatchCollection mc = Regex.Matches(textList[i], @"(?<=\[).+?(?=\])");
                int c = mc.Count;//可能有同行多个时间轴的可能性
                for (int k = 0; k < c; k++)
                {
                    j++;
                    LyricsLineText.Add(new LyricsLine());
                    LyricsLineText[j].Timeline = mc[k].Value;

                    if (Break != null)//有翻译
                    {
                        LyricsLineText[j].OriLyrics = Regex.Match(textList[i], @"(?<=\[.+\])[^\[\]]+(?=" + Break + @")").Value;
                        LyricsLineText[j].SetTransLyrics(Break, Regex.Match(textList[i], @"(?<=" + Break + @").+$").Value);
                    }
                    else
                        LyricsLineText[j].OriLyrics = Regex.Match(textList[i], @"(?<=\[.+\])[^\[\]]+$").Value;
                }
            }
        }
        public bool HasTransLyrics(int Line)
        {
            if(LyricsLineText[Line].Break!=null)
                return true;
            else
                return false;
        }
        public string[] GetWalkmanStyleLyrics(int ModeIndex, object[] args)
        {
            string ErrorLog = "";
            switch (ModeIndex)
            {
                case 0://翻译延迟，作为新行出现
                    try
                    {
                        int DelayMsec = Convert.ToInt32(args[0]);
                        StringBuilder returnString = new StringBuilder("");
                        for (int i = 0; i < Count; i++)
                        {
                            if (Count == 0)
                            {
                                ErrorLog = ErrorLog + "<MixedLyric COUNT ERROR>";
                                return new string[] { "", ErrorLog };
                            }
                            else if (this[i].HasTrans())
                            {//如果有翻译
                                if (returnString.ToString() != "")
                                {
                                    returnString.Append("\r\n[" + this[i].Timeline + "]" + this[i].OriLyrics);
                                    this[i].DelayTimeline(DelayMsec);
                                    returnString.Append("\r\n[" + this[i].Timeline + "]" + this[i].TransLyrics);
                                    this[i].DelayTimeline(-DelayMsec);//复原原本的时间轴
                                }

                                else
                                {
                                    returnString.Append("[" + this[i].Timeline + "]" + this[i].OriLyrics);
                                    this[i].DelayTimeline(DelayMsec);
                                    returnString.Append("\r\n[" + this[i].Timeline + "]" + this[i].TransLyrics);
                                    this[i].DelayTimeline(-DelayMsec);//复原原本的时间轴
                                }

                            }
                            else if (this[i].HasTrans() == false)
                            {
                                if (returnString.ToString() != "")
                                    returnString.Append("\r\n[" + this[i].Timeline + "]" + this[i].ToString());
                                else
                                    returnString.Append("[" + this[i].Timeline + "]" + this[i].ToString());
                            }
                            else
                            {
                                ErrorLog = ErrorLog + "<Interesting things happened...>";
                                return new string[] { "", ErrorLog };
                            }
                        }

                        return new string[] {(GetAllTags() != "" ? GetAllTags() + "\r\n" : "") + returnString.ToString(), ErrorLog };//写入 Tag 信息
                    }
                    catch (System.ArgumentNullException)
                    {
                        ErrorLog = ErrorLog + "<ArgumentNullException ERROR!>";
                        return new string[] { "", ErrorLog };
                    }
                    catch (System.NullReferenceException)
                    {
                        ErrorLog = ErrorLog + "<NullReferenceException ERROR!>";
                        return new string[] { "", ErrorLog };
                    }

                default:
                    return null;
            }


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
