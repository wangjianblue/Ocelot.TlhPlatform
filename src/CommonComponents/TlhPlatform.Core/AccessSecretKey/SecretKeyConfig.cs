using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace TlhPlatform.Core.AccessSecretKey
{
    [XmlRoot("AccessSecretKeyList")]
    public class SecretKeyConfig
    {
        [XmlElement("AccessSecretKey",typeof(AccessSecretKeys))]
        public List<AccessSecretKeys> AccessSecretKeys{get;set;}
    }
    public class AccessSecretKeys
    {
        
        [XmlAttribute("AccessSecretKeyId")]
        public string AccessSecretKeyId { get; set; }

        [XmlAttribute("AppId")]
        public string AppId { get; set; }
   

        [XmlAttribute("SecretKey")]
        public string SecretKey { get; set; }

    }
}