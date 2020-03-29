using System;
using System.Collections.Generic;
using System.Text;

namespace TlhPlatform.Core.Encryption
{

    /// <summary>
    /// 提供加密解密方法
    /// </summary>
    public static class EncryptionHelper
    {
        #region const
        /// <summary>
        /// 默认使用的适合于DES,RC2算法的Key
        /// </summary>
        private const string m_ShortDefaultKey = "Soft";
        /// <summary>
        /// 默认使用的TRIPLEDES，RIJNDAEL算法的Key
        /// </summary> 

        private const string m_LongDefaultKey = "Soft.VeryKey";

        /// <summary>
        /// 默认使用的适合于DES,RC2,TRIPLEDES算法的InitVector
        /// </summary>
        private const string m_ShortDefaultIV = "2001";

        /// <summary>
        /// 默认使用的适合于RIJNDAEL算法的InitVector
        /// </summary> 

        private const string m_LongDefaultIV = "VeryKey3";

        #endregion
        private static byte[] m_Key = null;



        private static byte[] m_initVec = null;

        #region properties
        /// <summary>
        /// 使用的加密明文密码
        /// </summary>
        public static string Key
        {
            get
            {
                string result = string.Empty;
                if (m_Key != null)
                {
                    result = Encoding.Unicode.GetString(m_Key);
                }
                return result;
            }

        }
        /// <summary>
        /// 使用加密的起始偏移量
        /// </summary>
        public static string InitVector
        {
            get
            {
                string result = string.Empty;
                if (m_initVec != null)
                {
                    result = Encoding.Unicode.GetString(m_initVec);
                }
                return result;
            }

        }
        #endregion

        /// <summary>
        /// 接受用户明文密码，本次加密使用的算法，完成对指定字符串的加密
        /// <example><![CDATA[string result = EncryptionHelper.Encrypt("1234", EncryptionAlgorithm.Rc2, "5678", text);]]></example>
        /// <remarks>各个算法对明文密码和初始化偏移量的要求不一样
        /// DES和RC2:明文密码长度和初始化偏移量长度 必须是4个字符;
        /// Rijndael:明文密码长度可以是12,16个字符,初始化偏移量为8个字符;
        /// TripleDes:明文密码长度可以为8个和12个字符,初始化偏移量为4个字符;
        /// 注意:如果提供的明文密码和偏移量不符合算法的标准,将使用随机密码,使用者在加密后通过提供的Key和InitVector属性获得加密时使用的明文密码和偏移量
        /// </remarks>
        /// </summary>
        /// <param name="plainText">使用的加密明文密码</param>
        /// <param name="algorithm">加密算法枚举对象</param>
        /// <param name="userInitialVector">使用的加密起始偏移量</param>
        /// <param name="encryptionText">需要加密的字符</param>
        /// <returns></returns>
        public static string Encrypt(string plainText, EncryptionAlgorithm algorithm, string userInitialVector, string encryptionText)
        {
            string result = string.Empty;

            if (encryptionText.Length > 0)
            {
                SetKey(algorithm, plainText);
                SetInitialVector(algorithm, userInitialVector);
                EncryptTransform transform = new EncryptTransform(m_Key, m_initVec);
                byte[] bytes = Encoding.Unicode.GetBytes(encryptionText);
                byte[] inArray = transform.Encrypt(algorithm, bytes);
                if (inArray.Length > 0)
                {
                    result = Convert.ToBase64String(inArray);
                }
                m_Key = transform.Key;
                m_initVec = transform.InitVec;

            }
            return result;

        }

        /// <summary>
        /// 指定需要加密的字符串使用默认的明文密码和算法(DES)进行加密
        /// </summary>
        /// <param name="encryptionText"></param>
        /// <returns></returns>
        public static string Encrypt(string encryptionText)
        {
            string result = string.Empty;
            result = Encrypt(m_ShortDefaultKey, EncryptionAlgorithm.Des, m_ShortDefaultIV, encryptionText);
            return result;
        }
        /// <summary>
        /// 指定需要加密的字符串和算法使用默认的明文密码进行加密
        /// </summary>
        /// <param name="algorithm"></param>
        /// <param name="encryptionText"></param>
        /// <returns></returns>
        public static string Encrypt(EncryptionAlgorithm algorithm, string encryptionText)
        {
            string result = string.Empty;
            string key = GetDefaultKey(algorithm);
            string iv = GetDefaultIV(algorithm);
            result = Encrypt(key, algorithm, iv, encryptionText);
            return result;
        }



        /// <summary>
        /// 接受用户明文密码，使用默认的加密算法完成对指定字符串的加密
        /// <remarks>默认为DES算法进行加密</remarks>
        ///  <example><![CDATA[string result = EncryptionHelper.Encrypt("1234", "5678", text);]]></example>
        /// </summary>
        /// <param name="plainText">使用的加密明文密码</param>
        /// <param name="userInitialVector">使用的加密起始偏移量</param>
        /// <param name="encryptionText">需要加密的字符串</param>
        /// <returns>加密后的字符串</returns>
        public static string Encrypt(string plainText, string userInitialVector, string encryptionText)
        {
            string result = string.Empty;
            result = Encrypt(plainText, EncryptionAlgorithm.Des, userInitialVector, encryptionText);
            return result;
        }

        /// <summary>
        /// 接受用户明文密码，本次使用的解密算法，完成对指定字符串的解密；
        /// 指定的解密算法必须和字符串使用加密的算法一致
        /// 各个算法对明文密码和初始化偏移量的要求不一样
        /// DES和RC2:明文密码长度和初始化偏移量长度 必须是4个字符;
        /// Rijndael:明文密码长度可以是12,16个字符,初始化偏移量为8个字符;
        /// TripleDes:明文密码长度可以为8个和12个字符,初始化偏移量为4个字符;
        /// </summary>
        /// <param name="plainText">加密明文密码</param>
        /// <param name="algorithm">使用的加密算法</param>
        /// <param name="userInitialVector">加密时使用的起始偏移量</param>
        /// <param name="encryptionText">加密后的字符串</param>
        /// <returns></returns>
        public static string Decrypt(string plainText, EncryptionAlgorithm algorithm, string userInitialVector, string encryptionText)
        {
            string result = string.Empty;
            SetKey(algorithm, plainText);
            SetInitialVector(algorithm, userInitialVector);
            EncryptTransform transform = new EncryptTransform(m_Key, m_initVec);
            byte[] bytesData = Convert.FromBase64String(encryptionText);
            byte[] bytes = null;
            try
            {
                bytes = transform.Decrypt(algorithm, bytesData);
            }
            catch (Exception exception)
            {
                throw exception;
            }
            if (bytes.Length > 0)
            {
                result = Encoding.Unicode.GetString(bytes);
            }
            m_Key = transform.Key;
            m_initVec = transform.InitVec;
            return result;

        }

        /// <summary>
        /// 接受用户明文密码，使用默认解密算法完成对指定字符串的解密
        /// </summary>
        /// <param name="plainText">加密时使用的明文密码</param>
        /// <param name="userInitialVector">加密时使用的起始偏移量</param>
        /// <param name="encryptionText">加密后的字符串</param>
        /// <returns></returns>
        public static string Decrypt(string plainText, string userInitialVector, string encryptionText)
        {
            string result = string.Empty;
            result = Decrypt(plainText, EncryptionAlgorithm.Des, userInitialVector, encryptionText);
            return result;

        }

        /// <summary>
        /// 使用默认的算法(DES)和默认的明文密码进行解密
        /// </summary>
        /// <param name="encryptionText"></param>
        /// <returns></returns>
        public static string Decrypt(string encryptionText)
        {
            string result = string.Empty;
            result = Decrypt(m_ShortDefaultKey, EncryptionAlgorithm.Des, m_ShortDefaultIV, encryptionText);
            return result;

        }
        /// <summary>
        /// 指定加密时使用的算法,使用默认的明文密码进行解密
        /// </summary>
        /// <param name="algorithm"></param>
        /// <param name="encryptionText"></param>
        /// <returns></returns>
        public static string Decrypt(EncryptionAlgorithm algorithm, string encryptionText)
        {
            string result = string.Empty;
            string key = GetDefaultKey(algorithm);
            string iv = GetDefaultIV(algorithm);
            result = Decrypt(key, algorithm, iv, encryptionText);
            return result;

        }

        /// <summary>
        /// 根据算法获得默认的Key
        /// </summary>
        /// <param name="algorithm"></param>
        /// <returns></returns>
        private static string GetDefaultKey(EncryptionAlgorithm algorithm)
        {
            string result = m_ShortDefaultKey;
            switch (algorithm)
            {
                case EncryptionAlgorithm.Rijndael:
                    { 
                        result = m_LongDefaultKey; 
                        break;
                    }
                case EncryptionAlgorithm.TripleDes:
                    {
                         
                        result = m_LongDefaultKey;
                   
                        break;
                    }
            }
            return result;
        }
        /// <summary>
        /// 根据算法获得默认的IV
        /// </summary>
        /// <param name="algorithm"></param>
        /// <returns></returns>
        private static string GetDefaultIV(EncryptionAlgorithm algorithm)
        {
            string result = m_ShortDefaultIV;
            switch (algorithm)
            {
                case EncryptionAlgorithm.Rijndael:
                    {
                        
                        result = m_LongDefaultIV;
                   
                        break;
                    }

            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="algorithm"></param>
        /// <param name="initialVector"></param>
        /// <returns></returns>
        private static void SetInitialVector(EncryptionAlgorithm algorithm, string initialVector)
        {
            if (null != initialVector)
            {
                byte[] bytes = Encoding.Unicode.GetBytes(initialVector);
                switch (algorithm)
                {
                    case EncryptionAlgorithm.Des:
                        {
                            if (bytes.Length == 8)
                            {
                                m_initVec = bytes;

                            }
                            break;
                        }

                    case EncryptionAlgorithm.Rc2:
                        {
                            if (bytes.Length == 8)
                            {
                                m_initVec = bytes;

                            }
                            break;
                        }
                    case EncryptionAlgorithm.Rijndael:
                        {
                            if (bytes.Length == 16)
                            {
                                m_initVec = bytes;

                            }
                            break;
                        }
                    case EncryptionAlgorithm.TripleDes:
                        {
                            if (bytes.Length == 8)
                            {
                                m_initVec = bytes;

                            }
                            break;
                        }
                }
            }
        }

        private static void SetKey(EncryptionAlgorithm algorithm, string key)
        {
            if (null != key)
            {
                byte[] keyByte = Encoding.Unicode.GetBytes(key);
                switch (algorithm)
                {
                    case EncryptionAlgorithm.Des:
                        {
                            if (keyByte.Length == 8)
                            {
                                m_Key = keyByte;
                            }
                            break;
                        }

                    case EncryptionAlgorithm.Rc2:
                        {
                            if (keyByte.Length == 8)
                            {
                                m_Key = keyByte;
                            }
                            break;
                        }
                    case EncryptionAlgorithm.Rijndael:
                        {
                            if (keyByte.Length == 24 || (keyByte.Length == 32))
                            {
                                m_Key = keyByte;
                            }
                            break;
                        }
                    case EncryptionAlgorithm.TripleDes:
                        {
                            if (keyByte.Length == 16 || (keyByte.Length == 24))
                            {
                                m_Key = keyByte;
                            }
                            break;
                        }
                }
            }
        }

    }
}
