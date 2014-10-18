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

        public Dictionary<string, string> GetDictionary()
        {
            return content;
        }
    }
}

