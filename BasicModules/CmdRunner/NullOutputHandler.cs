using System;
using System.Diagnostics;

namespace RLToolkit.Basic
{
    /// <summary>
    /// NullOutputHandler that implements the IOutputManager interface
    /// </summary>
	public class NullOutputHandler : IOutputManager
	{
        /// <summary>
        /// Method to define that will be called when an output line will be received. Note this is a default OutputHandler that won't do anything.
        /// </summary>
        /// <param name="sender">Sender. Should be the process</param>
        /// <param name="e">The arguments from the event.</param>
		public void OutputDataReceived(object sender, DataReceivedEventArgs e)
		{
            this.Log().Debug("Output received: " + e.Data.ToString());
			// does nothing
		}

        /// <summary>
        /// Method to define that will be called when an error line will be received. Note this is a default OutputHandler that won't do anything.
        /// </summary>
        /// <param name="sender">Sender. Should be the process</param>
        /// <param name="e">The arguments from the event.</param>
		public void ErrorDataReceived(object sender, DataReceivedEventArgs e)
		{
            this.Log().Debug("Error received: " + e.Data.ToString());
			// does nothing
		}

        /// <summary>
        /// Method to fetch the output information
        /// </summary>
        /// <returns>nothing.</returns>
		public object GetOutput()
		{
			// return nothing
			return null;
		}

        /// <summary>
        /// Method to fetch the error information
        /// </summary>
        /// <returns>nothing.</returns>
		public object GetError()
		{
			// return nothing
			return null;
		}
	}
}