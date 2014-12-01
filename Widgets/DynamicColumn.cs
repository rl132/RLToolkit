using System;

namespace RLToolkit.Widgets
{
    [System.ComponentModel.ToolboxItem(true)]
    public partial class DynamicColumn : Gtk.Bin
    {
        public DynamicColumn()
        {
            this.Build();
        }

        protected void OnBtnMinusClicked (object sender, EventArgs e)
        {
            throw new NotImplementedException ();
        }

        protected void OnBtnPlusClicked (object sender, EventArgs e)
        {
            throw new NotImplementedException ();
        }
    }
}

