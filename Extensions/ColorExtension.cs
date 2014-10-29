using System;
using System.Drawing;

namespace RLToolkit.Extensions
{
    public static class ColorExtension
    {
        public static bool CompareColor(this Color c, Color toCompare)
        {
            return CompareColor(c, toCompare, 0, true);
        }

        public static bool CompareColor(this Color c, Color toCompare, int tolerance)
        {
            return CompareColor(c, toCompare, tolerance, true);
        }

        public static bool CompareColor(this Color c, Color toCompare, bool alphaSupport)
        {
            return CompareColor(c, toCompare, 0, alphaSupport);
        }

        public static bool CompareColor(this Color c, Color toCompare, int tolerance, bool alphaSupport)
        {
            if (!ComparePixelChannel(c.R, toCompare.R, tolerance))
            {
                return false;
            }
            if (!ComparePixelChannel(c.G, toCompare.G, tolerance))
            {
                return false;
            }
            if (!ComparePixelChannel(c.B, toCompare.B, tolerance))
            {
                return false;
            }
            if (alphaSupport)
            {
                if (!ComparePixelChannel(c.A, toCompare.A, tolerance))
                {
                    return false;
                }
            }
            return true;
        }

        private static bool ComparePixelChannel(byte source, byte actual, int tolerance)
        {
            return (Math.Abs(source - actual) < tolerance);
        }
    }
}

