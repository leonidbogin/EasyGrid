using System;
using System.Xml.Serialization;

namespace EasyGrid.Core.Models.Gpx
{
    [Serializable]
    public class GpxTrackPoint
    {
        [XmlAttribute("lat")]
        public double Lat { get; set; }

        [XmlAttribute("lon")]
        public double Lon { get; set; }
    }
}
