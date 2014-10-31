using System;
using System.Diagnostics;
using System.Collections.Generic;

namespace RLToolkit.Basic
{
	public class ListStringOutputHandler : IOutputManager
	{
		public List<string> outputData = new List<string>();
		public List<string> errorData = new List<string>();

        /// <summary>
        /// Method to define that will be called when an output line will be received
        /// </summary>
        /// <param name="sender">Sender. Should be the process</param>
        /// <param name="e">The arguments from the event.</param>
		public void OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null && e.Data.ToString() != "")
            {
                this.Log().Debug("Output received: " + e.Data.ToString());
				outputData.Add(e.Data);
			}
        }

        /// <summary>
        /// Method to define that will be called when an error line will be received
        /// </summary>
        /// <param name="sender">Sender. Should be the process</param>
        /// <param name="e">The arguments from the event.</param>
		public void ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null && e.Data.ToString() != "")
            {
                this.Log().Debug("Error received: " + e.Data.ToString());
				errorData.Add(e.Data);
			}
        }

        /// <summary>
        /// Method to fetch the output information
        /// </summary>
        /// <returns>The output information as object.</returns>
		public object GetOutput()
		{
			return outputData;
		}

        /// <summary>
        /// Method to fetch the error information
        /// </summary>
        /// <returns>The error information as object.</returns>
		public object GetError()
		{
			return errorData;
		}
	}
}