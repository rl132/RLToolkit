using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

using RLToolkit.Logger;

// TODO: RL - Remove the Start/Stop ticking and do it automatically
// if we have data to tick.. if no event, no need to tick for nothing.

namespace RLToolkit.Basic
{
    /// <summary>
    /// Timer manager.
    /// </summary>
	public static class TimerManager
	{
		#region Local Variables
		private static bool isRunning = false;
        private static bool isVerbose = false;
        private static List<TimerManagerEventset> timerEvents = new List<TimerManagerEventset>();
		private static Thread tickThread;
        private static int tickCount = 0;
		#endregion

        #region initialization
        /// <summary>
        /// Trigger the timer start when it's first used
        /// </summary>
        static TimerManager()
        {
            LogManager.Instance.Log().Info("TimerManager started ticking.");
            tickThread = new Thread(tick);
            tickThread.Start();
            tickCount = 0;
        }
        #endregion

		#region EventSetManagement
		/// <summary>
        /// Method to Add EventSets
        /// </summary>
        /// <returns><c>true</c>, if EventSet was added, <c>false</c> otherwise.</returns>
        /// <param name="ID">the Identifier to associate this EventSet to</param>
        /// <param name="ms">the number of milliseconds inbetween ticks</param>
        /// <param name="handler">The event handler to call when ticking</param>
        /// <param name="tickNow">If set to <c>true</c> tick as soon as possible</param>
        public static bool AddEventSet (string ID, int ms, TimerManagerEventHandler handler, bool tickNow)
		{
            LogManager.Instance.Log().Debug(string.Format("Adding EventSet with parameters: ID: {0}, ms: {1}, handler: {2}, tickNow: {3}", ID, ms.ToString(), handler.Method, tickNow.ToString()));

			if (IsIdentExist (ID)) {
                LogManager.Instance.Log().Debug("ID already existed. abort.");
				return false;
			}

			TimerManagerEventset e = new TimerManagerEventset();
			e.eHandler += handler;
			e.timeInbetweenTick = ms;
			e.Id = ID;
			if (tickNow)
			{
				// fire at the next timer itteration
				e.nextTick = DateTime.UtcNow;
			}
			else
			{
				// fire after X seconds
				e.nextTick = DateTime.UtcNow.AddMilliseconds(ms);
			}
            LogManager.Instance.Log().Debug(string.Format("Next Tick at: {0}", e.nextTick.ToLongTimeString()));
			timerEvents.Add(e);

            // automatically restart the timer since we added something
            isRunning = true;

			return true;
		}

        /// <summary>
        /// Method to remove a known EventSet
        /// </summary>
        /// <returns><c>true</c>, if the eventset was removed, <c>false</c> otherwise.</returns>
        /// <param name="ID">The Identifier of the EventSet to remove</param>
        public static bool RemoveEventSet (string ID)
		{
            LogManager.Instance.Log().Debug(string.Format("Removing EventSet with parameters: ID: {0}", ID));
			Stack<TimerManagerEventset> st = new Stack<TimerManagerEventset> ();
			bool found = false;

			foreach (TimerManagerEventset t in timerEvents) {
				if (ID.ToLower () != t.Id.ToLower ()) {
					st.Push (t);
				} else {
                    LogManager.Instance.Log().Debug("Found the EventSet to remove.");
					found = true;
				}
			}

			// if we haven't found it, gtfo
            if (!found) {
                LogManager.Instance.Log().Debug("EventSet to remove not found. Aborting.");
				return false;
			}

			// replace the list with the stack since we found it (and removed it)
			timerEvents = new List<TimerManagerEventset>();
			foreach (TimerManagerEventset t in st) {
				timerEvents.Add(t);
			}

            if (!IsEventListFilled())
            {
                // shut down the ticking if we're empty
                isRunning = false;
            }

			return true;
		}

        /// <summary>
        /// Method to fetch the number of EventSet registered. Mostly for internal use only.
        /// </summary>
        /// <returns>The EventSet count.</returns>
        public static int GetEventSetsCount()
		{
            LogManager.Instance.Log().Debug(string.Format("Getting the EventSet count: {0}", timerEvents.Count));
            return timerEvents.Count;
		}

        /// <summary>
        /// Method that Clears all the EventSet in the timer manager
        /// </summary>
        public static void ClearAllEventSets ()
		{
			// wipe it
            LogManager.Instance.Log().Debug("Clearing the event list.");
            timerEvents.Clear();
            isRunning = false;
		}
		#endregion

		#region Ticking
        /// <summary>
        /// Method to start the timer
        /// </summary>
        /// <returns>always true</returns>
        /// <obsolete>Replaced by an automatic management of the timer</obsolete>
        [Obsolete("No need to call this anymore. should be handled automatically.")]
		public static bool StartTicking ()
		{
            LogManager.Instance.Log().Debug(string.Format("Trying to activate the ticking. Current state: {0}", isRunning.ToString()));
			isRunning = true;
			return true;
		}

        /// <summary>
        /// Method to stop the timer
        /// </summary>
        /// <returns>always false</returns>
        /// <obsolete>Replaced by an automatic management of the timer</obsolete>
        [Obsolete("No need to call this anymore. should be handled automatically.")]
		public static bool StopTicking ()
		{
            LogManager.Instance.Log().Debug(string.Format("Trying to deactivate the ticking. Current state: {0}", isRunning.ToString()));
            isRunning = false;
		    return false;
		}

        /// <summary>
        /// Forces the start of the timer thread. (internal use only)
        /// </summary>
        public static void ForceStartThread()
        {
            // force the start of the thread for unit tests
            if (isVerbose)
            {
                LogManager.Instance.Log().Debug("Thread status: " + tickThread.IsAlive);
            }
            if (!tickThread.IsAlive)
            {
                if (isVerbose)
                {
                    LogManager.Instance.Log().Debug("Force Starting thread.");
                }
                tickThread = new Thread(tick);
                tickThread.Start();
                tickCount = 0;
            }
        }

        /// <summary>
        /// Method that does the ticking
        /// </summary>
		private static void tick ()
		{// tick!
            bool skip = false;
			while (true) 
            {
                skip = false;
            
                if (isVerbose)
                {
                    LogManager.Instance.Log().Debug("Ticking!");
                }

				// if we're not running, don't tick
                if (!isRunning) {
                    if (isVerbose)
                    {
                        LogManager.Instance.Log().Debug("Timer not running while ticking. Abort.");
                    }
                    skip = true;
				}

                if (!skip)
                {
                    if (!IsEventListFilled())
                    {
                        // if we have nothing, don't tick
                        if (isVerbose)
                        {
                            LogManager.Instance.Log().Debug("Timer is empty while ticking. Abort.");
                        }
                        skip = true;
                    }
                }

                if (!skip)
                {
                    // increment our counter
                    tickCount++;

                    // make a new list that will replace the one once we're done with our tick
                    List<TimerManagerEventset> newList = new List<TimerManagerEventset>();

                    foreach (TimerManagerEventset t in timerEvents)
                    {
                        if (DateTime.UtcNow >= t.nextTick)
                        {// we're ready to tick

                            // if we're paused, don't even bother.  next!
                            if (t.isPaused)
                            {
                                if (isVerbose)
                                {
                                    LogManager.Instance.Log().Debug(string.Format("EventSet ID \"{0}\" is ready to tick but is paused.", t.Id));
                                }
                                continue;
                            }
                            LogManager.Instance.Log().Debug(string.Format("EventSet ID \"{0}\" is ready to tick", t.Id));
                            // cache the old data and add it to the new list after
                            TimerManagerEventset t2 = t;
                            t2.nextTick = DateTime.UtcNow.AddMilliseconds(t.timeInbetweenTick);
                            newList.Add(t2);
                            LogManager.Instance.Log().Debug(string.Format("Setting new tick time for EventSet ID \"{0}\" to {1}", t2.Id, t2.nextTick.ToLongTimeString()));

                            // fire
                            TimerManagerEventArg param = new TimerManagerEventArg();
                            param.tickTime = DateTime.UtcNow;
                            LogManager.Instance.Log().Debug("Firing event");
                            t.executeEvent(new object(), param);
                        } else
                        {
                            // nothing to do, just put back the data in the list
                            newList.Add(t);
                        }
                    }

                    // swap the old list with the new updated one
                    timerEvents = newList;
                }

				// sleep
                if (isVerbose)
                {
                    LogManager.Instance.Log().Debug("Timer Sleeping.");
                }
                skip = false;
				Thread.Sleep (1000);
			}
		}
		#endregion

		#region Helper functions
        /// <summary>
        /// Method that querry if an Identifier already exists with a provided name
        /// </summary>
        /// <returns><c>true</c> if the ident exist; otherwise, <c>false</c>.</returns>
        /// <param name="Ident">The identifier to look for</param>
		public static bool IsIdentExist (string Ident)
		{
            LogManager.Instance.Log().Debug(string.Format("Trying to find if Id \"{0}\" exists", Ident));
			foreach (TimerManagerEventset t in timerEvents)
			{
				if (t.Id.ToLower() == Ident.ToLower())
				{
                    LogManager.Instance.Log().Debug("Ident found.");
					return true;
				}
			}
            LogManager.Instance.Log().Debug("Ident not found.");
			return false;
		}

        /// <summary>
        /// Method to set the pause status of an event set.
        /// </summary>
        /// <returns>Returns if the eventset was found</returns>
        /// <param name="Ident">the identifier to look for</param>
        /// <param name="isPausing">if we pause or not</param>
        public static bool PauseIdent(string Ident, bool isPausing)
        {
            LogManager.Instance.Log().Debug(string.Format("Trying to set pause status to {0} for the Id \"{1}\"", isPausing, Ident));
            Stack<TimerManagerEventset> st = new Stack<TimerManagerEventset> ();
            bool found = false;

            foreach (TimerManagerEventset t in timerEvents) {
                if (Ident.ToLower () != t.Id.ToLower ()) {
                    st.Push (t);
                } else {
                    LogManager.Instance.Log().Debug("Found the EventSet to set pause state to " + isPausing);
                    found = true;
                    t.isPaused = isPausing;
                    st.Push(t);
                }
            }

            // if we haven't found it, gtfo
            if (!found) {
                LogManager.Instance.Log().Debug("EventSet to pause not found. Aborting.");
                return false;
            }

            // replace the list with the stack since we found it (and modified it)
            timerEvents = new List<TimerManagerEventset>();
            foreach (TimerManagerEventset t in st) {
                timerEvents.Add(t);
            }

            // if we don't have any more live eventset, pause the whole thing
            isRunning = IsEventListFilled(true);

            return true;
        }

        /// <summary>
        /// Method that verify if there's any event registered and not paused
        /// </summary>
        /// <returns>True if it does contain something not paused, else false</returns>
        public static bool IsEventListFilled()
        {
            return IsEventListFilled(true);
        }

        /// <summary>
        /// Method that verify if there's any event registered, with the possiblity to check paused items too
        /// </summary>
        /// <param name="includePaused">If we include or not the paused events in the search</param>
        /// <returns>True if it does contain something, else false</returns>
        public static bool IsEventListFilled(bool includePaused)
        {
            if (includePaused)
            {
                if (timerEvents.Count > 0)
                {
                    // we have stuff. but... anything worth checking?
                    foreach (TimerManagerEventset t in timerEvents)
                    {
                        if (isVerbose)
                        {
                            LogManager.Instance.Log().Debug("Event name: " + t.Id + " -> " + t.isPaused);
                        }
                        if (!t.isPaused)
                        {
                            // we have at least one not paused.
                            return true;
                        }
                    }
                }

                // we're empty or all paused, move along.
                return false;
            } else
            {
                return (timerEvents.Count > 0);
            }
        }

        /// <summary>
        /// Forces more verbose debug info
        /// </summary>
        /// <param name="input">If set to true, will make debug info more verbose</param>
        public static void SetDebugVerbose(bool input)
        {
            isVerbose = input;
        }

        /// <summary>
        /// Resets the tick counter.
        /// </summary>
        public static void ResetTickCounter()
        {
            LogManager.Instance.Log().Debug("reseting the tick counter.");
            tickCount = 0;
        }

        /// <summary>
        /// Gets the tick counter value
        /// </summary>
        /// <returns>The tick counter value</returns>
        public static int GetTickCounter()
        {
            if (isVerbose)
            {
                LogManager.Instance.Log().Debug("Fetching the tick counter (" + tickCount + ").");
            }
            return tickCount;
        }

        /// <summary>
        /// Method that will try to fetch the eventSet associated with an ID
        /// </summary>
        /// <returns>The event set if the ID is known, null if not found</returns>
        /// <param name="input">the ID to match</param>
        public static TimerManagerEventset GetEventSetByID(string input)
        {
            if (isVerbose)
            {
                LogManager.Instance.Log().Debug("Looking for event: " + input);
            }

            foreach (TimerManagerEventset t in timerEvents)
            {
                if (t.Id.ToLower() == input.ToLower().Trim())
                {
                    // that's our man!
                    return t;
                }
            }

            // null if we didn't found anything that matches
            return null;
        }
		#endregion	
	}
}