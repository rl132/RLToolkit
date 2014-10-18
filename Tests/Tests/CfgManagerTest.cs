using System;
using System.Collections.Generic;
using System.IO;

using RLToolkit;
using RLToolkit.Basic;
using NUnit.Framework;

namespace RLToolkit.Tests
{
    [TestFixture]
    public class CfgManagerTest : TestHarness, ITestBase
    {
        #region Local Variables
        private CfgManager configManager;
        private string localFolder = ""; // to be initialized later

        // paths and filename
        private string file_txt_1 = "file1.txt";
        private string file_txt_1_out = "file1_out.txt";
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
            CopyFile(Path.Combine(folder_testdata, file_txt_1), Path.Combine(localFolder, file_txt_1), true, false);

        }

        public override void DataCleanup()
        {
            // move the result files if there
            MoveFile (Path.Combine (localFolder, file_txt_1_out), Path.Combine (folder_testresult, file_txt_1_out), false);

            // cleanup the testdata
            CleanFile (Path.Combine (localFolder, file_txt_1), false);
        }
        #endregion

        #region Basic
        [Test]
        public void CfgMgr_Null()
        {
            string path = Path.Combine(localFolder, file_txt_1);
            NullConfigSystem cfgsys = new NullConfigSystem();
            configManager = new CfgManager(path, cfgsys);
            configManager.ReadConfig();

            Assert.AreEqual(0, configManager.GetDictionary().Count, "Dictionary Size should be 0");
        }

        [Test]
        public void CfgMgr_Text()
        {
            string path = Path.Combine(localFolder, file_txt_1);
            TextConfigSystem cfgsys = new TextConfigSystem();
            configManager = new CfgManager(path, cfgsys);
            configManager.ReadConfig();

            Assert.AreEqual(5, configManager.GetDictionary().Count, "Dictionary Size should be 5");
            // todo: add checks for values

        }
        #endregion
    }
}

