using System;
using System.Collections.Generic;

namespace RLToolkit.Basic
{
    /// <summary>
    /// Interface IConfigSystem for the CfgManager
    /// </summary>
    public interface IConfigSystem
    {
        // In order to implement an IConfigSystem, the class only needs to
        // declare these two functions. There is no versionning system embedded 
        // in the interface but you are free to make your own in case your format 
        // may change.

        // The structure of the directory is very simple. The first string represent
        // the variable you want to save, while the second is it content. Also to
        // note, the path supplied will always be a full path and if it already exists
        // it will be deleted before the write is called to ensure there's no data 
        // from the previous save.

        /// <summary>
        /// Method to define that will handle the reading of the config system
        /// </summary>
        /// <returns>The config dictionary</returns>
        /// <param name="path">The full path of the input file</param>
        Dictionary<string, string> readConfig(string path);

        /// <summary>
        /// Method to define that will handle the writing of the config system
        /// </summary>
        /// <param name="content">The config dictionary</param>
        /// <param name="path">The full path of the output file</param>
        void writeConfig(Dictionary<string, string> content, string path);
    }
}

