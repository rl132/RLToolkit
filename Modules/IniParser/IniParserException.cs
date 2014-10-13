using System;

namespace RLToolkit.Basic
{
	public class IniParserException : Exception
	{
		public IniParserException () : base() {}
		public IniParserException (string s1) : base(s1) {}
	}
}