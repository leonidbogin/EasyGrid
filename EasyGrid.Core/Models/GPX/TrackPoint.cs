using System;
using System.Xml.Serialization;

namespace EasyGrid.Core.Models.GPX
{
    [Serializable]
    public class TrackPoint
    {
        [XmlAttribute("lat")]
        public double Lat { get; set; }

        [XmlAttribute("lon")]
        public double Lon { get; set; }
    }
}
