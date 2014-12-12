using System;
using System.Collections.Generic;
using System.IO;
	
using RLToolkit;
using RLToolkit.Basic;
using NUnit.Framework;

namespace RLToolkit.UnitTests.Modules
{
	[TestFixture]
	public class OsDetectorTest : TestHarness, ITestBase
	{
		#region Local Variables
		private OperatingSystem testOsUnix = new OperatingSystem(PlatformID.Unix, new Version(0,0,0,0));
		private OperatingSystem testOsWin32 = new OperatingSystem(PlatformID.Win32NT, new Version(0,0,0,0));
		private OperatingSystem testOsMac = new OperatingSystem(PlatformID.MacOSX, new Version(0,0,0,0));
		#endregion

		#region Interface Override
		public string ModuleName()
		{
			return "OsDetector";
		}

		public override void SetFolderPaths()
		{
			localFolder = AppDomain.CurrentDomain.BaseDirectory;
			SetPaths (localFolder, ModuleName());
		}
		#endregion

		#region Tests
		[Test]
		public void OsD_SetOs_Unix()
		{
			OsDetector.SetOS(testOsUnix);
		    OsDetector.OsSelection selectedOS = OsDetector.DetectOs();
			Assert.AreEqual(selectedOS, OsDetector.OsSelection.Unix, "OS detected is not as expected");
			Assert.AreEqual(OsDetector.currentOs, OsDetector.OsSelection.Unix);
		}

		[Test]
		public void OsD_SetOs_Mac()
		{
			OsDetector.SetOS(testOsMac);
		    OsDetector.OsSelection selectedOS = OsDetector.DetectOs();
			Assert.AreEqual(selectedOS, OsDetector.OsSelection.Mac, "OS detected is not as expected");
			Assert.AreEqual(OsDetector.currentOs, OsDetector.OsSelection.Mac);
		}

		[Test]
		public void OsD_SetOs_Win32()
		{
			OsDetector.SetOS(testOsWin32);
		    OsDetector.OsSelection selectedOS = OsDetector.DetectOs();
			Assert.AreEqual(selectedOS, OsDetector.OsSelection.Windows, "OS detected is not as expected");
			Assert.AreEqual(OsDetector.currentOs, OsDetector.OsSelection.Windows);
		}

		#endregion	
	}
}

