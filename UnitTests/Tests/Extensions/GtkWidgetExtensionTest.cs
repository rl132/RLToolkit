using System;
using Gtk;

using RLToolkit;
using RLToolkit.Extensions;
using NUnit.Framework;

namespace RLToolkit.UnitTests.Extensions
{
	[TestFixture]
	public class GtkWidgetExtensionTest : TestHarness, ITestBase
	{
		#region Local Variables

		#endregion

		#region Interface Override
		public string ModuleName()
		{
			return "GtkWidgetExtension";
		}

		public override void SetFolderPaths()
		{
			localFolder = AppDomain.CurrentDomain.BaseDirectory;
			SetPaths (localFolder, ModuleName());
		}
		#endregion

		#region Tests
        [Test]
        public void ComboBox_NormalUsage()
        {
            Gtk.ComboBox cbo = new Gtk.ComboBox(new[] { "Test1", "Test2" });
            int index = cbo.FindIndex("Test2");

            Assert.AreEqual(1, index, "The value should be found at index 1");
        }

        [Test]
        public void ComboBox_Empty()
        {
            Gtk.ComboBox cbo = new Gtk.ComboBox();
            int index = cbo.FindIndex("Test2");

            Assert.AreEqual(-1, index, "The value should not be found and should return -1");
        }

        [Test]
        public void ComboBox_NotFound()
        {
            Gtk.ComboBox cbo = new Gtk.ComboBox(new[] { "Test1", "Test2" });
            int index = cbo.FindIndex("Test3");

            Assert.AreEqual(-1, index, "The value should not be found and should return -1");
        }

        [Test]
        public void ComboBox_Partial()
        {
            Gtk.ComboBox cbo = new Gtk.ComboBox(new[] { "Test1", "Test2" });
            int index = cbo.FindIndex("Test");

            Assert.AreEqual(-1, index, "The value should not be found and should return -1");
        }
        #endregion	
	}
}

