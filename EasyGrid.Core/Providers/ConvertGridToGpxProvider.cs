using EasyGrid.Core.Models;
using EasyGrid.Core.Models.Gpx;
using EasyGrid.Core.Models.GPX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using EasyGrid.Core.Services;

namespace EasyGrid.Core.Providers
{
    public class ConvertGridToGpxProvider : ProgressService
    {
        private readonly string creatorName;

        public ConvertGridToGpxProvider(string creatorName)
        {
            this.creatorName = creatorName;
        }

        public Gpx ConvertToGpx(GeoPoint[,] grid, CancellationToken cancellationToken)
        {
            LogProgress(0);
            LogStatus("Converting grid array to GPX");

            if (cancellationToken.IsCancellationRequested)
            {
                return null;
            }

            var gridArray = grid.Cast<GeoPoint>().ToArray();
            LogProgress(20);

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
            LogProgress(40);

            var track = new Track()
            {
                Name = "grid",
                Points = GenerateTrack(grid)
            };
            LogProgress(70);

            var points = gridArray.Select(s => new Point() { Name = s.Name, Lat = s.Lat, Lon = s.Lon }).ToArray();
            LogProgress(95);

            var gpx = new Gpx()
            {
                Creator = creatorName,
                MetaData = metadata,
                TrackCollection = track,
                Points = points
            };
            LogProgress(100);

            return gpx;
        }

        private static TrackPoint[] GenerateTrack(GeoPoint[,] grid)
        {
            var linePath = GenerateLinePath(grid.GetLength(0), grid.GetLength(1));
            return linePath.Select(s => new TrackPoint() { Lat = grid[s.i, s.j].Lat, Lon = grid[s.i, s.j].Lon }).ToArray();
        }

        private static IEnumerable<(int i, int j)> GenerateLinePath(int width, int height)
        {
            var points = new List<(int, int)>();

            var i = 0;
            var j = 0;
            var horizontalDirection = Direction.Right;
            var verticalDirection = Direction.Down;
            var iteration = 0;

            while (true)
            {
                points.Add((i, j));

                if (iteration == 0)
                {
                    if (!MovePoint(width, height, horizontalDirection, ref i, ref j))
                    {
                        horizontalDirection = RevertDirection(horizontalDirection);

                        if (!MovePoint(width, height, verticalDirection, ref i, ref j))
                        {
                            iteration++;
                            verticalDirection = RevertDirection(verticalDirection);
                        }
                    }
                }

                if (iteration == 1)
                {
                    if (!MovePoint(width, height, verticalDirection, ref i, ref j))
                    {
                        verticalDirection = RevertDirection(verticalDirection);

                        if (!MovePoint(width, height, horizontalDirection, ref i, ref j))
                        {
                            iteration++;
                            horizontalDirection = RevertDirection(horizontalDirection);
                        }
                    }
                }

                if (iteration == 2)
                {
                    break;
                }
            }

            return points.ToArray();
        }

        private static bool MovePoint(int width, int height, Direction direction, ref int i, ref int j)
        {
            switch (direction)
            {
                case Direction.Up when j > 0:
                    j--; return true;
                case Direction.Right when i < width - 1:
                    i++; return true;
                case Direction.Down when j < height - 1:
                    j++; return true;
                case Direction.Left when i > 0:
                    i--; return true;
                default:
                    return false;
            }
        }

        private static Direction RevertDirection(Direction direction)
        {
            return direction switch
            {
                Direction.Up => Direction.Down,
                Direction.Right => Direction.Left,
                Direction.Down => Direction.Up,
                Direction.Left => Direction.Right,
                _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
            };
        }

        private enum Direction
        {
            Up,
            Right,
            Down,
            Left
        }
    }
}
