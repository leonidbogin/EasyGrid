using System;
using System.Xml.Serialization;

namespace EasyGrid.Core.Models.Gpx
{
    [Serializable]
    public class GpxTrack
    {
        [XmlElement("name")]
        public string Name { get; set; }

        [XmlArray("trkseg")]
        [XmlArrayItem("trkpt")]
        public GpxTrackPoint[] Points { get; set; }
    }
}
