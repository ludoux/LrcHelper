using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LRCHelper
{
    class LyricWithTranslation
    {
        /// <summary>
        /// 本地版本.把翻译用#接到带时间轴的歌词后面
        /// </summary>
        /// <param name="Lyric">带时间轴的歌词, 可多行 [00:00.00]今夜恋にかわる しあわせな梦で会おう</param>
        /// <param name="Translation">翻译, 可多行 今夜恋にかわる しあわせな梦で会おう\r\n〖今晚化作恋情 相约幸福的梦中见〗</param>
        internal string ConnectTranslationToLyric(string Lyric, string Translation)
        {
            string[] LyricRow = Lyric.Split(new string[] {"\r\n"}, StringSplitOptions.RemoveEmptyEntries);
            string[] TranslationRow = Translation.Split(new string[] {"\r\n"}, StringSplitOptions.RemoveEmptyEntries);
            string FinalText="";
            if ((2 * Convert.ToInt32(LyricRow.Count())) != TranslationRow.Count())//出现了错误
                throw new FormatException("TranslationRow.Count != 2 * LyricRow.Count");
            for (int i = 1; i < TranslationRow.Count(); i = i + 2)
            {
                //TranslationRow[i]为翻译部分
                if (i == 1)
                    LyricRow[0] = LyricRow[0] + "#" + TranslationRow[1];
                else                       //Math.Ceiling() 向上取整                              输出 [00:00.00]今夜恋にかわる しあわせな梦で会おう#〖今晚化作恋情 相约幸福的梦中见〗
                    LyricRow[Convert.ToInt32(Math.Ceiling(Convert.ToDouble(i / 2)))] = LyricRow[Convert.ToInt32(Math.Ceiling(Convert.ToDouble(i / 2)))] + "#" + TranslationRow[i];
            }
            foreach (string str in LyricRow)
                FinalText += str + Environment.NewLine;
            FinalText = Regex.Replace(FinalText, @"\r\n$", "");//去除末尾的换行
            return FinalText;
        }
        /// <summary>
        /// ConnectTranslationToLyric 的后续操作，将#两边的歌词与翻译做成独立的歌词句并对翻译进行时间偏移
        /// </summary>
        /// <param name="ConnectedSongText">ConnectTranslationToLyric返回值 [00:00.00]今夜恋にかわる しあわせな梦で会おう#〖今晚化作恋情 相约幸福的梦中见〗</param>
        /// <param name="DelaySec">翻译向后偏移的秒速, 默认为1 写死</param>
        /// <returns></returns>
        internal string ArrangeLyricAndTranslation(string ConnectedSongText,int DelaySec = 1)
        {
            if (Regex.IsMatch(ConnectedSongText, "#") == false)//若无翻译
                return ConnectedSongText;//直接结束
            bool Over60s = false;//指示是否存在59秒, 存在则需要后续处理60秒进成1分钟
            string[] ConnectedSongTextRow = ConnectedSongText.Split(new string[] {"\r\n"}, StringSplitOptions.RemoveEmptyEntries);
            string[] FinalTextRow = new string[ConnectedSongTextRow.Count() * 2]; ;
            string FinalText = "";
            for(int i=0;i<ConnectedSongTextRow.Count();i++)//要将ConnectedSongTextRow[i]中的歌词与翻译分别填充到FinalTextRow[2*i]与[2*i+1]中
            {
                FinalTextRow[2 * i] = Regex.Match(ConnectedSongTextRow[i], "^.*(?=#)").Value;
                FinalTextRow[2 * i + 1] = "[" + string.Format("{0:D2}", Convert.ToInt32(Regex.Match(ConnectedSongTextRow[i], @"(?<=^\[).*?(?=:)").Value)) +
                                          ":" + string.Format("{0:D2}", Convert.ToInt32(Regex.Match(ConnectedSongTextRow[i], @"(?<=:).*?(?=\.)").Value)+DelaySec) +
                                          "." + string.Format("{0:D2}", Convert.ToInt32(Regex.Match(ConnectedSongTextRow[i], @"(?<=\.).*?(?=\])").Value)) +
                                          "]" + Regex.Match(ConnectedSongTextRow[i], "(?<=#).*$").Value;
                if (Convert.ToInt32(Regex.Match(FinalTextRow[2 * i + 1], @"(?<=:).*?(?=\.)").Value) > 59)
                    Over60s = true;
            }
            foreach (string str in FinalTextRow)
                FinalText += str + Environment.NewLine;
            FinalText = Regex.Replace(FinalText, "\r\n$", "");
            if(Over60s == true)
            {
                LyricFormat LF = new LyricFormat();
                return LF.FixOver60s(FinalText);
            }
            return FinalText;
            
        }
        /// <summary>
        /// 在线版本.把带时间轴的翻译用#接到带时间轴的歌词后面
        /// </summary>
        /// <param name="OnlineLyric">Online的带时间轴的歌词</param>
        /// <param name="OnlineTranslation">Online的带时间轴的翻译</param>
        /// <returns></returns>
        internal string ConnectTranslationToLyricOnlineVer(string OnlineLyric,string OnlineTranslation)
        {
            string[] OnlineLyricRow = OnlineLyric.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            string[] OnlineTranslationRow = OnlineTranslation.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            string FinalText = "";
            if ((Convert.ToInt32(OnlineLyricRow.Count())) != OnlineTranslationRow.Count()&& OnlineTranslationRow.Count()!=0)//有翻译且出现了错误
                throw new FormatException("带时间轴的歌词与翻译行数不同，软件无法执行");
            for (int i = 0; i < OnlineLyricRow.Count(); i++)
                if(OnlineTranslationRow.Count() != 0)//如果有翻译就加，没有就不动，到分割时根据有没有#来判断有没有翻译
                    OnlineLyricRow[i] += "#" + Regex.Replace(OnlineTranslationRow[i], @"^\[.*\]", "");
            foreach (string str in OnlineLyricRow)
                FinalText += str + Environment.NewLine;
            FinalText = Regex.Replace(FinalText, @"\r\n$", "");//去除末尾的换行
            return FinalText;
        }
    }
}
