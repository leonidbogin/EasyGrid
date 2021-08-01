using System;

namespace EasyGrid.Core.Models
{
    public class GeoPoint : ICloneable
    {
        public string Name { get; set; }
        public double Lat { get; set; }
        public double Lon { get; set; }

        public GeoPoint()
        {

        }

        public static GeoPoint operator +(GeoPoint point1, GeoPoint point2)
        {
            return new GeoPoint()
            {
                Name = point1.Name,
                Lat = point1.Lat + point2.Lat,
                Lon = point1.Lon + point2.Lon
            };
        }

        public object Clone()
        {
            return new GeoPoint { Name = this.Name, Lat = this.Lat, Lon = this.Lon };
        }
    }
}
