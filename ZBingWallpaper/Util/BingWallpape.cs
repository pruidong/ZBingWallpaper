using AutoUpdateBingWallpaper.Entity;
using AutoUpdateBingWallpaper.Util;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace AutoUpdateBingWallpaper
{
    internal class BingWallpaper
    {

        // BING API
        private static readonly string BING_API = "https://cn.bing.com/HPImageArchive.aspx?format=js&idx=0&n=10&nc=1612409408851&pid=hp&FORM=BEHPTB&uhd=1&uhdwidth=3840&uhdheight=2160";

        private static readonly string BING_URL = "https://cn.bing.com";

        private static readonly string BING_SAVE_PATH = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);





        /// <summary>
        /// 数据请求入口.
        /// 
        /// </summary>
        public static async Task<string> Request()
        {
            string result = await HttpRequest.GetAsync(BING_API);
            Root? roots =
               JsonSerializer.Deserialize<Root>(result);
            if (roots != null)
            {
                List<Image> imageList = roots.images;
                for (int i = 0; i < imageList.Count; i++)
                {
                    Image item = imageList[i];
                    // 日期作为key.
                    string startDate = item.startdate;
                    string imgObjString = JsonSerializer.Serialize(item);
                    await LocalDataUtil.SaveText(startDate, imgObjString);
                    // 
                    if (i == 0)
                    {
                        await LocalDataUtil.SaveCurrentText(imgObjString);
                        DownloadFile(item, true);
                    }
                    else
                    {
                        DownloadFile(item, false);
                    }

                }

            }
            return "success";

        }


        /// <summary>
        /// 下载文件并设置桌面壁纸.
        /// 
        /// </summary>
        /// <param name="image">图片对象</param>
        public static async void DownloadFile(Image image, bool isSetWallPaper)
        {
            if (image != null)
            {
                string fileSavePath = $@"{BING_SAVE_PATH}\{DateTime.Now.Year}\{DateTime.Now.Month}";
                System.IO.Directory.CreateDirectory(fileSavePath);

                string fileUrl = BING_URL + image.url;
                bool resultDownload = await HttpRequest.DownloadFile(fileUrl, fileSavePath, $"{image.startdate}.jpg");
                if (resultDownload && isSetWallPaper)
                {
                    SetWallPaper(image);
                }
            }
        }

        /// <summary>
        /// 设置桌面壁纸
        /// 
        /// </summary>
        /// <param name="image">图片对象</param>
        public static void SetWallPaper(Image image)
        {
            if (image != null)
            {
                string fileSavePath = $@"{BING_SAVE_PATH}\{DateTime.Now.Year}\{DateTime.Now.Month}";
                string imgPath = $@"{fileSavePath}\{image.startdate}.jpg";
                if (System.IO.File.Exists(imgPath))
                {
                    Wallpaper.SetWallPaper(imgPath, Wallpaper.Style.Fill);
                }
            }

        }

    }
}
