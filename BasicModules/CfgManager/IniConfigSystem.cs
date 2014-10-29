using System;
using System.Collections.Generic;

namespace RLToolkit.Basic
{
    public class IniConfigSystem : IConfigSystem
    {
        public Dictionary<string, string> readConfig(string path)
        {
            this.Log().Debug("Reading configuration from: " + path);
            Dictionary<string, string> dic = new Dictionary<string, string>();

            IniParser reader = new IniParser(path);
            List<DicConfiguration> input = reader.content;

            foreach (DicConfiguration d in input)
            {
                foreach (KeyValuePair<string, string> pair in d.dicto)
                {
                    this.Log().Info(string.Format("Adding configuration line: {0} _ {1} = {2}", d.header, pair.Key, pair.Value));
                    dic.Add(d.header + "_" + pair.Key, pair.Value);
                }
            }

            return dic;
        }

        public void writeConfig(Dictionary<string, string> content, string path)
        {
            this.Log().Debug("Writing configuration to: " + path + Environment.NewLine + "with " + content.Count.ToString() + " lines of data");
            // build the output

            List<string> output = new List<string>();
            string lastHeader = "";
            string header;
            foreach (KeyValuePair<string,string> pair in content)
            {
                int indexHeader = pair.Key.IndexOf('_');
                string varia;
                if (indexHeader != -1)
                {
                    header = pair.Key.Substring(0, indexHeader);
                    varia = pair.Key.Substring(indexHeader+1);
                }
                else
                {
                    // fallback solution
                    header = "gerneric";
                    varia = pair.Key;
                }

                if (header != lastHeader)
                {
                    // update the header
                    output.Add("[" + header + "]");
                    lastHeader = header;
                }

                output.Add(varia + "=" + pair.Value);
            }

            // write
            IniParser writer = new IniParser(output);
            writer.WriteContent(path);
        }
    }
}

