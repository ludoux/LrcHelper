using System;
using System.Diagnostics;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LrcHelper
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            using (var client = new WebClient())
            {

                client.Headers["User-Agent"] = @"Mozilla/5.0 (Windows NT 10.0; WOW64; rv:53.0) Gecko/20100101 Firefox/53.0";
                client.Encoding = System.Text.Encoding.UTF8;
                client.DownloadDataAsync(new Uri("https://raw.githubusercontent.com/ludoux/LrcHelper/master/UpdateInfo/UpInfo.txt"));
                client.DownloadDataCompleted += Client_DownloadDataCompleted;
                
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new LrcDownloader());
        }

        private static void Client_DownloadDataCompleted(object sender, DownloadDataCompletedEventArgs e)
        {
            try
            {
                // If the request was not canceled and did not throw
                // an exception, display the resource.
                if (!e.Cancelled && e.Error == null)
                {
                    byte[] data = e.Result;
                    string textData = System.Text.Encoding.UTF8.GetString(data);
                    if (Regex.IsMatch(textData, @"\[UnsupportedVer\].*\r\n.*\<All\>.*\r\n", RegexOptions.IgnoreCase) && Regex.IsMatch(textData, @"\[UnsupportedVer\].*\r\n.*\<" + FileVersionInfo.GetVersionInfo(Application.ExecutablePath).FileVersion + @"\>.*\r\n", RegexOptions.IgnoreCase))
                        return;//不继续检查
                    
                    string ver = Regex.Match(textData, @"(?<=\[LatestVer\].*\r\n.*\<).*(?=\>.*\r\n)", RegexOptions.IgnoreCase).Value;       //05-13 新版本检测的新字段。以区分旧版本的新版本检测方法。
                    MatchCollection LocalVer = Regex.Matches(FileVersionInfo.GetVersionInfo(Application.ExecutablePath).FileVersion, @"\d+(?($|\.))", RegexOptions.IgnoreCase);
                    MatchCollection LatestVer = Regex.Matches(ver, @"\d+(?($|\.))", RegexOptions.IgnoreCase);

                    //对比本地版本于新版本
                    //05-13 主版本/次版本/生成版本/修订版本依个比较
                    for (int i = 0; i < LocalVer.Count; i++)
                    {
                        if(Convert.ToInt32(LocalVer[i].Value) < Convert.ToInt32(LatestVer[i].Value)
                            && MessageBox.Show("Current version:" + FileVersionInfo.GetVersionInfo(Application.ExecutablePath).FileVersion + "\r\nLastest version:" + ver + "\r\n\r\nWould you like to visit the download page now?", "New Version Available", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                            Process.Start(Regex.Match(textData, @"(?<=\[WebLink\].*\r\n.*\<).*(?=\>.*\r\n)", RegexOptions.IgnoreCase).Value);
                    }
                    /*if (Convert.ToInt32(FileVersionInfo.GetVersionInfo(Application.ExecutablePath).FileVersion.Replace(@".", "")) < Convert.ToInt32(ver.Replace(@".", ""))
                        && MessageBox.Show("Current version:" + FileVersionInfo.GetVersionInfo(Application.ExecutablePath).FileVersion + "\r\nLastest version:" + ver + "\r\n\r\nWould you like to visit the download page now?", "New Version Available", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                        Process.Start(Regex.Match(textData, @"(?<=\[WebLink\].*\r\n.*\<).*(?=\>.*\r\n)", RegexOptions.IgnoreCase).Value);*/

                }
            }
            finally
            {
                // Let the main application thread resume.
                
            }
            
        }

    }
}
