using System;
using System.Collections.Generic;

namespace RLToolkit.Basic
{
    public class TextConfigSystem : IConfigSystem
    {
        public Dictionary<string, string> readConfig(string path)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();

            FileHandler reader = new FileHandler(path, false);
            List<string> input = reader.ReadLines();

            foreach (string s in input)
            {
                string[] splitData = s.Split("=".ToCharArray(0,1));
                if (splitData.GetLength(0) == 2)
                {
                    this.Log().Warn("Adding configuration line: " + s);
                    dic.Add(splitData[0], splitData[1]);
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

