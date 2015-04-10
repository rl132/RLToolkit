using System;
using System.Collections.Generic;

using RLToolkit;
using RLToolkit.Basic;
using NUnit.Framework;

// todo: RL:
// -Fix the windows version of the tests
// -replace the cmdrunner dummy file with something legit for envvarsearch
using System.IO;

namespace RLToolkit.UnitTests.Modules
{
    [TestFixture]
    public class EnvVarSearchTest : TestHarness, ITestBase
    {
        #region Interface Override
        public string ModuleName()
        {
            return "EnvVarSearch";
        }

        public override void SetFolderPaths()
        {
            localFolder = AppDomain.CurrentDomain.BaseDirectory;
            SetPaths (localFolder, ModuleName());
        }

        public override void DataPrepare()
        {
            // copy the data locally
            AddInputFile(Path.Combine(folder_testdata, "EnvVarSearch_File.txt"), true, false);
        }
        #endregion

        #region Basic
        [Test]
        public void EnvVarSearch_GetDictionary()
        {
            EnvVarSearch search = new EnvVarSearch ();
            Assert.Greater(search.GetDictionary().Count, 0, "Dictionary Size should be greater than 0");
        }

        [Test]
        public void EnvVarSearch_FindInPath_Valid()
        {
            Dictionary<string, string> setup = new Dictionary<string, string>();
            OsDetector.OsSelection os = OsDetector.DetectOs();
            bool isFound = false;
            if (os == OsDetector.OsSelection.Unix)
            {
                setup.Add("PATH", "/usr/local/sbin:/usr/local/bin:/usr/sbin:/usr/bin:/sbin:/bin");
                EnvVarSearch search = new EnvVarSearch(setup);
                isFound = search.IsPathInEnv("/usr/bin");
            } else if (os == OsDetector.OsSelection.Windows)
            {
                Assert.Fail("FixMe - FindInPath - Valid - Windows");
                setup.Add("PATH", "FIXME");
                EnvVarSearch search = new EnvVarSearch(setup);
                isFound = search.IsPathInEnv("");
            } else
            {
                Assert.Fail("Bad platform");
            }
            Assert.IsTrue(isFound, "The path should have been found");
        }

        [Test]
        public void EnvVarSearch_FindInPath_NotFound()
        {
            Dictionary<string, string> setup = new Dictionary<string, string>();
            OsDetector.OsSelection os = OsDetector.DetectOs();
            bool isFound = false;
            if (os == OsDetector.OsSelection.Unix)
            {
                setup.Add("PATH", "/usr/local/sbin:/usr/local/bin:/usr/sbin:/usr/bin:/sbin:/bin");
                EnvVarSearch search = new EnvVarSearch(setup);
                isFound = search.IsPathInEnv("/home/test/folder");
            } else if (os == OsDetector.OsSelection.Windows)
            {
                Assert.Fail("FixMe - FindInPath - not found - Windows");
                setup.Add("PATH", "FIXME");
                EnvVarSearch search = new EnvVarSearch(setup);
                isFound = search.IsPathInEnv("");
            } else
            {
                Assert.Fail("Bad platform");
            }
            Assert.IsFalse(isFound, "The path should not have been found");
        }

        [Test]
        public void EnvVarSearch_FindInPath_TrailingBackslash()
        {
            Dictionary<string, string> setup = new Dictionary<string, string>();
            OsDetector.OsSelection os = OsDetector.DetectOs();
            bool isFound = false;
            if (os == OsDetector.OsSelection.Unix)
            {
                setup.Add("PATH", "/usr/local/sbin:/usr/local/bin:/usr/sbin:/usr/bin:/sbin:/bin");
                EnvVarSearch search = new EnvVarSearch(setup);
                isFound = search.IsPathInEnv("/usr/bin/");
            } else if (os == OsDetector.OsSelection.Windows)
            {
                Assert.Fail("FixMe - FindInPath - trailing backslash - Windows");
                setup.Add("PATH", "FIXME");
                EnvVarSearch search = new EnvVarSearch(setup);
                isFound = search.IsPathInEnv("");
            } else
            {
                Assert.Fail("Bad platform");
            }
            Assert.IsTrue(isFound, "The path should have been found");
        }

        [Test]
        public void EnvVarSearch_FindInPath_Valid_File()
        {
            Dictionary<string, string> setup = new Dictionary<string, string>();
            OsDetector.OsSelection os = OsDetector.DetectOs();
            string retval = null;
            if (os == OsDetector.OsSelection.Unix)
            {
                setup.Add("PATH", "/usr/local/bin:/usr/bin:" + AppDomain.CurrentDomain.BaseDirectory);
                EnvVarSearch search = new EnvVarSearch(setup);
                retval = search.FindInPath("EnvVarSearch_File.txt");
            } else if (os == OsDetector.OsSelection.Windows)
            {
                Assert.Fail("FixMe - FindInPath - Valid - Windows");

            } else
            {
                Assert.Fail("Bad platform");
            }
            Assert.NotNull(retval, "The path should have been found");
        }

        [Test]
        public void EnvVarSearch_FindInPath_Valid_Folder()
        {
            Dictionary<string, string> setup = new Dictionary<string, string>();
            OsDetector.OsSelection os = OsDetector.DetectOs();
            string retval = null;
            if (os == OsDetector.OsSelection.Unix)
            {
                setup.Add("PATH", "/usr/local/bin:/usr/bin:" + AppDomain.CurrentDomain.BaseDirectory);
                EnvVarSearch search = new EnvVarSearch(setup);
                retval = search.FindInPath(@"TestData/");
            } else if (os == OsDetector.OsSelection.Windows)
            {
                Assert.Fail("FixMe - FindInPath - Valid - Windows");

            } else
            {
                Assert.Fail("Bad platform");
            }
            Assert.NotNull(retval, "The path should have been found");
        }

        [Test]
        public void EnvVarSearch_FindInPath_NotFound_File()
        {
            Dictionary<string, string> setup = new Dictionary<string, string>();
            OsDetector.OsSelection os = OsDetector.DetectOs();
            string retval = null;
            if (os == OsDetector.OsSelection.Unix)
            {
                setup.Add("PATH", "/usr/local/bin:/usr/bin");
                EnvVarSearch search = new EnvVarSearch(setup);
                retval = search.FindInPath(@"FooBarSomethingFolder.exe");
            } else if (os == OsDetector.OsSelection.Windows)
            {
                Assert.Fail("FixMe - FindInPath - Valid - Windows");

            } else
            {
                Assert.Fail("Bad platform");
            }
            Assert.IsNull(retval, "The path should not have been found");
        }

        [Test]
        public void EnvVarSearch_FindInPath_NotFound_Folder()
        {
            Dictionary<string, string> setup = new Dictionary<string, string>();
            OsDetector.OsSelection os = OsDetector.DetectOs();
            string retval = null;
            if (os == OsDetector.OsSelection.Unix)
            {
                setup.Add("PATH", "/usr/local/bin:/usr/bin");
                EnvVarSearch search = new EnvVarSearch(setup);
                retval = search.FindInPath(@"FooBarSomethingFolder/");
            } else if (os == OsDetector.OsSelection.Windows)
            {
                Assert.Fail("FixMe - FindInPath - Valid - Windows");

            } else
            {
                Assert.Fail("Bad platform");
            }
            Assert.IsNull(retval, "The path should not have been found");
        }

        [Test]
        public void EnvVarSearch_FindInPath_MultipleResult()
        {
            Dictionary<string, string> setup = new Dictionary<string, string>();
            OsDetector.OsSelection os = OsDetector.DetectOs();
            string retval = null;
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            if (os == OsDetector.OsSelection.Unix)
            {
                // TestData folder before base dir
                setup.Add("PATH", "/usr/local/bin:/usr/bin:" + folder_testdata + ":" + baseDir);
                EnvVarSearch search = new EnvVarSearch(setup);
                retval = search.FindInPath("EnvVarSearch_File.txt");
            } else if (os == OsDetector.OsSelection.Windows)
            {
                Assert.Fail("FixMe - FindInPath - Valid - Windows");

            } else
            {
                Assert.Fail("Bad platform");
            }
            Assert.NotNull(retval, "The path should have been found");
            Assert.AreEqual(Path.Combine(folder_testdata, "EnvVarSearch_File.txt"), retval, "The path found should be the TestData foldfer");
        }
        #endregion
    }
}
