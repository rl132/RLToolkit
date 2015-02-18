using System;
using System.IO;
using System.Drawing;

using RLToolkit;
using RLToolkit.Extensions;
using NUnit.Framework;

namespace RLToolkit.UnitTests.Extensions
{
    [TestFixture]
    public class BitmapExtensionTest : TestHarness, ITestBase
    {
        #region Local Variables
        private string image_Source_1 = "ConvertSource.bmp";
        private string image_Target_1 = "fillme";
        #endregion

        #region Interface Override
        public string ModuleName()
        {
            return "BitmapExtension";
        }

        public override void SetFolderPaths()
        {
            localFolder = AppDomain.CurrentDomain.BaseDirectory;
            SetPaths (localFolder, ModuleName());
        }

        public override void DataPrepare()
        {
            // copy the data files over
            AddInputFile(Path.Combine(folder_testdata, image_Source_1), true, false);
        }
        #endregion

        #region Tests
        [Test]
        public void BitmapToPixbuf_Convert()
        {
            Bitmap source = new Bitmap(Path.Combine(localFolder, image_Source_1));

            Random number = new Random();
            image_Target_1 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"tempmageconvert-" + number.Next(5000, 50000).ToString() + ".png");
            source.ToPixbuf().Save(image_Target_1, "png");

            Bitmap target = new Bitmap(Path.Combine(localFolder, image_Target_1));

            BitmapAssert.AreEqual(source, target, 10, "Source image should be resembling the new file");
            target.Dispose();
            source.Dispose();

            // output files
            AddOutputFile(Path.Combine(localFolder, image_Target_1), false);
        }
        #endregion  
    }
}

