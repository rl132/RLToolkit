using System;
using System.Collections.Generic;

namespace RLToolkit.Basic
{
    public class TextConfigSystem : IConfigSystem
    {
        public Dictionary<string, string> readConfig(string path)
        {
            this.Log().Debug("Reading configuration from: " + path);
            Dictionary<string, string> dic = new Dictionary<string, string>();

            FileHandler reader = new FileHandler(path, false);
            List<string> input = reader.ReadLines();

            foreach (string s in input)
            {
                int indexEqual = s.IndexOf('=');
                if (indexEqual != -1)
                {
                    this.Log().Info(string.Format("Adding configuration line: {0} = {1}", s.Substring(0, indexEqual), s.Substring(indexEqual +1)));
                    dic.Add(s.Substring(0, indexEqual), s.Substring(indexEqual +1));
                }
                else
                {
                    //invalid format. skipping
                    this.Log().Warn("invalid format for line: " + s);
                }
            }

            return dic;
        }

        public void writeConfig(Dictionary<string, string> content, string path)
        {
            this.Log().Debug("Writing configuration to: " + path + Environment.NewLine + "with " + content.Count.ToString() + " lines of data");
            // build the output
            List<string> output = new List<string>();
            foreach (KeyValuePair<string,string> pair in content)
            {
                output.Add(pair.Key + "=" + pair.Value);
            }

            // write
            FileHandler writer = new FileHandler(path,true);
            writer.WriteLines(output);
            writer.CloseStream();
        }
    }
}

