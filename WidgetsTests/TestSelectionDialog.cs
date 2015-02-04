using System;
using System.Collections.Generic;
using Gtk;

namespace RLToolkit.WidgetsTests
{
	public partial class TestSelectionDialog : Gtk.Dialog
	{
		public string SelectedTest = "";
        private ListStore store;

		public TestSelectionDialog(List<string> inputList)
		{
			this.Build();
			TreeViewColumn column = new TreeViewColumn();
			CellRendererText cellRendererText = new CellRendererText();
			column.Title = "Select a test to run:";
			column.PackStart((CellRenderer) cellRendererText, true);
			column.AddAttribute((CellRenderer) cellRendererText, "text", 0);
			this.treeview1.AppendColumn(column);
			store = new ListStore(typeof (string), typeof (string));
			this.treeview1.Model = (TreeModel) store;
            foreach (string str in inputList)
            {
                store.AppendValues(str);
            }
		}

		protected void OnButtonOkClicked(object sender, EventArgs e)
		{
			TreeModel model;
			TreeIter iter;
			if (treeview1.Selection.GetSelected(out model, out iter))
			{
				SelectedTest = model.GetValue(iter, 0).ToString();
			}
			Destroy();
		}

		protected void OnButtonCancelClicked(object sender, EventArgs e)
		{
			this.Destroy();
		}

		protected void OnDeleteEvent(object sender, EventArgs e)
		{
			this.Destroy();
		}
	}
}

