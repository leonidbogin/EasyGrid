using System;
using System.Xml.Serialization;

namespace EasyGrid.Core.Models.GPX
{
    [Serializable]
    public class Metadata
    {
        [XmlElement("time")]
        public string Time { get; set; }

        [XmlElement("bounds")]
        public Bounds Bounds { get; set; }
    }
}
