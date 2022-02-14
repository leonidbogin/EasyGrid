using System;

namespace EasyGrid.Core.Models
{
    public class GeoPoint : ICloneable
    {
        public string Title { get; set; }
        public double Lat { get; }
        public double Lon { get; }

        public GeoPoint(double lat, double lon)
        {
            Lat = lat;
            Lon = lon;
        }

        public GeoPoint(double lat, double lon, string title) : this(lat, lon)
        {
            Title = title;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as GeoPoint);
        }

        public bool Equals(GeoPoint other)
        {
            return other != null && Lat.Equals(other.Lat) && Lon.Equals(other.Lon);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Lat, Lon);
        }

        public object Clone()
        {
            return new GeoPoint(Lat, Lon, Title);
        }

        public static GeoPoint operator +(GeoPoint point1, GeoPoint point2)
        {
            return new GeoPoint(point1.Lat + point2.Lat, point1.Lon + point2.Lon, point1.Title);
        }

        public static GeoPoint operator -(GeoPoint point1, GeoPoint point2)
        {
            return new GeoPoint(point1.Lat - point2.Lat, point1.Lon - point2.Lon, point1.Title);
        }

        public override string ToString()
        {
            return $"{Title}({Lat};{Lon})";
        }
    }
}
