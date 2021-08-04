using System;
using System.Xml.Serialization;

namespace EasyGrid.Core.Models.GPX
{
    [Serializable]
    public class Point
    {
        [XmlAttribute("lat")]
        public double Lat { get; set; }

        [XmlAttribute("lon")]
        public double Lon { get; set; }

        [XmlElement("name")]
        public string Name { get; set; }
    }
}
