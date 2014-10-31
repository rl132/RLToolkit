using System;
using System.Drawing;

namespace RLToolkit.Extensions
{
    /// <summary>
    /// Class to extends the functionalities of the Color class
    /// </summary>
    public static class ColorExtension
    {
        /// <summary>
        /// Compares a color.
        /// </summary>
        /// <returns><c>true</c>, if color are similqr, <c>false</c> otherwise.</returns>
        /// <param name="c">Current color</param>
        /// <param name="toCompare">To compare against.</param>
        public static bool CompareColor(this Color c, Color toCompare)
        {
            return CompareColor(c, toCompare, 0, true);
        }

        /// <summary>
        /// Compares a color.
        /// </summary>
        /// <returns><c>true</c>, if color are similqr, <c>false</c> otherwise.</returns>
        /// <param name="c">Current color</param>
        /// <param name="toCompare">To compare against.</param>
        /// <param name="tolerance">Tolerance.</param>
        public static bool CompareColor(this Color c, Color toCompare, int tolerance)
        {
            return CompareColor(c, toCompare, tolerance, true);
        }

        /// <summary>
        /// Compares a color.
        /// </summary>
        /// <returns><c>true</c>, if color are similqr, <c>false</c> otherwise.</returns>
        /// <param name="c">Current color</param>
        /// <param name="toCompare">To compare against.</param>
        /// <param name="alphaSupport">If set to <c>true</c> alpha support.</param>
        public static bool CompareColor(this Color c, Color toCompare, bool alphaSupport)
        {
            return CompareColor(c, toCompare, 0, alphaSupport);
        }

        /// <summary>
        /// Compares a color.
        /// </summary>
        /// <returns><c>true</c>, if color are similqr, <c>false</c> otherwise.</returns>
        /// <param name="c">Current color</param>
        /// <param name="toCompare">To compare against.</param>
        /// <param name="tolerance">Tolerance.</param>
        /// <param name="alphaSupport">If set to <c>true</c> alpha support.</param>
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

        /// <summary>
        /// Compare a pixel byte data against another, with a tolerance
        /// </summary>
        /// <returns><c>true</c>, if pixel byte is within the tolerance range, <c>false</c> otherwise.</returns>
        /// <param name="source">Source pixel channel byte</param>
        /// <param name="actual">pixel channel byte to compare against</param>
        /// <param name="tolerance">Tolerance.</param>
        private static bool ComparePixelChannel(byte source, byte actual, int tolerance)
        {
            return (Math.Abs(source - actual) < tolerance);
        }
    }
}