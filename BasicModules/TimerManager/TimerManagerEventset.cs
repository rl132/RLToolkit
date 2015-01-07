using System;

namespace RLToolkit.Basic
{
    /// <summary>
    /// Timer manager EventSet.
    /// </summary>
	public class TimerManagerEventset
	{
        /// <summary>The EventSet identifier.</summary>
		public string Id;

        /// <summary> The next expected tick in DateTime format</summary>
		public DateTime nextTick;

        /// <summary>The number of milliseconds inbetween ticking</summary>
		public int timeInbetweenTick;

        /// summary>If the event is paused or not</summary>
        public bool isPaused = false;

        /// <summary>The event handler to call when ticking</summary>
		public event TimerManagerEventHandler eHandler;

        /// <summary>Method to fire the event</summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">The event information</param>
		public void executeEvent (object sender, TimerManagerEventArg e)
		{
			if (eHandler != null) {
				eHandler (sender, e);
			}
		}
	}
}