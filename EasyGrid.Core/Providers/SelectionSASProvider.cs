using System.Globalization;
using System.IO;
using EasyGrid.Core.Models;

namespace EasyGrid.Core.Providers
{
    public class SelectionSASProvider
    {
        private readonly IniFileParser.IniFileParser parser;
        private readonly string fileName;

        public SelectionSASProvider(string fileName)
        {
            parser = new IniFileParser.IniFileParser();
            this.fileName = fileName;
        }

        public LastSelection GetLastSelection(string directoryPath)
        {
            var path = Path.Combine(directoryPath, fileName);

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

        public bool FileExists(string directoryPath)
        {
            if (directoryPath is null)
            {
                return false;
            }

            var path = Path.Combine(directoryPath, fileName);
            return File.Exists(path);
        }

        private static double Parse(string value)
        {
            return double.Parse(value, CultureInfo.InvariantCulture);
        }
    }
}