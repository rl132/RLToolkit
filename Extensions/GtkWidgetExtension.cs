using System;
using Gtk;

namespace RLToolkit.Extensions
{
    /// <summary>
    /// Class to extends the functionalities of the Gtk Widgets
    /// </summary>
    public static class GtkWidgetExtension
    {

        /// <summary>
        /// Find the index from a text in a Gtk.ComboBox
        /// </summary>
        /// <returns>The index, -1 if not found or invalid widget</returns>
        /// <param name="input">The input control to use to search.</param>
        /// <param name="textToFind">The text to find.</param>
        public static int FindIndex(this Gtk.ComboBox input, string textToFind)
        {
            if (input == null)
            {
                // control invalid
                return -1;
            }

            ListStore store = (ListStore)input.Model;

            int i = 0;
            foreach (object[] row in store) {
                if (row [0].ToString().ToLower() == textToFind.ToLower()) {
                    // return the index of the found item
                    return i;
                }
                i++;
            }

            // not found
            return -1;
        }
    }
}