using System;
using System.Xml.Serialization;

namespace EasyGrid.Core.Models.Gpx
{
    [Serializable]
    public class GpxPoint
    {
        [XmlAttribute("lat")]
        public double Lat { get; set; }

        [XmlAttribute("lon")]
        public double Lon { get; set; }

        [XmlElement("name")]
        public string Name { get; set; }
    }
}
