using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text;
 
namespace TlhPlatform.Core.ConfigurationClass
{
    public interface IConfigurationSource
    {
        T GetSection<T>(string sectionName) where T :  ConfigurationSection;
    }

    public class LocalConfigurationSource : IConfigurationSource
    {
        public T GetSection<T>(string sectionName) where T : System.Configuration.ConfigurationSection
        {
            return (T)ConfigurationManager.GetSection(sectionName);
        }
    }

    public class GlobalConfigurationSource : IConfigurationSource
    {
        private static ExeConfigurationFileMap fileMap;
        private string configurationFilepath;

        private GlobalConfigurationSource(string configurationFilepath)
        {
            if (String.IsNullOrEmpty(configurationFilepath))
            {
                throw new ArgumentException("configurationFilepath");
            }

            this.configurationFilepath = RootConfigurationFilePath(configurationFilepath);

            if (!File.Exists(this.configurationFilepath))
            {
                throw new FileNotFoundException(String.Format("The configuration file {0} could not be found.", this.configurationFilepath));
            }
        }
        /// <summary>
        /// 初始化加载app.config 配置的 sectionGroup
        /// </summary>
        /// <returns></returns>
        public static GlobalConfigurationSource Create()
        {
            string str = string.Empty;   
            object ro = new object(); 
            str= "VOCs.Framework/globalSettings";  
            GlobalSettingsSection section = (GlobalSettingsSection)ConfigurationManager.GetSection(str);

            if (section == null)
            {
                throw new ConfigurationErrorsException("The global configuration is null, please check your config file, make sure the VOCs.Framework/globalSettings is exists.");
            }

            return new GlobalConfigurationSource(section.FilePath);
        }

        public T GetSection<T>(string sectionName) where T : System.Configuration.ConfigurationSection
        {
            if (fileMap == null)
            {
                fileMap = new ExeConfigurationFileMap();
                fileMap.ExeConfigFilename = RootConfigurationFilePath(configurationFilepath);
            }

            object section = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None).GetSection(sectionName);

            return (T)section;
        }

        private static string RootConfigurationFilePath(string configurationFile)
        {
            string rootedConfigurationFile = (string)configurationFile.Clone();

            if (!Path.IsPathRooted(rootedConfigurationFile))
            {
                rootedConfigurationFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, rootedConfigurationFile);
            }

            return rootedConfigurationFile;
        }
    }
}