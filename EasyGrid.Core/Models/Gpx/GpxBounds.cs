﻿using System;
using System.Xml.Serialization;

namespace EasyGrid.Core.Models.Gpx
{
    [Serializable]
    public class GpxBounds
    {
        [XmlAttribute("minlat")]
        public double MinLat { get; set; }

        [XmlAttribute("minlon")]
        public double MinLon { get; set; }

        [XmlAttribute("maxlat")]
        public double MaxLat { get; set; }

        [XmlAttribute("maxlon")]
        public double MaxLon { get; set; }
    }
}
