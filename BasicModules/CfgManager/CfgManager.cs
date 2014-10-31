using System;
using System.IO;
using System.Collections.Generic;

namespace RLToolkit.Basic
{
    public class CfgManager
    {
        private IConfigSystem configSystem;
        private string path;
        private Dictionary<string, string> content;

        /// <summary>
        /// Constructor using the config system type
        /// </summary>
        /// <param name="pathIn"> Path to the input/output file</param>
        /// <param name="configSystemType"> the TypeOf of your desired configuration system.</param> 
        public CfgManager(string pathIn, System.Type configSystemType) : this
            (pathIn, (IConfigSystem)Activator.CreateInstance(configSystemType)) { }

        /// <summary>
        /// Constructor using the config system instance
        /// </summary>
        /// <param name="pathIn"> Path to the input/output file</param>
        /// <param name="configSystemIn"> An instance of your desired configuration system.</param> 
        public CfgManager(string pathIn, IConfigSystem configSystemIn)
        {
            this.Log().Debug(string.Format("Creating a configuration manager from file \"{0}\" with habdler of type: {1}", pathIn, configSystemIn.ToString()));
            if (!File.Exists(pathIn))
            {
                this.Log().Info("File requested for config doesn't exist.");
            }

            path = pathIn;
            configSystem = configSystemIn;
            content = new Dictionary<string, string>();
        }

        /// <summary>
        /// Method to read the configuration from the config system supplied--
        /// </summary>
        public void ReadConfig()
        {
            this.Log().Debug("Reading configuration");
            content = configSystem.readConfig(path);
        }

        /// <summary>
        /// Method to write the configuration to the config system supplied--
        /// </summary>
        public void WriteConfig()
        {
            this.Log().Debug("Writing configuration");
            configSystem.writeConfig(content, path);
        }

        /// <summary>
        /// Method to fetch the content of a variable from the Configuration
        /// </summary>
        /// <param name="variable">The variable you want to fetch the content for</param>
        /// <returns>the string value of the content</returns>
        public string GetValue(string variable)
        {
            this.Log().Debug(string.Format("Trying to get value for {0}", variable));
            if (content.Count == 0)
            {
                // don't even bother
                return "";
            }

            string output;
            content.TryGetValue(variable, out output);
            return output;
        }

        /// <summary>
        /// Method to set the content of a variable from the Configuration
        /// </summary>
        /// <param name="variable">The variable you want to set the content for</param>
        /// <param name="value">The content you want to set the supplied variable</param>
        public void SetValue(string variable, string value)
        {
            this.Log().Debug(string.Format("Trying to set value for {0} to {1}", variable, value));
            if (content.ContainsKey(variable))
            {
                // we exist already
                this.Log().Info(string.Format("variable {0} already existed. replacing.", variable));
                content.Remove(variable);
            }
            content.Add(variable, value);
        }

        /// <summary>
        /// Method to remove the content of a variable from the Configuration
        /// </summary>
        /// <param name="variable">The variable you want to delete</param>
        public void RemoveVariable(string variable)
        {
            this.Log().Debug(string.Format("Trying to remove value for {0}", variable));
            if (content.ContainsKey(variable))
            {
                this.Log().Debug("Variable found. Removing.");
                content.Remove(variable);
            }
        }

        /// <summary>
        /// Method to fetch the entire Dictionary. Mostly for debugging purposes.
        /// </summary>
        /// <returns>The dictionary.</returns>
        public Dictionary<string, string> GetDictionary()
        {
            this.Log().Debug("Fetching the dictionary");
            return content;
        }

        /// <summary>
        /// Method to overrid the entire dictionary. Mostly for debugging purposes.
        /// </summary>
        /// <param name="input">The new Dictionary to use</param>
        public void SetDictionary (Dictionary<string, string> input)
        {
            this.Log().Debug("Manually setting the dictionary");
            content = input;
        }
    }
}

