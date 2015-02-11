using System;
using System.Collections.Generic;

using RLToolkit.Widgets;

namespace RLToolkit.WidgetsTests
{
	public class DynamicRowTest : TestBase
	{
        private DynamicRow DynRow1;
        private DynamicRow DynRow2;
        private DynamicRow DynRow3; 
        private int count = 1;

		public DynamicRowTest()
		{
			listTests.Add(DynamicRow_ControlArray());
			listTests.Add(DynamicRow_ChangedMax());
			listTests.Add(DynamicRow_UpdatedArray());
		}

		public TestDefinition DynamicRow_ControlArray()
		{
            DynRow1 = new DynamicRow(typeof(Gtk.Button), new object[] {"button"});

			TestDefinition info = new TestDefinition();

			info.testName = "DynamicRow_ControlArray";
			info.testDesc = "Test that sets a DynamicRow with a control type.";
			info.testWidget = DynRow1;

			return info;
		}

		public TestDefinition DynamicRow_ChangedMax()
		{
            DynRow2 = new DynamicRow(typeof(Gtk.Button), new object[] {"button"});
            DynRow2.UpdateMaxCount(12);

			TestDefinition info = new TestDefinition();

			info.testName = "DynamicRow_ChangedMax";
			info.testDesc = "Test that sets a DynamicRow with a larger number of controls.";
			info.testWidget = DynRow2;

			return info;
		}

		public TestDefinition DynamicRow_UpdatedArray()
		{
            DynRow3 = new DynamicRow(typeof(Gtk.Button), new object[] {"button"});

			TestDefinition info = new TestDefinition();

            info.testName = "DynamicRow_UpdatedArray";
			info.testDesc = "Test that sets a DynamicRow and on the button click, updates the set of control.";
			info.testWidget = DynRow3;
            info.buttonClick1 = OnClickUpdateArray;

			return info;
		}

        protected void OnClickUpdateArray(object sender, EventArgs e)
        {
            Gtk.Widget[] oldArray = DynRow3.GetControlArray();
            Gtk.Widget[] newArray = new Gtk.Widget[oldArray.Length];
            count = 1;
            foreach (Gtk.Button w in oldArray)
            {
                w.Label = "Button " + count;
                newArray[count - 1] = w;
                count++;
            }

            DynRow3.SetControlArray(newArray);
        }

	}
}
