using System;
using System.Collections.Generic;

namespace RLToolkit.Basic
{
    public class NullConfigSystem : IConfigSystem
    {
        public NullConfigSystem()
        {
            // empty handler. warn
            this.Log().Warn("Null Configuration system used for Configuration manager. No value can be read or writen with this.");
        }

        public Dictionary<string, string> readConfig(string path)
        {
            // return an emptu dictionary
            return new Dictionary<string, string>();
        }

        public void writeConfig(Dictionary<string, string> content, string path)
        {
            // don't do anything
        }
    }
}

