using AutoUpdateBingWallpaper;
using AutoUpdateBingWallpaper.Util;
using FluentScheduler;
using System.Windows;

namespace ZBingWallpaper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Visibility = Visibility.Hidden;
            InitSystem();
        }

        public static async void InitSystem()
        {
            await BingWallpaper.Request();
            WindowsTaskbarIcon.Open();

            JobManager.Initialize();
            JobManager.AddJob(
                async () =>
                {
                    await Application.Current.Dispatcher.Invoke(
                       async delegate
                       {
                           //代码块
                           await BingWallpaper.Request();
                           WindowsTaskbarIcon.Refresh();
                       });
                },
                s => s.ToRunEvery(1).Days().At(0, 5)
            );
        }
    }
}
