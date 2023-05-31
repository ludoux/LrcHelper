using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace cloudlrc_win.Services
{
    class CloudlrcLoginService
    {
        public CloudlrcLoginService() { }
        public bool GetLoginStatus(ref string msg)
        {
            //https://blog.csdn.net/wangzhichunnihao/article/details/111410953
            //同步读
            Process p = new Process();
            p.StartInfo.FileName = @"cloudlrc.exe";
            p.StartInfo.Arguments = "netease login status";//参数
            p.StartInfo.StandardOutputEncoding = Encoding.UTF8;
            p.StartInfo.StandardErrorEncoding = Encoding.UTF8;
            //p.StartInfo.StandardInputEncoding = Encoding.UTF8;
            p.StartInfo.UseShellExecute =false;//不使用操作系统外壳程序启动线程(一定为FALSE,详细的请看MSDN)
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError =true;
            p.StartInfo.CreateNoWindow = true;
            p.Start();
            p.WaitForExit();
            if (p.ExitCode == 0)
            {
                msg = p.StandardOutput.ReadToEnd();
                p.Close();//关闭进程
                p.Dispose();//释放资源
                return true;
            }
            else
            {
                msg = p.StandardError.ReadToEnd();
                p.Close();//关闭进程
                p.Dispose();//释放资源
                return false;
            }
        }

        public bool GenQrFile(ref string msg)
        {
            //https://blog.csdn.net/wangzhichunnihao/article/details/111410953
            //同步读
            Process p = new Process();
            p.StartInfo.FileName = @"cloudlrc.exe";
            p.StartInfo.Arguments = "netease login gen --qrfile";//参数
            p.StartInfo.StandardOutputEncoding = Encoding.UTF8;
            p.StartInfo.StandardErrorEncoding = Encoding.UTF8;
            //p.StartInfo.StandardInputEncoding = Encoding.UTF8;
            p.StartInfo.UseShellExecute = false;//不使用操作系统外壳程序启动线程(一定为FALSE,详细的请看MSDN)
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.CreateNoWindow = true;
            p.Start();
            p.WaitForExit();
            if (p.ExitCode == 0)
            {
                msg = p.StandardOutput.ReadToEnd();
                p.Close();//关闭进程
                p.Dispose();//释放资源
                return true;
            }
            else
            {
                msg = p.StandardError.ReadToEnd();
                p.Close();//关闭进程
                p.Dispose();//释放资源
                return false;
            }
        }

        public bool LoginViaUnikey(string unikey, ref string msg)
        {
            //https://blog.csdn.net/wangzhichunnihao/article/details/111410953
            //同步读
            Process p = new Process();
            p.StartInfo.FileName = @"cloudlrc.exe";
            p.StartInfo.Arguments = "netease login check " + unikey;//参数
            p.StartInfo.StandardOutputEncoding = Encoding.UTF8;
            p.StartInfo.StandardErrorEncoding = Encoding.UTF8;
            //p.StartInfo.StandardInputEncoding = Encoding.UTF8;
            p.StartInfo.UseShellExecute = false;//不使用操作系统外壳程序启动线程(一定为FALSE,详细的请看MSDN)
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.CreateNoWindow = true;
            p.Start();
            p.WaitForExit();
            if (p.ExitCode == 0)
            {
                msg = p.StandardOutput.ReadToEnd();
                p.Close();//关闭进程
                p.Dispose();//释放资源
                return true;
            }
            else
            {
                msg = p.StandardError.ReadToEnd();
                p.Close();//关闭进程
                p.Dispose();//释放资源
                return false;
            }
        }
    }
}
