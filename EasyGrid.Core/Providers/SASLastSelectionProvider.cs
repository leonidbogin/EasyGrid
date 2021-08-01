using EasyGrid.Core.Models;

namespace EasyGrid.Core.Providers
{
    public class SASLastSelectionProvider
    {
        public SASLastSelectionProvider()
        {

        }

        public GeoPoint[] GetLastSelection()
        {
            //var parser = new FileIniDataParser();
            //var data = parser.ReadFile("LastSelection.hlg");

            var points = new GeoPoint[2];

            //points[0].Lon = double.Parse(data["HIGHLIGHTING"]["PointLon_1"]);
            //points[0].Lat = double.Parse(data["HIGHLIGHTING"]["PointLat_1"]);
            
            //points[1].Lon = double.Parse(data["HIGHLIGHTING"]["PointLon_3"]);
            //points[1].Lat = double.Parse(data["HIGHLIGHTING"]["PointLat_3"]);

            return points;
        }
    }
}