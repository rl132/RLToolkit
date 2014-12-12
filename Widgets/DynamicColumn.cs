using System;

using RLToolkit;

// TODO: RL
//  - add event for when a new control is added/removed
//  - add event for when max/min are reached

namespace RLToolkit.Widgets
{
    /// <summary>
    /// Widget that allow the user to add multiple instances of controls as column
    /// </summary>
    [System.ComponentModel.ToolboxItem(true)]
    public partial class DynamicColumn : Gtk.Bin
    {
        private int numberControls = 1;
		private System.Type baseControl;
		private object[] baseParam;
		private Gtk.Widget[] arrayWidgets;
		private int maxControls = 8;

		#region Constructors
        /// <summary>
        /// Constructor for a DynamicColumn empty (will use empty Gtk.Label as empty controls).
        /// </summary>
		public DynamicColumn ()
		{
			this.Log ().Debug ("Empty Constructor for the DynamicColumn");
			this.Build ();

			SetControlType (typeof(Gtk.Label), null);
		}

        /// <summary>
        /// Constructor for a DynamicColumn with a supplied control type 
        /// </summary>
        /// <param name="controlType">The Control type to spawn.</param>
		public DynamicColumn (System.Type controlType)
		{
			this.Log ().Debug ("Constructor for the DynamicColumn using " + controlType.Name);
			this.Build ();

			SetControlType (controlType, null);
		}

        /// <summary>
        /// Constructor for a DynamicColumn with a supplied control type and parameters
        /// </summary>
        /// <param name="controlType">The Control type to spawn.</param>
        /// <param name="param">Parameter (optional).</param>
    	public DynamicColumn (System.Type controlType, object[] param)
		{
			this.Log ().Debug ("Constructor for the DynamicColumn using " + controlType.Name + " with params");
			this.Build ();

			SetControlType (controlType, param);
		}
		#endregion

		#region public methods
        /// <summary>
        /// Set the type of control to use as child control that will spawn
        /// </summary>
        /// <param name="controlType">The Control type to spawn.</param>
        /// <param name="param">Parameter (optional).</param>
		public void SetControlType(System.Type controlType, object[] param)
		{
			this.Log().Debug("Setting type to: " + controlType.Name);

			baseControl = controlType;
			baseParam = param;

			arrayWidgets = new Gtk.Widget[1];
			Gtk.Widget newWidget = CreateCtrl ();
             
			arrayWidgets [0] = newWidget;

			RefreshControl ();
		}

        /// <summary>
        /// Fetches the content of the control array.
        /// </summary>
        /// <returns>The control array.</returns>
		public Gtk.Widget[] GetControlArray()
        {
            this.Log().Debug("Fetching the control array");
            return arrayWidgets;
        }

        /// <summary>
        /// Sets the control array.
        /// </summary>
        /// <param name="newArray">New array that will replace the old one</param>
        public void SetControlArray(Gtk.Widget[] newArray)
        {
            this.Log().Debug("updating the control array");
            arrayWidgets = newArray;

            RefreshControl();
        }

        /// <summary>
        /// Updates the max count on control the user can add. Note: will cut the current control list is the new maximum is too high.
        /// </summary>
        /// <param name="newCount">New count.</param>
        public void UpdateMaxCount(int newCount)
        {
            this.Log().Debug("updating the Max count to " + newCount.ToString());

            // find if we're over the new count
            if (numberControls > newCount)
            {
                this.Log().Warn(string.Format("There is too many controls ({0}) to keep them all with the new count ({1}).", numberControls, newCount));
                numberControls = newCount;

                // update the array and max numbers
                Gtk.Widget[] newArray = new Gtk.Widget[numberControls];
                for (int i = 0; i<numberControls; i++) {
                    newArray [i] = arrayWidgets [i];
                }
                arrayWidgets = newArray;

                RefreshControl();
            }

            // update the max
            maxControls = newCount;
        }
        #endregion

        #region Helper methods
		private Gtk.Widget CreateCtrl()
		{
			this.Log().Debug("Creating a new control");
			if (baseParam == null) {
				return (Gtk.Widget)Activator.CreateInstance (baseControl);
			} else {
				return (Gtk.Widget)Activator.CreateInstance (baseControl, baseParam);
			}
		}

        private void RefreshControl()
        {
            this.Log ().Debug ("Refreshing the controls");

            // wipe everything
            foreach (Gtk.Widget ch in columnBox.Children) {
                columnBox.Remove (ch);
            }

            // add the widgets
            int i = 0;
            foreach (Gtk.Widget w in arrayWidgets) {
                columnBox.Spacing = i;
                columnBox.Add (w);
                i++;
            }

            // refresh all
            columnBox.ShowAll ();
        }
        #endregion

        #region events
        protected void OnBtnMinusClicked (object sender, EventArgs e)
        {
            this.Log ().Info ("Trying to remove a control");
            if (numberControls > 1) {
                Gtk.Widget[] newArray = new Gtk.Widget[numberControls-1];
                for (int i = 0; i<numberControls-1; i++) {
                    newArray [i] = arrayWidgets [i];
                }
                arrayWidgets = newArray;
                numberControls--;

                RefreshControl ();
            } else {
                // i'm sorry dave but i cannot let you do this.
                this.Log ().Warn ("Min number of control reached");
            }
        }

        protected void OnBtnPlusClicked (object sender, EventArgs e)
        {
            this.Log ().Info ("Trying to add a control");
            if (numberControls < maxControls) {
                Gtk.Widget[] newArray = new Gtk.Widget[numberControls + 1];
                for (int i = 0; i<numberControls; i++) {
                    newArray [i] = arrayWidgets [i];
                }
                numberControls++;
                arrayWidgets = newArray;
                Gtk.Widget newControl = CreateCtrl();
                arrayWidgets [numberControls - 1] = newControl;

                RefreshControl ();
            } else {
                // i'm sorry dave but i cannot let you do this.
                this.Log ().Warn ("Max number of controls reached");
            }
        }
        #endregion
    }
}