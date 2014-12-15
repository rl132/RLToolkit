using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Linq;

using RLToolkit;
using RLToolkit.Basic;
using NUnit.Framework;
using System.Linq;

namespace RLToolkit.UnitTests.Modules
{
	[TestFixture]
	public class XmlHelperTest : TestHarness, ITestBase
	{
		#region Local Variables
        // paths and filename
        private string file_1 = "fileXml1.xml";
        private string file_1_out = "fileXml1_out.xml";

        // documents XML
        private XmlDocument docFile1;
		#endregion

		#region Interface Override
		public string ModuleName()
		{
			return "XmlHelper";
		}

		public override void SetFolderPaths()
		{
			localFolder = AppDomain.CurrentDomain.BaseDirectory;
			SetPaths (localFolder, ModuleName());
		}

		public override void DataPrepare()
		{
            AddInputFile(Path.Combine(folder_testdata, file_1), true, false);

            // make a reference xml file for docFile1
            String xmlString =
                @"<?xml version=""1.0"" encoding=""utf-8"" standalone=""no""?>
                    <!--Comment!-->
                    <root>
                      <node attribute1=""test"" attribute2=""foo"">
                        <subnode attribute1=""foobar"">text 1</subnode>
                        <subnode>text 2</subnode>
                      </node>
                      <node>
                        <subnode>text 3</subnode>
                        <submode />
                      </node>
                    </root>";
              
            docFile1 = new XmlDocument();
            docFile1.LoadXml(xmlString);
        }

		public override void DataCleanup()
		{
            // move the test output to the result folder
            AddOutputFile(Path.Combine(localFolder, file_1_out), false);
        }
		#endregion

        #region Helper Functions
        private bool FileCompare(string path1, string path2)
        {
            return File.ReadLines(path1).SequenceEqual(File.ReadLines(path2));
        }
        #endregion

		#region Tests
        [Test]
        public void Xml_Read()
        {
            XmlDocument read = XmlHelper.Read(file_1, localFolder);

            // compare read with the ref docFile1
            Assert.AreEqual(docFile1.OuterXml, read.OuterXml, "the file read is not as expected.");
        }

        [Test]
        public void Xml_Write()
        {
            XmlHelper.Write(docFile1, file_1_out, localFolder);

            // compare write with the real File1
            Assert.IsTrue(FileCompare(Path.Combine(localFolder, file_1_out), Path.Combine(localFolder, file_1)), "Files are not the same");
        }
        #endregion
    }
}