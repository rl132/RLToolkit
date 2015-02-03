using System;
using System.Collections.Generic;

using RLToolkit.Widgets;

namespace RLToolkit.Widgets.Tests
{
	public class LabelTest : TestBase
	{
		private Gtk.Label lbl1 = new Gtk.Label();
		private int count;

		public LabelTest()
		{
			listTests.Add(test1());
		}

		public TestDefinition test1()
		{
			lbl1.Text = "Test";
			
			TestDefinition info = new TestDefinition();

			info.testName = "LabelTest_Normal";
			info.testDesc = "Test that uses the GTK.Label as a Proof Of Concept";
			info.testWidget = lbl1;
			info.buttonClick1 = new EventHandler(onClickIncrement);
			info.buttonClick2 = new EventHandler(onClickZero);
			
			return info;
			
		}

		public void onClickIncrement(object sender, EventArgs e)
		{
			lbl1.Text = "Test #" + count;
			count++;
		}

		public void onClickZero(object sender, EventArgs e)
		{
			count = 0;
			lbl1.Text = "Test #" + count;
		}
	}
}
