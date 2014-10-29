using System;
using NUnit.Framework;

namespace RLToolkit.UnitTests
{
	public class ExceptionCatcher
	{
		// TODO: add more checks expectations
		public string expectedMessage = "";

		public bool test(Action a)
		{
			try
			{
				a ();
			}
			catch (Exception e)
			{
				if (e.Message != expectedMessage) {
					Assert.Fail ("Exception message failed" + Environment.NewLine + "Was expecting:" + Environment.NewLine + expectedMessage + Environment.NewLine + "Got instead:" + Environment.NewLine + e.Message);
				}

				// TODO: add more checks here

				return true;
			}
			return false;
		}
	}
}