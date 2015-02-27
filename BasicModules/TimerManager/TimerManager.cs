using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

using RLToolkit.Logger;

namespace RLToolkit.Basic
{
    /// <summary>
    /// Timer manager.
    /// </summary>
	public static class TimerManager
	{
		#region Local Variables
        // states
		private static bool isRunning = false;
        private static bool isVerbose = false;
		
        // ticking-related
        private static Thread tickThread;
        private static int tickCount = 0;

        // list related
        private static List<TimerManagerEventset> timerEvents = new List<TimerManagerEventset>();
        private static List<TimerManagerEventset> deferredAddList = new List<TimerManagerEventset>();
        private static List<string> deferredRemList = new List<string>();
        private static List<string> deferredPauseList = new List<string>();
        private static List<string> deferredUnpauseList = new List<string>();
        private static bool deferredCleanFlag = false;
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
            deferredAddList.Add(e);

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
			
            if (ID.Trim() == "")
            {
                LogManager.Instance.Log().Warn("using an empty ident. Abort");
                return false;
            }
            if (ID == null)
            {
                LogManager.Instance.Log().Warn("using an null ident. Abort");
                return false;
            }

            deferredRemList.Add(ID);
            return true;
		}

        /// <summary>
        /// Method to fetch the number of EventSet registered. Mostly for internal use only.
        /// </summary>
        /// <returns>The EventSet count.</returns>
        internal static int GetEventSetsCount()
		{
            LogManager.Instance.Log().Debug(string.Format("Getting the EventSet count: {0}", timerEvents.Count));
            return timerEvents.Count;
		}

        /// <summary>
        /// Method that Clears all the EventSet in the timer manager
        /// </summary>
        public static void ClearAllEventSets ()
		{
            // schedule to wipe it
            LogManager.Instance.Log().Debug("Setting the flag to clear the event list.");
            deferredCleanFlag = true;
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
        internal static void ForceStartThread()
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
                    // increment our counter
                    tickCount++;

                    if (deferredCleanFlag)
                    {
                        LogManager.Instance.Log().Info("timer Event Cleared");
                        timerEvents = new List<TimerManagerEventset>();
                        deferredCleanFlag = false;
                        //continue;
                    }

                    // make a new list that will replace the one once we're done with our tick
                    List<TimerManagerEventset> newList = new List<TimerManagerEventset>();

                    // copy the lists and wipe the old one
                    List<TimerManagerEventset> defAddListCopy = new List<TimerManagerEventset>(deferredAddList);
                    List<string> defRemListCopy = new List<string>(deferredRemList);
                    List<string> defPauListCopy = new List<string>(deferredPauseList);
                    List<string> defUnpListCopy = new List<string>(deferredUnpauseList);
                    deferredAddList = new List<TimerManagerEventset>();
                    deferredRemList = new List<string>();
                    deferredPauseList = new List<string>();
                    deferredUnpauseList = new List<string>();

                    List<TimerManagerEventset> currentList = new List<TimerManagerEventset>(timerEvents);

                    // add the new stuff requested
                    if (defAddListCopy.Count > 0)
                    {
                        LogManager.Instance.Log().Debug("Adding " + defAddListCopy.Count.ToString() + " items");
                        currentList.AddRange(defAddListCopy);
                    }

                    // check each events
                    foreach (TimerManagerEventset t in currentList)
                    {
                        TimerManagerEventset newT = new TimerManagerEventset(t);

                        if (defRemListCopy.Contains(t.Id))
                        {
                            // we need to wipe that one.  don't even bother readding it to the new list
                            LogManager.Instance.Log().Debug("Removing " + t.Id);
                            continue;
                        }

                        if (defPauListCopy.Contains(t.Id))
                        {
                            LogManager.Instance.Log().Debug("Pausing " + t.Id);
                            newT.isPaused = true;
                            // we're not gonna fire since we just paused it
                            newList.Add(newT);
                            continue;
                        }
                        if (defUnpListCopy.Contains(t.Id))
                        {
                            // wake that one up! 
                            LogManager.Instance.Log().Debug("Unpausing " + t.Id);
                            newT.isPaused = false;
                        }

                        if (DateTime.UtcNow >= newT.nextTick)
                        {// we're ready to tick

                            // if we're paused, don't even bother.  next!
                            if (t.isPaused)
                            {
                                if (isVerbose)
                                {
                                    LogManager.Instance.Log().Debug(string.Format("EventSet ID \"{0}\" is ready to tick but is paused.", newT.Id));
                                }
                                continue;
                            }
                            LogManager.Instance.Log().Debug(string.Format("EventSet ID \"{0}\" is ready to tick", newT.Id));
                            newT.nextTick = DateTime.UtcNow.AddMilliseconds(newT.timeInbetweenTick);
                            newList.Add(newT);
                            LogManager.Instance.Log().Debug(string.Format("Setting new tick time for EventSet ID \"{0}\" to {1}", newT.Id, newT.nextTick.ToLongTimeString()));

                            // fire
                            TimerManagerEventArg param = new TimerManagerEventArg();
                            param.tickTime = DateTime.UtcNow;
                            LogManager.Instance.Log().Debug("Firing event");
                            newT.executeEvent(new object(), param);
                        } else
                        {
                            // nothing to do, just put back the data in the list
                            newList.Add(newT);
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

                // check if we need to tick next time
                isRunning = IsEventListFilled();

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
        internal static bool IsIdentExist (string Ident)
        {
            return IsIdentExist(Ident, timerEvents);
        }

        /// <summary>
        /// Method that querry if an Identifier already exists with a provided name, in a specified list
        /// </summary>
        /// <returns><c>true</c> if the ident exist; otherwise, <c>false</c>.</returns>
        /// <param name="Ident">The identifier to look for</param>
        /// <param name="inputList">what List&lt;TimerManagerEventset&gt; to use for the search</param>
        internal static bool IsIdentExist (string Ident, List<TimerManagerEventset> inputList)
		{
            LogManager.Instance.Log().Debug(string.Format("Trying to find if Id \"{0}\" exists", Ident));
            List<TimerManagerEventset> currentList = new List<TimerManagerEventset>(inputList);
            foreach (TimerManagerEventset t in currentList)
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

            if (Ident.Trim() == "")
            {
                LogManager.Instance.Log().Warn("using an empty ident. Abort");
                return false;
            }
            if (Ident == null)
            {
                LogManager.Instance.Log().Warn("using an null ident. Abort");
                return false;
            }

            if (!IsIdentExist(Ident, timerEvents))
            {
                if (!IsIdentExist(Ident, deferredAddList))
                {
                    LogManager.Instance.Log().Warn("using an ident that is not found. aborting.");
                    return false;
                }
            }

            if (isPausing)
            {
                deferredPauseList.Add(Ident);
            }
            else
            {
                deferredUnpauseList.Add(Ident);
            }           
            LogManager.Instance.Log().Debug("Added the ident to be paused/unpaused.");

            return true;
        }

        /// <summary>
        /// Method that verify if there's any event registered and not paused
        /// </summary>
        /// <returns>True if it does contain something not paused, else false</returns>
        internal static bool IsEventListFilled()
        {
            return IsEventListFilled(true);
        }

        /// <summary>
        /// Method that verify if there's any event registered, with the possiblity to check paused items too
        /// </summary>
        /// <param name="includePaused">If we include or not the paused events in the search</param>
        /// <returns>True if it does contain something, else false</returns>
        internal static bool IsEventListFilled(bool includePaused)
        {
            if (includePaused)
            {
                if (timerEvents.Count > 0)
                {
                    // we have stuff. but... anything worth checking?
                    List<TimerManagerEventset> currentList = new List<TimerManagerEventset>(timerEvents);
                    foreach (TimerManagerEventset t in currentList)
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
        internal static void ResetTickCounter()
        {
            LogManager.Instance.Log().Debug("reseting the tick counter.");
            tickCount = 0;
        }

        /// <summary>
        /// Gets the tick counter value
        /// </summary>
        /// <returns>The tick counter value</returns>
        internal static int GetTickCounter()
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
        internal static TimerManagerEventset GetEventSetByID(string input)
        {
            if (isVerbose)
            {
                LogManager.Instance.Log().Debug("Looking for event: " + input);
            }

            List<TimerManagerEventset> currentList = new List<TimerManagerEventset>(timerEvents);
            foreach (TimerManagerEventset t in currentList)
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