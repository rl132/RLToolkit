using System;
using System.Collections.Generic;

using RLToolkit.Widgets;

namespace RLToolkit.WidgetsTests
{
    public class DynamicColumnTest : TestBase
    {
        private DynamicColumn DynCol1;
        private DynamicColumn DynCol2;
        private DynamicColumn DynCol3; 
        private int count = 1;

        public DynamicColumnTest()
        {
            listTests.Add(DynamicColumn_ControlArray());
            listTests.Add(DynamicColumn_ChangedMax());
            listTests.Add(DynamicColumn_UpdatedArray());
        }

        public TestDefinition DynamicColumn_ControlArray()
        {
            DynCol1 = new DynamicColumn(typeof(Gtk.Button), new object[] {"button"});

            TestDefinition info = new TestDefinition();

            info.testName = "DynamicColumn_ControlArray";
            info.testDesc = "Test that sets a DynamicColumn with a control type.";
            info.testWidget = DynCol1;

            return info;
        }

        public TestDefinition DynamicColumn_ChangedMax()
        {
            DynCol2 = new DynamicColumn(typeof(Gtk.Button), new object[] {"button"});
            DynCol2.UpdateMaxCount(12);

            TestDefinition info = new TestDefinition();

            info.testName = "DynamicColumn_ChangedMax";
            info.testDesc = "Test that sets a DynamicColumn with a larger number of controls.";
            info.testWidget = DynCol2;

            return info;
        }

        public TestDefinition DynamicColumn_UpdatedArray()
        {
            DynCol3 = new DynamicColumn(typeof(Gtk.Button), new object[] {"button"});

            TestDefinition info = new TestDefinition();

            info.testName = "DynamicColumn_UpdatedArray";
            info.testDesc = "Test that sets a DynamicColumn and on the button click, updates the set of control.";
            info.testWidget = DynCol3;
            info.buttonClick1 = OnClickUpdateArray;

            return info;
        }

        protected void OnClickUpdateArray(object sender, EventArgs e)
        {
            Gtk.Widget[] oldArray = DynCol3.GetControlArray();
            Gtk.Widget[] newArray = new Gtk.Widget[oldArray.Length];
            count = 1;
            foreach (Gtk.Button w in oldArray)
            {
                w.Label = "Button " + count;
                newArray[count - 1] = w;
                count++;
            }

            DynCol3.SetControlArray(newArray);
        }

    }
}
