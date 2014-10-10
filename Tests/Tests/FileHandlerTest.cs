using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using RLToolkit;
using NUnit.Framework;

namespace RLToolkit.Tests
{
	[TestFixture]
	public class FileHandlerTest : TestHarness, ITestBase
	{
		#region Local Variables
		private FileHandler fh;
		private bool enforceClose = false;

		// files contents
		private List<String> File1ContentRaw = new List<string>();

		// paths and filename
		private string file_1 = "file1.txt";
		private string file_2 = "file2.txt";
		private string file_3 = "file3.txt";
		private string file_1_out = "file1_out.txt";
		private string file_2_out = "file2_out.txt";
		private string file_1_copy = "file1_copy.txt";
		private string file_empty = "empty.txt";
		private string file_notExist_1 = "somefile1.txt";
		private string file_notExist_2 = "somefile2.txt";
		private string file_notExist_3 = "somefile3.txt";
		private string file_notExist_4 = "somefile4.txt";
		private string file_notExist_5 = "somefile5.txt";
		private string localFolder = ""; // to be initialized later
		#endregion

		#region Interface Override
		public string ModuleName()
		{
			return "FileHandler";
		}

		public override void SetFolderPaths()
		{
			localFolder = AppDomain.CurrentDomain.BaseDirectory;
			SetPaths (localFolder, ModuleName());
		}

		public override void DataPrepare()
		{
			// copy the test files frpm the data folder
			CopyFile (Path.Combine(folder_testdata, file_1), Path.Combine(localFolder, file_1), true, false);
			CopyFile (Path.Combine(folder_testdata, file_2), Path.Combine(localFolder, file_2), true, false);
			CopyFile (Path.Combine(folder_testdata, file_3), Path.Combine(localFolder, file_3), true, false);
			CopyFile (Path.Combine(folder_testdata, file_empty), Path.Combine(localFolder, file_empty), true, false);
			CopyFile (Path.Combine(folder_testdata, file_1), Path.Combine(localFolder, file_1_copy), true, false);

			// prepare the content of File1
			File1ContentRaw.Add ("Line1");
			File1ContentRaw.Add ("Line2 ");
			File1ContentRaw.Add (" Line3");
			File1ContentRaw.Add (" Line4 ");
			File1ContentRaw.Add ("Line 5");
			File1ContentRaw.Add ("");
			File1ContentRaw.Add ("Line6");
		}

		public override void DataCleanup()
		{
			// move the test output to the result folder
			MoveFile (Path.Combine(localFolder, file_1_out), Path.Combine(folder_testresult, file_1_out), false);
			MoveFile (Path.Combine(localFolder, file_2_out), Path.Combine(folder_testresult, file_2_out), false);
			MoveFile (Path.Combine(localFolder, file_notExist_1), Path.Combine(folder_testresult, file_notExist_1), false);
			MoveFile (Path.Combine(localFolder, file_notExist_2), Path.Combine(folder_testresult, file_notExist_2), false);
			MoveFile (Path.Combine(localFolder, file_notExist_3), Path.Combine(folder_testresult, file_notExist_3), false);
			MoveFile (Path.Combine(localFolder, file_notExist_4), Path.Combine(folder_testresult, file_notExist_4), false);
			MoveFile (Path.Combine(localFolder, file_notExist_5), Path.Combine(folder_testresult, file_notExist_5), false);

			// delete the test files from the data folder
			CleanFile (Path.Combine (localFolder, file_1), false);
			CleanFile (Path.Combine (localFolder, file_2), false);
			CleanFile (Path.Combine (localFolder, file_3), false);
			CleanFile (Path.Combine (localFolder, file_empty), false);
			CleanFile (Path.Combine(localFolder, file_1_copy), false);
		}
		#endregion

		#region Helper Functions
		private bool FileCompare(string path1, string path2)
		{
			return File.ReadLines(path1).SequenceEqual(File.ReadLines(path2));
		}
		#endregion

		#region Test Prepare/Teardown
		[SetUp]
		public void SetUp ()
		{
			fh = null;
		}

		[TearDown]
		public void TearDown ()
		{
			if (fh != null) {
				if (fh.isReady) {
					fh.CloseStream ();
				}
				if (enforceClose) {
					fh.isReady = true;
					enforceClose = false;
					fh.CloseStream ();
				}
			}
		}
		#endregion

		#region Tests
		[Test]
		public void OpenFile_Read_LocalPath()
		{
			fh = new FileHandler (file_1);
			List<String> read = fh.ReadLines ();

			Assert.AreEqual (read, File1ContentRaw, "List content is not equal");
		}

		[Test]
		public void OpenFile_Raad_FullPath()
		{
			fh = new FileHandler (file_1, localFolder);
			List<String> read = fh.ReadLines ();

			Assert.AreEqual (read, File1ContentRaw, "List content is not equal");
		}

		[Test]
		public void OpenFile_Write_LocalPath()
		{
			fh = new FileHandler (file_1_out, true);
			fh.WriteLines (File1ContentRaw);
			fh.CloseStream();

			Assert.IsTrue(File.Exists(Path.Combine(localFolder, file_1_out)), "File doesn't exist");
			Assert.IsTrue(FileCompare(Path.Combine(localFolder, file_1_out), Path.Combine(localFolder, file_1)), "Files are not the same");
		}

		[Test]
		public void OpenFile_Write_FullPath()
		{
			fh = new FileHandler (file_2_out, localFolder, true);
			fh.WriteLines (File1ContentRaw);
			fh.CloseStream();

			Assert.IsTrue(File.Exists(Path.Combine(localFolder, file_2_out)), "File doesn't exist");
			Assert.IsTrue(FileCompare(Path.Combine(localFolder, file_2_out), Path.Combine(localFolder, file_2)), "Files are not the same");
		}

		[Test]
		public void OpenFile_Read_NotExist ()
		{
			ExceptionCatcher exc = new ExceptionCatcher ();
			string fullPath = Path.Combine (localFolder, file_notExist_1);
			exc.expectedMessage = String.Format ("File: \"{0}\" doesn't exist.", fullPath);

			exc.test(() => {
				fh = new FileHandler (file_notExist_1);
			}
			);
		}

		[Test]
		public void OpenFile_Write_Exist()
		{
			try{
				fh = new FileHandler (file_1_copy, localFolder, true);
				fh.WriteLines (File1ContentRaw);
				fh.CloseStream();
			}
			catch (Exception e) {
				// fail if we catch an exception
				Assert.Fail ("Not expecting to catch an exception. Got:\n" + e.Message);
			}
		}

		[Test]
		public void OpenFile_Read_GetStream()
		{
			fh = new FileHandler(file_1);
			System.Type typeStream = fh.GetStream().GetType();

			Assert.AreEqual(typeStream, typeof(StreamReader), "different type");
			fh.CloseStream();
		}

		[Test]
		public void OpenFile_Write_GetStream()
		{
			fh = new FileHandler(file_1_out, true);
			System.Type typeStream = fh.GetStream().GetType();

			Assert.AreEqual(typeStream, typeof(StreamWriter), "different type");
		}

		[Test]
		public void OpenFile_Read_ReadLines_NotReady()
		{
			ExceptionCatcher exc = new ExceptionCatcher ();
			exc.expectedMessage = "Not initialized.";

			exc.test(() => {
				fh = new FileHandler (file_1);
				fh.isReady = false;
				fh.ReadLines();
			}
			);
		}

		[Test]
		public void OpenFile_Read_ReadLines_NotRead()
		{
			ExceptionCatcher exc = new ExceptionCatcher ();
			exc.expectedMessage = "Wrong access mode.";

			exc.test(() => {
				fh = new FileHandler (file_notExist_3, true);
				fh.ReadLines();
			}
			);
		}

		[Test]
		public void OpenFile_Read_ReadLines_EmptyFile()
		{
			fh = new FileHandler (file_empty);
			List<String> read = fh.ReadLines ();

			Assert.IsFalse (read.Any (), "Read content should not have anything.");
		}

		[Test]
		public void OpenFile_Write_WriteLines_NotReady()
		{
			ExceptionCatcher exc = new ExceptionCatcher ();
			exc.expectedMessage = "Not initialized.";
			enforceClose = true;

			exc.test(() => {
				fh = new FileHandler (file_notExist_4, true);
				fh.isReady = false;
				fh.WriteLines(File1ContentRaw);
			}
			);
		}

		[Test]
		public void OpenFile_Write_WriteLines_NotWrite()
		{
			ExceptionCatcher exc = new ExceptionCatcher ();
			exc.expectedMessage = "Wrong access mode.";

			exc.test(() => {
				fh = new FileHandler (file_1);
				fh.WriteLines(File1ContentRaw);
			}
			);
		}

		[Test]
		public void OpenFile_Write_WriteLines_FileReadOnly()
		{
			if (!File.GetAttributes (file_3).HasFlag (FileAttributes.ReadOnly)) {
				File.SetAttributes (file_3, FileAttributes.ReadOnly);
			}

			ExceptionCatcher exc = new ExceptionCatcher ();
			exc.expectedMessage = "File is read-only. Cannot open for write.";

			exc.test(() => {
				fh = new FileHandler (file_3, true);
			}
			);
		}

		[Test]
		public void OpenFile_Read_CloseStream()
		{
			try{
				fh = new FileHandler (file_1);
				fh.CloseStream();
			}
			catch (Exception e) {
				// fail if we catch an exception
				Assert.Fail ("Not expecting to catch an exception. Got:\n" + e.Message);
			}
		}

		[Test]
		public void OpenFile_Write_CloseStream()
		{
			try{
				fh = new FileHandler (file_notExist_2, true);
				fh.CloseStream();
			}
			catch (Exception e) {
				// fail if we catch an exception
				Assert.Fail ("Not expecting to catch an exception. Got:\n" + e.Message);
			}
		}

		[Test]
		public void OpenFile_Read_CloseStream_NotReady()
		{
			ExceptionCatcher exc = new ExceptionCatcher ();
			exc.expectedMessage = "Not initialized.";

			exc.test(() => {
				fh = new FileHandler (file_1);
				fh.isReady = false;
				fh.CloseStream();
			}
			);
		}
		#endregion
	}
}


