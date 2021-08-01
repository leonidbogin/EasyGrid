using EasyGrid.Core.Models;
using EasyGrid.Core.Models.Gpx;
using EasyGrid.Core.Models.GPX;
using System;
using System.Linq;

namespace EasyGrid.Core.Providers
{
    public class ConvertGridToGpxProvider
    {
        public ConvertGridToGpxProvider() { }

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

            var track = new Track()
            {
                Name = "grid",
                Points = GenerateTrack(grid)
            };

            var points = gridArray.Select(s => new Point() { Name = s.Name, Lat = s.Lat, Lon = s.Lon }).ToArray();

            var gpx = new Gpx()
            {
                MetaData = metadata,
                TrackCollection = track,
                Points = points
            };

            return gpx;
        }

        private static TrackPoint[] GenerateTrack(GeoPoint[,] grid)
        {
            var lineGridProvider = new LineGridProvider(grid.GetLength(0), grid.GetLength(1));
            var linePath = lineGridProvider.GenerateLinePath();
            var trackPoints = linePath.Select(s => new TrackPoint() { Lat = grid[s.i, s.j].Lat, Lon = grid[s.i, s.j].Lon }).ToArray();

            return trackPoints;
        }
    }
}
