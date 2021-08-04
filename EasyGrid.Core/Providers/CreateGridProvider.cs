using EasyGrid.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Xml.Serialization;
using EasyGrid.Core.Services;

namespace EasyGrid.Core.Providers
{
    public class CreateGridProvider : ProgressService
    {
        public GeoPoint[,] CreateGrid(
            double topLat,
            double leftLon,
            double bottomLat,
            double rightLon,
            int squareSize,
            CancellationToken cancellationToken)
        {
            LogProgress(0);
            LogStatus("Calculating grid size");

            if (cancellationToken.IsCancellationRequested)
            {
                return null;
            }

            var startPoint = new GeoPoint()
            {
                Lat = topLat,
                Lon = leftLon
            };

            var leftBorder = CreateVerticalBorder(startPoint, bottomLat, squareSize, cancellationToken);
            var topBorder = CreateHorizontalBorder(startPoint, rightLon, squareSize, cancellationToken);

            var numberPoints = (leftBorder.Count * topBorder.Count);
            ProgressStep = 100.0 / (numberPoints + leftBorder.Count + topBorder.Count);
            LogAddStep(ProgressStep * (leftBorder.Count + topBorder.Count));
            LogStatus($"Calculating {numberPoints} grid points");

            var grid = new GeoPoint[topBorder.Count, leftBorder.Count];

            for(var i = 0; i < topBorder.Count; i++)
            {
                if (cancellationToken.IsCancellationRequested) return null;
                grid[i, 0] = topBorder[i];
                grid[i, 0].Name = GeneratePointName(i, 0, cancellationToken);
                LogAddStep();
            }

            for (var j = 1; j < leftBorder.Count; j++)
            {
                if (cancellationToken.IsCancellationRequested) return null;
                grid[0, j] = leftBorder[j];
                grid[0, j].Name = GeneratePointName(0, j, cancellationToken);
                LogAddStep();

                for (var i = 1; i < topBorder.Count; i++)
                {
                    if (cancellationToken.IsCancellationRequested) return null;
                    grid[i, j] = grid[i - 1, j] + CalcMove(grid[i - 1, j], squareSize, CoreConstants.Angle.Right);
                    grid[i, j].Name = GeneratePointName(i, j, cancellationToken);
                    LogAddStep();
                }
            }

            LogProgress(100);
            return grid;
        }

        private static List<GeoPoint> CreateVerticalBorder(GeoPoint startPoint, double bottomLat, int squareSize, CancellationToken cancellationToken)
        {
            GeoPoint move;
            var point = (GeoPoint)startPoint.Clone();
            var pointsList = new List<GeoPoint>();

            do
            {
                if (cancellationToken.IsCancellationRequested) return null;
                pointsList.Add(point);
                move = CalcMove(point, squareSize, CoreConstants.Angle.Down);
                point += move;
            } while (point.Lat - (move.Lat / 2) > bottomLat);

            return pointsList;
        }

        private static List<GeoPoint> CreateHorizontalBorder(GeoPoint startPoint, double rightLon, int squareSize, CancellationToken cancellationToken)
        {
            GeoPoint move;
            var point = (GeoPoint)startPoint.Clone();
            var pointsList = new List<GeoPoint>();

            do
            {
                if (cancellationToken.IsCancellationRequested) return null;
                pointsList.Add(point);
                move = CalcMove(point, squareSize, CoreConstants.Angle.Right);
                point += move;
            } while (point.Lon - (move.Lon / 2) < rightLon);

            return pointsList;
        }

        private static GeoPoint CalcMove(GeoPoint point, int distance, double angle)
        {
            var stepLat = 360.0 * Math.Sin(angle) * distance / CoreConstants.Planet.Lat;
            var stepLon = 360.0 * Math.Cos(angle) * distance / (CoreConstants.Planet.Lon * Math.Cos(point.Lat * CoreConstants.Rad));

            return new GeoPoint()
            {
                Lat = stepLat,
                Lon = stepLon,
            };
        }

        private static string GeneratePointName(int i, int j, CancellationToken cancellationToken)
        {
            return $"{GenerateLetterFromIndex(i + 1, cancellationToken)}{j + 1}";
        }

        private static string GenerateLetterFromIndex(int index, CancellationToken cancellationToken)
        {
            var colLetter = string.Empty;

            while (index > 0)
            {
                if (cancellationToken.IsCancellationRequested) return null;
                var mod = (index - 1) % 26;
                colLetter = (char)(65 + mod) + colLetter;
                index = (index - mod) / 26;
            }

            return colLetter;
        }
    }
}
