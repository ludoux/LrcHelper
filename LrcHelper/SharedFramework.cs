using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
namespace Ludoux.LrcHelper.SharedFramework
{
    
     public class LyricsLine : IComparable<LyricsLine>
    {
        private int _offset = 0;//以十毫秒数来保存，100=1000ms=1s
        private int _timeline;//以十毫秒数来保存，100=1000ms=1s
        internal string Timeline//返回不带[]
        {
            get
            {
                int _tmptimeline = _timeline;//
                if (_offset != 0)
                    _tmptimeline = _tmptimeline - _offset;//假如正值提前的话，那么就应该是减号
                int MSec = 0, Sec = 0, Min = 0;//此处Msec为10毫秒
                if (_tmptimeline > 99)
                {
                    MSec = _tmptimeline % 100;
                    Sec = Convert.ToInt32(Math.Floor(Convert.ToDecimal(_tmptimeline / 100)));
                }
                else
                    return $"00:00.{_tmptimeline:D2}";
                
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
                Sec = Convert.ToInt32(Regex.Match(value, @"(?<=:)\d+(?=\.)").Value != "" ? Regex.Match(value, @"(?<=:)\d+(?=\.)").Value : Regex.Match(value, @"(?<=:)\d+$").Value);
                MSec = Convert.ToInt32(Regex.Match(value, @"(?<=\.)\d+$").Value != "" ? Regex.Match(value, @"(?<=\.)\d+$").Value : "0"); //考虑像 00:37 这样的玩意，使用三目计算符
                if (MSec > 99)
                    MSec = Convert.ToInt32(Math.Round(Convert.ToDouble(MSec / 10)));
                int tl = MSec + Sec * 100 + Min * 100 * 60;
                if (tl > 0)
                    _timeline = tl;
                else
                    _timeline = 0;
            }
        }


        private string _oriLyrics;
        internal string OriLyrics
        {
            get => _oriLyrics;
            set => _oriLyrics = value;
        }
        private string _break;// 用是否为空来判断是否有翻译
        internal string Break
        {
            get => _break;
            private set => _break = value;
        }
        private string _transLyrics;
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
        internal void SetTransLyrics(string breakText,string transLyricsText,bool claerIt=false)//ClearIt清除
        {
            if(claerIt)
            {
                Break=null;
                TransLyrics=null;
                return;
            }
            Break=breakText;
            TransLyrics=transLyricsText;
        }
        internal void DelayTimeline(int mSec)//以十毫秒数来保存，100=1000ms=1s
        {
            if (_timeline + mSec > 0)
                _timeline = _timeline + mSec;
        }
        public int CompareTo(LyricsLine other)
        {
            return _timeline.CompareTo(other._timeline);
        }

    }
    public class Lyrics
    {
        List<LyricsLine> LyricsLineText=new List<LyricsLine>();
        public bool HasTags
        {
            get => GetAllTags() != "";
        }
        public string GetAllTags()
        {
            string allTags = "";
            foreach(var item in Tags)
            {
                if (allTags == "")
                    allTags = string.Format("[{0}:{1}]", item.Key, item.Value);
                else
                    allTags += string.Format("\r\n[{0}:{1}]", item.Key, item.Value);
            }
            return allTags;
        }
        public enum LyricsTags {[Description("艺人名")] Ar, [Description("曲名")] Ti, [Description("专辑名")] Al, [Description("歌词编写者")] By };
        public Dictionary<LyricsTags, string> Tags = new Dictionary<LyricsTags, string>();//设计是可以对外访问的
        public int Count
        {
            get
            {
                return LyricsLineText.Count;
            }
        }
        /// <summary>
        /// 有 Tag ，没有时间轴
        /// </summary>
        /// <returns></returns>
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
        public Lyrics(string text, string breakText = null)
        {
            ArrangeLyrics(text, breakText);
        }
        
        public void Sort()
        {
            LyricsLineText.Sort();
        }
        public void ArrangeLyrics(string text,string breakText=null)
        {
             string formatNewline(string rowText)
            {
                
                // \r.length=1   \r\n.length=2
                StringBuilder tmp = new StringBuilder(rowText);

                tmp.Replace(@"\r", "\r");
                tmp.Replace(@"\n", "\n");//将明码出的\r \n 成为转义符

                if (Regex.IsMatch(tmp.ToString(), @"\r(?!\n)"))//存在\r单独存在的情况
                {
                    MatchCollection mc =  Regex.Matches(tmp.ToString(), @"\r(?!\n)");
                    for (int i = 0; i < mc.Count; i++)
                    {
                        tmp.Remove(mc[i].Index + i, 1);//删除插入会影响sb的index
                        tmp.Insert(mc[i].Index + i, "\r\n");
                    }
                }

                if (Regex.IsMatch(tmp.ToString(), @"(?<!\r)\n"))//存在\n单独存在的情况
                {
                    MatchCollection mc = Regex.Matches(tmp.ToString(), @"(?<!\r)\n");
                    for (int i = 0; i < mc.Count; i++)
                    {
                        tmp.Remove(mc[i].Index + i, 1);//删除插入会影响sb的index
                        tmp.Insert(mc[i].Index + i, "\r\n");
                    }
                }

                if (Regex.IsMatch(tmp.ToString(), @"\r\n"))//存在\r\n单独存在的情况
                {
                    MatchCollection mc = Regex.Matches(tmp.ToString(), @"(?<!\r)\n");
                    for (int i = 0; i < mc.Count; i++)
                    {
                        tmp.Remove(mc[i].Index + i, 1);//删除插入会影响sb的index
                        tmp.Insert(mc[i].Index + i, "\r\n");
                    }
                }
                return tmp.ToString();
            }
            text = formatNewline(text);
            string[] textList = text.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            int totalCount=textList.Count();//总行数
            
            int j = -1;//指示List，可能有同行多个时间轴
            for (int i=0;i<totalCount;i++)//i指示原文本行，只在歌词行时跳
            {//要考虑到有些写了标签却没写内容的小婊砸
                //要考虑到有些有多个相同标签的小婊砸
                if(Regex.IsMatch(textList[i], @"^\[Ar:.*\]$", RegexOptions.IgnoreCase))//命中则说明为tags Ar
                {
                    string tagText = Regex.Match(textList[i], @"(?<=\[Ar:).*(?=\])", RegexOptions.IgnoreCase).Value;
                    if (Tags.ContainsKey(LyricsTags.Ar))
                        Tags[LyricsTags.Ar] += "/" + tagText;
                    else
                        Tags.Add(LyricsTags.Ar, tagText);
                    continue;
                }
                if(Regex.IsMatch(textList[i], @"^\[Ti:.*\]$", RegexOptions.IgnoreCase))//命中则说明为tags Ti
                {
                    string tagText = Regex.Match(textList[i], @"(?<=\[Ti:).*(?=\])", RegexOptions.IgnoreCase).Value;
                    if (Tags.ContainsKey(LyricsTags.Ti))
                        Tags[LyricsTags.Ti] += "/" + tagText;
                    else
                        Tags.Add(LyricsTags.Ti, tagText);
                    continue;
                }
                if(Regex.IsMatch(textList[i], @"^\[Al:.*\]$", RegexOptions.IgnoreCase))//命中则说明为tags Al
                {
                    string tagText = Regex.Match(textList[i], @"(?<=\[Al:).*(?=\])", RegexOptions.IgnoreCase).Value;
                    if (Tags.ContainsKey(LyricsTags.Al))
                        Tags[LyricsTags.Al] += "/" + tagText;
                    else
                        Tags.Add(LyricsTags.Al, tagText);
                    continue;
                }
                if(Regex.IsMatch(textList[i], @"^\[By:.*\]$", RegexOptions.IgnoreCase))//命中则说明为tags By
                {
                    string tagText = Regex.Match(textList[i], @"(?<=\[By:).*(?=\])", RegexOptions.IgnoreCase).Value;
                    if (Tags.ContainsKey(LyricsTags.By))
                        Tags[LyricsTags.By] += "/" + tagText;
                    else
                        Tags.Add(LyricsTags.By, tagText);
                    continue;
                }
                if (Regex.IsMatch(textList[i], @"^\[\D+:.*\]$", RegexOptions.IgnoreCase))//匹配一些可能是tag但不支持的文本，直接忽略掉以免当成时间轴（还有怎么有人的tag是中！文！的！居然有[作词:xxx]这样的东西！！！
                    continue;
                
                MatchCollection mc = Regex.Matches(textList[i], @"(?<=\[).+?(?=\])");
                int c = mc.Count;//可能有同行多个时间轴的可能性
                for (int k = 0; k < c; k++)
                {
                    j++;
                    LyricsLineText.Add(new LyricsLine());
                    LyricsLineText[j].Timeline = mc[k].Value;

                    if (breakText != null)//有翻译
                    {
                        LyricsLineText[j].OriLyrics = Regex.Match(textList[i], @"(?<=\[.+\])[^\[\]]+(?=" + breakText + @")").Value;
                        LyricsLineText[j].SetTransLyrics(breakText, Regex.Match(textList[i], @"(?<=" + breakText + @").+$").Value);
                    }
                    else
                        LyricsLineText[j].OriLyrics = Regex.Match(textList[i], @"(?<=\[.+\])[^\[\]]+$").Value;
                }
            }
        }
        public bool HasTransLyrics(int line)
        {
            if(LyricsLineText[line].Break!=null)
                return true;
            else
                return false;
        }
        public string[] GetWalkmanStyleLyrics(int modelIndex, object[] args)
        {
            string errorLog = "";
            switch (modelIndex)
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
                                errorLog = errorLog + "<MixedLyric COUNT ERROR>";
                                return new string[] { "", errorLog };
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
                                errorLog = errorLog + "<Interesting things happened...>";
                                return new string[] { "", errorLog };
                            }
                        }

                        return new string[] {(GetAllTags() != "" ? GetAllTags() + "\r\n" : "") + returnString.ToString(), errorLog };//写入 Tag 信息
                    }
                    catch (System.ArgumentNullException)
                    {
                        errorLog = errorLog + "<ArgumentNullException ERROR!>";
                        return new string[] { "", errorLog };
                    }
                    catch (System.NullReferenceException)
                    {
                        errorLog = errorLog + "<NullReferenceException ERROR!>";
                        return new string[] { "", errorLog };
                    }

                case 1://处理标点/文字的占位，倘若能同屏显示那么就优先同屏显示，否则按 case 0 一样翻译换行
                       /* 
                        * 在 NW-A27 的环境下测试。2.2 英寸屏幕，320*240 分辨率
                        * 倘若一行为 10 ，同屏三行共 30 textSize
                        * 纯中文/大写字母 10/13 = 0.76
                        * 小写字母 10/16 = 0.62
                        * 全半角感叹号逗号都会转成半角显示以及英文半角句号，10/54 = 0.18
                        * 汉字全角句号 10/19 = 0.52
                        * 【 10/30 = 0.33
                        * （会转成半角， 10/33 = 0.30
                        * 空格处理得很诡异，索尼对于过长的空格直接换行就不理它了。短空格按逗号算，0.18
                        * 全角引号，10/28 = 0.35
                        * 半角引号，10/36 = 0.27
                        * 「 10/36 = 0.27
                        * 』 10/18 = 0.55
                        * 半角冒号 10/65 = 0.15，全角冒号会转为半角显示
                        * 数字 10/19 = 0.52
                        * ' 10/72 = 0.13
                        */
                    Dictionary<string, double> textSize = new Dictionary<string, double>()
                    {
                        {@"。", 0.52 },
                        {@"[【】]", 0.33 },
                        {@"[「」]", 0.27 },
                        {@"[『』]", 0.55 },
                        {@"[“”]", 0.35 },
                        {@"[""]", 0.27 },
                        {@"[！!，,. ]", 0.18 },
                        {@"[（）()]", 0.30 },
                        {@"[\u2E80-\u9FFF]", 0.76 },
                        {@"[\uac00-\ud7ff]", 0.76 },
                        {@"[A-Z]", 0.76 },
                        {@"[a-z]", 0.62 },
                        {@"[0-9]", 0.52 },
                        {@"'", 0.13 },
                        {@"[:：]", 0.15 },
                    };
                    try
                    {
                        int DelayMsec = Convert.ToInt32(args[0]);
                        StringBuilder returnString = new StringBuilder("");
                        for (int i = 0; i < Count; i++)
                        {
                            if (Count == 0)
                            {
                                errorLog = errorLog + "<MixedLyric COUNT ERROR>";
                                return new string[] { "", errorLog };
                            }
                            else if (this[i].HasTrans())
                            {//如果有翻译
                                double totalTextSize = 0;
                                string connectedText = this[i].OriLyrics + this[i].TransLyrics;//将原文和翻译合并，计算来确定能否同屏显示
                                void getSize(string pattern, double multiple)//获取指定正则规则下的字符 size，然后在 connectedText 中去掉
                                {
                                    MatchCollection mc = Regex.Matches(connectedText, pattern);
                                    totalTextSize += mc.Count * multiple;
                                    for (int j = 0; j < mc.Count; j++)
                                        connectedText = connectedText.Replace(mc[j].Value.ToString(), "");
                                }
                                foreach (var item in textSize)
                                    getSize(item.Key, item.Value);
                                if (connectedText != "")//假如还有剩，就是上面没有命中，属于遗漏的
                                {
                                    totalTextSize += connectedText.Count() * 0.76;
                                    errorLog = errorLog + "<connectedText{(" + connectedText.Count().ToString() + ") " + connectedText + "} is not empty>";
                                }
                                System.Diagnostics.Debug.WriteLine(this[i].OriLyrics + this[i].TransLyrics + "\r\n" + connectedText.Count().ToString() + ") " + connectedText + "\r\n" + totalTextSize + "\r\n============");
                                if (totalTextSize < 30)//30 为三行同屏的 size
                                {
                                    if (returnString.ToString() != "")
                                        returnString.Append("\r\n[" + this[i].Timeline + "]" + this[i].ToString());
                                    else
                                        returnString.Append("[" + this[i].Timeline + "]" + this[i].ToString());
                                }
                                else//同case0
                                {//TODO 不要copy
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
                                errorLog = errorLog + "<Interesting things happened...>";
                                return new string[] { "", errorLog };
                            }
                        }

                        return new string[] { (GetAllTags() != "" ? GetAllTags() + "\r\n" : "") + returnString.ToString(), errorLog };//写入 Tag 信息
                    }
                    catch (System.ArgumentNullException)
                    {
                        errorLog = errorLog + "<ArgumentNullException ERROR!>";
                        return new string[] { "", errorLog };
                    }
                    catch (System.NullReferenceException)
                    {
                        errorLog = errorLog + "<NullReferenceException ERROR!>";
                        return new string[] { "", errorLog };
                    }
                default:
                    return null;
            }


        }
    }
    

}
