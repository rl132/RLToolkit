using System;
using System.Collections.Generic;
using System.IO;

using RLToolkit.Widgets;
using RLToolkit.Extensions;
using System.Drawing;
using Gtk;

namespace RLToolkit.WidgetsTests
{
	public class ButtonImageTest : TestBase
	{
        private ButtonImage bImg1;
        private ButtonImage bImg2;
        private ButtonImage bImg3;

		public ButtonImageTest()
		{
			listTests.Add(ButtonImage_NormalUsage());
			listTests.Add(ButtonImage_ChangeSize());
			listTests.Add(ButtonImage_LoadImages());
		}

		public TestDefinition ButtonImage_NormalUsage()
        {
            // create the control
            bImg1 = new ButtonImage();

            // prepare the  testDefinition
            TestDefinition info = new TestDefinition();

            info.testName = "ButtonImage_NormalUsage";
            info.testDesc = "Test where the button Image is used in normal conditions. B1 will get the image, B2 will get the filename, B3 will clear.";
            info.testWidget = bImg1;
            info.buttonClick1 = new EventHandler(onClick1GetImage);
            info.buttonClick2 = new EventHandler(onClick1GetFilename);
            info.buttonClick3 = new EventHandler(onClick1Clear);

            return info;            
        }

        public void onClick1GetImage(object sender, EventArgs e)
        {
            // fetch the image
            Bitmap image = bImg1.GetImage();
            if (image == null)
            {
                return;
            }

            Gdk.Pixbuf img = image.ToPixbuf();

            MessageDialog result = new MessageDialog (new Gtk.Window(WindowType.Popup), DialogFlags.Modal, MessageType.Error, ButtonsType.Ok, "Image result");
            result.Image = new Gtk.Image(img);
            result.Image.ShowAll();

            if (result.Run () == (int)ResponseType.Ok) {
                result.Destroy ();
            }
        }

        public void onClick1GetFilename(object sender, EventArgs e)
        {
            string text = bImg1.GetFilename();
            MessageDialog result = new MessageDialog (new Gtk.Window(WindowType.Popup), DialogFlags.Modal, MessageType.Error, ButtonsType.Ok, "Value of filename: " + text);
            if (result.Run () == (int)ResponseType.Ok) {
                result.Destroy ();
            }
        }

        public void onClick1Clear(object sender, EventArgs e)
        {
            bImg1.Clear();
        }

        public TestDefinition ButtonImage_ChangeSize()
        {
            // create the control
            bImg2 = new ButtonImage(16);

            // prepare the testDefinition
            TestDefinition info = new TestDefinition();

            info.testName = "ButtonImage_ChangeSize";
            info.testDesc = "Test where the button Image is Switching the size of the image. B1 is 16px, B2 is 32px, B3 is 128px";
            info.testWidget = bImg2;
            info.buttonClick1 = new EventHandler(onClick2set16);
            info.buttonClick2 = new EventHandler(onClick2set32);
            info.buttonClick3 = new EventHandler(onClick2set128);

            return info;            
        }

        public void onClick2set16(object sender, EventArgs e)
        {
            bImg2.UpdateSize(16);
        }

        public void onClick2set32(object sender, EventArgs e)
        {
            bImg2.UpdateSize(32);
        }

        public void onClick2set128(object sender, EventArgs e)
        {
            bImg2.UpdateSize(128);
        }

        public TestDefinition ButtonImage_LoadImages()
        {
            // create the control
            bImg3 = new ButtonImage();


            // prepare the  testDefinition
            TestDefinition info = new TestDefinition();

            info.testName = "ButtonImage_LoadImages";
            info.testDesc = "Test where the button Image is used to display an image. B1 will set an image, B2 will set a stock, B3 will clear.";
            info.testWidget = bImg3;
            info.buttonClick1 = new EventHandler(onClick3SetImage);
            info.buttonClick2 = new EventHandler(onClick3SetStock);
            info.buttonClick3 = new EventHandler(onClick3Clear);

            return info;            
        }

        public void onClick3SetImage(object sender, EventArgs e)
        {
            bImg3.SetImage(new Bitmap(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestData", "ButtonImageTest.bmp")));
        }

        public void onClick3SetStock(object sender, EventArgs e)
        {
            bImg3.SetImageStock(Gtk.Stock.About);
        }

        public void onClick3Clear(object sender, EventArgs e)
        {
            bImg3.Clear();
        }
	}
}