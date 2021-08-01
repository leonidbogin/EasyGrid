using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace EasyGrid.Core.Models.GPX
{
    [Serializable]
    public class TrackCollection
    {
        [XmlElement("name")]
        public string Name { get; set; }

        [XmlArray("trkseg")]
        [XmlArrayItem("trkpt")]
        public TrackCollectionSegment[] Points { get; set; }
    }
}
