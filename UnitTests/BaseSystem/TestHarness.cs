using System;
using System.IO;
using System.Collections.Generic;
using NUnit.Framework;
using System.Threading;

using RLToolkit.Logger;

namespace RLToolkit.UnitTests
{
	public class TestHarness
	{
		#region variables
		public string folder_base;
		public string folder_testdata;
		public string folder_testresult;
        public List<Tuple<string,bool,bool>> filesInput = new List<Tuple<string,bool,bool>>();
        public List<Tuple<string,bool>> filesOutput = new List<Tuple<string, bool>>();
        public string localFolder = ""; // to be initialized later
		#endregion

		#region fixture setup/cleanup
		[TestFixtureSetUp]
		public void Initialize ()
		{
			SetFolderPaths ();

			ClearTestResultFolder ();

			try
			{
				DataPrepare ();
                ExecutePrepare ();
			}
			catch (Exception e) 
			{
				throw e;
			}
		}

		[TestFixtureTearDown]
		public void CleanUp ()
		{
			CreateTestResultFolder ();

			try
			{
				DataCleanup ();
                ExecuteCleanup ();
			}
			catch (Exception e)
			{
				throw e;
			}
		}
		#endregion

		#region Methods To Override
		public virtual void SetFolderPaths()
		{
			// replace me
			SetPaths (AppDomain.CurrentDomain.BaseDirectory, "Undefined");
		}

		public virtual void DataPrepare()
		{
			// to be replaced
		}

		public virtual void DataCleanup()
		{
			// to be replaced
		}
		#endregion

        #region Execute functions to move/prepare/transfer/report/etc
        public void ExecutePrepare()
        {
            // copy the tests data
            if (filesInput.Count > 0)
            {
                foreach (Tuple<string, bool, bool> t in filesInput)
                {
                    LogManager.Instance.Log().Debug("copying file: " + t.Item1 + " to: " + Path.Combine(localFolder, Path.GetFileName(t.Item1)));
                    CopyFile(t.Item1, Path.Combine(localFolder, Path.GetFileName(t.Item1)), t.Item2, t.Item3);
                }
            }
        }

        public void ExecuteCleanup()
        {
            // move the test results
            if (filesOutput.Count > 0)
            {
                foreach (Tuple<string, bool> t in filesOutput)
                {
                    LogManager.Instance.Log().Debug("moving file: " + t.Item1 + " to: " + Path.Combine(folder_testresult, Path.GetFileName(t.Item1)));
                    MoveFile(t.Item1, Path.Combine(folder_testresult, Path.GetFileName(t.Item1)), t.Item2);
                }
            }

            // clean the test data
            if (filesInput.Count > 0)
            {
                foreach (Tuple<string, bool, bool> t in filesInput)
                {
                    LogManager.Instance.Log().Debug("cleaning file: " + Path.Combine(localFolder, Path.GetFileName(t.Item1)));
                    CleanFile(Path.Combine(localFolder, Path.GetFileName(t.Item1)), t.Item3);
                }
            }
        }
        #endregion

		#region Helper functions
		public void SetPaths(string baseFolder, string module)
		{
			folder_base = baseFolder;
			folder_testdata = Path.Combine(folder_base, "TestData", module);
			folder_testresult = Path.Combine(folder_base, "TestResults", module);
		}

        public void AddInputFile(string file, bool isDelete, bool exceptionNotFound)
        {
            Tuple<string, bool, bool> t = new Tuple<string, bool, bool>(file, isDelete, exceptionNotFound);     
            filesInput.Add(t);
        }

        public void AddOutputFile(string file, bool exceptionNotFound)
        {
            Tuple<string, bool> t = new Tuple<string, bool>(file, exceptionNotFound);
            filesOutput.Add(t);
        }

		public void CleanFile(string file, bool errorIfNotFound)
		{
			try 
			{
                RetryAction(5, () => {File.Delete(file);});
			}
			catch (Exception e)
			{
				if (errorIfNotFound)
				{
					throw new Exception ("File could not be deleted." + Environment.NewLine + e.Message);
				}
			}
		}

		public void MoveFile(string fileSource, string fileOut, bool errorIfNotFound)
		{
			try 
			{
                RetryAction(5, () => {File.Move(fileSource, fileOut);});
			}
			catch (Exception e)
			{
				if (errorIfNotFound)
				{
					throw new Exception ("File could not be moved." + Environment.NewLine + e.Message);
				}
			}
		}

		public void CopyFile (string fileSource, string fileOut, bool isDelete, bool errorIfNotFound)
		{
			try
			{
                RetryAction(5, () => {File.Copy(fileSource, fileOut, isDelete);});
			}
			catch (Exception e)
			{
				if (errorIfNotFound)
				{
					throw new Exception ("File could not be copied." + Environment.NewLine + e.Message);
				}
			}
		}

		public void ClearTestResultFolder()
		{
			// if the folder exist, wipe it
			if (Directory.Exists(folder_testresult))
			{
				Directory.Delete(folder_testresult, true);
			}
		}

		public void CreateTestResultFolder()
		{
			// make sure our test result folder is created
			if (!Directory.Exists(folder_testresult))
			{
				Directory.CreateDirectory(folder_testresult);
			}
		}

        public void RetryAction(int count, Action a)
        {
            bool error = false;
            for (int i = 0; i<count;i++)
            {
                error = false;
                try 
                {
                    // execute the action
                    a();
                }
                catch (FileNotFoundException e)
                {
                    // no point trying many times, file doesn't exist
                    LogManager.Instance.Log().Debug("Action failed FileNotFound - " + e.Message);
                    return;
                }
                catch (Exception e)
                {
                    LogManager.Instance.Log().Debug("Action failed - " + e.Message);
                    error = true;
                    Thread.Sleep(1000);
                }

                if (!error)
                {
                    // success!
                    LogManager.Instance.Log().Debug("Action completed");
                    return;
                }
            } 

            // we've tried
            LogManager.Instance.Log().Warn("Action couldn't be completed within " + count + " tries");
            return;
        }
		#endregion
	}
}