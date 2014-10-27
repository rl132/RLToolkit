using System;
using System.Drawing;
using NUnit.Framework;

namespace RLToolkit.Asserts
{
    public static class BitmapAssert
    {
        #region AreEqual
        public static void AreEqual(Bitmap source, Bitmap actual, string message)
        {
            AreEqual(source, actual, 0, true, message);
        }

        public static void AreEqual(Bitmap source, Bitmap actual, bool alphaSupport)
        {
            AreEqual(source, actual, 0, true, "");
        }

        public static void AreEqual(Bitmap source, Bitmap actual, int tolerence)
        {
            AreEqual(source, actual, tolerence, true, "");
        }

        public static void AreEqual(Bitmap source, Bitmap actual, int tolerence, bool alphaSupport)
        {
            AreEqual(source, actual, tolerence, alphaSupport, "");
        }

        public static void AreEqual(Bitmap source, Bitmap actual, int tolerence, string message)
        {
            AreEqual(source, actual, tolerence, true, message);
        }

        public static void AreEqual(Bitmap source, Bitmap actual, int tolerence, bool alphaSupport, string message)
        {
            Test(true, source, actual, tolerence, alphaSupport, message);
        }
        #endregion

        #region AreNotEqual
        public static void AreNotEqual(Bitmap source, Bitmap actual, string message)
        {
            AreNotEqual(source, actual, 0, true, message);
        }

        public static void AreNotEqual(Bitmap source, Bitmap actual, bool alphaSupport)
        {
            AreNotEqual(source, actual, 0, true, "");
        }

        public static void AreNotEqual(Bitmap source, Bitmap actual, int tolerence)
        {
            AreNotEqual(source, actual, tolerence, true, "");
        }

        public static void AreNotEqual(Bitmap source, Bitmap actual, int tolerence, bool alphaSupport)
        {
            AreNotEqual(source, actual, tolerence, alphaSupport, "");
        }

        public static void AreNotEqual(Bitmap source, Bitmap actual, int tolerence, string message)
        {
            AreNotEqual(source, actual, tolerence, true, message);
        }

        public static void AreNotEqual(Bitmap source, Bitmap actual, int tolerence, bool alphaSupport, string message)
        {
            Test(false, source, actual, tolerence, alphaSupport, message);
        }
        #endregion

        #region Test
        private static void Test(bool expectEquals, Bitmap source, Bitmap actual, int tolerence, bool alphaSupport, string message)
        {
            LogManager.Instance.Log().Debug(String.Format("Comparing bitmaps with tolerence {0} and alphaSupport: {1}", tolerence, alphaSupport));
            //make sure we have a source and actual, or that both are null
            if ((source == null) && (actual == null))
            {
                if (expectEquals)
                {
                    // they are equal
                    return;
                }
                // not equal
                throw new AssertionException("." + Environment.NewLine + message);
            }

            if (((source == null) && (actual != null)) || 
                ((source != null) && (actual == null)))
            {
                if (expectEquals)
                {
                    // not equal
                    throw new AssertionException("One of the Images is null while the other is not." + Environment.NewLine + message);
                }
                // equals
                return;
            }

            // compre sizes
            if (source.Size != actual.Size)
            {
                if (expectEquals)
                {
                    // not equal
                    throw new AssertionException("Sizes are different." + Environment.NewLine + message);
                }
                // equals
                return;
            }

            // foreach pixels on each row, compare
            bool notSame = false;
            for (int row = 0; row < source.Width; row++)
            {
                LogManager.Instance.Log().Debug(string.Format("row {0} out of {1}", row, source.Width-1));
                for (int col = 0; col < source.Height; col++)
                {
                    //LogManager.Instance.Log().Verbose(string.Format("column {0} out of {1}", col, source.Height));
                    Color sPixel = source.GetPixel(row, col);
                    Color aPixel = actual.GetPixel(row, col);
                    if (!ComparePixelColor(sPixel, aPixel, tolerence, alphaSupport))
                    {
                        notSame = true;
                        if (expectEquals)
                        {
                            // not equal
                            throw new AssertionException(string.Format("Pixel at {0}x{1} is above the treshold of tollerence." + Environment.NewLine + message, row, col));
                        }
                    } 
                }
            }

            if (notSame)
            {
                if (expectEquals)
                {
                    // not equal
                    throw new AssertionException(string.Format("Image shows no sign of difference and we expected it to be."));
                }
            }
        }
        #endregion

        #region helper
        private static bool ComparePixelColor(Color source, Color actual, int tolerance, bool alphaSupport)
        {
            //LogManager.Instance.Log().Debug(String.Format("Comparing pixel {0} to {1} with tolerence {2}, alpha support: {3}",source.ToArgb().ToString(), actual.ToArgb().ToString(), tolerance, alphaSupport));
            if (!ComparePixelChannel(source.R, actual.R, tolerance))
            {
                return false;
            }
            if (!ComparePixelChannel(source.G, actual.G, tolerance))
            {
                return false;
            }
            if (!ComparePixelChannel(source.B, actual.B, tolerance))
            {
                return false;
            }
            if (alphaSupport)
            {
                if (!ComparePixelChannel(source.A, actual.A, tolerance))
                {
                    return false;
                }
            }
            return true;
        }

        private static bool ComparePixelChannel(byte source, byte actual, int tolerance)
        {
            //LogManager.Instance.Log().Debug(String.Format("Comparing pixel channel {0} to {1} with tolerence {2}",source.ToString(), actual.ToString(), tolerance));
            return (Math.Abs(source - actual) < tolerance);
        }
        #endregion
    }
}

