using System.Collections.Generic;
using System.Xml.Serialization;

namespace TlhPlatform.Core.ConfigurationClass
{
    [XmlRoot("AccessChannelList")]
    public class AccessChannelConfig
    {
        [XmlElement("AccessChannel", typeof(AccessChannel))]
        public List<AccessChannel> AccessChannelList { get; set; }
    }

    public class AccessChannel
    {
        [XmlAttribute("AppID")]
        public string AppID { get; set; }

        [XmlAttribute("AppType")]
        public string AppType { get; set; }

        [XmlAttribute("AccessChannelID")]
        public string AccessChannelID { get; set; }

        [XmlAttribute("SecretKey")]
        public string SecretKey { get; set; }

    }
}
