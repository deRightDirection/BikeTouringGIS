using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
    public partial class trksegType
    {
        [XmlElementAttribute("trkpt")]
        public wptType[] trkpt { get; set; }

        public extensionsType extensions { get; set; }

        [XmlIgnoreAttribute()]
        public DateTime StartTime
        {
            get { return trkpt.First().time; }
            set { trkpt.First().time = value; }
        }

        [XmlIgnoreAttribute()]
        public DateTime EndTime
        {
            get { return trkpt.Last().time; }
            set { trkpt.Last().time = value; }
        }
    }
}