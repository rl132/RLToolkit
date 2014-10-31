using System;

namespace RLToolkit.Basic
{
    /// <summary>Timer manager event handler</summary>
	public delegate void TimerManagerEventHandler(object sender, TimerManagerEventArg e);

    /// <summary>
    /// Timer manager event arguments.
    /// </summary>
	public class TimerManagerEventArg : EventArgs
	{
		// add more parameters here if needed
		
        /// <summary>Gets or sets the tick time</summary>
        public DateTime tickTime { get; set; }
	}
}