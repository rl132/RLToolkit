using System;
using System.Collections.Generic;

using RLToolkit.Widgets;

namespace RLToolkit.WidgetsTests
{
	public class GridSelectorTest : TestBase
	{
        private GridSelector selector1 = new GridSelector();
        private Gtk.Button[] selector1Array;
        private GridSelector.eState selector1State;

		public GridSelectorTest()
		{
			listTests.Add(GridSelector_ElementList());
		}

		public TestDefinition GridSelector_ElementList()
        {
            // create the control array
            selector1Array = new Gtk.Button[20];
            for (int i = 0; i< 20; i++)
            {
                Gtk.Button newButton = new Gtk.Button();
                newButton.Label = i.ToString();
                selector1Array[i] = newButton;
            }

            // create the control
            selector1.NbCol = 4;
            selector1.NbRow = 4;
            selector1State = GridSelector.eState.Horizontal;
            selector1.Initialize(GridSelector.eState.Horizontal, selector1Array);

            // prepare the  testDefinition
            TestDefinition info = new TestDefinition();

            info.testName = "GrisSelector_ElementList";
			info.testDesc = "Test where the GridSelector is initialize with an array of controls. Button 1 will switch intween the differnt mode,";
            info.testWidget = selector1;
            info.buttonClick1 = new EventHandler(onClickSel1SwitchMode);
			info.buttonClick2 = new EventHandler(onClickSel1Reduce);
			info.buttonClick3 = new EventHandler(onClickSel1Increase);

            return info;            
        }

		public void onClickSel1SwitchMode(object sender, EventArgs e)
        {
            if (selector1State == GridSelector.eState.Horizontal)
            {
                selector1State = GridSelector.eState.Vertical;
                selector1.SetVerticalMode();
            } else if (selector1State == GridSelector.eState.Vertical)
            {
                selector1State = GridSelector.eState.None;
                selector1.SetNoControlMode();
            } else
            {
                selector1State = GridSelector.eState.Horizontal;
                selector1.SetHorizontalMode();
            }
        }

		public void onClickSel1Reduce(object sender, EventArgs e)
		{
            if (selector1.NbCol >= 20)
            {
                return;
            }
            selector1.NbCol++;
		}

		public void onClickSel1Increase(object sender, EventArgs e)
        {
            if (selector1.NbCol <= 1)
            {
                return;
            }
            selector1.NbCol--;
        }


	}
}