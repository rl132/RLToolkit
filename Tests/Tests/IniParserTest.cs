using System;
using System.Collections.Generic;
using System.IO;

using RLToolkit;
using RLToolkit.Basic;
using NUnit.Framework;

namespace RLToolkit.Tests
{
	[TestFixture]
	public class IniParserTest : TestHarness, ITestBase
	{
		#region Local Variables
		private IniParser parser;

		// files contents
		private List<string> File1ContentRaw = new List<string>();
		private List<string> File2ContentRaw = new List<string>();
		private List<string> File3ContentRaw = new List<string>();

		// DicConfig setup
		private DicConfiguration dicoTest1 = new DicConfiguration();
		private DicConfiguration dicoTest2 = new DicConfiguration();


		// paths and filename
		private string file_1 = "file1.ini";
		private string file_1_out = "file1_out.ini";
		private string file_2_out = "file2_out.ini";
		private string localFolder = ""; // to be initialized later
		#endregion

		#region Interface Override
		public string ModuleName()
		{
			return "IniParser";
		}

		public override void SetFolderPaths()
		{
			localFolder = AppDomain.CurrentDomain.BaseDirectory;
			SetPaths (localFolder, ModuleName());
		}

		public override void DataPrepare()
		{
			// copy the data locally
			CopyFile(Path.Combine(folder_testdata, file_1), Path.Combine(localFolder, file_1), true, false);

			// prepare the content of File1
			File1ContentRaw.Add ("#comment");
			File1ContentRaw.Add ("");
			File1ContentRaw.Add ("[head]");
			File1ContentRaw.Add ("var=val");
			File1ContentRaw.Add ("testvar1=value1");
			File1ContentRaw.Add ("");
			File1ContentRaw.Add ("[head2]");
			File1ContentRaw.Add ("var=val2");
			File1ContentRaw.Add ("testvar2=value2");

			// prepare the content of File2 from File1 (bad headers)
			File2ContentRaw = new List<string>(File1ContentRaw);
			File2ContentRaw[2] = "head]";
			File2ContentRaw[6] = "[head2";

			// prepare the content of File3 from File1 (bad variables)
			File3ContentRaw = new List<string>(File1ContentRaw);
			File3ContentRaw[3] = "var val";
			File3ContentRaw[8] = "foobar";

			// prepare the dicoConfig stuff
			dicoTest1.header = "head";
			dicoTest1.dicto.Add("var", "val");
			dicoTest1.dicto.Add("testvar1", "value1");

			dicoTest2.header = "head2";
			dicoTest2.dicto.Add("var", "val2");
			dicoTest2.dicto.Add("testvar2", "value2");

		}

		public override void DataCleanup()
		{
			// move the result files if there
			MoveFile (Path.Combine (localFolder, file_1_out), Path.Combine (folder_testresult, file_1_out), false);
			MoveFile (Path.Combine (localFolder, file_2_out), Path.Combine (folder_testresult, file_2_out), false);

			// cleanup the testdata
			CleanFile (Path.Combine (localFolder, file_1), false);

		}
		#endregion

		#region Tests-Read
		[Test]
		public void Ini_Read_Local()
		{
			parser = new IniParser(file_1);
			string result = parser.GetValue("head", "testvar1");

			Assert.AreEqual(result, "value1", "Value is not as expected");
			Assert.IsFalse(parser.isEmpty, "isEmpty flag should be false");
		}

		[Test]
		public void Ini_Read_FullPath()
		{
			parser = new IniParser(file_1, localFolder);
			string result = parser.GetValue("head", "testvar1");

			Assert.AreEqual(result, "value1", "Value is not as expected");
			Assert.IsFalse(parser.isEmpty, "isEmpty flag should be false");
		}

		[Test]
		public void Ini_Read_FromList()
		{
			parser = new IniParser(File1ContentRaw);
			string result = parser.GetValue("head", "testvar1");

			Assert.AreEqual("value1",result, "Value is not as expected");
			Assert.IsFalse(parser.isEmpty, "isEmpty flag should be false");
		}

		[Test]
		public void Ini_Read_EmptyList()
		{
			// empty file, try to fetch from it
			parser = new IniParser(new List<string>());

			Assert.IsTrue(parser.isEmpty, "isEmpty flag should be true");
		}

		[Test]
		public void Ini_Read_EmptyConstructor()
		{
			// empty file, try to fetch from it
			parser = new IniParser();

			Assert.IsTrue(parser.isEmpty, "isEmpty flag should be true");
		}

		[Test]
		public void Ini_Read_SameName_DiffHead()
		{
			// fetch from both header and make sure it is the right thing
			parser = new IniParser(File1ContentRaw);
			string result1 = parser.GetValue("head", "var");   // Val
			string result2 = parser.GetValue("head2", "var");  // Val2

			Assert.AreNotEqual(result1, result2, "Result from both header same variable are the same.");
		}

		[Test]
		public void Ini_Read_WrongHeader()
		{
			// try t fetch from a header that doesn't have the variable
			parser = new IniParser(File1ContentRaw);
			string result1 = parser.GetValue("head3", "var");

			Assert.AreEqual(result1, "", "Value is not as expected");
		}

		[Test]
		public void Ini_Read_WrongVariable()
		{
			// try t fetch from a header that doesn't have the variable
			parser = new IniParser(File1ContentRaw);
			string result1 = parser.GetValue("head", "varX");

			Assert.AreEqual(result1, "", "Value is not as expected");
		}
		#endregion

		#region Tests-Add
		[Test]
		public void Ini_Add_AddDico()
		{
			// try t fetch from a header that doesn't have the variable
			parser = new IniParser();
			Assert.IsTrue(parser.isEmpty, "isEmpty flag should be true");

			bool add = parser.AddDicConf(dicoTest1);
			Assert.IsTrue(add, "Adding the dicConfig should return true");
			Assert.IsFalse(parser.isEmpty, "isEmpty flag should be false");

			string result1 = parser.GetValue("head", "var");
			Assert.AreEqual(result1, "val", "Value is not as expected");
		}

		[Test]
		public void Ini_Add_AddBadDico()
		{
			// try t fetch from a header that doesn't have the variable
			parser = new IniParser();
			Assert.IsTrue(parser.isEmpty, "isEmpty flag should be true");

			bool add = parser.AddDicConf(new DicConfiguration());
			Assert.IsFalse(add, "Should return false for this add");
			Assert.IsTrue(parser.isEmpty, "isEmpty flag should still be true");
		}

		[Test]
		public void Ini_Add_AddSameDico()
		{
			// try t fetch from a header that doesn't have the variable
			parser = new IniParser();
			Assert.IsTrue(parser.isEmpty, "isEmpty flag should be true");

			bool add = parser.AddDicConf(dicoTest1);
			Assert.IsTrue(add, "Adding the dicConfig should return true");
			Assert.IsFalse(parser.isEmpty, "isEmpty flag should be false");

			// try to readd the same DicConfig
			bool add2 = parser.AddDicConf(dicoTest1);
			Assert.IsFalse(add2, "Adding the second dicConfig should return false");

			// fetch should still work
			string result1 = parser.GetValue("head", "var");
			Assert.AreEqual(result1, "val", "Value is not as expected");
		}

		[Test]
		public void Ini_Add_AddDicEntry_New()
		{
			parser = new IniParser(File1ContentRaw);
			bool add = parser.AddDicEntry ("head", "varX", "valueX");
			Assert.IsTrue(add, "Adding the dicEntry should return true");

			// fetch should work
			string result1 = parser.GetValue("head", "varX");
			Assert.AreEqual(result1, "valueX", "Value is not as expected");
		}

		[Test]
		public void Ini_Add_AddDicEntry_ExistingVar()
		{
			parser = new IniParser(File1ContentRaw);
			bool add = parser.AddDicEntry ("head", "var", "valueX");
			Assert.IsFalse(add, "Adding the dicEntry should return false");

			// fetch should work with the old value
			string result1 = parser.GetValue("head", "var");
			Assert.AreEqual(result1, "val", "Value is not as expected");
		}

		[Test]
		public void Ini_Add_AddDicEntry_NonExistingHeader()
		{
			parser = new IniParser(File1ContentRaw);
			bool add = parser.AddDicEntry ("headX", "var", "val");
			Assert.IsFalse(add, "Adding the dicEntry should return false");

			// fetch should return empty since the header doesn't exist
			string result1 = parser.GetValue("head3", "var");
			Assert.AreEqual(result1, "", "Value is not as expected");

		}
		#endregion

		#region Tests-Write
		[Test]
		public void Ini_Write_NormalContent()
		{
			parser = new IniParser ();
			parser.AddDicConf (dicoTest1);
			parser.AddDicConf (dicoTest2);
			bool success = parser.WriteContent (file_1_out, AppDomain.CurrentDomain.BaseDirectory);

			Assert.IsTrue (success, "The write should have been successful");
		}

		public void Ini_Write_EmptyContent()
		{
			parser = new IniParser ();
			bool success = parser.WriteContent (file_2_out, AppDomain.CurrentDomain.BaseDirectory);

			Assert.IsTrue (success, "The write should have been unsuccessful");
		}
		#endregion	
	}
}

