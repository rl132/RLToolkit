﻿using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

using RLToolkit;
using RLToolkit.Basic;
using RLToolkit.Extensions;
using NUnit.Framework;
using System.Linq;

namespace RLToolkit.UnitTests.Modules
{
    [TestFixture]
    public class TextureHandlerTest : TestHarness, ITestBase
    {
        #region Local Variables
        // paths and filename
        private string image_bmp_1 = "file1.bmp";
        private string image_bmp_1_big = "file1_big.bmp";
        private string image_bmp_1_big_red = "file1_big_red.bmp";
        private string image_bmp_1_big_green = "file1_big_green.bmp";
        private string image_bmp_1_big_blue = "file1_big_blue.bmp";
        private string image_bmp_1_big_black = "file1_big_black.bmp";
        private string image_bmp_1_small = "file1_small.bmp";
        private string image_bmp_1_Uneven = "file1_Uneven.bmp";
        private string image_bmp_1_out = "file1_out.bmp";
        private string image_bmp_1_big_out = "file1_out_big.bmp";
        private string image_bmp_1_small_out = "file1_out_small.bmp";
        private string image_bmp_1_combine_out = "file1_out_combine.bmp";
        private string image_bmp_1_combineUneven_out = "file1_out_combineUneven.bmp";
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
            // copy the data files over
            AddInputFile(Path.Combine(folder_testdata, image_bmp_1), true, false);
            AddInputFile(Path.Combine(folder_testdata, image_bmp_1_small), true, false);
            AddInputFile(Path.Combine(folder_testdata, image_bmp_1_big), true, false);

            AddInputFile(Path.Combine(folder_testdata, image_bmp_1_big_red), true, false);
            AddInputFile(Path.Combine(folder_testdata, image_bmp_1_big_green), true, false);
            AddInputFile(Path.Combine(folder_testdata, image_bmp_1_big_blue), true, false);
            AddInputFile(Path.Combine(folder_testdata, image_bmp_1_big_black), true, false);

            AddInputFile(Path.Combine(folder_testdata, image_bmp_1_Uneven), true, false);
        }

        public override void DataCleanup()
        {
            // move the test output to the result folder
            AddOutputFile(Path.Combine(localFolder, image_bmp_1_out), false);
            AddOutputFile(Path.Combine(localFolder, image_bmp_1_big_out), false);
            AddOutputFile(Path.Combine(localFolder, image_bmp_1_small_out), false);
            AddOutputFile(Path.Combine(localFolder, image_bmp_1_combine_out), false);
            AddOutputFile(Path.Combine(localFolder, image_bmp_1_combineUneven_out), false);
            AddOutputFile(Path.Combine(localFolder, image_bmp_1_combineFail_out), false);
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

            // cleanup - dispose of the bitmaps
            original.Dispose();
            output.Dispose();
        }

        [Test]
        public void TextureHandler_Bmp_ResizeUp()
        {
            // RL: This is a stress test, if it doesn't pass, it's not the end of the world as
            // most video card/driver will handle the upscaling very differently.
            TextureHandler t1 = new TextureHandler(Path.Combine(localFolder, image_bmp_1));
            t1.Resize(32, 32, true);
            t1.Save(Path.Combine(localFolder, image_bmp_1_big_out), ImageFormat.Bmp);
            t1.Dispose();

            Bitmap original = new Bitmap(Path.Combine(localFolder, image_bmp_1_big));
            Bitmap output = new Bitmap(Path.Combine(localFolder, image_bmp_1_big_out));

            BitmapAssert.AreEqual(original, output, 200, "Written image should be like the reference.");
        
            // cleanup - dispose of the bitmaps
            original.Dispose();
            output.Dispose();
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
        
            // cleanup - dispose of the bitmaps
            original.Dispose();
            output.Dispose();
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

            // cleanup - dispose of the bitmaps
            original.Dispose();
            output.Dispose();
            input1.Dispose();
            input2.Dispose();
            input3.Dispose();
            input4.Dispose();
        }

        [Test]
        public void TextureHandler_Bmp_CombineUneven()
        {
            TextureHandler target = new TextureHandler(32, 16);

            Bitmap input1 = new Bitmap(Path.Combine(localFolder, image_bmp_1_big_red));
            Bitmap input2 = new Bitmap(Path.Combine(localFolder, image_bmp_1_big_black));

            Bitmap[][] inputArray = new Bitmap[1][];
            inputArray[0] = new Bitmap[2] { input1, input2};

            target.Combine(inputArray, 32, 16, true);
            target.Save(Path.Combine(localFolder, image_bmp_1_combineUneven_out), ImageFormat.Bmp);
            target.Dispose();

            Bitmap original = new Bitmap(Path.Combine(localFolder, image_bmp_1_Uneven));
            Bitmap output = new Bitmap(Path.Combine(localFolder, image_bmp_1_combineUneven_out));

            BitmapAssert.AreEqual(original, output, 10, "Written image should be like the reference.");

            // cleanup - dispose of the bitmaps
            original.Dispose();
            output.Dispose();
            input1.Dispose();
            input2.Dispose();
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

            // cleanup - dispose of the bitmaps
            original.Dispose();
            output.Dispose();
            input1.Dispose();
            input2.Dispose();
            input3.Dispose();
            input4.Dispose();
        }
        #endregion
    }
}