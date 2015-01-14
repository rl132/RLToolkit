using System;
using System.Collections.Generic;

using RLToolkit.Basic;
using RLToolkit.Logger;
using Gtk;
using System.Text;

namespace RLToolkit.Widgets
{
    /// <summary>
    /// Widget that is used to do an async list-like status/output.
    /// </summary>
    [System.ComponentModel.ToolboxItem(true)]
    public partial class OutputList : Gtk.Bin
    {
        #region properties
        // TODO: make sure it behave nicely in the stetic gui designer
        /// <summary>
        /// Proerty used to set the outputList identifier (used in the timer system). 
        /// </summary>
        /// <remarks>Make sure this is unique</remarks>
        /// <value>The identifier.</value>
        public string Identifier
        {
            get {
                if (identifier == "")
                {
                    this.Log().Warn("Invalid ID for Output List, using default");
                    return "OutputListID";
                }
                return identifier;
            }
            set {
                identifier = value;
            }
        }

        // TODO: make sure it behave nicely in the stetic gui designer
        private const int MINIMUM_TICK_SPEED = 250;
        /// <summary>
        /// Property used to set the ticking speed used for the timer.
        /// </summary>
        /// <remarks>Make sure this is at least 250 (miliseconds)</remarks>
        /// <value>The ticking speed.</value>
        public int TickSpeed
        {
            get {
                if (tickSpeed < 250)
                {
                    return MINIMUM_TICK_SPEED;
                }
                return tickSpeed;
            }
            set {
                tickSpeed = value;
            }
        }
        #endregion

        #region Variables
        // Properties
        private string identifier = "OutputListId";
        private int tickSpeed = 500;

        // Global variables
        private List<string> bufferToAdd = new List<string>();
        private bool isTicking = false;
        private bool isRegistered = false;
        #endregion

        #region Constructor/Init
        /// <summary>
        /// Constructor for the OutputList.
        /// </summary>
        public OutputList()
        {
            this.Log().Debug("initializing a new OutputList");
            this.Build();
        }

        /// <summary>
        /// Method to call to initialize the ticking and register the output list in the timer moduke
        /// </summary>
        public void Initialize()
        {
            // TODO: see if we can't put that somewhere in the constructor to ease life and eventually deprecate this.
            this.Log().Debug("initializing the outputlist");

            // register 
            RegisterEvent();

            // Set the timer to live and set it delay
            StartTick();
        }

        /// <summary>
        /// Method to update the delay of the ticking used in the timer
        /// </summary>
        /// <param name="delayMs">Delay in ms.</param>
        public void SetTimerSpeed(int delayMs)
        {
            // TODO: update this. this is not working anymore
            this.Log().Debug("changing the outputlist delay to " + delayMs.ToString() + "ms");
            // cache the new speed
            tickSpeed = delayMs;

            // set a new timer delay in case we want faster or slower for our ticking
            StartTick();
        }
        #endregion

        #region Event Reg/Unreg.
        private void RegisterEvent()
        {
            if (isRegistered)
            {
                this.Log().Warn("Trying to register twice");
                return;
            }
            this.Log().Info("Event is being registered.");
            isRegistered = TimerManager.AddEventSet(identifier, tickSpeed, Ticking, false);        
            if (!isRegistered)
            {
                this.Log().Warn("Event failed to register.");
            } else
            {
                // set the event to pause
                TimerManager.PauseIdent(identifier, true);
            }
        }

        private void UnregisterEvent()
        {
            if (!isRegistered)
            {
                this.Log().Warn("Trying to Unregister twice");
                return;
            }
            this.Log().Info("Event is being unregistered.");
            bool success = TimerManager.RemoveEventSet(identifier);
            if (!success)
            {
                this.Log().Warn("Event failed to unregister.");
            } else
            {
                isRegistered = false;
            }
        }
        #endregion

        #region Start/Stop Ticking
        private void StartTick()
        {
            this.Log().Debug("Trying to start the ticking");

            // unpause
            isTicking = true;
            TimerManager.PauseIdent(identifier, false);
        }

        private void StopTick()
        {
            this.Log().Debug("Trying to stop the ticking");

            isTicking = false;
            TimerManager.PauseIdent(identifier, true);
        }
        #endregion

        #region Data Methods
        /// <summary>
        /// Method to queue the new data to output
        /// </summary>
        /// <param name="data">the input string to add</param>
        public void QueueData(string data)
        {
            this.Log().Debug("Adding data to buffer:" + Environment.NewLine + data);

            if (!isTicking)
            {
                // if we were sleeping, wake up!
                StartTick();
            }

            // put the data to be added to the list
            bufferToAdd.Add(data);
        }

        /// <summary>
        /// Method to clear the output List.
        /// </summary>
        public void Clear()
        {
            this.Log().Debug("Clearing the buffer and output");
            // clean the output list, queued messages and buffer
            bufferToAdd.Clear();
            outputList.Buffer.Clear();

            // force to sleep
            isTicking = false;
        }
        #endregion

        #region Tick
        private void Ticking(object sender, TimerManagerEventArg e)
        {
            this.Log().Debug("Ticking at " + e.tickTime.ToString());

            if (bufferToAdd.Count == 0)
            {
                this.Log().Debug("No more data to add, stopping the ticking");
                // don't tick unnecesarly
                StopTick();
                return;
            }

            // duplicate the queue so new stuff can be added
            List<string> toAdd = new List<string>(bufferToAdd);
            bufferToAdd = new List<string>();

            // add what's in the buffer
            this.Log().Debug("Trying to add " + toAdd.Count.ToString() + " items");

            // invoke to make sure it runs on the UI thread (and update the UI properly)
            Gtk.Application.Invoke (delegate {
                TextIter mIter = outputList.Buffer.EndIter;
                StringBuilder stringToAdd = new StringBuilder(""); // TODO: not sure if we should do this. to see
                foreach (string s in toAdd)
                {
                    // add the line + a new line
                    stringToAdd.Append(s + Environment.NewLine);
                }
                outputList.Buffer.Insert(ref mIter, stringToAdd.ToString());
                stringToAdd.Clear();
                outputList.ScrollToIter(outputList.Buffer.EndIter, 0, false, 0, 0);
            });
        }
        #endregion
    }
}