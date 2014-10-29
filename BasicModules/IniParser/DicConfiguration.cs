using System;
using System.Collections.Generic;

namespace RLToolkit.Basic
{
	public class DicConfiguration
	{
		public string header;
		public Dictionary<string, string> dicto;

        public DicConfiguration () : this("") {}

        public DicConfiguration(string headerIn)
        {
            this.Log().Debug(string.Format("Creating a new DictionaryConfiguration with header: {0}", headerIn));
            header = headerIn;
            dicto = new Dictionary<string, string>();
        }
	}
}