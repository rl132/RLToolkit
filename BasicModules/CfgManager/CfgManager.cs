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

        // constructor using the config system type instead of an instance of it
        public CfgManager(string pathIn, System.Type configSystemType) : this
            (pathIn, (IConfigSystem)Activator.CreateInstance(configSystemType)) { }

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

        public void ReadConfig()
        {
            this.Log().Debug("Reading configuration");
            content = configSystem.readConfig(path);
        }

        public void WriteConfig()
        {
            this.Log().Debug("Writing configuration");
            configSystem.writeConfig(content, path);
        }

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

        public void RemoveVariable(string variable)
        {
            this.Log().Debug(string.Format("Trying to remove value for {0}", variable));
            if (content.ContainsKey(variable))
            {
                this.Log().Debug("Variable found. Removing.");
                content.Remove(variable);
            }
        }

        public Dictionary<string, string> GetDictionary()
        {
            this.Log().Debug("Fetching the dictionary");
            return content;
        }

        public void SetDictionary (Dictionary<string, string> input)
        {
            this.Log().Debug("Manually setting the dictionary");
            content = input;
        }
    }
}

