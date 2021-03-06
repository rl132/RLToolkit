﻿using System;
using System.Drawing;
using NUnit.Framework;

using RLToolkit.Extensions;
using RLToolkit.Logger;

namespace RLToolkit.Extensions
{
    /// <summary>
    /// Class to extends the functionalities of nUnit by adding support for comparing Bitmaps
    /// </summary>
    public static class BitmapAssert
    {
        #region AreEqual
        /// <summary>
        /// Are equal.
        /// </summary>
        /// <param name="source">Source.</param>
        /// <param name="actual">Actual.</param>
        /// <param name="message">Message.</param>
        public static void AreEqual(Bitmap source, Bitmap actual, string message)
        {
            AreEqual(source, actual, 0, true, message);
        }

        /// <summary>
        /// Are equal.
        /// </summary>
        /// <param name="source">Source.</param>
        /// <param name="actual">Actual.</param>
        /// <param name="alphaSupport">If set to <c>true</c> alpha support.</param>
        public static void AreEqual(Bitmap source, Bitmap actual, bool alphaSupport)
        {
            AreEqual(source, actual, 0, true, "");
        }

        /// <summary>
        /// Are equal.
        /// </summary>
        /// <param name="source">Source.</param>
        /// <param name="actual">Actual.</param>
        /// <param name="tolerence">Tolerence.</param>
        public static void AreEqual(Bitmap source, Bitmap actual, int tolerence)
        {
            AreEqual(source, actual, tolerence, true, "");
        }

        /// <summary>
        /// Are equal.
        /// </summary>
        /// <param name="source">Source.</param>
        /// <param name="actual">Actual.</param>
        /// <param name="tolerence">Tolerence.</param>
        /// <param name="alphaSupport">If set to <c>true</c> alpha support.</param>
        public static void AreEqual(Bitmap source, Bitmap actual, int tolerence, bool alphaSupport)
        {
            AreEqual(source, actual, tolerence, alphaSupport, "");
        }

        /// <summary>
        /// Are equal.
        /// </summary>
        /// <param name="source">Source.</param>
        /// <param name="actual">Actual.</param>
        /// <param name="tolerence">Tolerence.</param>
        /// <param name="message">Message.</param>
        public static void AreEqual(Bitmap source, Bitmap actual, int tolerence, string message)
        {
            AreEqual(source, actual, tolerence, true, message);
        }

        /// <summary>
        /// Are equal
        /// </summary>
        /// <param name="source">Source.</param>
        /// <param name="actual">Actual.</param>
        /// <param name="tolerence">Tolerence.</param>
        /// <param name="alphaSupport">If set to <c>true</c> alpha support.</param>
        /// <param name="message">Message.</param>
        public static void AreEqual(Bitmap source, Bitmap actual, int tolerence, bool alphaSupport, string message)
        {
            Test(true, source, actual, tolerence, alphaSupport, message);
        }
        #endregion

        #region AreNotEqual
        /// <summary>
        /// Are not equal
        /// </summary>
        /// <param name="source">Source.</param>
        /// <param name="actual">Actual.</param>
        /// <param name="message">Message.</param>
        public static void AreNotEqual(Bitmap source, Bitmap actual, string message)
        {
            AreNotEqual(source, actual, 0, true, message);
        }

        /// <summary>
        /// Are not equal
        /// </summary>
        /// <param name="source">Source.</param>
        /// <param name="actual">Actual.</param>
        /// <param name="alphaSupport">If set to <c>true</c> alpha support.</param>
        public static void AreNotEqual(Bitmap source, Bitmap actual, bool alphaSupport)
        {
            AreNotEqual(source, actual, 0, true, "");
        }

        /// <summary>
        /// Are not equal
        /// </summary>
        /// <param name="source">Source.</param>
        /// <param name="actual">Actual.</param>
        /// <param name="tolerence">Tolerence.</param>
        public static void AreNotEqual(Bitmap source, Bitmap actual, int tolerence)
        {
            AreNotEqual(source, actual, tolerence, true, "");
        }

        /// <summary>
        /// Are not equal
        /// </summary>
        /// <param name="source">Source.</param>
        /// <param name="actual">Actual.</param>
        /// <param name="tolerence">Tolerence.</param>
        /// <param name="alphaSupport">If set to <c>true</c> alpha support.</param>
        public static void AreNotEqual(Bitmap source, Bitmap actual, int tolerence, bool alphaSupport)
        {
            AreNotEqual(source, actual, tolerence, alphaSupport, "");
        }

        /// <summary>
        /// Are not equal
        /// </summary>
        /// <param name="source">Source.</param>
        /// <param name="actual">Actual.</param>
        /// <param name="tolerence">Tolerence.</param>
        /// <param name="message">Message.</param>
        public static void AreNotEqual(Bitmap source, Bitmap actual, int tolerence, string message)
        {
            AreNotEqual(source, actual, tolerence, true, message);
        }

        /// <summary>
        /// Are not equal
        /// </summary>
        /// <param name="source">Source.</param>
        /// <param name="actual">Actual.</param>
        /// <param name="tolerence">Tolerence.</param>
        /// <param name="alphaSupport">If set to <c>true</c> alpha support.</param>
        /// <param name="message">Message.</param>
        public static void AreNotEqual(Bitmap source, Bitmap actual, int tolerence, bool alphaSupport, string message)
        {
            Test(false, source, actual, tolerence, alphaSupport, message);
        }
        #endregion

        #region Test
        /// <summary>
        /// Method to test if two bitmaps are equals with a tolerence support
        /// </summary>
        /// <param name="expectEquals">If set to <c>true</c> expect equals.</param>
        /// <param name="source">Source.</param>
        /// <param name="actual">Actual.</param>
        /// <param name="tolerence">Tolerence.</param>
        /// <param name="alphaSupport">If set to <c>true</c> alpha support.</param>
        /// <param name="message">Message.</param>
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

                    if (!sPixel.CompareColor(aPixel, tolerence, alphaSupport))
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
    }
}