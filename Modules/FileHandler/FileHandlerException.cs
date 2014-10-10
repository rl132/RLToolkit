using System;

namespace RLToolkit
{
	public class FileHandlerException : Exception
	{
		public FileHandlerException () : base() {}
		public FileHandlerException (string s1) : base(s1) {}
	}
}

