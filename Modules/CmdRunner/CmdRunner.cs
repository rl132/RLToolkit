using System;
using System.Diagnostics;

namespace RLToolkit
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
		public CmdRunner () 
		{
			this.Log().Debug("Creating a CmdRunner instance with no parameters.");
			process = new Process();
			startInfo = new ProcessStartInfo();
		}

		public CmdRunner (string filePath, string args) : this(filePath, args, new NullOutputHandler()){}

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

