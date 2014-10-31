using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace RLToolkit.Basic
{
	public static class TimerManager
	{
		#region Local Variables
		private static bool isRunning = false;
		private static List<TimerManagerEventset> timerEvents = new List<TimerManagerEventset>();
		private static Thread tickThread;
		#endregion

		#region ActionManagement
		public static bool AddAction (string ID, int ms, TimerManagerEventHandler handler, bool tickNow)
		{
            LogManager.Instance.Log().Debug(string.Format("Adding Action with parameters: ID: {0}, ms: {1}, handler: {2}, tickNow: {3}", ID, ms.ToString(), handler.Method, tickNow.ToString()));

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

			return true;
		}

		public static bool RemoveAction (string ID)
		{
            LogManager.Instance.Log().Debug(string.Format("Removing Action with parameters: ID: {0}", ID));
			Stack<TimerManagerEventset> st = new Stack<TimerManagerEventset> ();
			bool found = false;

			foreach (TimerManagerEventset t in timerEvents) {
				if (ID.ToLower () != t.Id.ToLower ()) {
					st.Push (t);
				} else {
                    LogManager.Instance.Log().Debug("Found the Action to remove.");
					found = true;
				}
			}

			// if we haven't found it, gtfo
            if (!found) {
                LogManager.Instance.Log().Debug("Action to remove not found. Aborting.");
				return false;
			}

			// replace the list with the stack since we found it (and removed it)
			timerEvents = new List<TimerManagerEventset>();
			foreach (TimerManagerEventset t in st) {
				timerEvents.Add(t);
			}

			return true;
		}

		public static int GetActionCount()
		{
            LogManager.Instance.Log().Debug(string.Format("Getting the action count: {0}", timerEvents.Count));
            return timerEvents.Count;
		}

		public static void ClearAllActions ()
		{
			// wipe it
            LogManager.Instance.Log().Debug("Clearing the event list.");
            timerEvents.Clear();
		}
		#endregion

		#region Ticking
		public static bool StartTicking ()
		{
            LogManager.Instance.Log().Debug(string.Format("Trying to start the ticking. Current state: {0}", isRunning.ToString()));
			if (!isRunning) {
				// start the ticking
				tickThread = new Thread(tick);
				tickThread.Start();
			}
			isRunning = true;
			return true;
		}

		public static bool StopTicking ()
		{
            LogManager.Instance.Log().Debug(string.Format("Trying to stop the ticking. Current state: {0}", isRunning.ToString()));
			if (isRunning) {
				// stop the timer
				isRunning = false;
				tickThread.Abort();
			}
			return false;
		}

		public static void tick ()
		{// tick!
			while (true) {
                LogManager.Instance.Log().Debug("Ticking!");

				// if we're not running, don't tick
                if (!isRunning) {
                    LogManager.Instance.Log().Debug("Timer not running while ticking. Abort.");
					return;
				}

				// make a new list that will replace the one once we're done with our tick
				List<TimerManagerEventset> newList = new List<TimerManagerEventset> ();

				foreach (TimerManagerEventset t in timerEvents) {
					if (DateTime.UtcNow >= t.nextTick) {// we're ready to tick
                        LogManager.Instance.Log().Debug(string.Format("Action ID \"{0}\" is ready to tick", t.Id));

						// cache the old data and add it to the new list after
						TimerManagerEventset t2 = t;
						t2.nextTick = DateTime.UtcNow.AddMilliseconds (t.timeInbetweenTick);
						newList.Add (t2);
                        LogManager.Instance.Log().Debug(string.Format("Setting new tick time for action ID \"{0}\" to {1}", t2.Id, t2.nextTick.ToLongTimeString()));

						// fire
						TimerManagerEventArg param = new TimerManagerEventArg();
						param.tickTime = DateTime.UtcNow;
                        LogManager.Instance.Log().Debug("Firing event");
                        t.executeEvent(new object(), param);
					} else {
						// nothing to do, just put back the data in the list
						newList.Add (t);
					}
				}

				// swap the old list with the new updated one
				timerEvents = newList;

				// sleep
                LogManager.Instance.Log().Debug("Timer Sleeping.");
				Thread.Sleep (1000);
			}
		}
		#endregion

		#region Helper functions
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
		#endregion	
	}
}