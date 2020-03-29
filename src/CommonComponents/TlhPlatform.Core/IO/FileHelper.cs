// -----------------------------------------------------------------------
//  <copyright file="FileHelper.cs" company="TlhPlatform.Core开源团队">
//      Copyright (c) 2014 TlhPlatform.Core. All rights reserved.
//  </copyright>
//  <last-editor>Andy</last-editor>
//  <last-date>2014-07-18 18:25</last-date>
// -----------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;


namespace TlhPlatform.Core.IO
{
    /// <summary>
    /// 文件辅助操作类
    /// </summary>
    public class FileHelper
    {

        /// <summary>
        /// 反射获取类信息
        /// </summary>
        /// <param name="assemblyName">程序集名称</param>
        /// <param name="className">类名</param>
        /// <returns></returns>
        public static Type GetAbsolutePath(string assemblyName, string className)
        {
            Assembly assembly = Assembly.Load(new AssemblyName(assemblyName));
            Type type = assembly.GetType(className);
            return type;
        }
        /// <summary>
        /// 创建文件，如果文件不存在
        /// </summary>
        /// <param name="fileName">要创建的文件</param>
        public static void CreateIfNotExists(string fileName)
        {
            if (File.Exists(fileName))
            {
                return;
            }
            File.Create(fileName);
        }

        /// <summary>
        /// 删除指定文件
        /// </summary>
        /// <param name="fileName">要删除的文件名</param>
        public static void DeleteIfExists(string fileName)
        {
            if (!File.Exists(fileName))
            {
                return;
            }
            File.Delete(fileName);
        }

        /// <summary>
        /// 设置或取消文件的指定<see cref="FileAttributes"/>属性
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="attribute">要设置的文件属性</param>
        /// <param name="isSet">true为设置，false为取消</param>
        public static void SetAttribute(string fileName, FileAttributes attribute, bool isSet)
        {
            FileInfo fi = new FileInfo(fileName);
            if (!fi.Exists)
            {
                throw new FileNotFoundException("要设置属性的文件不存在。", fileName);
            }
            if (isSet)
            {
                fi.Attributes = fi.Attributes | attribute;
            }
            else
            {
                fi.Attributes = fi.Attributes & ~attribute;
            }
        }

        /// <summary>
        /// 获取文件版本号
        /// </summary>
        /// <param name="fileName"> 完整文件名 </param>
        /// <returns> 文件版本号 </returns>
        public static string GetVersion(string fileName)
        {
            if (File.Exists(fileName))
            {
                FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(fileName);
                return fvi.FileVersion;
            }
            return null;
        }

        /// <summary>
        /// 获取文件的MD5值
        /// </summary>
        /// <param name="fileName"> 文件名 </param>
        /// <returns> 32位MD5 </returns>
        public static string GetFileMd5(string fileName)
        {
            using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                const int bufferSize = 1024 * 1024;
                byte[] buffer = new byte[bufferSize];
                using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
                {
                    md5.Initialize();
                    long offset = 0;
                    while (offset < fs.Length)
                    {
                        long readSize = bufferSize;
                        if (offset + readSize > fs.Length)
                        {
                            readSize = fs.Length - offset;
                        }
                        fs.Read(buffer, 0, (int)readSize);
                        if (offset + readSize < fs.Length)
                        {
                            md5.TransformBlock(buffer, 0, (int)readSize, buffer, 0);
                        }
                        else
                        {
                            md5.TransformFinalBlock(buffer, 0, (int)readSize);
                        }
                        offset += bufferSize;
                    }
                    fs.Close();
                    byte[] result = md5.Hash;
                    md5.Clear();
                    StringBuilder sb = new StringBuilder(32);
                    foreach (byte b in result)
                    {
                        sb.Append(b.ToString("X2"));
                    }
                    return sb.ToString();
                }
            }
        }
        /// <summary>
        /// 生成文件的MD5校验码
        /// </summary>
        /// <param name="fileName">文件路径</param>
        /// <returns></returns>
        public static string GetMD5HashFromFile(string fileName)
        {

            FileStream file = new FileStream(fileName, System.IO.FileMode.Open, FileAccess.Read);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] retVal = md5.ComputeHash(file);
            file.Close();
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < retVal.Length; i++)
            {
                sb.Append(retVal[i].ToString("x2"));
            }
            return sb.ToString();

        }
        /// <summary>
        /// 生成字符串的MD5校验码
        /// </summary>
        /// <param name="strText">字符串内容</param>
        /// <returns></returns>
        public static string GetMD5HashFromString(string strText)
        {

            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(strText);
            bytes = md5.ComputeHash(bytes);
            md5.Clear();
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                sb.Append(Convert.ToString(bytes[i], 16).PadLeft(2, '0'));
            }
            return sb.ToString();
        }
    }
}