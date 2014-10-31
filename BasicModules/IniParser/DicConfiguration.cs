using System;
using System.Collections.Generic;

namespace RLToolkit.Basic
{
    /// <summary>
    /// Dictionary configuration used for the IniParser.
    /// </summary>
	public class DicConfiguration
	{
        /// <summary>The header identifier</summary>
		public string header;

        /// <summary>The dictionary that holds the data for this header</summary>
		public Dictionary<string, string> dicto;

        /// <summary>
        /// Constructor for an empty DictionaryConfiguration
        /// </summary>
        public DicConfiguration () : this("") {}

        /// <summary>
        /// Constructor for a DictionaryConfiguration that has a Header
        /// </summary>
        /// <param name="headerIn">Header input.</param>
        public DicConfiguration(string headerIn)
        {
            this.Log().Debug(string.Format("Creating a new DictionaryConfiguration with header: {0}", headerIn));
            header = headerIn;
            dicto = new Dictionary<string, string>();
        }
	}
}