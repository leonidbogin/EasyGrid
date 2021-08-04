using System.Globalization;
using System.IO;
using EasyGrid.Core.Models;

namespace EasyGrid.Core.Providers
{
    public class SelectionSASProvider
    {
        private readonly IniFileParser.IniFileParser parser;

        public SelectionSASProvider()
        {
            parser = new IniFileParser.IniFileParser();
        }

        public LastSelection GetLastSelection(string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"{path} file not found");
            }

            var data = parser.ReadFile(path);
            var lastSelection = new LastSelection()
            {
                FirstPoint = new GeoPoint()
                {
                    Lon = Parse(data["HIGHLIGHTING"]["PointLon_1"]),
                    Lat = Parse(data["HIGHLIGHTING"]["PointLat_1"])
                },
                SecondPoint = new GeoPoint()
                {
                    Lon = Parse(data["HIGHLIGHTING"]["PointLon_3"]),
                    Lat = Parse(data["HIGHLIGHTING"]["PointLat_3"])
                }
            };

            return lastSelection;
        }

        private static double Parse(string value)
        {
            return double.Parse(value, CultureInfo.InvariantCulture);
        }
    }
}