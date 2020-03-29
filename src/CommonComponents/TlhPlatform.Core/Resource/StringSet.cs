using System;
using System.Collections.Generic;
using System.Text;

namespace TlhPlatform.Core.Resource
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.VOCs.com/framework/web")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.VOCs.com/framework/web", IsNullable = false, ElementName = "StringSet")]
    public partial class StringSet
    {

        private StringItem[] stringItemField;

        [System.Xml.Serialization.XmlElementAttribute("StringItem")]
        public StringItem[] StringItem
        {
            get
            {
                return this.stringItemField;
            }
            set
            {
                this.stringItemField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.VOCs.com/framework/web")]
    public partial class StringItem
    {

        private string keyField;

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(AttributeName = "key")]
        public string key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(AttributeName = "value")]
        public string value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }
}
