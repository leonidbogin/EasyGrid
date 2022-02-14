using System;
using System.Xml.Serialization;

namespace EasyGrid.Core.Models.Gpx
{
    [Serializable]
    public class GpxMetadata
    {
        [XmlElement("time")]
        public string Time { get; set; }

        [XmlElement("bounds")]
        public GpxBounds Bounds { get; set; }
    }
}
