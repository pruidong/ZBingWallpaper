using AutoUpdateBingWallpaper.Entity;
using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace AutoUpdateBingWallpaper.Util
{
    /// <summary>
    /// 
    /// 本地数据操作.
    /// 
    /// 
    /// </summary>
    internal class LocalDataUtil
    {

        private static readonly string LOCAL_DATA_PATH = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

        public static readonly string CURRENT = "current";

        private static string GetDataSavePath()
        {
            return $@"{LOCAL_DATA_PATH}\AutoUpdateBingWallpaper\LocalData\{DateTime.Now.Year}\{DateTime.Now.Month}\";
        }

        /// <summary>
        /// 保存文件
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="content"></param>
        public static async Task<bool> SaveText(string fileName, string content)
        {
            var savePath = GetDataSavePath();
            System.IO.Directory.CreateDirectory(savePath);
            var fileSavePath = savePath + fileName + ".json";
            if (System.IO.File.Exists(fileSavePath) && !fileName.Equals(CURRENT))
            {
                return false;
            }
            await File.WriteAllTextAsync(fileSavePath, content);
            return true;
        }


        /// <summary>
        /// 读取文件
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static async Task<string> GetText(string fileName)
        {
            var savePath = GetDataSavePath();
            var fileSavePath = savePath + fileName + ".json";
            if (System.IO.File.Exists(fileSavePath))
            {
                return await File.ReadAllTextAsync(fileSavePath);
            }
            return "";
        }

        /// <summary>
        /// 读取文件
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static async Task<Image> GetImageByText(string fileName)
        {
            var imageJson = await GetText(fileName);
            if (imageJson == null || imageJson == "")
            {
                return null;

            }
            return JsonSerializer.Deserialize<Image>(imageJson);
        }


        /// <summary>
        /// 读取当前
        /// 
        /// </summary>
        /// <returns></returns>
        public static async Task<Image> GetCurrentImage()
        {
            var imageJson = await GetText(CURRENT);
            if (imageJson == null || imageJson == "")
            {
                return null;

            }
            return JsonSerializer.Deserialize<Image>(imageJson);
        }


        /// <summary>
        /// 保存当前.
        /// 
        /// </summary>
        /// <param name="image"></param>
        public static async Task<bool> SaveCurrentImage(Image image)
        {
            string content = JsonSerializer.Serialize(image);
            return await SaveText(CURRENT, content);
        }

        /// <summary>
        /// 保存当前.
        /// 
        /// </summary>
        /// <param name="content"></param>
        public static async Task<bool> SaveCurrentText(string content)
        {
            return await SaveText(CURRENT, content);
        }


        /// <summary>
        /// 读取当前.
        /// 
        /// </summary>
        public static async Task<string> GetCurrentText()
        {
            return await GetText(CURRENT);
        }

    }
}
