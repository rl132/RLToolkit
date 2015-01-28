using System;

using RLToolkit.Logger;

namespace RLToolkit.Extensions
{
    /// <summary>
    /// Class that will add a 'single-only' functionality to the Gtk.ToggleButton
    /// </summary>
    public class ToggleButtonExclusiveSelectionGroup
    {
        private Gtk.ToggleButton[] controlList = new Gtk.ToggleButton[0];
        private int selected = -1;

        /// <summary>
        /// Method to append a ToggleButton in the control array
        /// </summary>
        /// <param name="input">the ToggleButton to add</param>
        /// <returns>true if the button has been added</returns>
        public bool Append(Gtk.ToggleButton input)
        {
            if (FindInGroup(input) != -1)
            {
                // already exist
                this.Log().Warn("Cannot add the same control twice. Aborting");
                return false;
            }

            // add to the list
            this.Log().Debug("Adding a control to the control list");
            Gtk.ToggleButton[] newArray = new Gtk.ToggleButton[controlList.Length+1];
            for (int i = 0; i<controlList.Length; i++)
            {
                newArray[i] = controlList[i];
            }
            newArray[controlList.Length] = input;

            // replace the list with the new one
            controlList = newArray;

            return true;
        }

        /// <summary>
        /// Method to remove a control from the control Array, using the index in the array
        /// </summary>
        /// <param name="index">Index to remove</param>
        /// <returns>True if the button has been removed</returns>
        /// <remarks>Will unselect the current selection. (to prevent keeping ghost selection)</remarks>
        public bool Remove(int index)
        {
            // validation
            if (index < 0)
            {
                this.Log().Warn("Index to remove is outside of bound (negative), check for errors.");
                return false;
            }

            if (index >= controlList.Length)
            {
                this.Log().Warn("Index to remove is outside of bound (positive), check for errors.");
                return false;
            }

            // unselect everything
            Unselect();

            // removal
            this.Log().Debug("Trying to remove the control");
            Gtk.ToggleButton[] newArray = new Gtk.ToggleButton[controlList.Length-1];
            for (int i = 0; i<index; i++)
            {
                newArray[i] = controlList[i];
            }
            for (int i = (index+1); i<controlList.Length; i++)
            {
                newArray[i-1] = controlList[i];
            }

            // swap the list
            this.Log().Debug("Swapping the list with the new one");
            controlList = newArray;
            return true;
        }

        /// <summary>
        /// Method that removes a control from the control array, using the control itself
        /// </summary>
        /// <param name="input">The control to remove</param>
        /// <returns>True if the control has been removed</returns>
        /// <remarks>Will unselect the current selection. (to prevent keeping ghost selection)</remarks>
        public bool Remove(Gtk.ToggleButton input)
        {
            int index = FindInGroup(input);
            if (index == -1)
            {
                this.Log().Info("Trying to remove a control that doesn't exist.");
                return false;
            }

            // use the real select method
            return Remove(index);
        }

        /// <summary>
        /// Method to remove all the controls from the control array
        /// </summary>
        /// <remarks>Will unselect the current selection. (to prevent keeping ghost selection)</remarks>
        public void RemoveAll()
        {
            this.Log().Debug("Removed all controls from the list.");
            Unselect();
            controlList = new Gtk.ToggleButton[0];
        }

        /// <summary>
        /// Method to toggle a specific control using it index.
        /// </summary>
        /// <param name="input">The index of the control in the array to turn on.</param>
        /// <returns>True, if the selection is successful</returns>
        /// <remarks>Will unselect the current selection if there is.</remarks>
        public bool Select(int index)
        {
            if (selected == index)
            {
                this.Log().Debug("Trying to select the same index.");
                return false;
            }

            // unselect the old selection only if something is selected
            if (selected > -1)
            {
                if (!SetSelection(selected, false))
                {
                    this.Log().Debug("An Error occured while trying to deselect the previously selected button.");
                    return false;
                }
            }

            // select the new one
            selected = index;
            if (!SetSelection(index, true))
            {
                this.Log().Debug("An Erro occured while trying to select the new button.");
                return false;
            }

            // everything went well
            return true;
        }

        /// <summary>
        /// Method to toggle a specific control.
        /// </summary>
        /// <param name="input">The control to Toggle 'ON'</param>
        /// <returns>True, if the selection is successful</returns>
        /// <remarks>Will unselect the current selection if there is.</remarks>
        public bool Select(Gtk.ToggleButton input)
        {
            int index = FindInGroup(input);
            if (index == -1)
            {
                this.Log().Info("Trying to select a control that doesn't exist.");
                return false;
            }

            // use the real select method
            return Select(index);
        }

        public bool Unselect()
        {
            if (selected == -1)
            {
                this.Log().Debug("Trying to deselect while nothing selected.");
                return false;
            }

            // unselecting
            this.Log().Debug("Deselecting all buttons.");
            SetSelection(selected, false);
            selected = -1;

            return true;
        }

        /// <summary>
        /// Method to return the number of button in the control array.
        /// </summary>
        /// <returns>The count of controls</returns>
        public int GetCountButton()
        {
            return controlList.Length;
        }

        /// <summary>
        /// Method that will return the index of a control if found, in the Control Array
        /// </summary>
        /// <returns>The index of the control in the group, 0-based,  -1 if not found.</returns>
        /// <param name="input">The control to find.</param>
        public int FindInGroup(Gtk.ToggleButton input)
        {
            this.Log().Debug("Trying to find input: " + input.Name);
            for (int i = 0; i<controlList.Length; i++)
            {
                if (input == controlList[i])
                {
                    this.Log().Debug("Control found.");
                    return i;
                }
            }

            this.Log().Debug("Control not found.");
            return -1;
        }

        private bool SetSelection(int index, bool state)
        {
            if (index < 0)
            {
                this.Log().Warn("Index is outside of bound (negative), check for errors.");
                return false;
            }

            if (index >= controlList.Length)
            {
                this.Log().Warn("Index is outside of bound (positive), check for errors.");
                return false;
            }

            this.Log().Debug("Setting control state to " + state.ToString());
            controlList[index].Active = state;
            return true;
        }
    }
}

