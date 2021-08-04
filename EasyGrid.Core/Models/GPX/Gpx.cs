using System;
using System.Xml.Serialization;
using EasyGrid.Core.Models.GPX;

namespace EasyGrid.Core.Models.Gpx
{
    [Serializable]
    [XmlRoot("gpx", Namespace = "http://www.topografix.com/GPX/1/0")]
    public class Gpx
    {
        [XmlAttribute("creator")]
        public string Creator { get; set; }

        [XmlElement("metadata")]
        public Metadata MetaData { get; set; }

        [XmlElement("trk")]
        public Track TrackCollection { get; set; }

        [XmlElement("wpt")]
        public Point[] Points { get; set; }
    }
}
