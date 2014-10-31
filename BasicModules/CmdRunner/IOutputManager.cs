using System;
using System.Diagnostics;

namespace RLToolkit.Basic
{
    /// <summary>
    /// Interface IOoutputManager for the CmdRunner
    /// </summary>
	public interface IOutputManager
	{
        // In order to implement an IOutputHandler, the class will have to implement
        // both event handlers for when the information is received, and 2 helper
        // functions to fetch the finished output/error information.

        // In order to fetch the information for each line when the Method is called,
        // simply fetch 'e.Data'. It does contain the information from the process.

        // The GetOutout and GetError methods require the user to cast their returning
        // value to Object in order to not limit the possible structures for the results
        // that could be handled in a different OutoutHandler.

        /// <summary>
        /// Method to define that will be called when an output line will be received
        /// </summary>
        /// <param name="sender">Sender. Should be the process</param>
        /// <param name="e">The arguments from the event.</param>
		void OutputDataReceived(object sender, DataReceivedEventArgs e);

        /// <summary>
        /// Method to define that will be called when an error line will be received
        /// </summary>
        /// <param name="sender">Sender. Should be the process</param>
        /// <param name="e">The arguments from the event.</param>
        void ErrorDataReceived(object sender, DataReceivedEventArgs e);

        /// <summary>
        /// Method to fetch the output information
        /// </summary>
        /// <returns>The output information as object.</returns>
		object GetOutput();

        /// <summary>
        /// Method to fetch the error information
        /// </summary>
        /// <returns>The error information as object.</returns>
        object GetError();
	}
}