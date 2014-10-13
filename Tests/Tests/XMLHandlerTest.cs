using System;
using System.Collections.Generic;
using System.IO;

using RLToolkit;
using RLToolkit.Basic;
using NUnit.Framework;

namespace RLToolkit.Tests
{
	[TestFixture]
	public class XmlHandlerTest : TestHarness, ITestBase
	{
		#region Local Variables
		private XMLHandlerReader reader;

		private string localFolder = ""; // to be initialized later
		#endregion

		#region Interface Override
		public string ModuleName()
		{
			return "XmlHandler";
		}

		public override void SetFolderPaths()
		{
			localFolder = AppDomain.CurrentDomain.BaseDirectory;
			SetPaths (localFolder, ModuleName());
		}

		public override void DataPrepare()
		{

        }

		public override void DataCleanup()
		{

        }
		#endregion

		#region Tests-Read
		[Test]
		public void Xml_Read()
		{
            reader = new XMLHandlerReader("xml1.xml", AppDomain.CurrentDomain.BaseDirectory);

		}
        #endregion
    }
}