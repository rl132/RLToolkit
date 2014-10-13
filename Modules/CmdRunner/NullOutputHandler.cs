using System;
using System.Diagnostics;

namespace RLToolkit.Basic
{
	public class NullOutputHandler : IOutputManager
	{
		public void OutputDataReceived(object sender, DataReceivedEventArgs e)
		{
            this.Log().Debug("Output received: " + e.Data.ToString());
			// does nothing
		}

		public void ErrorDataReceived(object sender, DataReceivedEventArgs e)
		{
            this.Log().Debug("Error received: " + e.Data.ToString());
			// does nothing
		}

		public object GetOutput()
		{
			// return nothing
			return null;
		}

		public object GetError()
		{
			// return nothing
			return null;
		}
	}
}