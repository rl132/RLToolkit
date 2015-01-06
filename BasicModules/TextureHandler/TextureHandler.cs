using System;
using System.Drawing;
using System.IO;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

using RLToolkit.Logger;

namespace RLToolkit.Basic
{
    /// <summary>
    /// Texture handler.
    /// </summary>
    public class TextureHandler
    {
        /// <summary>Gets or sets the texture (bitmap)</summary>
        public Bitmap texture { get; set; }

        /// <summary>Gets or sets the texture width</summary>
        public int width { get; set; }

        /// <summary>Gets or sets the texture height</summary>
        public int height { get; set; }

        #region Constructors/Dispose
        /// <summary>
        /// Constructor that makes an empty texture with specified dimentions
        /// </summary>
        /// <param name="width">Width of the texture</param>
        /// <param name="height">Height of the texture</param>
        public TextureHandler(int width, int height)
        {
            this.Log().Debug(string.Format("Creating a new texture with size {0}x{1}", width.ToString(), height.ToString()));
            texture = new Bitmap(width, height);
            this.width = width;
            this.height = height;
        }

        /// <summary>
        /// Constructor that makes a texture from a file
        /// </summary>
        /// <param name="fullpath">Fullpath of the file</param>
        public TextureHandler(string fullpath)
        {
            this.Log ().Debug (string.Format("Creating a new texture from file: {0}", fullpath));
            texture = (Bitmap) Image.FromFile(fullpath);
            this.width = texture.Width;
            this.height = texture.Height;
        }

        /// <summary>
        /// Constructor that makes a texture from a bitmap
        /// </summary>
        /// <param name="image">The source bitmap</param>
        public TextureHandler(Bitmap image)
        {
            this.Log ().Debug (string.Format("Creating a new texture from another texture"));
            texture = image;
            this.width = texture.Width;
            this.height = texture.Height;
        }

        /// <summary>
        /// Releases all resource used by the <see cref="RLToolkit.Basic.TextureHandler"/> object.
        /// </summary>
        /// <remarks>Call <see cref="Dispose"/> when you are finished using the <see cref="RLToolkit.Basic.TextureHandler"/>. The
        /// <see cref="Dispose"/> method leaves the <see cref="RLToolkit.Basic.TextureHandler"/> in an unusable state.
        /// After calling <see cref="Dispose"/>, you must release all references to the
        /// <see cref="RLToolkit.Basic.TextureHandler"/> so the garbage collector can reclaim the memory that the
        /// <see cref="RLToolkit.Basic.TextureHandler"/> was occupying.</remarks>
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
        /// <summary>
        /// Method to save the current Texture into a specified file, using default format
        /// </summary>
        /// <param name="fullpath">Fullpath.</param>
        public void Save(string fullpath)
        {
            this.Log ().Debug (string.Format("Implicit Save Called to: {0}", fullpath));
            Save(fullpath, ImageFormat.Png);
        }

        /// <summary>
        /// Method to save the current Texture into a specific file, using a specific format.
        /// </summary>
        /// <param name="fullpath">Fullpath.</param>
        /// <param name="format">Format.</param>
        public void Save(string fullpath, ImageFormat format)
        {
            // TODO: add some arguments Exceptions for the path
            this.Log ().Debug (string.Format("Save Called to \"{0}\"  with format: {1}", fullpath, format.ToString()));
            texture.Save(fullpath, format);
        }
        #endregion

        #region Resize
        /// <summary>
        /// Method to resize the current texture
        /// </summary>
        /// <param name="newWidth">New width.</param>
        /// <param name="newHeight">New height.</param>
        public void Resize(int newWidth, int newHeight)
        {
            Resize(newWidth, newHeight, true);
        }

        /// <summary>
        /// Method to resize the current texture, with the possiblity to define if the proportion are kept
        /// </summary>
        /// <param name="newWidth">New width.</param>
        /// <param name="newHeight">New height.</param>
        /// <param name="keepRatio">If set to <c>true</c> keep ratio.</param>
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
        /// <summary>
        /// Method to combine a Bitmap Jagged Array to the current texture
        /// </summary>
        /// <param name="input">Bitmap jagged array</param>
        public void Combine(Bitmap[][] input)
        {
            Combine(input, width, height, true);
        }

        /// <summary>
        /// Method to combine a Bitmap Jagged Array into the current texture, with specific height and width
        /// </summary>
        /// <param name="input">Bitmap jagged array</param>
        /// <param name="newWidth">New width.</param>
        /// <param name="newHeight">New height.</param>
        public void Combine(Bitmap[][] input, int newWidth, int newHeight)
        {
            Combine(input, newWidth, newHeight, true);
        }

        /// <summary>
        /// Method to combine a Bitmap Jagged Array into the current texture, specifying if the ratio are kept
        /// </summary>
        /// <param name="input">Bitmap jagged array</param>
        /// <param name="keepRatio">If set to <c>true</c> keep ratio.</param>
        public void Combine(Bitmap[][] input, bool keepRatio)
        {
            Combine(input, width, height, keepRatio);
        }

        /// <summary>
        /// Method to combine a Bitmap Jagged Array into the current texture, specifying if the ratio are kept and the new width/height
        /// </summary>
        /// <param name="input">Bitmap jagged array</param>
        /// <param name="newWidth">New width.</param>
        /// <param name="newHeight">New height.</param>
        /// <param name="keepRatio">If set to <c>true</c> keep ratio.</param>
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