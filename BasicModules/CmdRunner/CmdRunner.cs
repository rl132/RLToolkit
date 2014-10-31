using System;
using System.Diagnostics;

namespace RLToolkit.Basic
{
	public class CmdRunner
	{
		#region parameters
		public Process process;
		public ProcessStartInfo startInfo;
		public IOutputManager outputHandler;
        public event EventHandler processFinished;
		#endregion

		#region constants
		public const int UNDEFINED_EXITCODE = -39827;
		#endregion
	 
		#region constructors
        /// <summary>
        /// Constructor with no parameters
        /// </summary>
		public CmdRunner () 
		{
			this.Log().Debug("Creating a CmdRunner instance with no parameters.");
			process = new Process();
			startInfo = new ProcessStartInfo();
		}

        /// <summary>
        /// Constructor that uses the default OutputHandler (NullOutputHandler)
        /// </summary>
        /// <param name="filePath">the complete file path of the target executable</param>
        /// <param name="args">The arguments to supply to the process
		public CmdRunner (string filePath, string args) : this(filePath, args, new NullOutputHandler()){}

        /// <summary>
        /// Constructor that uses a specified OutputHandler
        /// </summary>
        /// <param name="filePath">the complete file path of the target executable</param>
        /// <param name="args">The arguments to supply to the process
        /// <param name="handler">an instance of the OutputHandler</param>
        public CmdRunner (string filePath, string args, IOutputManager handler)
		{
			this.Log().Debug (string.Format("Creating a CmdRunner instance with these parameters: filePath= \"{0}\", args= \"{1}\", handler= \"{2}\"", filePath, args, handler.GetType().FullName));
			startInfo = new ProcessStartInfo (filePath, args);
			startInfo.UseShellExecute = false;
			startInfo.RedirectStandardOutput = true;
			startInfo.CreateNoWindow = true;

			process = new Process ();
			process.StartInfo = startInfo;

			outputHandler = handler;
			process.OutputDataReceived += outputHandler.OutputDataReceived;
			process.ErrorDataReceived += outputHandler.ErrorDataReceived;
            process.Exited += OnExitedReceived;
            process.EnableRaisingEvents = true;
		}
		#endregion

		#region methods
        /// <summary>
        /// Method to execute a command
        /// </summary>
        /// <param name="waitFinish">If set to <c>true</c>, wait for the process to finish it execution</param>
		public void Run(bool waitFinish)
        {
            this.Log().Debug(string.Format("Running the process: {0}", process.StartInfo.FileName));
            if (process == null)
            {
                return;
            }
            process.Start();
            process.BeginOutputReadLine();

            if (waitFinish)
            {
                process.WaitForExit();
            } 
		}

        /// <summary>
        /// Method to fetch the return code of a process. 
        /// </summary>
        /// <returns>The return code. returns UNDEFINED_EXITCODE (-39827) if the process is not finished</returns>
		public int GetReturnCode ()
		{
			this.Log().Debug("Fetching the process return code.");
			if (process.HasExited) {
				return process.ExitCode;
			}
			return UNDEFINED_EXITCODE;
		}
		#endregion

        #region event handling
        /// <summary>
        /// Raises this event when the process finishes. internal use only.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        public void OnExitedReceived(object sender, EventArgs e)
        {
            // throw it up!
            if (processFinished != null) {
                this.Log().Info("sending exit event");
                processFinished (sender, e);
            }
        }
        #endregion
	}
}