using System;

namespace RLToolkit
{
	public class TimerManagerEventset
	{
		public string Id;
		public DateTime nextTick;
		public int timeInbetweenTick;
		public event TimerManagerEventHandler eHandler;

		public void executeEvent (object sender, TimerManagerEventArg e)
		{
			if (eHandler != null) {
				eHandler (sender, e);
			}
		}
	}
}

