using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LRCHelper
{

    class LyricFormat
    {

        /// <summary>
        /// \n换行符统一成\r\n
        /// </summary>
        /// <param name="text">待操作的字符串, \n 或 \r\n 均可</param>
        /// <returns>操作结束的字符串</returns>
        internal string NewlineFormat(string text)
        {
            text = text.Replace("\r", "");
            text = text.Replace("\n", "\r\n");
            return text;
        }
        /// <summary>
        /// 自行检测[(0)0:(0)0.0(0)(0)]多种情况并统一[00:00.00]
        /// </summary>
        /// <param name="text">待操作的字符串</param>
        /// <returns>操作结束的字符串</returns>
        internal string LyricTimeFormat(string text)
        {
            if (Regex.IsMatch(text, @"\[\d\:") == true)//为*[0:*
            {
                MatchCollection mc = new Regex(@"\[\d\:").Matches(text);
                for (int i = 0; i < mc.Count; i++)
                    text = text.Replace(mc[i].Value, mc[i].Value.Replace("[", "[0"));
            }
            if (Regex.IsMatch(text, @"\:\d\.") == true)//为*:0.*
            {
                MatchCollection mc = new Regex(@"\:\d\.").Matches(text);
                for (int i = 0; i < mc.Count; i++)
                    text = text.Replace(mc[i].Value, mc[i].Value.Replace(":", ":0"));
            }
            if (Regex.IsMatch(text, @"(?<=\.)\d\]") == true)       //为*.0]*
            {
                MatchCollection mc = new Regex(@"(?<=\.)\d\]").Matches(text);
                for (int i = 0; i < mc.Count; i++)
                    text = text.Replace(mc[i].Value, mc[i].Value.Replace("]", "0]"));
            }
            //if (Regex.IsMatch(text, @"(?<=\.)\d\d\]") == true)  //为*.00]*
            //;
            if (Regex.IsMatch(text, @"(?<=\.)\d\d\d\]") == true)//为*.000]*
            {
                MatchCollection mc = new Regex(@"(?<=\.)\d\d\d\]").Matches(text);
                for (int i = 0; i < mc.Count; i++)
                    text = text.Replace(mc[i].Value, Regex.Replace(mc[i].Value, @"\d\]", "]"));
            }
            return text;
        }
        /// <summary>
        /// 去除空白的歌词:[00:00.00]\r\n 或\s*(空白符)\r\n  和空行 和头尾换行
        /// </summary>
        /// <param name="text">待操作的字符串, 时间轴和换行符应(默认)格式化[00:00.00]\r\n</param>
        /// <param name="FormatNewLineAndTimeFirst">默认True先格式化时间轴和换行符 NF LTF</param>
        /// <returns></returns>
        internal string BlankLyricFormat(string text, bool FormatNewLineAndTimeFirst = true)
        {
            if (FormatNewLineAndTimeFirst == true)
            {
                text = NewlineFormat(text);
                text = LyricTimeFormat(text);
            }
            text = Regex.Replace(text, @"\[.*?\]\s*\r\n", "");
            text = Regex.Replace(text, @"\[.*?\]\s*$", "");
            text = Regex.Replace(text, @"\s*\r\n", "\r\n");
            text = Regex.Replace(text, @"^\r\n", "");
            text = Regex.Replace(text, @"\r\n$", "");
            return text;
        }
        internal string FixOver60s(string text)
        {
            string[] TextRow = text.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            MatchCollection mc = new Regex(@"\[.*\]").Matches(text);
            string FinalText = "";
            string[] FinalTextRow = new string[mc.Count];
            for (int i = 0; i < mc.Count; i++)
            {
                if (Convert.ToInt32(Regex.Match(mc[i].Value, @"(?<=:).*(?=\.)").Value) > 59)
                {
                    FinalTextRow[i] = "[" + string.Format("{0:D2}", Convert.ToInt32(Regex.Match(mc[i].Value, @"(?<=^\[).*?(?=:)").Value) + 1) +
                                      ":" + string.Format("{0:D2}", Convert.ToInt32(Regex.Match(mc[i].Value, @"(?<=:).*?(?=\.)").Value) - 60) +
                                      "." + string.Format("{0:D2}", Convert.ToInt32(Regex.Match(mc[i].Value, @"(?<=\.).*?(?=\])").Value)) +
                                      Regex.Match(TextRow[i], @"\].*$").Value;
                }
                else
                    FinalTextRow[i] = TextRow[i];
            }
            foreach (string str in FinalTextRow)
                FinalText += str + Environment.NewLine;
            FinalText = Regex.Replace(FinalText, "\r\n$", "");
            return FinalText;
        }
        /// <summary>
        /// 去除[]中的有汉字或英文的[],例如[翻译人:XXX]
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        internal string EngChiFormat(string text)
        {
            string FinalText = text;
            FinalText = Regex.Replace(FinalText, @"^\[.*[\u4e00-\u9fa5].*\]", "");//去除[]带汉字
            FinalText = Regex.Replace(FinalText, @"^\[.*[a-zA-Z].*\]", "");//去除[]带英文
            return FinalText;
        }
    }
    class FileNameFormat
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
