using cloudlrc_win.Services;
using cloudlrc_win.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Wpf.Ui.Common;
using Wpf.Ui.Controls;

namespace cloudlrc_win.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        CloudlrcLoginService LoginService;
        CloudlrcLrcService LrcService;
        private bool _isInitialized = false;
        public enum IDType
        {
            //单曲
            Single,
            //专辑
            Album,
            //歌单
            Playlist
        }

        [ObservableProperty]
        private IDType _currentIDType = IDType.Single;
        [ObservableProperty]
        private string _id = "28151022";
        [ObservableProperty]
        private string _loginStatusText = "点击查看登录状态";
        [ObservableProperty]
        private string _resultStatusText = "结果状态信息";

        public MainViewModel()
        {
            //CheckLoginCommand = new AsyncRelayCommand(CheckLogin);
            //DownloadLrcCommand = new AsyncRelayCommand(DownloadLrc);
        }
        public void InitializeViewModel()
        {
            _isInitialized = true;

            LoginService = new CloudlrcLoginService();
            LrcService = new CloudlrcLrcService();
            CheckLoginAsync();

        }

        private async void login()
        {
            if (File.Exists("qr.png"))
            {
                File.Delete("qr.png");
            }
            var login = new CloudlrcLoginService();
            string msg = "";
            string unikey;
            bool ok = false;
            ok = login.GenQrFile(ref msg);
            if (ok)
            {
                unikey = msg.Split(" ")[1];
                var w = new LoginWindow();
                w.ShowDialog();
                if (File.Exists("qr.png"))
                {
                    File.Delete("qr.png");
                }
                await Task.Run(() => ok = login.LoginViaUnikey(unikey, ref msg));
                await CheckLoginAsync();
            }
            else
            {
                (Application.Current.MainWindow as MainWindow)?.RootSnackbar.SetValue(Snackbar.CloseButtonEnabledProperty, false);
                (Application.Current.MainWindow as MainWindow)?.RootSnackbar.SetValue(Snackbar.TimeoutProperty, 3000);
                (Application.Current.MainWindow as MainWindow)?.RootSnackbar.SetValue(Snackbar.IconProperty, SymbolRegular.Dismiss12);
                (Application.Current.MainWindow as MainWindow)?.RootSnackbar.SetValue(Snackbar.AppearanceProperty, ControlAppearance.Danger);
                (Application.Current.MainWindow as MainWindow)?.RootSnackbar.ShowAsync(String.Format("生成登录 QR 码失败-> {0}", msg));
            }
            
        }
        [RelayCommand]
        private async Task StatusBadgeClick()
        {
            if (LoginStatusText.Contains("已登录"))
            {
                var m = new Wpf.Ui.Controls.MessageBox
                {
                    Title = "已登录，需要使用新账户重新登录么",
                    Content = string.Format("{0}{1}以新身份登录将会移除当前的登录凭证，并生成登录 QR 码", LoginStatusText, Environment.NewLine),
                    ButtonLeftAppearance = ControlAppearance.Primary,
                    ButtonLeftName = "以新身份登录",
                    ButtonRightName = "维持现状"
                };
                m.ButtonLeftClick += (o, i) =>
                {
                    m.Close();
                    login();
                };
                m.ButtonRightClick += (o, i) => { m.Close(); };
                m.ShowDialog();
            }
            else if (LoginStatusText.Contains("未登录"))
            {
                login();
            }
        }
        private async Task CheckLoginAsync()
        {
            //Window lw = new LoginWindow();
            //lw.ShowDialog();
            string msg = "";
            bool ok = false;
            await Task.Run(() => ok = LoginService.GetLoginStatus(ref msg));
            LoginStatusText = msg;
            if (ok)
            {
                if (msg.Contains("已登录"))
                {
                    (Application.Current.MainWindow as MainWindow)?.LoginStatusBadge.SetValue(Badge.AppearanceProperty, ControlAppearance.Success);
                }
                else
                {
                    (Application.Current.MainWindow as MainWindow)?.LoginStatusBadge.SetValue(Badge.AppearanceProperty, ControlAppearance.Caution);
                }
            }
            else
            {
                (Application.Current.MainWindow as MainWindow)?.LoginStatusBadge.SetValue(Badge.AppearanceProperty, ControlAppearance.Dark);
            }
        }

        private void callback(string msg)
        {
            ResultStatusText = msg;
        }

        [RelayCommand]
        private async Task DownloadLrcAsync()
        {
            //先把 snackbar 关了
            (Application.Current.MainWindow as MainWindow)?.RootSnackbar.HideAsync();
            string msg = "";
            bool ok = false;
            await Task.Run(() => ok = LrcService.DownloadLrc(Id, CurrentIDType, ref msg, callback));
            if (ok)
            {
                (Application.Current.MainWindow as MainWindow)?.RootSnackbar.SetValue(Snackbar.CloseButtonEnabledProperty, false);
                (Application.Current.MainWindow as MainWindow)?.RootSnackbar.SetValue(Snackbar.TimeoutProperty, 3000);
                (Application.Current.MainWindow as MainWindow)?.RootSnackbar.SetValue(Snackbar.IconProperty, SymbolRegular.Checkmark12);
                (Application.Current.MainWindow as MainWindow)?.RootSnackbar.SetValue(Snackbar.AppearanceProperty, ControlAppearance.Success);
                (Application.Current.MainWindow as MainWindow)?.RootSnackbar.ShowAsync(String.Format("下载成功-> {0}", msg));
                ResultStatusText = msg;
            }
            else
            {
                (Application.Current.MainWindow as MainWindow)?.RootSnackbar.SetValue(Snackbar.CloseButtonEnabledProperty, false);
                (Application.Current.MainWindow as MainWindow)?.RootSnackbar.SetValue(Snackbar.TimeoutProperty, 3000);
                (Application.Current.MainWindow as MainWindow)?.RootSnackbar.SetValue(Snackbar.IconProperty, SymbolRegular.Dismiss12);
                (Application.Current.MainWindow as MainWindow)?.RootSnackbar.SetValue(Snackbar.AppearanceProperty, ControlAppearance.Danger);
                (Application.Current.MainWindow as MainWindow)?.RootSnackbar.ShowAsync(String.Format("下载失败-> {0}", msg));
                ResultStatusText = msg;
            }

            //return "";
        }
    }
}
