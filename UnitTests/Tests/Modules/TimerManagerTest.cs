using System;

using RLToolkit;
using RLToolkit.Basic;
using NUnit.Framework;
using System.Threading;

namespace RLToolkit.UnitTests.Modules
{
	[TestFixture]
	public class TimerManagerTest : TestHarness, ITestBase
	{
		#region Local Variables
		private static int countFired = 0; 

		#endregion

		#region Interface Override
		public string ModuleName()
		{
			return "TimerManager";
		}
		#endregion

		#region Test Prepare/Teardown
		[TestFixtureSetUp]
		public void TestFixtureSetUp()
		{
			// make sure we're ticking
            TimerManager.SetDebugVerbose(true);
		}

		[SetUp]
		public void SetUp ()
		{
			countFired = 0;
            TimerManager.ForceStartThread();
            TimerManager.ClearAllEventSets();
		}

		[TearDown]
		public void TearDown ()
		{
            TimerManager.ClearAllEventSets();
		}
		#endregion

		#region Helper Methods
		public void IncreaseCount (object sender, TimerManagerEventArg e)
		{
			countFired++;
		}
		#endregion

		#region Tests-ActionSections
		[Test]
		public void Timer_Action_AddEvent()
		{
            bool add = TimerManager.AddEventSet("test1", 3000, IncreaseCount, false);
			Assert.AreEqual(1, TimerManager.GetEventSetsCount(), "Timer event count should be 1");
			Assert.AreEqual(true, add, "Adding should have been successful");
		}

		[Test]
		public void Timer_Action_RemoveEvent()
		{
			Assert.AreEqual(0, TimerManager.GetEventSetsCount(), "Timer event count should be 0 at the start");

			TimerManager.AddEventSet("test2", 3000, IncreaseCount, false);
			Assert.AreEqual(true, TimerManager.IsIdentExist("test2"), "The manager should find the 'Test2' ID");

			bool rem = TimerManager.RemoveEventSet("test2");
			Assert.AreEqual(true, rem, "Removing should have been successful");
			Assert.AreEqual(0, TimerManager.GetEventSetsCount(), "Timer event count should be 0 after removing");
		}

		[Test]
		public void Timer_Action_ClearEvent()
		{
			TimerManager.AddEventSet("test3a", 3000, IncreaseCount, false);
			TimerManager.AddEventSet("test3b", 3000, IncreaseCount, false);
			TimerManager.AddEventSet("test3c", 3000, IncreaseCount, false);
			Assert.AreEqual(3, TimerManager.GetEventSetsCount(), "Timer event count should be 3");

			TimerManager.ClearAllEventSets();
			Assert.AreEqual(0, TimerManager.GetEventSetsCount(), "Timer event count should be 0 after clearing");
		}
		#endregion

		#region Tests-Ticking
		[Test]
		public void Timer_Ticking_SmallTimeNoSleepNotInstant()
		{
			// no sleep, unlikely to trigger
			TimerManager.AddEventSet("test4", 250, IncreaseCount, false);
			Assert.AreEqual(0, countFired, "Count should not have been fired here.");
		}

		[Test]
		public void Timer_Ticking_SmallTimeNoSleepInstant()
		{
			// since no sleep, not likely to trigger here.
			TimerManager.AddEventSet("test5", 250, IncreaseCount, true);
			Assert.AreEqual(0, countFired, "Count should not have been fired here.");
		}

		[Test]
		public void Timer_Ticking_SmallTime3_5sSleepNotInstant()
        {   
            int count = 0;
            ManualResetEvent mre = new ManualResetEvent(false);

            TimerManager.AddEventSet("test6", 250, delegate(object sender, TimerManagerEventArg e)
                                   {
                count++;
                if (count >= 3)
                {
                    mre.Set();
                }
            }
            , false);
			mre.WaitOne(3500, false);

			Assert.GreaterOrEqual(count, 3, "Count should have fired 3 times already.");
		}

		[Test]
		public void Timer_Ticking_SmallTime3sSleepInstant()
		{
            int count = 0;
            ManualResetEvent mre = new ManualResetEvent(false);

            TimerManager.AddEventSet("test7", 250, delegate(object sender, TimerManagerEventArg e)
                                   {
                count++;
                if (count >= 3)
                {
                    mre.Set();
                }
            }
            , true);
            mre.WaitOne(3000, false);

            Assert.GreaterOrEqual(count, 3, "Count should have fired 3 times already.");
		}

		[Test]
		public void Timer_Ticking_LongTime3sSleepNotInstant()
		{
            int count = 0;
            ManualResetEvent mre = new ManualResetEvent(false);

            TimerManager.AddEventSet("test8", 2000, delegate(object sender, TimerManagerEventArg e)
                                   {
                count++;
                if (count >= 1)
                {
                    mre.Set();
                }
            }
            , false);
            mre.WaitOne(3000, false);

            Assert.GreaterOrEqual(count, 1, "Count should have fired once.");
		}

		[Test]
		public void Timer_Ticking_LongTime3sSleepInstant()
		{
            int count = 0;
            ManualResetEvent mre = new ManualResetEvent(false);

            TimerManager.AddEventSet("test9", 1600, delegate(object sender, TimerManagerEventArg e)
                                   {
                count++;
                if (count >= 2)
                {
                    mre.Set();
                }
            }
            , true);
            mre.WaitOne(4000, false);

            Assert.GreaterOrEqual(count, 2, "Count should have fired 2 times already");
		}

		[Test]
		public void Timer_Pause_StartWaitStopWait()
		{
            int count = 0;
            ManualResetEvent mre = new ManualResetEvent(false);

            TimerManager.AddEventSet("test11", 250, delegate(object sender, TimerManagerEventArg e)
                                   {
                count++;
                mre.Set();
            }
            , false);
            mre.WaitOne(1500, false);

            Assert.GreaterOrEqual(count, 1, "Count should have fired once.");

            mre.Reset();
            int countSnapshot = count;
            TimerManager.PauseIdent("test11", true);

            mre.WaitOne(1500, false);

            Assert.AreEqual(count, countSnapshot, "Count should still be at 1 after the stop");
		}

        [Test]
        public void Timer_Pause_StartPauseWaitUnpauseWait()
        {
            int count = 0;
            ManualResetEvent mre = new ManualResetEvent(false);

            TimerManager.AddEventSet("test12", 250, delegate(object sender, TimerManagerEventArg e)
                                     {
                count++;
                mre.Set();
            }
            , false);
            mre.WaitOne(1500, false);

            Assert.GreaterOrEqual(count, 1, "Count should have fired once.");

            mre.Reset();
            int countSnapshot = count;
            TimerManager.PauseIdent("test12", true);

            mre.WaitOne(1500, false);

            Assert.AreEqual(count, countSnapshot, "Count should still be at 1 after the stop");

            mre.Reset();
            TimerManager.PauseIdent("test12", false);

            mre.WaitOne(1500, false);

            Assert.GreaterOrEqual(count, countSnapshot, "Count should have increased");
        }

        [Test]
        public void Timer_Pause_Wait()
        {
            int count = 0;
            ManualResetEvent mre = new ManualResetEvent(false);

            TimerManager.AddEventSet("test13", 250, delegate(object sender, TimerManagerEventArg e)
                                     {
                // so we're sure it fired
                count = 5;
                mre.Set();
            }
            , false);
            TimerManager.PauseIdent("test13", true);

            mre.WaitOne(3000, false);
            Assert.AreEqual(count, 0, "Count should be 0.");
        }

        [Test]
        public void Timer_Pause_IdentNotFound()
        {
            int count = 0;
            ManualResetEvent mre = new ManualResetEvent(false);

            TimerManager.AddEventSet("test14", 250, delegate(object sender, TimerManagerEventArg e)
                                     {
                count++;
                mre.Set();
            }
            , false);

            bool isFound = TimerManager.PauseIdent("FoobarNotFound", true);
            Assert.AreEqual(false, isFound, "The event 'FoobarNotFound' shouldn't be found");

            isFound = TimerManager.PauseIdent("test14", true);
            Assert.AreEqual(true, isFound, "The event 'test14' should be found");
        }

        [Test]
        public void Timer_Count_Normal()
        {
            int count = 0;
            ManualResetEvent mre = new ManualResetEvent(false);

            TimerManager.AddEventSet("test15", 250, delegate(object sender, TimerManagerEventArg e)
                                     {
                // so we're sure it fired
                count++;
                // never set the MRE so it waits the full time
            }
            , false);

            int countNow = TimerManager.GetTickCounter();
            mre.WaitOne(3000, false);
            Assert.Greater(TimerManager.GetTickCounter(), countNow, "Tick Count should be greater than previously");
        }
        
        [Test]
        public void Timer_Count_Paused()
        {
            int count = 0;
            ManualResetEvent mre = new ManualResetEvent(false);

            TimerManager.AddEventSet("test16", 250, delegate(object sender, TimerManagerEventArg e)
                                     {
                // so we're sure it fired
                count++;
                // never set the MRE so it waits the full time
            }
            , false);

            TimerManager.PauseIdent("test16", true);
            int countNow = TimerManager.GetTickCounter();
            mre.WaitOne(3000, false);
            Assert.AreEqual(countNow, TimerManager.GetTickCounter(), "Tick Count should be the same.");
        }
        
        [Test]
        public void Timer_EventSetFetch_Normal()
        {
            TimerManager.AddEventSet("test17", 2456, IncreaseCount, false);
            TimerManagerEventset retVal = TimerManager.GetEventSetByID("test17");


            Assert.AreEqual(2456, retVal.timeInbetweenTick, "Ticking time should be the right thing");
            Assert.AreEqual("test17", retVal.Id, "ID should be the right thing");
            Assert.AreEqual(false, retVal.isPaused, "Pause status should be the right thing");
        }

        [Test]
        public void Timer_EventSetFetch_NotFound()
        {
            TimerManager.AddEventSet("test18", 250, IncreaseCount, false);
            TimerManagerEventset retVal = TimerManager.GetEventSetByID("test_foo");


            Assert.AreEqual(null, retVal, "value returned by the method should be null");
        }
        #endregion
	}
}