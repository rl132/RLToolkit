using System;
using System.Collections.Generic;

namespace RLToolkit.Basic
{
    public class NullConfigSystem : IConfigSystem
    {
        /// <summary>
        /// NullConfigSystem constructor, just to inform the user that a NullConfigSystem is in use
        /// </summary>
        public NullConfigSystem()
        {
            // empty handler. warn
            this.Log().Warn("Null Configuration system used for Configuration manager. No value can be read or writen with this.");
        }

        /// <summary>
        /// Method to define that will handle the reading of the config system
        /// </summary>
        /// <returns>The config dictionary</returns>
        /// <param name="path">The full path of the input file</param>
        public Dictionary<string, string> readConfig(string path)
        {
            // return an empty dictionary
            return new Dictionary<string, string>();
        }

        /// <summary>
        /// Method to define that will handle the writing of the config system
        /// </summary>
        /// <param name="content">The config dictionary</param>
        /// <param name="path">The full path of the output file</param>
        public void writeConfig(Dictionary<string, string> content, string path)
        {
            // don't do anything
        }
    }
}

