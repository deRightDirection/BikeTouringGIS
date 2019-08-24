using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BikeTouringGISLibrary.GPX
{
    [GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [SerializableAttribute()]
    [DebuggerStepThroughAttribute()]
    [DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(Namespace = "http://www.topografix.com/GPX/1/1")]
    public partial class wptType
    {

        private decimal eleField;

        private bool eleFieldSpecified;

        private decimal magvarField;

        private bool magvarFieldSpecified;

        private decimal geoidheightField;

        private bool geoidheightFieldSpecified;

        private string nameField;

        private string cmtField;

        private string descField;

        private string srcField;

        private linkType[] linkField;

        private string symField;

        private string typeField;

        private fixType fixField;

        private bool fixFieldSpecified;

        private string satField;

        private decimal hdopField;

        private bool hdopFieldSpecified;

        private decimal vdopField;

        private bool vdopFieldSpecified;

        private decimal pdopField;

        private bool pdopFieldSpecified;

        private decimal ageofdgpsdataField;

        private bool ageofdgpsdataFieldSpecified;

        private string dgpsidField;

        private extensionsType extensionsField;

        private decimal latField;

        private decimal lonField;

        /// <remarks/>
        public decimal ele
        {
            get
            {
                return this.eleField;
            }
            set
            {
                this.eleField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool eleSpecified
        {
            get
            {
                return this.eleFieldSpecified;
            }
            set
            {
                this.eleFieldSpecified = value;
            }
        }

        [XmlIgnore]
        public DateTime time { get; set; }


        [XmlElement("time")]
        public string SomeDateString
        {
            get { return time.ToUniversalTime().ToString(@"yyyy-MM-dd\THH:mm:ss\Z"); }
            set { time = DateTime.Parse(value); }
        }

        [XmlIgnoreAttribute()]
        public bool timeSpecified { get; set; }

        /// <remarks/>
        public decimal magvar
        {
            get
            {
                return this.magvarField;
            }
            set
            {
                this.magvarField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool magvarSpecified
        {
            get
            {
                return this.magvarFieldSpecified;
            }
            set
            {
                this.magvarFieldSpecified = value;
            }
        }

        /// <remarks/>
        public decimal geoidheight
        {
            get
            {
                return this.geoidheightField;
            }
            set
            {
                this.geoidheightField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool geoidheightSpecified
        {
            get
            {
                return this.geoidheightFieldSpecified;
            }
            set
            {
                this.geoidheightFieldSpecified = value;
            }
        }

        /// <remarks/>
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        public string cmt
        {
            get
            {
                return this.cmtField;
            }
            set
            {
                this.cmtField = value;
            }
        }

        /// <remarks/>
        public string desc
        {
            get
            {
                return this.descField;
            }
            set
            {
                this.descField = value;
            }
        }

        /// <remarks/>
        public string src
        {
            get
            {
                return this.srcField;
            }
            set
            {
                this.srcField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("link")]
        public linkType[] link
        {
            get
            {
                return this.linkField;
            }
            set
            {
                this.linkField = value;
            }
        }

        /// <remarks/>
        public string sym
        {
            get
            {
                return this.symField;
            }
            set
            {
                this.symField = value;
            }
        }

        /// <remarks/>
        public string type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }

        /// <remarks/>
        public fixType fix
        {
            get
            {
                return this.fixField;
            }
            set
            {
                this.fixField = value;
            }
        }

        /// <remarks/>
        [XmlIgnoreAttribute()]
        public bool fixSpecified
        {
            get
            {
                return this.fixFieldSpecified;
            }
            set
            {
                this.fixFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "nonNegativeInteger")]
        public string sat
        {
            get
            {
                return this.satField;
            }
            set
            {
                this.satField = value;
            }
        }

        /// <remarks/>
        public decimal hdop
        {
            get
            {
                return this.hdopField;
            }
            set
            {
                this.hdopField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool hdopSpecified
        {
            get
            {
                return this.hdopFieldSpecified;
            }
            set
            {
                this.hdopFieldSpecified = value;
            }
        }

        /// <remarks/>
        public decimal vdop
        {
            get
            {
                return this.vdopField;
            }
            set
            {
                this.vdopField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool vdopSpecified
        {
            get
            {
                return this.vdopFieldSpecified;
            }
            set
            {
                this.vdopFieldSpecified = value;
            }
        }

        /// <remarks/>
        public decimal pdop
        {
            get
            {
                return this.pdopField;
            }
            set
            {
                this.pdopField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool pdopSpecified
        {
            get
            {
                return this.pdopFieldSpecified;
            }
            set
            {
                this.pdopFieldSpecified = value;
            }
        }

        /// <remarks/>
        public decimal ageofdgpsdata
        {
            get
            {
                return this.ageofdgpsdataField;
            }
            set
            {
                this.ageofdgpsdataField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ageofdgpsdataSpecified
        {
            get
            {
                return this.ageofdgpsdataFieldSpecified;
            }
            set
            {
                this.ageofdgpsdataFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer")]
        public string dgpsid
        {
            get
            {
                return this.dgpsidField;
            }
            set
            {
                this.dgpsidField = value;
            }
        }

        /// <remarks/>
        public extensionsType extensions
        {
            get
            {
                return this.extensionsField;
            }
            set
            {
                this.extensionsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal lat
        {
            get
            {
                return this.latField;
            }
            set
            {
                this.latField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal lon
        {
            get
            {
                return this.lonField;
            }
            set
            {
                this.lonField = value;
            }
        }
    }
}
