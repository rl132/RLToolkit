using System;
using System.Drawing;

using RLToolkit;

// TODO: RL
//  - add event for when a new control is added/removed
//  - add event for when max/min are reached

namespace RLToolkit.Widgets
{
    [System.ComponentModel.ToolboxItem(true)]
    public partial class DynamicColumn : Gtk.Bin
    {
        private int numberControls = 1;
        private System.Type controlType;
        private Gtk.Widget[] arrayWidgets;
        private int maxControls = 8;

        #region Constructors
        public DynamicColumn ()
        {
            this.Log ().Debug ("Empty Constructor for the DynamicColumn");
            this.Build ();

            // init with a generic empty label widget
            SetControlType(typeof(Gtk.Label));
        }

        public DynamicColumn (System.Type controlType)
        {
            this.Log ().Debug ("Constructor for the DynamicColumn using " + controlType.Name);
            this.Build ();

            SetControlType(controlType);
        }
        #endregion

        #region public methods
        public void SetControlType(System.Type controlType)
        {
            this.Log().Debug("Setting type to: " + controlType.Name);
            this.controlType = controlType;

            arrayWidgets = new Gtk.Widget[1];
            Gtk.Widget newWidget = CreateCtrl();

            arrayWidgets [0] = newWidget;

            RefreshControl ();
        }

        public Gtk.Widget[] GetControlArray()
        {
            this.Log().Debug("Fetching the control array");
            return arrayWidgets;
        }

        public void SetControlArray(Gtk.Widget[] newArray)
        {
            this.Log().Debug("updating the control array");
            arrayWidgets = newArray;

            RefreshControl();
        }

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
            }

            // update the max
            maxControls = newCount;
        }
        #endregion

        #region Helper methods
        private Gtk.Widget CreateCtrl()
        {
            this.Log().Debug("Creating a new control");
            return (Gtk.Widget)(Activator.CreateInstance(controlType));
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