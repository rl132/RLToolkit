using System;

namespace RLToolkit.Basic
{
	public class XMLHandler
	{
		public XMLHandler (string filename) : this(filename, AppDomain.CurrentDomain.BaseDirectory, false) {}
		public XMLHandler (string filename, string folder) : this(filename, folder, false) {}
		public XMLHandler (string filename, bool forWrite) : this(filename, AppDomain.CurrentDomain.BaseDirectory, forWrite) {}
		
		public XMLHandler (string filename, string folder, bool forWrite)
		{ // exhaustive constructor

		}

	}
}

