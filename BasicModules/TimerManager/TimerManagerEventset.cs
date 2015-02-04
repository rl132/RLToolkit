using System;

namespace RLToolkit.Basic
{
    /// <summary>
    /// Timer manager EventSet.
    /// </summary>
	public class TimerManagerEventset
	{
        /// <summary>
        /// Constuctors for a plain eventset
        /// </summary>
        public TimerManagerEventset()
        {
            Id = "";
            nextTick = DateTime.Now;
            timeInbetweenTick = 1000;
            isPaused = false;
            eHandler = null;
        }

        /// <summary>
        /// Constuctor for an eventset based on a previous one.
        /// </summary>
        /// <param name="t">T.</param>
        public TimerManagerEventset(TimerManagerEventset t)
        {
            Id = t.Id;
            nextTick = t.nextTick;
            timeInbetweenTick = t.timeInbetweenTick;
            isPaused = t.isPaused;
            eHandler = t.eHandler;
        }

        /// <summary>The EventSet identifier.</summary>
		public string Id;

        /// <summary> The next expected tick in DateTime format</summary>
		public DateTime nextTick;

        /// <summary>The number of milliseconds inbetween ticking</summary>
		public int timeInbetweenTick;

        /// <summary>If the event is paused or not</summary>
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