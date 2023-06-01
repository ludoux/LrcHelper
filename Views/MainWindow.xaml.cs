using cloudlrc_win.Services;
using cloudlrc_win.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Wpf.Ui.Controls;
using Wpf.Ui.Mvvm.Interfaces;

namespace cloudlrc_win.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : UiWindow
    {

        public MainWindow()
        {
            InitializeComponent();
            MainViewModel viewModel = new MainViewModel();
            viewModel.InitializeViewModel();
            this.DataContext = viewModel;

            Loaded += (sender, args) =>
            {
                //var currentTheme = Wpf.Ui.Appearance.Theme.GetAppTheme();
                var lightTheme = Wpf.Ui.Appearance.ThemeType.Light;
                Wpf.Ui.Appearance.Watcher.Watch(
                    this,                                  // Window class
                    Wpf.Ui.Appearance.BackgroundType.None, // Background type
                    true                                   // Whether to change accents automatically
                );
                Wpf.Ui.Appearance.Theme.Apply(
  lightTheme,     // Theme type
  Wpf.Ui.Appearance.BackgroundType.None, // Background type
  true                                   // Whether to change accents automatically
);
            };
            Closing += (sender, args) => {
                //为了直接关闭主窗口时候，可以把 cloudlrc 的进程也给关闭
                Hide();
                viewModel.DownloadLrcCancel();
                System.Threading.Thread.Sleep(2000);
            };
        }




    }
}
