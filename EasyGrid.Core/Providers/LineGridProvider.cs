using System;
using System.Collections.Generic;
using System.Text;
using EasyGrid.Core.Models;
using EasyGrid.Core.Models.GPX;

namespace EasyGrid.Core.Providers
{
    public class LineGridProvider
    {
        private readonly int width;
        private readonly int height;

        public LineGridProvider(int width, int height)
        {
            this.width = width;
            this.height = height;
        }

        public (int lat, int lon)[] GenerateLinePath()
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
                    if (!Move(horizontalDirection, ref i, ref j))
                    {
                        horizontalDirection = RevertDirection(horizontalDirection);

                        if (!Move(verticalDirection, ref i, ref j))
                        {
                            iteration++;
                            verticalDirection = RevertDirection(verticalDirection);
                        }
                    }
                }

                if (iteration == 1)
                {
                    if (!Move(verticalDirection, ref i, ref j))
                    {
                        verticalDirection = RevertDirection(verticalDirection);

                        if (!Move(horizontalDirection, ref i, ref j))
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

        private bool Move(Direction direction, ref int i, ref int j)
        {
            switch (direction)
            {
                case Direction.Up:
                    if (j > 0)
                    {
                        j--;
                        return true;
                    }
                    break;
                case Direction.Right:
                    if (i < width - 1)
                    {
                        i++;
                        return true;
                    }
                    break;
                case Direction.Down:
                    if (j < height - 1)
                    {
                        j++;
                        return true;
                    }
                    break;
                case Direction.Left:
                    if (i > 0)
                    {
                        i--;
                        return true;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }

            return false;
        }

        private static Direction RevertDirection(Direction direction)
        {
            switch (direction)
            {
                case Direction.Up: return Direction.Down;
                case Direction.Right: return Direction.Left;
                case Direction.Down: return Direction.Up;
                case Direction.Left: return Direction.Right;
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }
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
