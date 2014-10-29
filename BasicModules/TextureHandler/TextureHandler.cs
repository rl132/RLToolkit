using System;
using System.Drawing;
using System.IO;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace RLToolkit.Basic
{
    public class TextureHandler
    {
        public Bitmap texture { get; set; }
        public int width { get; set; }
        public int height { get; set; }

        #region Constructors/Dispose
        public TextureHandler(int width, int height)
        {
            this.Log().Debug(string.Format("Creating a new texture with size {0}x{1}", width.ToString(), height.ToString()));
            texture = new Bitmap(width, height);
            this.width = width;
            this.height = height;
        }

        public TextureHandler(string fullpath)
        {
            this.Log ().Debug (string.Format("Creating a new texture from file: {0}", fullpath));
            texture = (Bitmap) Image.FromFile(fullpath);
            this.width = texture.Width;
            this.height = texture.Height;
        }

        public TextureHandler(Bitmap image)
        {
            this.Log ().Debug (string.Format("Creating a new texture from another texture"));
            texture = image;
            this.width = texture.Width;
            this.height = texture.Height;
        }

        public void Dispose()
        {
            // clean everything
            texture.Dispose();
            texture = null;
            width = -1;
            height = -1;
        }
        #endregion

        #region Save
        public void Save(string fullpath)
        {
            this.Log ().Debug (string.Format("Implicit Save Called to: {0}", fullpath));
            Save(fullpath, ImageFormat.Png);
        }

        public void Save(string fullpath, ImageFormat format)
        {
            // TODO: add some arguments Exceptions for the path
            this.Log ().Debug (string.Format("Save Called to \"{0}\"  with format: {1}", fullpath, format.ToString()));
            texture.Save(fullpath, format);
        }
        #endregion

        #region Resize
        public void Resize(int newWidth, int newHeight)
        {
            Resize(newWidth, newHeight, true);
        }

        public void Resize(int newWidth, int newHeight, bool keepRatio)
        {
            this.Log().Debug(string.Format("Resizing an image to {0}x{1}, keepRatio= {2}", newWidth.ToString(), newHeight.ToString(), keepRatio.ToString()));

            if (newWidth <= 0)
            {
                throw new ArgumentException("Width and Height of the new target image can't be negative or zero", "newWidth");
            }
            if (newHeight <= 0)
            {
                throw new ArgumentException("Width and Height of the new target image can't be negative or zero", "newHeight");
            }

            if (keepRatio)
            {
                float pWidth = ((float)newWidth / (float)width);
                float pHeight = ((float)newHeight / (float)height);
                float pRatio = (pHeight < pWidth) ? pHeight : pWidth;

                newWidth = (int)(width * pRatio);
                newHeight = (int)(height * pRatio);
                this.Log().Info(string.Format("Changing size to {0}x{1}", newWidth.ToString(), newHeight.ToString()));
            } 
           
            Bitmap bOut = new Bitmap(newWidth, newHeight);
            Graphics g = Graphics.FromImage((Image)bOut);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            g.DrawImage((Image)texture, 0, 0, newWidth, newHeight);
            g.Dispose();

            texture = bOut;
            width = newWidth;
            height = newHeight;
        }
        #endregion

        #region combine
        public void Combine(Bitmap[][] input)
        {
            Combine(input, width, height, true);
        }

        public void Combine(Bitmap[][] input, int newWidth, int newHeight)
        {
            Combine(input, newWidth, newHeight, true);
        }

        public void Combine(Bitmap[][] input, bool keepRatio)
        {
            Combine(input, width, height, keepRatio);
        }

        public void Combine(Bitmap[][] input, int newWidth, int newHeight, bool keepRatio)
        {
            // TODO: add alpha support

            this.Log().Debug(string.Format("Combining the image array to {0}x{1}, keepRatio= {2}", newWidth.ToString(), newHeight.ToString(), keepRatio.ToString()));

            // some checks
            if ((input == null) || (input.Length == 0))
            {
                throw new ArgumentException("Bitmap Array canot be null or empty", "input");
            }

            if (newWidth <= 0)
            {
                throw new ArgumentException("Width and Height of the new target image can't be negative or zero", "newWidth");
            }
            if (newHeight <= 0)
            {
                throw new ArgumentException("Width and Height of the new target image can't be negative or zero", "newHeight");
            }

            // figure out our max dimentions
            int maxLength = input[0].Length;
            for (int x = 1; x < input.Length; x++)
            {
                if (input[x].Length > maxLength)
                {
                    maxLength = input[x].Length;
                }
            }

            // figure out the size for each pictures
            int eachWidth = (int)(newWidth / maxLength);
            int eachHeight = (int)(newHeight / input.Length);

            // create our target image
            Bitmap bOut = new Bitmap(newWidth, newHeight);
            Graphics g = Graphics.FromImage((Image)bOut);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.Clear(SystemColors.AppWorkspace);
            TextureHandler img;

            // combine
            for(int row = 0; row < input.Length; row++)
            {
                for(int col = 0; col < input[row].Length; col++)
                {
                    if (input[row][col] == null)
                    {
                        // skip if we have a missing element
                        continue;
                    }

                    img = new TextureHandler(input[row][col]);
                    img.Resize(eachWidth, eachHeight, keepRatio);
                    g.DrawImage(img.texture, new Point(col*eachWidth, row*eachHeight));
                    img.Dispose ();
                }
            }

            // push back as the result value
            texture = bOut;
            width = newWidth;
            height = newHeight;
        }
        #endregion
    }
}