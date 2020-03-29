using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TlhPlatform.Core.Extensions
{
    public static class StreamExtensions
    {
        public static void CopyTo(this Stream fromStream, Stream toStream)
        {
            if (fromStream == null)
                throw new ArgumentNullException("fromStream");
            if (toStream == null)
                throw new ArgumentNullException("toStream");

            var bytes = new byte[8092];
            int dataRead;
            while ((dataRead = fromStream.Read(bytes, 0, bytes.Length)) > 0)
                toStream.Write(bytes, 0, dataRead);
        }

        public static byte[] ReadFully(this Stream stream)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
        /// <summary>
        /// 将流读为字符串
        /// 注：使用默认编码
        /// </summary>
        /// <param name="stream">流</param>
        /// <returns></returns>
        //public static string ReadToString(this Stream stream)
        //{
        //    string resStr = string.Empty;
        //    stream.Seek(0, SeekOrigin.Begin);
        //    resStr = new StreamReader(stream).ReadToEnd();
        //    stream.Seek(0, SeekOrigin.Begin);

        //    return resStr;
        //}
        public static string ReadToString(this Stream stream)
        {
            // convert stream to string
            StreamReader reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }

        /// <summary>
        /// Equals invariant
        /// </summary>
        /// <param name="str1">The STR1.</param>
        /// <param name="str2">The STR2.</param>
        /// <returns></returns>
        public static bool EqualsInvariant(this string str1, string str2)
        {
            return string.Equals(str1, str2, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// 将流Stream转为byte数组
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static byte[] ReadToBytes(this Stream stream)
        {
            stream.Seek(0, SeekOrigin.Begin);
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            stream.Seek(0, SeekOrigin.Begin);

            return bytes;
        }

      

        /// <summary>
        /// 将流读为字符串
        /// 注：使用指定编码
        /// </summary>
        /// <param name="stream">流</param>
        /// <param name="encoding">指定编码</param>
        /// <returns></returns>
        public static string ReadToString(this Stream stream, Encoding encoding)
        {
            string resStr = string.Empty;
            stream.Seek(0, SeekOrigin.Begin);
            resStr = new StreamReader(stream, encoding).ReadToEnd();
            stream.Seek(0, SeekOrigin.Begin);

            return resStr;
        }

        /// <summary>
        /// 获取Stream的Base64字符串
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static string GetBase64String(Stream stream)
        {
            byte[] arr = new byte[stream.Length];
            stream.Position = 0;
            stream.Read(arr, 0, (int)stream.Length);
            return Convert.ToBase64String(arr);
        }

        /// <summary>
        /// 将base64String反序列化到流，或保存成文件
        /// </summary>
        /// <param name="base64String"></param>
        /// <param name="savePath">如果为null则不保存</param>
        /// <returns></returns>
        public static Stream GetStreamFromBase64String(string base64String, string savePath)
        {
            byte[] bytes = Convert.FromBase64String(base64String);

            var memoryStream = new MemoryStream(bytes, 0, bytes.Length);
            memoryStream.Write(bytes, 0, bytes.Length);

            if (!string.IsNullOrEmpty(savePath))
            {
                SaveFileFromStream(memoryStream, savePath);
            }

            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }

        /// <summary>
        /// 将memoryStream保存到文件
        /// </summary>
        /// <param name="memoryStream"></param>
        /// <param name="savePath"></param>
        public static void SaveFileFromStream(MemoryStream memoryStream, string savePath)
        {
            memoryStream.Seek(0, SeekOrigin.Begin);
            using (var localFile = new FileStream(savePath, FileMode.OpenOrCreate))
            {
                localFile.Write(memoryStream.ToArray(), 0, (int)memoryStream.Length);
            }
        }



        /// <summary>
        /// 【异步方法】获取Stream的Base64字符串
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static async Task<string> GetBase64StringAsync(Stream stream)
        {
            byte[] arr = new byte[stream.Length];
            stream.Position = 0;
            await stream.ReadAsync(arr, 0, (int)stream.Length);
            return Convert.ToBase64String(arr);

        }

        /// <summary>
        /// 【异步方法】将base64String反序列化到流，或保存成文件
        /// </summary>
        /// <param name="base64String"></param>
        /// <param name="savePath">如果为null则不保存</param>
        /// <returns></returns>
        public static async Task<Stream> GetStreamFromBase64StringAsync(string base64String, string savePath)
        {
            byte[] bytes = Convert.FromBase64String(base64String);

            var memoryStream = new MemoryStream(bytes, 0, bytes.Length);
            await memoryStream.WriteAsync(bytes, 0, bytes.Length);

            if (!string.IsNullOrEmpty(savePath))
            {
                await SaveFileFromStreamAsync(memoryStream, savePath);
            }

            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }

        /// <summary>
        /// 【异步方法】将memoryStream保存到文件
        /// </summary>
        /// <param name="memoryStream"></param>
        /// <param name="savePath"></param>
        public static async Task SaveFileFromStreamAsync(MemoryStream memoryStream, string savePath)
        {
            memoryStream.Seek(0, SeekOrigin.Begin);
            using (var localFile = new FileStream(savePath, FileMode.OpenOrCreate))
            {
                await localFile.WriteAsync(memoryStream.ToArray(), 0, (int)memoryStream.Length);
            }
        }
    }
}
