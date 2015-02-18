using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

using RLToolkit.Logger;

namespace RLToolkit.Extensions
{
    /// <summary>
    /// Class to extends the functionalities of the Bitmap class
    /// </summary>
    public static class BitmapExtension
    {
        /// <summary>
        /// Convert a bitmap into a Pixbuf.
        /// </summary>
        /// <returns>The output pixbuf</returns>
        /// <param name="input">The input bitmap</param>
        /// <remarks>Uses a temp folder and file to save the bitmap then reread it.</remarks>
        public static Gdk.Pixbuf ToPixbuf(this Bitmap input)
        {
            LogManager.Instance.Log().Info("trying to convert a bitmap to pixbuf. Could cause issues.");
            Random number = new Random();
            string tempPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"tempmageconvert-" + number.Next(5000, 50000).ToString() + ".png");
            input.Save(tempPath, ImageFormat.Png);
            Gdk.Pixbuf pBuf = new Gdk.Pixbuf(tempPath);
            File.Delete(tempPath);
            return pBuf;
       }
    }
}