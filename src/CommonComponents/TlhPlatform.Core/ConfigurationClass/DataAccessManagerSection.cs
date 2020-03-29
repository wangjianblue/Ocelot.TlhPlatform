using System.Configuration;

namespace TlhPlatform.Core.ConfigurationClass
{
    public sealed class DataAccessManagerSection : ConfigurationSection
    {
        private static readonly ConfigurationProperty propDataCommandFile;
        public static ConfigurationPropertyCollection properties;
        static DataAccessManagerSection()
        {
            propDataCommandFile = new ConfigurationProperty("DataCommandFile", typeof(string), null, ConfigurationPropertyOptions.IsRequired);

            properties = new ConfigurationPropertyCollection();
            properties.Add(propDataCommandFile);
        }

        [ConfigurationProperty("DataCommandFile")]
        public string DataCommandFile
        {
            get
            {
                return base[propDataCommandFile] as string;
            }
            set
            {
                base[propDataCommandFile] = value;
            }
        }

        protected override ConfigurationPropertyCollection Properties
        {
            get
            {
                return properties;
            }
        }
    }
}