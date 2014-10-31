using System;

namespace RLToolkit.Basic
{
    /// <summary>
    /// Custom XmlHandler exception.
    /// </summary>
	public class XMLHandlerException : Exception
	{
        /// <summary>
        /// XmlHandler empty exception
        /// </summary>
		public XMLHandlerException () : base() {}

        /// <summary>
        /// XmlHandler exception that supply a message
        /// </summary>
		public XMLHandlerException (string s1) : base(s1) {}
	}
}

