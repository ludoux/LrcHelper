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
                MatchCollection LocalVer = Regex.Matches(FileVersionInfo.GetVersionInfo(Application.ExecutablePath).FileVersion, @"\d+(?($|\.))", RegexOptions.IgnoreCase);
                
                client.Headers["User-Agent"] = @"163lrc" + LocalVer[0] + "." + LocalVer[1] + "." + LocalVer[2];
                client.Encoding = System.Text.Encoding.UTF8;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                client.DownloadStringAsync(new Uri("https://api.ludoux.com/163lrcwin/update?cver=" + LocalVer[0] + "." + LocalVer[1] + "." + LocalVer[2]));
                client.DownloadStringCompleted += Client_DownloadStringCompleted;
                
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new LrcDownloader());
        }

        private static void Client_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                // If the request was not canceled and did not throw
                // an exception, display the resource.
                if (!e.Cancelled && e.Error == null)
                {
                    MatchCollection UpdateMc = Regex.Matches(e.Result, @"(?<="")[^,]+?(?="")");
                    if(UpdateMc[0].Value == "T" && UpdateMc[1].Value == "T")
                    {
                        MessageBox.Show("当前版本：" + Regex.Match(FileVersionInfo.GetVersionInfo(Application.ExecutablePath).FileVersion, @"\d+?\.\d+?\.\d+?(?=\.)") + "\r\n最新版本：" + UpdateMc[2].Value + "\r\n更新内容：" + UpdateMc[3].Value + "\r\n由于新版本包含安全及重大功能更新，旧版本不再可用。请立即下载新版本", "新版本可用", MessageBoxButtons.OK ,MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                        Process.Start(UpdateMc[4].Value);
                        Application.Exit();
                    }
                    else if(UpdateMc[0].Value == "T" && UpdateMc[1].Value == "F")
                    {
                        if(MessageBox.Show("当前版本：" + Regex.Match(FileVersionInfo.GetVersionInfo(Application.ExecutablePath).FileVersion, @"\d+?\.\d+?\.\d+?(?=\.)") + "\r\n最新版本：" + UpdateMc[2].Value + "\r\n更新内容：" + UpdateMc[3].Value + "\r\n请立即下载新版本", "新版本可用", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                            Process.Start(UpdateMc[4].Value);
                    }


                            // MessageBox.Show("Current version:" + FileVersionInfo.GetVersionInfo(Application.ExecutablePath).FileVersion + "\r\nLastest version:" + ver + "\r\n\r\nWould you like to visit the download page now?", "New Version Available", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                            //Process.Start(Regex.Match(textData, @"(?<=\[WebLink\].*\r\n.*\<).*(?=\>.*\r\n)", RegexOptions.IgnoreCase).Value);

                }
            }
            finally
            {
                // Let the main application thread resume.
                
            }
            
        }

    }
}
