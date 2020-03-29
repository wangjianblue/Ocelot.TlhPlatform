using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace TlhPlatform.Core.ConfigurationClass
{
    public class ConfigurationHelper
    {
        public static AccessChannelConfig AccessChannelSetting = null;
        static ConfigurationHelper()
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
                using (StreamReader sr = new System.IO.StreamReader(path))
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
            AccessChannelSetting = LoadConfigurations<AccessChannelConfig>("AccessChannel.xml");
        }
    }
}
