using System;
using System.IO;
using System.Xml;

namespace RLToolkit.Basic
{
	public class XMLHandlerReader
	{
		private string fullPath;
		private XmlDocument reader;
		public XMLHandlerReader (string filename, string folder)
		{
			fullPath = Path.Combine(folder,filename);

			if (!File.Exists(fullPath))
			{
				throw new FileNotFoundException(String.Format ("File: \"{0}\" doesn't exist."), fullPath);
			}

            reader = new XmlDocument();
            reader.Load(fullPath);
		}

        public XmlNodeList GetChildNodes(XmlElement parent)
        {
            return parent.ChildNodes;
        }
    
    }
}

