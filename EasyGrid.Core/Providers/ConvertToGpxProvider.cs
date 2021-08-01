using EasyGrid.Core.Models;
using EasyGrid.Core.Models.Gpx;
using EasyGrid.Core.Models.GPX;
using System;
using System.Globalization;
using System.Linq;
using System.Threading;

namespace EasyGrid.Core.Providers
{
    public class ConvertToGpxProvider
    {

        public ConvertToGpxProvider()
        {

        }

        public Gpx ConvertToGpx(GeoPoint[,] grid)
        {
            var gridArray = grid.Cast<GeoPoint>().ToArray();

            var metadata = new Metadata()
            {
                Time = DateTime.Now.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'"),
                Bounds = new Bounds()
                {
                    MinLat = gridArray.Min(m => m.Lat),
                    MinLon = gridArray.Min(m => m.Lon),
                    MaxLat = gridArray.Max(m => m.Lat),
                    MaxLon = gridArray.Max(m => m.Lon)
                }
            };

            var track = new TrackCollection()
            {
                Name = "grid",
                Points = GenerateTrack(grid)
            };

            var gpx = new Gpx()
            {
                MetaData = metadata,
                TrackCollection = track
            };

            return gpx;
        }

        private static TrackCollectionSegment[] GenerateTrack(GeoPoint[,] grid)
        {
            var width = grid.GetLength(0);
            var height = grid.GetLength(1);

            var lineGridProvider = new LineGridProvider(width, height);
            var linePath = lineGridProvider.GenerateLinePath();
            var points = new TrackCollectionSegment[linePath.Length];

            for (var i = 0; i < linePath.Length; i++)
            {
                points[i] = new TrackCollectionSegment()
                {
                    Lat = grid[linePath[i].lat, linePath[i].lon].Lat,
                    Lon = grid[linePath[i].lat, linePath[i].lon].Lon
                };
            }

            return points;
        }

        
    }
}
