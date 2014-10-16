using System;
using System.Xml;
using System.IO;

namespace RLToolkit.Basic
{
	public static class XmlHelper
	{
        public static void Write(XmlDocument doc, string filename, string folder)
        {
            LogManager.Instance.Log().Debug(string.Format("About to write XML file {0} in folder {1}", filename, folder));
            XmlWriter writer;
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            string fullPath = Path.Combine(folder,filename);

            if (File.Exists(fullPath))
            {
                LogManager.Instance.Log().Info("XML File already existed. About to delete");
                File.Delete(fullPath);
            }

            try
            {
                writer = XmlWriter.Create(fullPath, settings);
                doc.WriteTo(writer);
                writer.Close();
            }
            catch (Exception e)
            {
                throw new XMLHandlerException("Something went wrong while writing the XML file." + Environment.NewLine + e.Data);
            }
        }

        public static XmlDocument Read(string filename, string folder)
        {
            LogManager.Instance.Log().Debug(string.Format("About to read XML file {0} in folder {1}", filename, folder));
            string fullPath = Path.Combine(folder,filename);

            if (!File.Exists(fullPath))
            {
                throw new FileNotFoundException(String.Format ("File: \"{0}\" doesn't exist."), fullPath);
            }

            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(fullPath);
            }
            catch (Exception e)
            {
                throw new XMLHandlerException("Something went wrong while reading the XML file." + Environment.NewLine + e.Data);
            }
            return doc;
        }

        public static XmlNodeList GetChildNodes(XmlElement parent)
        {
            LogManager.Instance.Log().Debug(string.Format("Fetching child nodes"));
            return parent.ChildNodes;
        }
	}
}