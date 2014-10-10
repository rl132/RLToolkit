using System;

namespace RLToolkit
{
	public delegate void TimerManagerEventHandler(object sender, TimerManagerEventArg e);

	public class TimerManagerEventArg : EventArgs
	{
		// add more parameters here if needed
		public DateTime tickTime { get; set; }
	}
}

