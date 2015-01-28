using System;

using RLToolkit.Logger;

namespace RLToolkit.Widgets
{
    /// <summary>
    /// Widget that allow the user to Multiple controls in a grid-like interface, with the possibility to +/- in the list and with possibility to use either vertically or horizontally.
    /// </summary>
    [System.ComponentModel.ToolboxItem(true)]
    public partial class GridSelector : Gtk.Bin
    {
        #region variables
        // the control array
        private Gtk.Widget[] controlArray;

        // the filler control type
        private System.Type fillerControlType = typeof(Gtk.Label);

        // the offset variables
        private int offset = 0;

        // if we're initialized yet
        private bool isInitialized = false;
        #endregion

        #region state
        // The state of the control controls

        /// <summary>
        /// The state (using the eState enum) of the control. used to handle vertical or horizontal mode.
        /// </summary>
        public eState controlState = eState.None;

        /// <summary> The various possible state the control can take.</summary>
        public enum eState
        {
            /// <summary>No mode selected, will not display +/- buttons. </summary>
            None = 0,
            /// <summary>Vertical mode.</summary>
            Vertical,
            /// <summary>Horizontal mode</summary>
            Horizontal
        };
        #endregion

        #region Properties
        // property
        private int nbCol = 3;
        private int nbRow = 3;

        /// <summary>
        /// Property used for the number of columns in the grid
        /// </summary>
        /// <value>The number of columns to use.</value>
        /// <remarks>Needs to be in between 1 and 20</remarks>
        public int NbCol
        {
            get {
                return nbCol;
            }
            set {
                nbCol = value;
            }
        }

        /// <summary>
        /// Property used for the number of rows in the grid
        /// </summary>
        /// <value>The number of rows to use.</value>
        /// <remarks>Needs to be in between 1 and 20</remarks>
        public int NbRow
        {
            get {
                return nbRow;
            }
            set {
                nbRow = value;
            }
        }
        #endregion

        #region Constructor/Initialization
        /// <summary>
        /// Method that builds the control. 
        /// </summary>
        /// <remarks>Make sure to call the 'Initialize' method before using the control</remarks>
        public GridSelector()
        {
            this.Build();
        }

        /// <summary>
        /// Method to fill the control with the controls used for the grid and to define it state
        /// </summary>
        /// <param name="s">The state of the control to use using the eState Enum</param>
        /// <param name="inputControls">The control array of Gtk.Widgets to use.</param>
        public void Initialize(eState s, Gtk.Widget[] inputControls)
        {
            // initialize the state
            if (s == eState.None)
            {
                SetNoControlMode();
            } else if (s == eState.Horizontal)
            {
                SetHorizontalMode();
            } else if (s == eState.Vertical)
            {
                SetVerticalMode();
            } else
            {
                // something went terribly wrong
                this.Log().Warn("Invalid state requested. Using 'none'");
                SetNoControlMode();
            }

            // set the number of column/row
            if (nbCol <= 0)
            {
                this.Log().Warn("Tried to use a number of column invalid. (needs to be over 0)");
                nbCol = 1;
            }
            if (nbCol > 20)
            {
                this.Log().Warn("Tried to use a number of column invalid. (needs to be under 21)");
                nbCol = 20;
            }
            if (nbRow <= 0)
            {
                this.Log().Warn("Tried to use a number of row invalid. (needs to be over 0)");
                nbRow = 1;
            }
            if (nbRow > 20)
            {
                this.Log().Warn("Tried to use a number of row invalid. (needs to be under 21)");
                nbRow = 20;
            }
            tableContent.NColumns = (uint)nbCol;
            tableContent.NRows = (uint)nbRow;

            // initialize the controls
            SetControlArray(inputControls);

            // done initializing
            isInitialized = true;
            RefreshControlShown();
        }
        #endregion

        #region Mode/FillerType/Array setter
        /// <summary>
        /// Set the filler type of control to use. Default is Gtk.Label.
        /// </summary>
        /// <param name="input">the typeof your desired GTK widget control.</param>
        /// <remarks>Do not use GTK.Widget as filler.</remarks>
        public void SetFillerControlType(System.Type input)
        {
            // updating the filler type
            fillerControlType = input;

            // redraw in case we have some shown
            RefreshControlShown();
        }

        /// <summary>
        /// Method to update the control array.
        /// </summary>
        /// <param name="inputArray">the new control array to use</param>
        public void SetControlArray(Gtk.Widget[] inputArray)
        {
            controlArray = inputArray;
            RefreshControlShown();
        }

        /// <summary>
        /// Method to set the Grid in Horizontal mode.
        /// </summary>
        public void SetHorizontalMode()
        {
            // uses the H controls, start top+left, bottom+left, Top+right
            controlState = eState.Horizontal;
            verticalControl.Hide();
            horizontalControl.Show();
        }

        /// <summary>
        /// Method to set the Grid in Vertical mode.
        /// </summary>
        public void SetVerticalMode()
        {
            // uses the V controls, start top+left, top+right, Bottom+left
            controlState = eState.Vertical;
            horizontalControl.Hide();
            verticalControl.Show();
        }

        /// <summary>
        /// Method to set the Grid in 'none' mode. No way to change controls in this mode.
        /// </summary>
        public void SetNoControlMode()
        {
            // No movement controls, only use what's shown
            controlState = eState.None;
            horizontalControl.Hide();
            verticalControl.Hide();
        }
        #endregion

        #region Refresh
        private void RefreshControlShown()
        {
            if (!isInitialized)
            {
                // not initialized.  don't try anything yet.
                return;
            }

            // fill the tableContent with the right controls
            foreach (Gtk.Widget c in tableContent.Children)
            {
                // clear all controls
                tableContent.Remove(c);
            }

            int numX;
            int numY;
            bool hmode = false;

            // Fetch the info for our state/max
            switch (controlState)
            {
                case eState.Horizontal:
                    hmode = true;
                    numX = (int)tableContent.NColumns;
                    numY = (int)tableContent.NRows;
                    break;
                case eState.Vertical:
                    hmode = false;
                    numX = (int)tableContent.NRows;
                    numY = (int)tableContent.NColumns;
                    break;
                case eState.None:
                    hmode = true;
                    numX = (int)tableContent.NColumns;
                    numY = (int)tableContent.NRows;
                    break;
                default:
                    // we have a problem here
                    this.Log().Warn("Something went wrong. Invalid state.");
                    hmode = true;
                    numX = (int)tableContent.NColumns;
                    numY = (int)tableContent.NRows;
                    break;
            }

            // Fill the taqble with our content
            int nControl = 0;
            for (int x = 0; x < numX; x++)
            {
                for (int y = 0; y < numY; y++)
                {
                    // figure out which control to use
                    int index = nControl + (offset * numY);
                    Gtk.Widget controlToUse;
                    if (index >= controlArray.Length)
                    {
                        // filler
                        controlToUse = (Gtk.Widget)Activator.CreateInstance(fillerControlType);
                        controlToUse.Sensitive = false;
                    } 
                    else
                    {
                        controlToUse = controlArray[index];
                    }

                    // depending on the control mode, find out where to attach
                    if (hmode)
                    {
                        tableContent.Attach(controlToUse, (uint)x, (uint)x+1, (uint)y, (uint)y+1);
                    } 
                    else
                    {
                        tableContent.Attach(controlToUse, (uint)y, (uint)y+1, (uint)x, (uint)x+1);
                    }

                    // increment the count
                    nControl++;
                }
            }

            // make sure we show everything
            tableContent.ShowAll();
        }
        #endregion

        #region Validation
        private void ValidateOffset()
        {
            if (offset < 0)
            {
                this.Log().Info("Negative Offset found, setting to 0.");
                offset = 0;
            } 
            else
            {
                int max;
                switch (controlState)
                {
                    case eState.Horizontal:
                        max = (int)Math.Floor((float)(controlArray.Length - 1) / (float)tableContent.NRows);
                        break;
                    case eState.Vertical:
                        max = (int)Math.Floor((float)(controlArray.Length - 1) / (float)tableContent.NColumns);
                        break;
                    case eState.None:
                        max = 0;
                        break;
                    default:
                        max = 0;
                        break;
                }
                if (offset > max)
                {
                    this.Log().Info("Offset too high, setting to maximum");
                    offset = max;
                }
            }
        }
        #endregion

        #region Events
        /// <summary>
        /// EventHandler for the vertical minus button pressed
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        protected void OnBtnVMinusClicked (object sender, EventArgs e)
        {
            offset--;
            ValidateOffset();
            RefreshControlShown();
        }

        /// <summary>
        /// EventHandler for the vertical plus button pressed
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        protected void OnBtnVPlusClicked (object sender, EventArgs e)
        {
            offset++;
            ValidateOffset();
            RefreshControlShown();
        }

        /// <summary>
        /// EventHandler for the Horizontal minus button pressed
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        protected void OnBtnHMinusClicked (object sender, EventArgs e)
        {
            offset--;
            ValidateOffset();
            RefreshControlShown();
        }

        /// <summary>
        /// EventHandler for the Horizontal plus button pressed
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        protected void OnBtnHPlusClicked (object sender, EventArgs e)
        {
            offset++;
            ValidateOffset();
            RefreshControlShown();
        }
        #endregion
    }
}