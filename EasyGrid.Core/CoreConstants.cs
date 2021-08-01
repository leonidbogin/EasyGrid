using System;

namespace EasyGrid.Core
{
    internal static class CoreConstants
    {
        public const double Rad = Math.PI / 180.0;

        public static class Planet
        {
            public const double Lat = 40035000;
            public const double Lon = 40075000;
        }

        public static class Angle
        {
            public const double Down = 270.0 * Rad;
            public const double Right = 0;
        }
    }
}
