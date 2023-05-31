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
    public partial class LoginViewModel : ObservableObject
    {
        private bool _isInitialized = false;
        

        [ObservableProperty]
        private string _imgPath = "28151022";

        public LoginViewModel()
        {
        }
        public void InitializeViewModel()
        {
            _isInitialized = true;
            ImgPath = Directory.GetCurrentDirectory() + "\\qr.png";

        }

    }
}
