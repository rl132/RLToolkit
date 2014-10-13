using System;
using System.IO;
using System.Xml;

namespace RLToolkit.Basic
{
	public class XMLHandlerWriter
	{
		private string fullPath;
		private XmlWriter writer;
		private XmlWriterSettings settings;
		public XMLHandlerWriter (string filename, string folder)
		{
			fullPath = Path.Combine(folder,filename);

			if (File.Exists(fullPath))
			{
				// todo: global config -> keep backups?

				// delete the old file
				File.Delete(fullPath);
			}

			// initialize
			settings = new XmlWriterSettings();
			settings.Indent = true;
			writer = XmlWriter.Create(fullPath, settings);
		}

		public void writeFile(XmlDocument input) // todo: fix the return value/params
		{
			if (writer == null)
			{
				throw new XMLHandlerException("XML File not initialized.");
			}

			// todo: write
			input.WriteTo(writer);
		}

		public void writeElement()
		{

		}
	}
}

