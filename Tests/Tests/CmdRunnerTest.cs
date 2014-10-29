using System;
using System.IO;
using System.Threading;
using System.Collections.Generic;

using RLToolkit;
using RLToolkit.Basic;
using NUnit.Framework;

namespace RLToolkit.Tests
{
	[TestFixture]
	public class CmdRunnerTest : TestHarness, ITestBase
	{
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
		#endregion

		#region Tests
		[Test]
		public void CmdRunner_Basic_ImplicitOutput()
		{
			CmdRunner runner = new CmdRunner("echo", "test");
			runner.Run(true);

			Assert.AreEqual(null, runner.outputHandler.GetOutput(), "The output from the command is not as expected.");		
		}

		[Test]
		public void CmdRunner_Basic_ExplicitOutput()
		{
			ListStringOutputHandler handler = new ListStringOutputHandler();
			CmdRunner runner = new CmdRunner("echo", "test", handler);
			runner.Run(true);

			List<string> testAgainst = new List<string>();
			testAgainst.Add("test");
			List<string> res = (List<string>)runner.outputHandler.GetOutput();
			Assert.AreEqual(testAgainst, handler.outputData, "Local output is not as expected");
			Assert.AreEqual(testAgainst, res, "The output from the command is not as expected.");
		}

		[Test]
		public void CmdRunner_Basic_NullOutput()
		{
			NullOutputHandler nullHandler = new NullOutputHandler();
			CmdRunner runner = new CmdRunner("echo", "test", nullHandler);
			runner.Run(true);

			Assert.AreEqual(null, nullHandler.GetOutput(), "The output from the command is not as expected.");
		}

		[Test]
		public void CmdRunner_ExitCode_Successful()
		{
			CmdRunner runner = new CmdRunner("echo", "test", new ListStringOutputHandler());
			runner.Run(true);
			int ret = runner.GetReturnCode();

			Assert.AreEqual(0, ret, "return value for the exit code is not as expected");
		}


		[Test]
		public void CmdRunner_ExitCode_Failed()
		{
			CmdRunner runner = new CmdRunner("ping", "-badargs", new ListStringOutputHandler());
			runner.Run(true);
			int ret = runner.GetReturnCode();

			Assert.AreEqual(2, ret, "return value for the exit code is not as expected");
		}

		[Test]
		public void CmdRunner_ExitCode_InBetween ()
		{
			string args = "";
			OsDetector.OsSelection os = OsDetector.DetectOs ();
			if (os == OsDetector.OsSelection.Unix) {
				args = "-c 2 127.0.0.1";
			} else if (os == OsDetector.OsSelection.Windows) {
				args = "-n 2 127.0.0.1";
			} else if (os == OsDetector.OsSelection.Mac) {
				Assert.Fail ("Unsupported platform.");
			} else {
				Assert.Fail ("Unknown platform.");
			}

            bool finalized = false;
            ManualResetEvent mre = new ManualResetEvent(false);

            CmdRunner runner = new CmdRunner("ping", args, new ListStringOutputHandler());
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

