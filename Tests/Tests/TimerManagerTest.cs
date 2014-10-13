using System;

using RLToolkit;
using RLToolkit.Basic;
using NUnit.Framework;
using System.Threading;

namespace RLToolkit.Tests
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
			TimerManager.StartTicking();
		}

		[SetUp]
		public void SetUp ()
		{
			countFired = 0;
			TimerManager.StopTicking();
		}

		[TearDown]
		public void TearDown ()
		{
			TimerManager.ClearAllActions();
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
			bool add = TimerManager.AddAction("test1", 3000, IncreaseCount, false);
			Assert.AreEqual(1, TimerManager.GetActionCount(), "Timer event count should be 1");
			Assert.AreEqual(true, add, "Adding should have been successful");
		}

		[Test]
		public void Timer_Action_RemoveEvent()
		{
			Assert.AreEqual(0, TimerManager.GetActionCount(), "Timer event count should be 0 at the start");

			TimerManager.AddAction("test2", 3000, IncreaseCount, false);
			Assert.AreEqual(true, TimerManager.IsIdentExist("test2"), "The manager should find the 'Test2' ID");

			bool rem = TimerManager.RemoveAction("test2");
			Assert.AreEqual(true, rem, "Removing should have been successful");
			Assert.AreEqual(0, TimerManager.GetActionCount(), "Timer event count should be 0 after removing");
		}

		[Test]
		public void Timer_Action_ClearEvent()
		{
			TimerManager.AddAction("test3a", 3000, IncreaseCount, false);
			TimerManager.AddAction("test3b", 3000, IncreaseCount, false);
			TimerManager.AddAction("test3c", 3000, IncreaseCount, false);
			Assert.AreEqual(3, TimerManager.GetActionCount(), "Timer event count should be 3");

			TimerManager.ClearAllActions();
			Assert.AreEqual(0, TimerManager.GetActionCount(), "Timer event count should be 0 after clearing");
		}
		#endregion

		#region Tests-Ticking
		[Test]
		public void Timer_Ticking_SmallTimeNoSleepNotInstant()
		{
			// no sleep, unlikely to trigger
			TimerManager.AddAction("test4", 250, IncreaseCount, false);
			TimerManager.StartTicking();

			Assert.AreEqual(0, countFired, "Count should not have been fired here.");
		}

		[Test]
		public void Timer_Ticking_SmallTimeNoSleepInstant()
		{
			// since no sleep, not likely to trigger here.
			TimerManager.AddAction("test5", 250, IncreaseCount, true);
			TimerManager.StartTicking();

			Assert.AreEqual(0, countFired, "Count should not have been fired here.");
		}

		[Test]
		public void Timer_Ticking_SmallTime3_5sSleepNotInstant()
        {   

            int count = 0;
            ManualResetEvent mre = new ManualResetEvent(false);

            TimerManager.AddAction("test6", 250, delegate(object sender, TimerManagerEventArg e)
                                   {
                count++;
                if (count >= 3)
                {
                    mre.Set();
                }
            }
            , false);
			TimerManager.StartTicking();

            mre.WaitOne(3500, false);

			Assert.GreaterOrEqual(count, 3, "Count should have fired 3 times already.");
		}

		[Test]
		public void Timer_Ticking_SmallTime3sSleepInstant()
		{
            int count = 0;
            ManualResetEvent mre = new ManualResetEvent(false);

            TimerManager.AddAction("test7", 250, delegate(object sender, TimerManagerEventArg e)
                                   {
                count++;
                if (count >= 3)
                {
                    mre.Set();
                }
            }
            , true);
            TimerManager.StartTicking();

            mre.WaitOne(3000, false);

            Assert.GreaterOrEqual(count, 3, "Count should have fired 3 times already.");
		}

		[Test]
		public void Timer_Ticking_LongTime3sSleepNotInstant()
		{
            int count = 0;
            ManualResetEvent mre = new ManualResetEvent(false);

            TimerManager.AddAction("test8", 2000, delegate(object sender, TimerManagerEventArg e)
                                   {
                count++;
                if (count >= 1)
                {
                    mre.Set();
                }
            }
            , false);
            TimerManager.StartTicking();

            mre.WaitOne(3000, false);

            Assert.GreaterOrEqual(count, 1, "Count should have fired once.");
		}

		[Test]
		public void Timer_Ticking_LongTime3sSleepInstant()
		{
            int count = 0;
            ManualResetEvent mre = new ManualResetEvent(false);

            TimerManager.AddAction("test9", 1600, delegate(object sender, TimerManagerEventArg e)
                                   {
                count++;
                if (count >= 2)
                {
                    mre.Set();
                }
            }
            , true);
            TimerManager.StartTicking();

            mre.WaitOne(4000, false);

            Assert.GreaterOrEqual(count, 2, "Count should have fired 2 times already");
		}

		[Test]
		public void Timer_StartStop_Basic()
		{
            int count = 0;
            ManualResetEvent mre = new ManualResetEvent(false);

            TimerManager.AddAction("test10", 250, delegate(object sender, TimerManagerEventArg e)
                                   {
                count++;
                mre.Set();
            }
            , false);
            TimerManager.StartTicking();
            TimerManager.StopTicking();

            mre.WaitOne(3000, false);

            Assert.GreaterOrEqual(count, 0, "Count should not have fired at all.");
		}

		[Test]
		public void Timer_StartStop_StartWaitStopWait()
		{
            int count = 0;
            ManualResetEvent mre = new ManualResetEvent(false);

            TimerManager.AddAction("test11", 250, delegate(object sender, TimerManagerEventArg e)
                                   {
                count++;
                mre.Set();
            }
            , false);
            TimerManager.StartTicking();

            mre.WaitOne(1500, false);

            Assert.GreaterOrEqual(count, 1, "Count should have fired once.");

            mre.Reset();
            int countSnapshot = count;
            TimerManager.StopTicking();

            mre.WaitOne(1500, false);

            Assert.GreaterOrEqual(count, countSnapshot, "Count shoulkd still be at 1 after the stop");
		}
		#endregion
	}
}