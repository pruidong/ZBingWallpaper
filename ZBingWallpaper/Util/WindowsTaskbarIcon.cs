using Hardcodet.Wpf.TaskbarNotification;
using System;
using System.Windows;
using System.Windows.Controls;
using Image = AutoUpdateBingWallpaper.Entity.Image;

namespace AutoUpdateBingWallpaper.Util
{
    public static class WindowsTaskbarIcon
    {
        static TaskbarIcon WindowsNotifyIcon { get; set; }

        static readonly string DATE_FORMAT = "yyyyMMdd";

        static Image current { get; set; }
        public static void Open()
        {
            if (WindowsNotifyIcon is null)
            {
                InitNotifyIcon();
            }
        }
        public static void Exit()
        {
            if (WindowsNotifyIcon is null)
            {
                return;
            }

            WindowsNotifyIcon.Visibility = System.Windows.Visibility.Collapsed;
            WindowsNotifyIcon.Dispose();
        }

        static MenuItem GetTitleItem(string imgTitle, string imgCopyright)
        {
            MenuItem detail = new();
            detail.Header = imgTitle;
            detail.ToolTip = imgCopyright;
            detail.Click += delegate (object sender, RoutedEventArgs e)
            {
                Clipboard.SetDataObject(imgTitle + "\n" + imgCopyright);
                SendMessage("复制成功!");
            };
            return detail;
        }

        ///初始化托盘控件
        static async void InitNotifyIcon()
        {
            WindowsNotifyIcon = new TaskbarIcon
            {
                Icon = new System.Drawing.Icon("app.ico")
            };
            ContextMenu context = new();

            current = await LocalDataUtil.GetCurrentImage();

            string imgTitle = "", imgCopyright = ""; ;
            if (current != null)
            {
                imgTitle = current.title;
                imgCopyright = current.copyright;
            }
            context.Items.Add(GetTitleItem(imgTitle, imgCopyright));

            MenuItem prev = new();
            MenuItem next = new();
            prev.Header = "上一项";
            prev.Click += async delegate (object sender, RoutedEventArgs e)
            {
                if (current != null)
                {
                    DateTime startDate = DateTime.ParseExact(current.startdate, DATE_FORMAT, System.Globalization.CultureInfo.CurrentCulture);
                    var prevDate = startDate.AddDays(-1).ToString(DATE_FORMAT);
                    var imgObj = await LocalDataUtil.GetImageByText(prevDate);
                    if (imgObj != null)
                    {
                        next.IsEnabled = true;
                        current = imgObj;
                        BingWallpaper.SetWallPaper(imgObj);
                        _ = await LocalDataUtil.SaveCurrentImage(imgObj);
                        context.Items[0] = GetTitleItem(imgObj.title, imgObj.copyright);
                    }
                    else
                    {
                        SendMessage("没有更多可访问了!~");
                        prev.IsEnabled = false;
                    }

                }

            };
            context.Items.Add(prev);



            next.Header = "下一项";
            next.Click += async delegate (object sender, RoutedEventArgs e)
            {
                if (current != null)
                {
                    DateTime startDate = DateTime.ParseExact(current.startdate, DATE_FORMAT, System.Globalization.CultureInfo.CurrentCulture);
                    var nextDate = startDate.AddDays(1).ToString(DATE_FORMAT);
                    var imgObj = await LocalDataUtil.GetImageByText(nextDate);
                    if (imgObj != null)
                    {
                        prev.IsEnabled = true;
                        current = imgObj;
                        BingWallpaper.SetWallPaper(imgObj);
                        _ = await LocalDataUtil.SaveCurrentImage(imgObj);
                        context.Items[0] = GetTitleItem(imgObj.title, imgObj.copyright);
                    }
                    else
                    {
                        SendMessage("没有更多可访问了!~");
                        next.IsEnabled = false;
                    }
                }
            };
            next.IsEnabled = true;
            context.Items.Add(next);

            MenuItem refresh = new();
            refresh.Header = "刷新";
            refresh.Click += async delegate (object sender, RoutedEventArgs e)
            {
                _ = await BingWallpaper.Request();
            };
            context.Items.Add(refresh);

            MenuItem exit = new MenuItem();
            exit.Header = "退出";
            exit.Click += delegate (object sender, RoutedEventArgs e)
            {
                Environment.Exit(0);
            };
            context.Items.Add(exit);
            WindowsNotifyIcon.ContextMenu = context;
        }


        public static void SendMessage(string message)
        {
            if (message != null && message.Length > 0)
            {
                WindowsNotifyIcon.ShowBalloonTip("提示", message, BalloonIcon.Info);
            }
        }
    }
}
