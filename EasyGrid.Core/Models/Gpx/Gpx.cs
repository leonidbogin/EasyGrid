using System;
using System.Xml.Serialization;

namespace EasyGrid.Core.Models.Gpx
{
    [Serializable]
    [XmlRoot("gpx", Namespace = "http://www.topografix.com/GPX/1/0")]
    public class Gpx
    {
        [XmlAttribute("creator")]
        public string Creator { get; set; }

        [XmlElement("metadata")]
        public GpxMetadata MetaData { get; set; }

        [XmlElement("trk")]
        public GpxTrack TrackCollection { get; set; }

        [XmlElement("wpt")]
        public GpxPoint[] Points { get; set; }
    }
}
