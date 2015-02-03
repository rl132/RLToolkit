using System;

namespace RLToolkit.Widgets.Tests
{
	public class TestDefinition
	{
		public string testName = "Undefined";
		public string testDesc = "Undefined";
		public Gtk.Widget testWidget = new Gtk.Label("Undefined");
		public EventHandler buttonClick1 = null;
		public EventHandler buttonClick2 = null;
		public EventHandler buttonClick3 = null;

		public TestDefinition (
			string name,
			string desc,
			Gtk.Widget widget,
			EventHandler click1,
			EventHandler click2,
			EventHandler click3
			)
		{
			testName = name;
			testDesc = desc;
			testWidget = widget;
			buttonClick1 = click1;
			buttonClick2 = click2;
			buttonClick3 = click3;
		}

		public TestDefinition (
			string name,
			string desc,
			Gtk.Widget widget,
			EventHandler click1,
			EventHandler click2
			) : this(
			name, desc, widget, click1, click2, null)
		{
			// using the base contructor.
		}

		public TestDefinition (
			string name,
			string desc,
			Gtk.Widget widget,
			EventHandler click1
			) : this(
			name, desc, widget, click1, null, null)
		{
			// using the base contructor.
		}

		public TestDefinition (
			string name,
			string desc,
			Gtk.Widget widget
			) : this(
			name, desc, widget, null, null, null)
		{
			// using the base contructor.
		}
		
		public TestDefinition (
			string name,
			string desc
			) : this(
			name, desc, new Gtk.Label("Undefined"), null, null, null)
		{
			// using the base contructor.
		}
		
		public TestDefinition (
			string name
			) : this(
			name, "Undefined", new Gtk.Label("Undefined"), null, null, null)
		{
			// using the base contructor.
		}

		public TestDefinition (
			) : this(
			"Undefined", "Undefined", new Gtk.Label("Undefined"), null, null, null)
		{
			// using the base contructor.
		}
	}
}

