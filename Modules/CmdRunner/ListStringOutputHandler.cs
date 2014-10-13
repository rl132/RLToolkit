using System;
using System.Diagnostics;
using System.Collections.Generic;

namespace RLToolkit.Basic
{
	public class ListStringOutputHandler : IOutputManager
	{
		public List<string> outputData = new List<string>();
		public List<string> errorData = new List<string>();

		public void OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null && e.Data.ToString() != "")
            {
                this.Log().Debug("Output received: " + e.Data.ToString());
				outputData.Add(e.Data);
			}
        }

		public void ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null && e.Data.ToString() != "")
            {
                this.Log().Debug("Error received: " + e.Data.ToString());
				errorData.Add(e.Data);
			}
        }

		public object GetOutput()
		{
			return outputData;
		}

		public object GetError()
		{
			return errorData;
		}
	}
}