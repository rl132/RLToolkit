using System;
using System.IO;
using System.Threading;
using System.Collections.Generic;

using RLToolkit;
using RLToolkit.Basic;
using NUnit.Framework;

namespace RLToolkit.UnitTests.Modules
{
	[TestFixture]
	public class CmdRunnerTest : TestHarness, ITestBase
	{
        #region Local variables
        public string runnerFile;
        public string runnerArgs;
        #endregion

		#region Interface Override
		public string ModuleName()
		{
			return "CmdRunner";
		}

		public override void SetFolderPaths()
		{
			localFolder = AppDomain.CurrentDomain.BaseDirectory;
			SetPaths (localFolder, ModuleName());
		}

        public override void DataPrepare()
        {
            OsDetector.OsSelection os = OsDetector.DetectOs ();
            if (os == OsDetector.OsSelection.Unix) {
                runnerFile = "mono";
                runnerArgs = "DummyRunner-Linux.exe ";
                AddInputFile(Path.Combine(folder_testdata, "DummyRunner-Linux.exe"), true, false);
            } else if (os == OsDetector.OsSelection.Windows) {
                runnerFile = "DummyRunner-Windows.exe";
                runnerArgs = "";
                AddInputFile(Path.Combine(folder_testdata, "DummyRunner-Windows.exe"), true, false);
            } else {
                // do nothing, unsuported
            }
        }
		#endregion

		#region Tests
		[Test]
		public void CmdRunner_Basic_ImplicitOutput()
		{
            CmdRunner runner = new CmdRunner(runnerFile, runnerArgs);
			runner.Run(true);

			Assert.AreEqual(null, runner.outputHandler.GetOutput(), "The output from the command is not as expected.");		
		}

		[Test]
		public void CmdRunner_Basic_ExplicitOutput()
		{
			ListStringOutputHandler handler = new ListStringOutputHandler();
            CmdRunner runner = new CmdRunner(runnerFile, runnerArgs, handler);
			runner.Run(true);

			List<string> testAgainst = new List<string>();
            testAgainst.Add("Start");
            testAgainst.Add("Finished");
			List<string> res = (List<string>)runner.outputHandler.GetOutput();
			Assert.AreEqual(testAgainst, (List<string>)handler.GetOutput(), "Local output is not as expected");
			Assert.AreEqual(testAgainst, res, "The output from the command is not as expected.");
		}

		[Test]
		public void CmdRunner_Basic_NullOutput()
		{
			NullOutputHandler nullHandler = new NullOutputHandler();
            CmdRunner runner = new CmdRunner(runnerFile, runnerArgs, nullHandler);
			runner.Run(true);

			Assert.AreEqual(null, nullHandler.GetOutput(), "The output from the command is not as expected.");
		}

		[Test]
		public void CmdRunner_ExitCode_Successful()
		{
            CmdRunner runner = new CmdRunner(runnerFile, runnerArgs, new ListStringOutputHandler());
			runner.Run(true);
			int ret = runner.GetReturnCode();

			Assert.AreEqual(1, ret, "return value for the exit code is not as expected");
		}


		[Test]
		public void CmdRunner_ExitCode_Failed()
		{
            CmdRunner runner = new CmdRunner(runnerFile, runnerArgs + "-error", new ListStringOutputHandler());
			runner.Run(true);
			int ret = runner.GetReturnCode();

			Assert.AreEqual(2, ret, "return value for the exit code is not as expected");
		}

		[Test]
		public void CmdRunner_ExitCode_InBetween ()
		{
            bool finalized = false;
            ManualResetEvent mre = new ManualResetEvent(false);

            CmdRunner runner = new CmdRunner(runnerFile, runnerArgs + "-delay:2", new ListStringOutputHandler());
			runner.Run(false);
			int ret = runner.GetReturnCode();
            Assert.AreEqual(CmdRunner.UNDEFINED_EXITCODE, ret, "While waiting, retCode not as expected");
            Assert.IsFalse(finalized, "Finalized should NOT be true here");

            runner.processFinished += delegate(object sender, EventArgs e)
            {
                mre.Set();
                finalized = true;
            };

            mre.WaitOne(2500, false);

            ret = runner.GetReturnCode();
			Assert.That(ret, Is.Not.EqualTo(CmdRunner.UNDEFINED_EXITCODE).After(100), "After the wait, retcode not as expected.");
            Assert.IsTrue(finalized, "Finalized should be true here");
		}
		#endregion	
	}
}

