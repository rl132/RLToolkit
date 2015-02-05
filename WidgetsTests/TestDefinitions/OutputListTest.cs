using System;
using System.Collections.Generic;

using RLToolkit.Widgets;

namespace RLToolkit.WidgetsTests
{
	public class OutputListTest : TestBase
	{
        private OutputList ol1 = new OutputList();
        private int count1;
        private OutputList ol2 = new OutputList();
        private int count2;
        private OutputList ol3 = new OutputList();
        private int count3;

		public OutputListTest()
		{
            listTests.Add(OutputList_NormalSpeed());
            listTests.Add(OutputList_FastSpeed());
            listTests.Add(OutputList_SlowSpeed());
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
            info.buttonClick1 = new EventHandler(onClickOl1AddText);
            info.buttonClick2 = new EventHandler(onClickOl1ClearText);

            return info;            
        }

        public void onClickOl1AddText(object sender, EventArgs e)
        {
            ol1.QueueData("Touch #" + count1);
            count1++;
        }

        public void onClickOl1ClearText(object sender, EventArgs e)
        {
            ol1.Clear();
        }

        public TestDefinition OutputList_FastSpeed()
        {
            // create the control
            ol2.Identifier = "WidgetTest2";
            ol2.TickSpeed = 100;
            ol2.Initialize();

            // prepare the testDefinition
            TestDefinition info = new TestDefinition();

            info.testName = "OutputList_Fast";
            info.testDesc = "Test where the OutputList is put in 'fast' conditioons. Button 1 will add new text, Button 2 will Clear, and Button 3 will not be used.";
            info.testWidget = ol2;
            info.buttonClick1 = new EventHandler(onClickOl2AddText);
            info.buttonClick2 = new EventHandler(onClickOl2ClearText);

            return info;            
        }

        public void onClickOl2AddText(object sender, EventArgs e)
        {
            ol2.QueueData("Touch #" + count2);
            count2++;
        }

        public void onClickOl2ClearText(object sender, EventArgs e)
        {
            ol2.Clear();
        }

        public TestDefinition OutputList_SlowSpeed()
        {
            // create the control
            ol3.Identifier = "WidgetTest3";
            ol3.TickSpeed = 3000;
            ol3.Initialize();

            // prepare the  testDefinition
            TestDefinition info = new TestDefinition();

            info.testName = "OutputList_Slow";
            info.testDesc = "Test where the OutputList is put in 'slow' conditioons. Button 1 will add new text, Button 2 will Clear, and Button 3 will not be used.";
            info.testWidget = ol3;
            info.buttonClick1 = new EventHandler(onClickOl3AddText);
            info.buttonClick2 = new EventHandler(onClickOl3ClearText);

            return info;            
        }

        public void onClickOl3AddText(object sender, EventArgs e)
        {
            ol3.QueueData("Touch #" + count3);
            count3++;
        }

        public void onClickOl3ClearText(object sender, EventArgs e)
        {
            ol3.Clear();
        }
	}
}