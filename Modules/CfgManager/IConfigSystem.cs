using System;
using System.Collections.Generic;

namespace RLToolkit.Basic
{
    public interface IConfigSystem
    {
        Dictionary<string, string> readConfig(string path);
        void writeConfig(Dictionary<string, string> content, string path);
    }
}

