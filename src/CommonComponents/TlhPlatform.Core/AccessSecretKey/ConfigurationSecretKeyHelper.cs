using System;
using System.Collections.Generic;
using System.IO; 
using System.Text;
using System.Xml.Serialization;
using TlhPlatform.Core.ConfigurationClass;
using TlhPlatform.Core.AccessSecretKey;
namespace TlhPlatform.Core.AccessSecretKey
{
    public class ConfigurationSecretKeyHelper
    {
        public static SecretKeyConfig SecretKeyConfigSetting = null;
        public  static readonly object lockObject=new object();
        public static Dictionary<string,SecretKeyConfig> listSecretKeyConfig=new Dictionary<string,SecretKeyConfig>();
        static ConfigurationSecretKeyHelper()
        {
            LoadConfigurations();
        }

        public static T LoadConfigurations<T>(string fileName)
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Configuration", fileName);
            if (!File.Exists(path))
            {
                return default(T);
            }
            try
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    XmlSerializer xs = new XmlSerializer(typeof(T));
                    T config = (T)xs.Deserialize(sr);
                    return config;
                }
            }
            catch (Exception ex)
            {
                return default(T);
            }
        }

        public static void LoadConfigurations()
        {
            lock(lockObject)
            {
                if(listSecretKeyConfig==null||listSecretKeyConfig.Count==0)
                {
                    SecretKeyConfigSetting = LoadConfigurations<SecretKeyConfig>("AccessSecretKey.xml");
                    listSecretKeyConfig.Add("key",SecretKeyConfigSetting);
                    return;
                }
                SecretKeyConfigSetting=listSecretKeyConfig["key"];
            } 
        }
    }
}