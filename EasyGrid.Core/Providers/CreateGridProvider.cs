using EasyGrid.Core.Models;
using System;
using System.Collections.Generic;

namespace EasyGrid.Core.Providers
{
    public class CreateGridProvider
    {
        public CreateGridProvider() { }

        public GeoPoint[,] CreateGrid(
            double topLat,
            double leftLon,
            double bottomLat,
            double rightLon,
            int squareSize)
        {
            var startPoint = new GeoPoint()
            {
                Lat = topLat,
                Lon = leftLon
            };

            var leftBorder = CreateVerticalBorder(startPoint, bottomLat, squareSize);
            var topBorder = CreateHorizontalBorder(startPoint, rightLon, squareSize);
            var grid = new GeoPoint[topBorder.Count, leftBorder.Count];

            for(var i = 0; i < topBorder.Count; i++)
            {
                grid[i, 0] = topBorder[i];
            }

            for (var j = 1; j < leftBorder.Count; j++)
            {
                grid[0, j] = leftBorder[j];

                for (var i = 1; i < topBorder.Count; i++)
                {
                    grid[i, j] = grid[i - 1, j] + CalcMove(grid[i - 1, j], squareSize, CoreConstants.Angle.Right);
                }
            }

            return grid;
        }

        private List<GeoPoint> CreateVerticalBorder(GeoPoint startPoint, double bottomLat, int squareSize)
        {
            GeoPoint move;
            var point = (GeoPoint)startPoint.Clone();
            var pointsList = new List<GeoPoint>();

            do
            {
                pointsList.Add(point);
                move = CalcMove(point, squareSize, CoreConstants.Angle.Down);
                point += move;
            } while (point.Lat - (move.Lat / 2) > bottomLat);

            return pointsList;
        }

        private List<GeoPoint> CreateHorizontalBorder(GeoPoint startPoint, double rightLon, int squareSize)
        {
            GeoPoint move;
            var point = (GeoPoint)startPoint.Clone();
            var pointsList = new List<GeoPoint>();

            do
            {
                pointsList.Add(point);
                move = CalcMove(point, squareSize, CoreConstants.Angle.Right);
                point += move;
            } while (point.Lon - (move.Lon / 2) < rightLon);

            return pointsList;
        }

        private GeoPoint CalcMove(GeoPoint point, int distance, double angle)
        {
            var stepLat = 360.0 * Math.Sin(angle) * distance / CoreConstants.Planet.Lat;
            var stepLon = 360.0 * Math.Cos(angle) * distance / (CoreConstants.Planet.Lon * Math.Cos(point.Lat * CoreConstants.Rad));

            return new GeoPoint()
            {
                Lat = stepLat,
                Lon = stepLon,
            };
        }
    }
}
