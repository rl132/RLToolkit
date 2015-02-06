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
 
        private GridSelector selector2 = new GridSelector();
        private Gtk.Button[] selector2Array;
        private GridSelector.eState selector2State;

		public GridSelectorTest()
		{
			listTests.Add(GridSelector_ElementList());
            listTests.Add(GrisSelector_EmptyElements());
		}

        public TestDefinition GridSelector_ElementList()
        {
            // create the control array
            selector1Array = new Gtk.Button[18];
            for (int i = 0; i< 18; i++)
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

        public TestDefinition GrisSelector_EmptyElements()
        {
            // create the control array
            selector2Array = new Gtk.Button[18];
            for (int i = 0; i< 18; i++)
            {
                Gtk.Button newButton = new Gtk.Button();
                newButton.Label = i.ToString();
                selector2Array[i] = newButton;
            }

            // create the control
            selector2.NbCol = 4;
            selector2.NbRow = 4;
            selector2State = GridSelector.eState.Horizontal;
            selector2.SetFillerControlType(typeof(Gtk.Button));
            selector2.Initialize(GridSelector.eState.Horizontal, selector2Array);

            // prepare the  testDefinition
            TestDefinition info = new TestDefinition();

            info.testName = "GrisSelector_EmptyElements";
            info.testDesc = "Test where the GridSelector has a disabled button when the max element is being reached. Button 1 will switch intween the differnt mode,";
            info.testWidget = selector2;
            info.buttonClick1 = new EventHandler(onClickSel2SwitchMode);

            return info;            
        }

        public void onClickSel2SwitchMode(object sender, EventArgs e)
        {
            if (selector2State == GridSelector.eState.Horizontal)
            {
                selector2State = GridSelector.eState.Vertical;
                selector2.SetVerticalMode();
            } else if (selector2State == GridSelector.eState.Vertical)
            {
                selector2State = GridSelector.eState.None;
                selector2.SetNoControlMode();
            } else
            {
                selector2State = GridSelector.eState.Horizontal;
                selector2.SetHorizontalMode();
            }
        }

    }
}