using System;
using System.Drawing;
using Gtk;

using RLToolkit.Logger;
using RLToolkit.Extensions;

// TODO: RL
// - Add a way to initialize with an image
// - Add check on size if it's a value of 2^n

namespace RLToolkit.Widgets
{
    /// <summary>
    /// Widget that allow the user to make a button with only an image.
    /// </summary>
    [System.ComponentModel.ToolboxItem(true)]
    public partial class ButtonImage : Gtk.Bin
    {
        private string filename;
        private int imageSize = 64;
        private Bitmap fromFile;

        #region Constructor and Init
        /// <summary>
        /// Constructor for an empty ButtonImage
        /// </summary>
        public ButtonImage ()
        {
            this.Log ().Debug ("Constructor with default size");
            Init ();
        }

        /// <summary>
        /// Constructor for a ButtonImage that has a non-default image size
        /// </summary>
        /// <param name="size">The new size</param>
        public ButtonImage(int size)
        {
            this.Log ().Debug ("Constructor with size " + size.ToString());
            imageSize = size;
            Init ();
        }

        private void Init()
        {
            this.Log ().Debug ("Initialization");
            this.Build ();

            UpdateImage ();
        }
        #endregion

        #region Public function
        /// <summary>
        /// Fetch the image data
        /// </summary>
        /// <returns>The image bitmap</returns>
        public Bitmap GetImage()
        {
            this.Log ().Debug ("Fetching image");
            return fromFile;
        }

        /// <summary>
        /// Fetch the full filenasme of the selected file
        /// </summary>
        /// <returns>The filename </returns>
        public string GetFilename()
        {
            this.Log ().Debug ("Fetching filename");
            return filename;
        }

        /// <summary>
        /// Clear the ButtonImage and bring it back to it default
        /// </summary>
        public void Clear()
        {
            this.Log ().Debug ("Cleaning button");
            filename = null;
            imageSize = 64;
            fromFile = null;
            UpdateImage ();
        }

        /// <summary>
        /// Updates the size of the image on the ButtonImage
        /// </summary>
        /// <param name="newSize">the new size to use</param>
        public void UpdateSize(int newSize)
        {
            imageSize = newSize;
            this.HeightRequest = newSize;
            this.WidthRequest = newSize;
            UpdateImage ();
        }

        /// <summary>
        /// Sets the image by forcing a filename in, and updating the image internally
        /// </summary>
        /// <param name="filename">Filename of the image to load.</param>
        public void SetImage(string filename)
        {
            this.Log ().Debug ("Force setting a new image from file");
            filename = filename;
            fromFile = new Bitmap(filename);
            UpdateImage ();
        }

        /// <summary>
        /// Sets the image by forcing a bitmap in
        /// </summary>
        /// <param name="image">the new image</param>
        public void SetImage(Bitmap image)
        {
            this.Log ().Debug ("Force setting a new image from bitmap");
            filename = null;
            fromFile = image;
            UpdateImage ();
        }
        #endregion

        #region helper methods
        private void UpdateImage()
        {
            this.Log ().Info ("Updating image preview");
            if (string.IsNullOrWhiteSpace(filename)) 
            {
                if (fromFile == null)
                {
                    this.Log().Debug("No filename nor image, using stock image");
                    img.SetSizeRequest(imageSize, imageSize);
                    img.SetFromStock(Gtk.Stock.No, Gtk.IconSize.Button);
                }
                else
                {
                    this.Log().Debug("No filename but image available");
                    img.Pixbuf = fromFile.ToPixbuf();
                }
            }
            else
            {
                this.Log().Debug("filename supplied, using file");
                img.Pixbuf = new Gdk.Pixbuf(filename).ScaleSimple(imageSize, imageSize, Gdk.InterpType.Bilinear);
            }
        }
 
        private void ValidateImage()
        {
            this.Log ().Info ("Validating image");
            // make sure it's an image
            try
            {
                fromFile = new Bitmap(System.Drawing.Image.FromFile(filename));
                this.WidthRequest = imageSize;
                this.HeightRequest = imageSize;
            }
            catch (Exception e) 
            {
                this.Log ().Warn ("Error encountered while opening image.");
                this.Log ().Debug ("Error info:" + Environment.NewLine + e.Data);
                MessageDialog dialog = new MessageDialog (null, DialogFlags.Modal, MessageType.Error, ButtonsType.Ok, "Cannot fetch image. This file cannot be used." + Environment.NewLine + Environment.NewLine + "Error info:" + Environment.NewLine + e.Data);
                if (dialog.Run () == (int)ResponseType.Ok) {
                    dialog.Destroy ();
                }
                filename = null;
            }
        }
        #endregion

        #region Event handler
        /// <summary>
        /// Raises the button clicked event.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        protected void OnBtnClicked (object sender, EventArgs e)
        {
            this.Log ().Info ("Button clicked");
            FileChooserDialog chooser = new FileChooserDialog(
                "Please select an image",
                null,
                FileChooserAction.Open,
                new object[] {
                "Cancel", ResponseType.Cancel,
                "Open", ResponseType.Accept 
            });

            int result = chooser.Run ();
            this.Log ().Info ("Dialog result: " + result.ToString());
            if( result == ( int )ResponseType.Accept )
            {
                this.Log ().Info ("Dialog accepted, filename picked: " + chooser.Filename);
                filename = chooser.Filename;

                ValidateImage ();
            }

            UpdateImage ();

            chooser.Hide ();        
        }
        #endregion
    }
}