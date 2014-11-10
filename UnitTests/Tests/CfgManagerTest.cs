using System;
using System.Collections.Generic;
using System.IO;

using RLToolkit;
using RLToolkit.Basic;
using NUnit.Framework;
using System.Linq;

namespace RLToolkit.UnitTests
{
    [TestFixture]
    public class CfgManagerTest : TestHarness, ITestBase
    {
        #region Local Variables
        private CfgManager configManager;

        // paths and filename
        private string file_txt_1 = "file1.txt";
        private string file_txt_1_out = "file1_out.txt";
        private string file_txt_1_notExist = "file1_bar.txt";
        private string file_ini_1 = "file1.ini";
        private string file_ini_1_out = "file1_out.ini";
        private string file_ini_1_notExist = "file1_bar.ini";
        private string file_xml_1 = "file1.xml";
        private string file_xml_1_out = "file1_out.xml";
        private string file_xml_1_notExist = "file1_bar.xml";
        private string file_1_empty = "file1.abc";

        // dictionaries
        private Dictionary<string, string> dicoTxt = new Dictionary<string, string>();
        private Dictionary<string, string> dicoIni = new Dictionary<string, string>();
        private Dictionary<string, string> dicoXml = new Dictionary<string, string>();
        #endregion

        #region Interface Override
        public string ModuleName()
        {
            return "CfgManager";
        }

        public override void SetFolderPaths()
        {
            localFolder = AppDomain.CurrentDomain.BaseDirectory;
            SetPaths (localFolder, ModuleName());
        }

        public override void DataPrepare()
        {
            // copy the data locally
            AddInputFile(Path.Combine(folder_testdata, file_txt_1), true, false);
            AddInputFile(Path.Combine(folder_testdata, file_ini_1), true, false);
            AddInputFile(Path.Combine(folder_testdata, file_xml_1), true, false);

            // fill in the dictionary
            dicoTxt.Add("foo", "bar");
            dicoTxt.Add("123", "abc");
            dicoTxt.Add("long", "string with more data");
            dicoTxt.Add("multiple", "equal sign=is it working?");
            dicoTxt.Add("bar", "");

            dicoIni.Add("general_foo", "bar");
            dicoIni.Add("general_123", "abc");
            dicoIni.Add("general_long", "string with more data");
            dicoIni.Add("general_bar", "");
            dicoIni.Add("specific_foo", "blerp");

            dicoXml.Add("test", "value");
            dicoXml.Add("long", "string with spaces");
            dicoXml.Add("multiple", "equals signs = working?");
            dicoXml.Add("bar", "");
            dicoXml.Add("useless", "value");
        }

        public override void DataCleanup()
        {
            // move the result files if there's any
            AddOutputFile(Path.Combine(localFolder, file_txt_1_out), false);
            AddOutputFile(Path.Combine(localFolder, file_ini_1_out), false);
            AddOutputFile(Path.Combine(localFolder, file_xml_1_out), false);
        }
        #endregion

        #region Helper Functions
        private bool FileCompare(string path1, string path2)
        {
            return File.ReadLines(path1).SequenceEqual(File.ReadLines(path2));
        }
        #endregion

        #region null handler
        [Test]
        public void CfgMgr_Null_CfgSysType()
        {
            string path = Path.Combine(localFolder, file_txt_1);
            configManager = new CfgManager(path, typeof(NullConfigSystem));
            configManager.ReadConfig();

            Assert.AreEqual(0, configManager.GetDictionary().Count, "Dictionary Size should be 0");
        }

        [Test]
        public void CfgMgr_Null_CfgSysInstance()
        {
            string path = Path.Combine(localFolder, file_txt_1);
            NullConfigSystem cfgsys = new NullConfigSystem();
            configManager = new CfgManager(path, cfgsys);
            configManager.ReadConfig();

            Assert.AreEqual(0, configManager.GetDictionary().Count, "Dictionary Size should be 0");
        }
        #endregion

        #region text handler
        [Test]
        public void CfgMgr_Text_Read_CfgSysType()
        {
            string path = Path.Combine(localFolder, file_txt_1);
            configManager = new CfgManager(path, typeof(TextConfigSystem));
            configManager.ReadConfig();

            Assert.AreEqual(5, configManager.GetDictionary().Count, "Dictionary Size should be 5");
            string value1 = configManager.GetValue("long");
            Assert.AreEqual("string with more data", value1, "Content of \"long\" should be \"string with more data\"");
            string value2 = configManager.GetValue("foo");
            Assert.AreEqual("bar", value2, "Content for \"foo\" should be \"bar\"");
            string value3 = configManager.GetValue("bar");
            Assert.AreEqual("", value3, "Content for \"bar\" should be empty");
        }

        [Test]
        public void CfgMgr_Text_Read_CfgSysInstance()
        {
            string path = Path.Combine(localFolder, file_txt_1);
            TextConfigSystem cfgsys = new TextConfigSystem();
            configManager = new CfgManager(path, cfgsys);
            configManager.ReadConfig();

            Assert.AreEqual(5, configManager.GetDictionary().Count, "Dictionary Size should be 5");
            string value1 = configManager.GetValue("long");
            Assert.AreEqual("string with more data", value1, "Content of \"long\" should be \"string with more data\"");
            string value2 = configManager.GetValue("foo");
            Assert.AreEqual("bar", value2, "Content for \"foo\" should be \"bar\"");
            string value3 = configManager.GetValue("bar");
            Assert.AreEqual("", value3, "Content for \"bar\" should be empty");
        }

        [Test]
        public void CfgMgr_Text_Write()
        {
            string path = Path.Combine(localFolder, file_txt_1_out);
            TextConfigSystem cfgsys = new TextConfigSystem();
            configManager = new CfgManager(path, cfgsys);
            configManager.SetDictionary(dicoTxt);
            configManager.WriteConfig();

            Assert.IsTrue(FileCompare(Path.Combine(localFolder, file_txt_1_out), Path.Combine(localFolder, file_txt_1)), "Files are not the same");
        }
        
        [Test]
        public void CfgMgr_Text_Read_NotExist()
        {
            string path = Path.Combine(localFolder, file_txt_1_notExist);
            TextConfigSystem cfgsys = new TextConfigSystem();
            configManager = new CfgManager(path, cfgsys);
            configManager.ReadConfig();

            Assert.AreEqual(0, configManager.GetDictionary().Count, "Dictionary should be empty");
        }
        
        [Test]
        public void CfgMgr_Text_Read_Empty()
        {
            string path = Path.Combine(localFolder, file_1_empty);
            TextConfigSystem cfgsys = new TextConfigSystem();
            configManager = new CfgManager(path, cfgsys);
            configManager.ReadConfig();

            Assert.AreEqual(0, configManager.GetDictionary().Count, "Dictionary should be empty");
        }
        #endregion

        #region ini handler
        [Test]
        public void CfgMgr_Ini_Read_CfgSysType()
        {
            string path = Path.Combine(localFolder, file_ini_1);
            configManager = new CfgManager(path, typeof(IniConfigSystem));
            configManager.ReadConfig();

            Assert.AreEqual(5, configManager.GetDictionary().Count, "Dictionary Size should be 5");
            string value1 = configManager.GetValue("general_long");
            Assert.AreEqual("string with more data", value1, "Content of \"general_long\" should be \"string with more data\"");
            string value2 = configManager.GetValue("general_foo");
            Assert.AreEqual("bar", value2, "Content for \"general_foo\" should be \"bar\"");
            string value3 = configManager.GetValue("general_bar");
            Assert.AreEqual("", value3, "Content for \"general_bar\" should be empty");
        }

        [Test]
        public void CfgMgr_Ini_Read_CfgSysInstance()
        {
            string path = Path.Combine(localFolder, file_ini_1);
            IniConfigSystem cfgsys = new IniConfigSystem();
            configManager = new CfgManager(path, cfgsys);
            configManager.ReadConfig();

            Assert.AreEqual(5, configManager.GetDictionary().Count, "Dictionary Size should be 5");
            string value1 = configManager.GetValue("general_long");
            Assert.AreEqual("string with more data", value1, "Content of \"general_long\" should be \"string with more data\"");
            string value2 = configManager.GetValue("general_foo");
            Assert.AreEqual("bar", value2, "Content for \"general_foo\" should be \"bar\"");
            string value3 = configManager.GetValue("general_bar");
            Assert.AreEqual("", value3, "Content for \"general_bar\" should be empty");
        }

        [Test]
        public void CfgMgr_Ini_Write()
        {
            string path = Path.Combine(localFolder, file_ini_1_out);
            IniConfigSystem cfgsys = new IniConfigSystem();
            configManager = new CfgManager(path, cfgsys);
            configManager.SetDictionary(dicoIni);
            configManager.WriteConfig();

            Assert.IsTrue(FileCompare(Path.Combine(localFolder, file_ini_1_out), Path.Combine(localFolder, file_ini_1)), "Files are not the same");
        }

        [Test]
        public void CfgMgr_Ini_Read_NotExist()
        {
            string path = Path.Combine(localFolder, file_ini_1_notExist);
            IniConfigSystem cfgsys = new IniConfigSystem();
            configManager = new CfgManager(path, cfgsys);
            configManager.ReadConfig();

            Assert.AreEqual(0, configManager.GetDictionary().Count, "Dictionary should be empty");
        }
        
        [Test]
        public void CfgMgr_Ini_Read_Empty()
        {
            string path = Path.Combine(localFolder, file_1_empty);
            IniConfigSystem cfgsys = new IniConfigSystem();
            configManager = new CfgManager(path, cfgsys);
            configManager.ReadConfig();

            Assert.AreEqual(0, configManager.GetDictionary().Count, "Dictionary should be empty");
        }
        #endregion

        #region xml handler
        [Test]
        public void CfgMgr_Xml_Read_CfgSysType()
        {
            string path = Path.Combine(localFolder, file_xml_1);
            configManager = new CfgManager(path, typeof(XmlConfigSystem));
            configManager.ReadConfig();

            Assert.AreEqual(5, configManager.GetDictionary().Count, "Dictionary Size should be 5");
            string value1 = configManager.GetValue("long");
            Assert.AreEqual("string with spaces", value1, "Content of \"long\" should be \"string with spaces\"");
            string value2 = configManager.GetValue("test");
            Assert.AreEqual("value", value2, "Content for \"test\" should be \"value\"");
            string value3 = configManager.GetValue("bar");
            Assert.AreEqual("", value3, "Content for \"bar\" should be empty");
        }

        [Test]
        public void CfgMgr_Xml_Read_CfgSysInstance()
        {
            string path = Path.Combine(localFolder, file_xml_1);
            XmlConfigSystem cfgsys = new XmlConfigSystem();
            configManager = new CfgManager(path, cfgsys);
            configManager.ReadConfig();

            Assert.AreEqual(5, configManager.GetDictionary().Count, "Dictionary Size should be 5");
            string value1 = configManager.GetValue("long");
            Assert.AreEqual("string with spaces", value1, "Content of \"long\" should be \"string with spaces\"");
            string value2 = configManager.GetValue("test");
            Assert.AreEqual("value", value2, "Content for \"test\" should be \"value\"");
            string value3 = configManager.GetValue("bar");
            Assert.AreEqual("", value3, "Content for \"bar\" should be empty");
        }

        [Test]
        public void CfgMgr_Xml_Write()
        {
            string path = Path.Combine(localFolder, file_xml_1_out);
            XmlConfigSystem cfgsys = new XmlConfigSystem();
            configManager = new CfgManager(path, cfgsys);
            configManager.SetDictionary(dicoXml);
            configManager.WriteConfig();

            Assert.IsTrue(FileCompare(Path.Combine(localFolder, file_xml_1_out), Path.Combine(localFolder, file_xml_1)), "Files are not the same");
        }

        [Test]
        public void CfgMgr_Xml_Read_NotExist()
        {
            string path = Path.Combine(localFolder, file_xml_1_notExist);
            XmlConfigSystem cfgsys = new XmlConfigSystem();
            configManager = new CfgManager(path, cfgsys);
            configManager.ReadConfig();

            Assert.AreEqual(0, configManager.GetDictionary().Count, "Dictionary should be empty");
        }
        
        [Test]
        public void CfgMgr_Xml_Read_Empty()
        {
            string path = Path.Combine(localFolder, file_1_empty);
            XmlConfigSystem cfgsys = new XmlConfigSystem();
            configManager = new CfgManager(path, cfgsys);
            configManager.ReadConfig();

            Assert.AreEqual(0, configManager.GetDictionary().Count, "Dictionary should be empty");
        }
        #endregion
    }
}