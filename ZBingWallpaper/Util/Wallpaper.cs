using Microsoft.Win32;
using System.Runtime.InteropServices;

namespace AutoUpdateBingWallpaper.Util
{
    public static class Wallpaper
    {
        const int SPI_SETDESKWALLPAPER = 20;
        const int SPIF_UPDATEINIFILE = 0x01;
        const int SPIF_SENDWININICHANGE = 0x02;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);

        public enum Style : int
        {
            Fill,
            Fit,
            Span,
            Stretch,
            Tile,
            Center
        }


        public static void SetWallPaper(string wpaper, Style style)
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop", true))
            {
                if (style == Style.Fill)
                {
                    key.SetValue(@"WallpaperStyle", 10.ToString());
                    key.SetValue(@"TileWallpaper", 0.ToString());
                }
                if (style == Style.Fit)
                {
                    key.SetValue(@"WallpaperStyle", 6.ToString());
                    key.SetValue(@"TileWallpaper", 0.ToString());
                }
                if (style == Style.Span) // Windows 8 or newer only!
                {
                    key.SetValue(@"WallpaperStyle", 22.ToString());
                    key.SetValue(@"TileWallpaper", 0.ToString());
                }
                if (style == Style.Stretch)
                {
                    key.SetValue(@"WallpaperStyle", 2.ToString());
                    key.SetValue(@"TileWallpaper", 0.ToString());
                }
                if (style == Style.Tile)
                {
                    key.SetValue(@"WallpaperStyle", 0.ToString());
                    key.SetValue(@"TileWallpaper", 1.ToString());
                }
                if (style == Style.Center)
                {
                    key.SetValue(@"WallpaperStyle", 0.ToString());
                    key.SetValue(@"TileWallpaper", 0.ToString());
                }
            }

            SystemParametersInfo(SPI_SETDESKWALLPAPER,
                    0,
                    wpaper,
                    SPIF_UPDATEINIFILE | SPIF_SENDWININICHANGE);
        }
    }

}
