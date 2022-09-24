using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace AutoUpdateBingWallpaper.Util
{
    internal class HttpRequest
    {

        private static readonly HttpClient _httpClient = new HttpClient();

        private static readonly int BufferSize = 1024;

        /// <summary>
        /// 从网页上下载文件并保存到指定目录
        /// </summary>
        /// <param name="url">文件下载地址</param>
        /// <param name="directoryName">文件下载目录</param>
        /// <param name="fileName">文件名</param>
        /// <returns>下载是否成功</returns>
        public static async Task<bool> DownloadFile(string url, string directoryName, string fileName)
        {
            bool sign = true;
            var filePath = $"{directoryName}/{fileName}";
            if (System.IO.File.Exists(filePath))
            {
                return sign;
            }
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(url);
                using Stream stream = await response.Content.ReadAsStreamAsync();
                if (response != null && response.RequestMessage != null && response.RequestMessage.RequestUri != null)
                {
                    using (FileStream fileStream = new FileStream($"{directoryName}/{fileName}", FileMode.CreateNew))
                    {


                        byte[] buffer = new byte[BufferSize];
                        int readLength = 0;
                        int length;
                        while ((length = await stream.ReadAsync(buffer, 0, buffer.Length)) != 0)
                        {
                            readLength += length;
                            // 写入到文件
                            fileStream.Write(buffer, 0, length);
                        }
                    }
                }
            }
            catch (IOException)
            {
                //这里的异常捕获并不完善，请结合实际操作而定
                sign = false;
            }
            return sign;
        }


        /// <summary>
        /// Get请求.
        /// 
        /// 
        /// </summary>
        /// <param name="url">url</param>
        /// <returns>返回字符串</returns>
        public static async Task<string> GetAsync(string url)
        {
            return await _httpClient.GetStringAsync(url);
        }

        /// <summary>
        /// Post请求-待完善.
        /// 
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="httpContent">请求参数</param>
        /// <returns>返回数据</returns>
        public static async Task<HttpResponseMessage> PostAsync(string url, HttpContent httpContent)
        {
            return await _httpClient.PostAsync(url, httpContent);
        }
    }
}
