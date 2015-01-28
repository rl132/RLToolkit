
// This file has been generated by the GUI designer. Do not modify.
namespace RLToolkit.Widgets
{
	public partial class OutputList
	{
		private global::Gtk.VBox verticalControl;
		private global::Gtk.HBox labelAlignment;
		private global::Gtk.Label lblOutput;
		private global::Gtk.Label lblEmpty;
		private global::Gtk.ScrolledWindow GtkScrolledWindow;
		private global::Gtk.TextView outputList;

        /// <summary>
        /// Build this instance.
        /// </summary>
		protected virtual void Build ()
		{
			global::Stetic.Gui.Initialize (this);
			// Widget RLToolkit.Widgets.OutputList
			global::Stetic.BinContainer.Attach (this);
			this.Name = "RLToolkit.Widgets.OutputList";
			// Container child RLToolkit.Widgets.OutputList.Gtk.Container+ContainerChild
			this.verticalControl = new global::Gtk.VBox ();
			this.verticalControl.Name = "verticalControl";
			this.verticalControl.Spacing = 6;
			// Container child verticalControl.Gtk.Box+BoxChild
			this.labelAlignment = new global::Gtk.HBox ();
			this.labelAlignment.Name = "labelAlignment";
			this.labelAlignment.Spacing = 6;
			// Container child labelAlignment.Gtk.Box+BoxChild
			this.lblOutput = new global::Gtk.Label ();
			this.lblOutput.WidthRequest = 50;
			this.lblOutput.HeightRequest = 16;
			this.lblOutput.Name = "lblOutput";
			this.lblOutput.LabelProp = global::Mono.Unix.Catalog.GetString ("Output:");
			this.labelAlignment.Add (this.lblOutput);
			global::Gtk.Box.BoxChild w1 = ((global::Gtk.Box.BoxChild)(this.labelAlignment [this.lblOutput]));
			w1.Position = 0;
			w1.Expand = false;
			w1.Fill = false;
			// Container child labelAlignment.Gtk.Box+BoxChild
			this.lblEmpty = new global::Gtk.Label ();
			this.lblEmpty.HeightRequest = 16;
			this.lblEmpty.Name = "lblEmpty";
			this.labelAlignment.Add (this.lblEmpty);
			global::Gtk.Box.BoxChild w2 = ((global::Gtk.Box.BoxChild)(this.labelAlignment [this.lblEmpty]));
			w2.Position = 1;
			this.verticalControl.Add (this.labelAlignment);
			global::Gtk.Box.BoxChild w3 = ((global::Gtk.Box.BoxChild)(this.verticalControl [this.labelAlignment]));
			w3.Position = 0;
			w3.Expand = false;
			w3.Fill = false;
			// Container child verticalControl.Gtk.Box+BoxChild
			this.GtkScrolledWindow = new global::Gtk.ScrolledWindow ();
			this.GtkScrolledWindow.Name = "GtkScrolledWindow";
			this.GtkScrolledWindow.ShadowType = ((global::Gtk.ShadowType)(1));
			// Container child GtkScrolledWindow.Gtk.Container+ContainerChild
			this.outputList = new global::Gtk.TextView ();
			this.outputList.CanFocus = true;
			this.outputList.Name = "outputList";
			this.GtkScrolledWindow.Add (this.outputList);
			this.verticalControl.Add (this.GtkScrolledWindow);
			global::Gtk.Box.BoxChild w5 = ((global::Gtk.Box.BoxChild)(this.verticalControl [this.GtkScrolledWindow]));
			w5.Position = 1;
			this.Add (this.verticalControl);
			if ((this.Child != null)) {
				this.Child.ShowAll ();
			}
			this.Hide ();
		}
	}
}
