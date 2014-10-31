using System;

namespace RLToolkit.Basic
{
	public class FileHandlerException : Exception
	{
        /// <summary>
        /// FileHandler empty exception
        /// </summary>
		public FileHandlerException () : base() {}

        /// <summary>
        /// FileHandler exception that supply a message
        /// </summary>
        public FileHandlerException (string s1) : base(s1) {}
	}
}