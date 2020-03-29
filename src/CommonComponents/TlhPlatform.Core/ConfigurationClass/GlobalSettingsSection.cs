using System;
using System.Collections.Generic;

using System.Text;
using System.Configuration;

namespace TlhPlatform.Core.ConfigurationClass
{
    public sealed class GlobalSettingsSection : ConfigurationSection
    {
        #region Fields

        private static readonly ConfigurationProperty propFilePath;

        private static ConfigurationPropertyCollection properties;

        #endregion

        #region Methods

        static GlobalSettingsSection()
        {
            propFilePath = new ConfigurationProperty("filePath", typeof(string));

            properties = new ConfigurationPropertyCollection();

            properties.Add(propFilePath);
        }

        #endregion

        #region Properties

        protected override ConfigurationPropertyCollection Properties
        {
            get
            {
                return properties;
            }
        }

        [ConfigurationProperty("filePath", IsRequired = true)]
        public string FilePath
        {
            get
            {
                return (string)base[propFilePath];
            }
            set
            {
                base[propFilePath] = value;
            }
        }

        #endregion
    }
}