using System;
using System.Diagnostics;

namespace RLToolkit.Basic
{
	public interface IOutputManager
	{
		void OutputDataReceived(object sender, DataReceivedEventArgs e);
		void ErrorDataReceived(object sender, DataReceivedEventArgs e);

		object GetOutput();
		object GetError();
	}
}