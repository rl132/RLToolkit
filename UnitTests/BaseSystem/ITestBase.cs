using System;

namespace RLToolkit.UnitTests
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