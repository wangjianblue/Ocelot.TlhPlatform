using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace TlhPlatform.Core.Encryption
{
    /// <summary>
    /// 提供加密解密方法
    /// </summary>
    internal class EncryptTransform
    {
        //private const int c_MaxLengthOf_IV_DES = 4;
        //private const int c_MaxLengthOf_IV_RC2 = 4;
        //private const int c_MaxLengthOf_IV_RIJNDAEL = 8;
        //private const int c_MaxLengthOf_IV_TRIPLEDES = 4;
        //private const int c_MaxLengthOf_Key_DES = 4;
        //private const int c_MaxLengthOf_Key_RC2 = 4;
        //private const int c_MaxLengthOf_Key_RIJNDAEL_1 = 8;
        //private const int c_MaxLengthOf_Key_RIJNDAEL_2 = 12;
        //private const int c_MaxLengthOf_Key_TRIPLEDES_1 = 8;
        //private const int c_MaxLengthOf_Key_TRIPLEDES_2 = 12;

        private byte[] m_Key = null;

        /// <summary>
        /// 获取或设置加密解密过程中使用的明文密码
        /// </summary>
        public byte[] Key
        {
            get { return m_Key; }
            set { m_Key = value; }
        }


        private byte[] m_initVec = null;
        /// <summary>
        /// 获取或设置加密解密过程中使用的初始化向量
        /// </summary>
        public byte[] InitVec
        {
            get { return m_initVec; }
            set { m_initVec = value; }
        }

        public EncryptTransform(byte[] key, byte[] initVector)
        {
            this.m_Key = key;
            this.m_initVec = initVector;
        }
        /// <summary>
        /// 根据提供的枚举信息,获得需要使用的解密算法的接口
        /// </summary>
        /// <param name="algorithm"></param>
        /// <returns></returns>
        internal ICryptoTransform GetDecryptoServiceProvider(EncryptionAlgorithm algorithm)
        {
            switch (algorithm)
            {
                case EncryptionAlgorithm.Des:
                    {
                        DES des = new DESCryptoServiceProvider();
                        des.Mode = CipherMode.CBC;
                        if (this.m_Key != null)
                        {
                            des.Key = m_Key;
                        }
                        if (m_initVec != null)
                        {
                            des.IV = m_initVec;
                        }
                        return des.CreateDecryptor();
                    }
                case EncryptionAlgorithm.Rc2:
                    {
                        RC2 rc = new RC2CryptoServiceProvider();
                        rc.Mode = CipherMode.CBC;
                        return rc.CreateDecryptor(m_Key, m_initVec);
                    }
                case EncryptionAlgorithm.Rijndael:
                    {
                        Rijndael rijndael = new RijndaelManaged();
                        rijndael.Mode = CipherMode.CBC;
                        return rijndael.CreateDecryptor(m_Key, m_initVec);
                    }
                case EncryptionAlgorithm.TripleDes:
                    {
                        TripleDES edes = new TripleDESCryptoServiceProvider();
                        edes.Mode = CipherMode.CBC;
                        return edes.CreateDecryptor(m_Key, m_initVec);
                    }
            }
            throw new CryptographicException("Algorithm ID '" + algorithm + "' not supported.");
        }

        /// <summary>
        /// 根据提供的枚举信息,获得需要使用的加密算法的接口
        /// </summary>
        /// <param name="algorithm">算法枚举</param>
        /// <returns></returns>
        internal ICryptoTransform GetEncryptoServiceProvider(EncryptionAlgorithm algorithm)
        {

            switch (algorithm)
            {
                case EncryptionAlgorithm.Des:
                    {
                        DES des = new DESCryptoServiceProvider();
                        des.Mode = CipherMode.CBC;
                        if (null != m_Key)
                        {
                            des.Key = m_Key;
                        }
                        else
                        {
                            m_Key = des.Key;
                        }
                        if (null != m_initVec)
                        {
                            des.IV = m_initVec;
                        }
                        else
                        {
                            m_initVec = des.IV;
                        }
                        return des.CreateEncryptor();
                    }


                case EncryptionAlgorithm.Rc2:
                    {
                        RC2 rc = new RC2CryptoServiceProvider();
                        rc.Mode = CipherMode.CBC;
                        if (null != m_Key)
                        {
                            rc.Key = m_Key;
                        }
                        else
                        {
                            m_Key = rc.Key;
                        }
                        if (null != m_initVec)
                        {
                            rc.IV = m_initVec;
                        }
                        else
                        {
                            m_initVec = rc.IV;
                        }
                        return rc.CreateEncryptor();
                    }
                case EncryptionAlgorithm.Rijndael:
                    {
                        Rijndael rijndael = new RijndaelManaged();
                        rijndael.Mode = CipherMode.CBC;
                        if (null != m_Key)
                        {
                            rijndael.Key = m_Key;
                        }
                        else
                        {
                            m_Key = rijndael.Key;
                        }
                        if (null != m_initVec)
                        {
                            rijndael.IV = m_initVec;
                        }
                        else
                        {
                            m_initVec = rijndael.IV;
                        }
                        return rijndael.CreateEncryptor();
                    }
                case EncryptionAlgorithm.TripleDes:
                    {
                        TripleDES edes = new TripleDESCryptoServiceProvider();
                        edes.Mode = CipherMode.CBC;
                        if (null != m_Key)
                        {
                            edes.Key = m_Key;
                        }
                        else
                        {
                            m_Key = edes.Key;
                        }
                        if (null != m_initVec)
                        {
                            edes.IV = m_initVec;
                        }
                        else
                        {
                            m_initVec = edes.IV;
                        }
                        return edes.CreateEncryptor();
                    }
                default:
                    throw new CryptographicException("Algorithm ID '" + algorithm + "' not supported.");
            }

        }

        /// <summary>
        /// 提供具体实现解密的方法
        /// </summary>
        /// <param name="algorithm"></param>
        /// <param name="bytesData">需要解密的信息</param>
        /// <returns></returns>
        public byte[] Decrypt(EncryptionAlgorithm algorithm, byte[] bytesData)
        {
            byte[] result = new byte[0];
            using (MemoryStream stream = new MemoryStream())
            {
                ICryptoTransform cryptoServiceProvider = GetDecryptoServiceProvider(algorithm);
                using (CryptoStream stream2 = new CryptoStream(stream, cryptoServiceProvider, CryptoStreamMode.Write))
                {
                    try
                    {
                        stream2.Write(bytesData, 0, bytesData.Length);
                        stream2.FlushFinalBlock();
                        stream2.Close();
                        result = stream.ToArray();
                    }
                    catch (Exception exception)
                    {
                        throw new Exception("Error while writing decrypted data to the stream: \n" + exception.Message);
                    }
                }
                stream.Close();
            }
            return result;
        }
        /// <summary>
        /// 提供具体实现加密的方法
        /// </summary>
        /// <param name="algorithm"></param>
        /// <param name="bytesData">需要加密的信息</param>
        /// <returns></returns>
        public byte[] Encrypt(EncryptionAlgorithm algorithm, byte[] bytesData)
        {
            byte[] result = new byte[0];
            using (MemoryStream stream = new MemoryStream())
            {
                ICryptoTransform cryptoServiceProvider = this.GetEncryptoServiceProvider(algorithm);
                using (CryptoStream stream2 = new CryptoStream(stream, cryptoServiceProvider, CryptoStreamMode.Write))
                {
                    try
                    {
                        stream2.Write(bytesData, 0, bytesData.Length);
                        stream2.FlushFinalBlock();
                        stream2.Close();
                        result = stream.ToArray();
                    }
                    catch (Exception exception)
                    {
                        throw new Exception("Error while writing encrypted data to the stream: \n" + exception.Message);
                    }
                }
                stream.Close();
            }
            return result;
        }


    }
}
