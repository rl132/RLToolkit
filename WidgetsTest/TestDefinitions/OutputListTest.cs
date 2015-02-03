using System;
using System.Collections.Generic;

using RLToolkit.Widgets;

namespace RLToolkit.Widgets.Tests
{
	public class OutputListTest : TestBase
	{
		private OutputList ol1 = new OutputList();
		private int count;

		public OutputListTest()
		{
			listTests.Add(OutputList_NormalSpeed());
		}

		public TestDefinition OutputList_NormalSpeed()
		{
			// create the control
			ol1.Identifier = "WidgetTest1";
			ol1.TickSpeed = 500;
			ol1.Initialize();

			// prepare the  testDefinition
			TestDefinition info = new TestDefinition();
			
			info.testName = "OutputList_Normal";
			info.testDesc = "Test where the OutputList is put in normal conditioons. Button 1 will add new text, Button 2 will Clear, and Button 3 will not be used.";
			info.testWidget = ol1;
			info.buttonClick1 = new EventHandler(onClickAddText);
			info.buttonClick2 = new EventHandler(onClickClearText);
			
			return info;			
		}

		public void onClickAddText(object sender, EventArgs e)
		{
			ol1.QueueData("Touch #" + count);
			count++;
		}

		public void onClickClearText(object sender, EventArgs e)
		{
			ol1.Clear();
		}
	}
}