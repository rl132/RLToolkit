using System;
using Gtk;

namespace RLToolkit.WidgetsTests
{
    public partial class MainWindow: Gtk.Window
    {    
        public EventHandler oldClick1;
        public EventHandler oldClick2;
        public EventHandler oldClick3;

        public MainWindow(): base (Gtk.WindowType.Toplevel)
        {
            Build();
            lblTestCount.Text = TestList.bigList.Count.ToString();
        }
		
		private bool showSelectionDialog()
		{
			TestSelectionDialog testSelectionDialog = new TestSelectionDialog(TestList.bigList);
			if (testSelectionDialog.Run() == ResponseType.Ok)
			{
				SetTestEnvironment(TestList.FindDefinition(testSelectionDialog.SelectedTest));
			}
			return false;
		}

		public void SetTestEnvironment(TestDefinition input)
		{
			CleanTestArea();
			if (input.buttonClick1 != null)
			{
				btnClick1.Clicked += input.buttonClick1;
				oldClick1 = input.buttonClick1;
			}
			if (input.buttonClick2 != null)
			{
				btnClick2.Clicked += input.buttonClick2;
				oldClick2 = input.buttonClick2;
			}
			if (input.buttonClick3 != null)
			{
				btnClick3.Clicked += input.buttonClick3;
				oldClick3 = input.buttonClick3;
			}
			lblTestDesc.Text = input.testDesc;
			lblTestName.Text = input.testName;
			vboxWidget.Add(input.testWidget);
            foreach (Gtk.Widget w in vboxWidget.Children)
            {
                w.Show();
            }
            vboxWidget.Show();
		}

		private void CleanTestArea()
		{
			foreach (Widget widget in vboxWidget.Children)
			vboxWidget.Remove(widget);
			if (oldClick1 != null)
			{
				btnClick1.Clicked -= oldClick1;
				oldClick1 = null;
			}
			if (oldClick2 != null)
			{
				btnClick2.Clicked -= oldClick2;
				oldClick2 = null;
			}
			if (oldClick3 != null)
			{
				btnClick3.Clicked -= oldClick3;
				oldClick3 = null;
			}
			lblTestDesc.Text = "";
			lblTestName.Text = "";
		}

		protected void OnDeleteEvent(object sender, DeleteEventArgs a)
		{
			Environment.Exit(0);
		}

		protected void OnSelectTestActionActivated(object sender, EventArgs e)
		{
			GLib.Idle.Add(new GLib.IdleHandler(showSelectionDialog));
		}

		protected void OnMnuQuitActivated(object sender, EventArgs e)
		{
			Environment.Exit(0);
		}
    }
}