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
            SetupList(inputList);
        }

        public TestSelectionDialog(List<TestDefinition> inputList)
        {
            this.Build();
            SetupList(inputList);
        }

        private void SetupList(List<string> inputString)
        {
            TreeViewColumn column = new TreeViewColumn();
            CellRendererText cellRendererText = new CellRendererText();
            column.Title = "Select a test to run:";
            column.PackStart((CellRenderer) cellRendererText, true);
            column.AddAttribute((CellRenderer) cellRendererText, "text", 0);
            this.treeview1.AppendColumn(column);
            store = new ListStore(typeof (string), typeof (string));
            this.treeview1.Model = (TreeModel) store;
            foreach (string str in inputString)
            {
                store.AppendValues(str);
            }
        }

        private void SetupList(List<TestDefinition> inputDefs)
        {
            TreeViewColumn column = new TreeViewColumn();
            CellRendererText cellRendererText = new CellRendererText();
            column.Title = "Test to run:";
            column.PackStart((CellRenderer) cellRendererText, true);
            column.AddAttribute((CellRenderer) cellRendererText, "text", 0);
            this.treeview1.AppendColumn(column);

            TreeViewColumn column2 = new TreeViewColumn();
            CellRendererText cellRendererText2 = new CellRendererText();
            column2.Title = "Test Description";
            column2.PackStart(cellRendererText2, true);
            column2.AddAttribute(cellRendererText2, "text", 1);
            this.treeview1.AppendColumn(column2);

            store = new ListStore(typeof (string), typeof (string));
            this.treeview1.Model = (TreeModel) store;
            foreach (TestDefinition info in inputDefs)
            {
                store.AppendValues(info.testName, info.testDesc);
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

