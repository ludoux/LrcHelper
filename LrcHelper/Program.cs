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
                client.DownloadDataAsync(new Uri("https://raw.githubusercontent.com/Ludoux/LrcHelper/master/LrcHelper/LrcHelper/UpdateInfo/UpInfo.txt"));
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

                    string ver = Regex.Match(textData, @"(?<=\[Ver\].*\r\n.*\<).*(?=\>.*\r\n)", RegexOptions.IgnoreCase).Value;
                    if (Convert.ToInt32(FileVersionInfo.GetVersionInfo(Application.ExecutablePath).FileVersion.Replace(@".", "")) < Convert.ToInt32(ver.Replace(@".", ""))
                        && MessageBox.Show("We found lastest version" + ver + "\r\n Do you want to visit the download page and get it?", "New Version Found", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                    {
                        Process.Start(Regex.Match(textData, @"(?<=\[WebLink\].*\r\n.*\<).*(?=\>.*\r\n)", RegexOptions.IgnoreCase).Value);
                    }

                }
            }
            finally
            {
                // Let the main application thread resume.
                
            }
            
        }

    }
}
