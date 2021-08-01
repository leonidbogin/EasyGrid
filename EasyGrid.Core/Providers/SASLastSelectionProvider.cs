using System.Globalization;
using EasyGrid.Core.Models;

namespace EasyGrid.Core.Providers
{
    public class SASLastSelectionProvider
    {
        private readonly IniFileParser.IniFileParser parser;

        public SASLastSelectionProvider()
        {
            parser = new IniFileParser.IniFileParser();
        }

        public GeoPoint[] GetLastSelection()
        {
            var data = parser.ReadFile("LastSelection.hlg");
            var points = new GeoPoint[2];

            points[0] = new GeoPoint()
            {
                Lon = Parse(data["HIGHLIGHTING"]["PointLon_1"]),
                Lat = Parse(data["HIGHLIGHTING"]["PointLat_1"])
            };

            points[1] = new GeoPoint()
            {
                Lon = Parse(data["HIGHLIGHTING"]["PointLon_3"]),
                Lat = Parse(data["HIGHLIGHTING"]["PointLat_3"])
            };

            return points;
        }

        private static double Parse(string value)
        {
            return double.Parse(value, CultureInfo.InvariantCulture);
        }
    }
}