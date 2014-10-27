using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

using RLToolkit;
using RLToolkit.Basic;
using RLToolkit.Asserts;
using NUnit.Framework;
using System.Linq;

namespace RLToolkit.Tests
{
    [TestFixture]
    public class TextureHandlerTest : TestHarness, ITestBase
    {
        #region Local Variables
        private string localFolder = ""; // to be initialized later

        // paths and filename
        private string image_bmp_1 = "file1.bmp";
        private string image_bmp_1_big = "file1_big.bmp";
        private string image_bmp_1_big_red = "file1_big_red.bmp";
        private string image_bmp_1_big_green = "file1_big_green.bmp";
        private string image_bmp_1_big_blue = "file1_big_blue.bmp";
        private string image_bmp_1_big_black = "file1_big_black.bmp";
        private string image_bmp_1_small = "file1_small.bmp";
        private string image_bmp_1_out = "file1_out.bmp";
        private string image_bmp_1_big_out = "file1_out_big.bmp";
        private string image_bmp_1_small_out = "file1_out_small.bmp";
        private string image_bmp_1_combine_out = "file1_out_combine.bmp";
        private string image_bmp_1_combineFail_out = "file1_out_combine_fail.bmp";
        #endregion

        #region Interface Override
        public string ModuleName()
        {
            return "TextureHandler";
        }

        public override void SetFolderPaths()
        {
            localFolder = AppDomain.CurrentDomain.BaseDirectory;
            SetPaths (localFolder, ModuleName());
        }

        public override void DataPrepare()
        {
            CopyFile (Path.Combine(folder_testdata, image_bmp_1), Path.Combine(localFolder, image_bmp_1), true, false);
            CopyFile (Path.Combine(folder_testdata, image_bmp_1_small), Path.Combine(localFolder, image_bmp_1_small), true, false);
            CopyFile (Path.Combine(folder_testdata, image_bmp_1_big), Path.Combine(localFolder, image_bmp_1_big), true, false);

            CopyFile (Path.Combine(folder_testdata, image_bmp_1_big_red), Path.Combine(localFolder, image_bmp_1_big_red), true, false);
            CopyFile (Path.Combine(folder_testdata, image_bmp_1_big_green), Path.Combine(localFolder, image_bmp_1_big_green), true, false);
            CopyFile (Path.Combine(folder_testdata, image_bmp_1_big_blue), Path.Combine(localFolder, image_bmp_1_big_blue), true, false);
            CopyFile (Path.Combine(folder_testdata, image_bmp_1_big_black), Path.Combine(localFolder, image_bmp_1_big_black), true, false);
        }

        public override void DataCleanup()
        {
            // move the test output to the result folder
            MoveFile (Path.Combine(localFolder, image_bmp_1_out), Path.Combine(folder_testresult, image_bmp_1_out), false);
            MoveFile (Path.Combine(localFolder, image_bmp_1_big_out), Path.Combine(folder_testresult, image_bmp_1_big_out), false);
            MoveFile (Path.Combine(localFolder, image_bmp_1_small_out), Path.Combine(folder_testresult, image_bmp_1_small_out), false);
            MoveFile (Path.Combine(localFolder, image_bmp_1_combine_out), Path.Combine(folder_testresult, image_bmp_1_combine_out), false);
            MoveFile (Path.Combine(localFolder, image_bmp_1_combineFail_out), Path.Combine(folder_testresult, image_bmp_1_combineFail_out), false);

            // delete the test files from the data folder
            CleanFile (Path.Combine (localFolder, image_bmp_1), false);
            CleanFile (Path.Combine (localFolder, image_bmp_1_small), false);
            CleanFile (Path.Combine (localFolder, image_bmp_1_big), false);

            CleanFile (Path.Combine (localFolder, image_bmp_1_big_red), false);
            CleanFile (Path.Combine (localFolder, image_bmp_1_big_green), false);
            CleanFile (Path.Combine (localFolder, image_bmp_1_big_blue), false);
            CleanFile (Path.Combine (localFolder, image_bmp_1_big_black), false);
        }
        #endregion

        #region Tests
        [Test]
        public void TextureHandler_Bmp_ReadWrite()
        {
            TextureHandler t1 = new TextureHandler(Path.Combine(localFolder, image_bmp_1));
            t1.Save(Path.Combine(localFolder, image_bmp_1_out), ImageFormat.Bmp);
            t1.Dispose();

            Bitmap original = new Bitmap(Path.Combine(localFolder, image_bmp_1));
            Bitmap output = new Bitmap(Path.Combine(localFolder, image_bmp_1_out));


            BitmapAssert.AreEqual(original, output, 2, "Written image should be as the original.");
        }

        [Test]
        public void TextureHandler_Bmp_ResizeUp()
        {
            TextureHandler t1 = new TextureHandler(Path.Combine(localFolder, image_bmp_1));
            t1.Resize(32, 32, true);
            t1.Save(Path.Combine(localFolder, image_bmp_1_big_out), ImageFormat.Bmp);
            t1.Dispose();

            Bitmap original = new Bitmap(Path.Combine(localFolder, image_bmp_1_big));
            Bitmap output = new Bitmap(Path.Combine(localFolder, image_bmp_1_big_out));

            BitmapAssert.AreEqual(original, output, 128, "Written image should be like the reference.");
        }

        [Test]
        public void TextureHandler_Bmp_ResizeDown()
        {
            TextureHandler t1 = new TextureHandler(Path.Combine(localFolder, image_bmp_1));
            t1.Resize(8, 8, true);
            t1.Save(Path.Combine(localFolder, image_bmp_1_small_out), ImageFormat.Bmp);
            t1.Dispose();

            Bitmap original = new Bitmap(Path.Combine(localFolder, image_bmp_1_small));
            Bitmap output = new Bitmap(Path.Combine(localFolder, image_bmp_1_small_out));

            BitmapAssert.AreEqual(original, output, 128, "Written image should be like the reference.");
        }

        [Test]
        public void TextureHandler_Bmp_Combine()
        {
            TextureHandler target = new TextureHandler(32,32);

            Bitmap input1 = new Bitmap(Path.Combine(localFolder, image_bmp_1_big_red));
            Bitmap input2 = new Bitmap(Path.Combine(localFolder, image_bmp_1_big_black));
            Bitmap input3 = new Bitmap(Path.Combine(localFolder, image_bmp_1_big_blue));
            Bitmap input4 = new Bitmap(Path.Combine(localFolder, image_bmp_1_big_green));

            Bitmap[][] inputArray = new Bitmap[2][];
            inputArray[0] = new Bitmap[2] { input1, input2};
            inputArray[1] = new Bitmap[2] { input3, input4};

            target.Combine(inputArray, 32, 32, true);
            target.Save(Path.Combine(localFolder, image_bmp_1_combine_out), ImageFormat.Bmp);
            target.Dispose();

            Bitmap original = new Bitmap(Path.Combine(localFolder, image_bmp_1_big));
            Bitmap output = new Bitmap(Path.Combine(localFolder, image_bmp_1_combine_out));

            BitmapAssert.AreEqual(original, output, 10, "Written image should be like the reference.");
        }

        [Test]
        public void TextureHandler_Bmp_CombineFailed()
        {
            TextureHandler target = new TextureHandler(32,32);

            Bitmap input1 = new Bitmap(Path.Combine(localFolder, image_bmp_1_big_red));
            Bitmap input2 = new Bitmap(Path.Combine(localFolder, image_bmp_1_big_black));
            Bitmap input3 = new Bitmap(Path.Combine(localFolder, image_bmp_1_big_red));
            Bitmap input4 = new Bitmap(Path.Combine(localFolder, image_bmp_1_big_green));

            Bitmap[][] inputArray = new Bitmap[2][];
            inputArray[0] = new Bitmap[2] { input1, input2};
            inputArray[1] = new Bitmap[2] { input3, input4};

            target.Combine(inputArray, 32, 32, true);
            target.Save(Path.Combine(localFolder, image_bmp_1_combineFail_out), ImageFormat.Bmp);
            target.Dispose();

            Bitmap original = new Bitmap(Path.Combine(localFolder, image_bmp_1_big));
            Bitmap output = new Bitmap(Path.Combine(localFolder, image_bmp_1_combineFail_out));

            BitmapAssert.AreNotEqual(original, output, 10, "Written image should not be like the reference.");
        }
        #endregion
    }
}