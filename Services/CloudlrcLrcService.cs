using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static cloudlrc_win.ViewModels.MainViewModel;

namespace cloudlrc_win.Services
{
    class CloudlrcLrcService
    {
        public CloudlrcLrcService() { }
        public delegate void callback(string smg);
        public bool DownloadLrc(string id, IDType idType, ref string msg, callback c,ref CancellationToken cancellationToken)
        {
            if (idType == IDType.Single)
            {
                return DownloadSingle(id,ref msg);
            }
            else
            {
                return DownloadAlbumOrPlaylist(id, idType, ref msg, c, cancellationToken);
            }
            msg = "不支持的类型 " + idType.ToString();
            
            return false;
        }
        
        private bool DownloadSingle(string id, ref string msg)
        {
            //https://blog.csdn.net/wangzhichunnihao/article/details/111410953
            //同步读
            Process p = new Process();
            p.StartInfo.FileName = @"cloudlrc.exe";
            p.StartInfo.Arguments = "lrc music " + id;//参数
            p.StartInfo.StandardOutputEncoding = Encoding.UTF8;
            p.StartInfo.StandardErrorEncoding = Encoding.UTF8;
            //p.StartInfo.StandardInputEncoding = Encoding.UTF8;
            p.StartInfo.UseShellExecute =false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError =true;
            p.StartInfo.CreateNoWindow = true;
            p.Start();
            p.WaitForExit();
            
            string output = p.StandardOutput.ReadToEnd();
            string error = p.StandardError.ReadToEnd();
            int code = p.ExitCode;
            p.Close();
            p.Dispose();
            if (code == 0)
            {
                msg = output.TrimEnd();
                return true;
            }
            else
            {
                msg = error.TrimEnd();
                return false;
            }
        }
        private bool DownloadAlbumOrPlaylist(string id, IDType idType, ref string msg, callback c, CancellationToken cancellationToken)
        {
            string argu;
            if (idType == IDType.Album) { argu = "lrc album "+id; } else { argu = "lrc playlist " + id; }
            Process p = new Process();
            p.StartInfo.FileName = @"cloudlrc.exe";
            p.StartInfo.Arguments = argu;
            p.StartInfo.StandardOutputEncoding = Encoding.UTF8;
            p.StartInfo.StandardErrorEncoding = Encoding.UTF8;
            //p.StartInfo.StandardInputEncoding = Encoding.UTF8;
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.CreateNoWindow = true;
            p.Start();
            StreamReader sr = p.StandardOutput;
            while (!sr.EndOfStream)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    msg = "用户取消";
                    p.Kill();
                    p.Close();
                    p.Dispose();
                    return false;
                }
                c.Invoke(sr.ReadLine().TrimEnd());
            }
            p.WaitForExit();
            int code = p.ExitCode;
            p.Close();
            p.Dispose();
            if (code == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
