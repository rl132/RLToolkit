using System;

namespace RLToolkit.Tests
{
	public interface ITestBase 
	{
		// TODO: comment and describe me!
		// methods to implement
		void DataPrepare();
		void DataCleanup();
		string ModuleName();
		void SetFolderPaths();
	}
}