using EasyGrid.Core.Models;
using IniParser;
using System;
using System.IO;

namespace EasyGrid.Core.Providers
{
    public class SasPlanetSelectionProvider
    {
        private readonly FileIniDataParser _parser;
        private readonly string _path;

        public SasPlanetSelectionProvider(string path)
        {
            _parser = new FileIniDataParser();
            _path = path;
        }

        public SasPlanetSelection GetSelection()
        {
            if (!File.Exists(_path))
            {
                throw new FileNotFoundException($"{_path} file not found");
            }

            var data = _parser.ReadFile(_path);

            return new SasPlanetSelection()
            {
                Zoom = Parse<int>(data["HIGHLIGHTING"]["PointLon_1"]),
                PointLon1 = Parse<double>(data["HIGHLIGHTING"]["PointLon_1"]),
                PointLat1 = Parse<double>(data["HIGHLIGHTING"]["PointLat_1"]),
                PointLon2 = Parse<double>(data["HIGHLIGHTING"]["PointLon_2"]),
                PointLat2 = Parse<double>(data["HIGHLIGHTING"]["PointLat_2"]),
                PointLon3 = Parse<double>(data["HIGHLIGHTING"]["PointLon_3"]),
                PointLat3 = Parse<double>(data["HIGHLIGHTING"]["PointLat_3"]),
                PointLon4 = Parse<double>(data["HIGHLIGHTING"]["PointLon_4"]),
                PointLat4 = Parse<double>(data["HIGHLIGHTING"]["PointLat_4"]),
                PointLon5 = Parse<double>(data["HIGHLIGHTING"]["PointLon_5"]),
                PointLat5 = Parse<double>(data["HIGHLIGHTING"]["PointLat_5"]),
            };
        }

        public bool SelectionFileExists()
        {
            return File.Exists(_path);
        }

        private static T Parse<T>(string value)
        {
            return (T)Convert.ChangeType(value, typeof(T));
        }
    }
}
