using System;
using System.Drawing;

using RLToolkit;
using RLToolkit.Extensions;
using NUnit.Framework;

namespace RLToolkit.UnitTests.Extensions
{
	[TestFixture]
	public class ColorExtensionTest : TestHarness, ITestBase
	{
		#region Local Variables
        Color toTest;
		#endregion

		#region Interface Override
		public string ModuleName()
		{

            return "ColorExtension";
		}

		public override void SetFolderPaths()
		{
			localFolder = AppDomain.CurrentDomain.BaseDirectory;
			SetPaths (localFolder, ModuleName());
		}

        public override void DataPrepare()
        {

        }
		#endregion

		#region Tests
        [Test]
        public void ColorCompare_Same()
        {
            toTest = Color.FromKnownColor(KnownColor.Red);
            bool result = toTest.CompareColor(toTest);

            Assert.AreEqual(true, result, "Red color should compare to red properly");
        }
        
        [Test]
        public void ColorCompare_Normal_InRange_Negative()
        {
            Color gray = Color.FromKnownColor(KnownColor.DimGray);
            toTest = gray;
            toTest = Color.FromArgb(gray.A, gray.R-5, gray.G, gray.B);
            bool result = gray.CompareColor(toTest, 10);

            Assert.AreEqual(true, result, "The compare should have worked");
        }

        [Test]
        public void ColorCompare_Normal_InRange_Positive()
        {
            Color gray = Color.FromKnownColor(KnownColor.DimGray);
            toTest = gray;
            toTest = Color.FromArgb(gray.A, gray.R+5, gray.G, gray.B);
            bool result = gray.CompareColor(toTest, 10);

            Assert.AreEqual(true, result, "The compare should have worked");
        }

        [Test]
        public void ColorCompare_Normal_InRange_Positive_DiffChannel()
        {
            Color gray = Color.FromKnownColor(KnownColor.DimGray);
            toTest = gray;
            toTest = Color.FromArgb(gray.A, gray.R, gray.G-5, gray.B);

            bool result = gray.CompareColor(toTest, 10);

            Assert.AreEqual(true, result, "The compare should have worked");
        }

        [Test]
        public void ColorCompare_Normal_NotInRange()
        {
            Color gray = Color.FromKnownColor(KnownColor.DimGray);
            toTest = gray;
            toTest = Color.FromArgb(gray.A, gray.R+15, gray.G, gray.B);
            bool result = gray.CompareColor(toTest, 10);

            Assert.AreEqual(false, result, "The compare should not have worked");
        }

        [Test]
        public void ColorCompare_Opposite_NoTol()
        {
            Color white = Color.FromKnownColor(KnownColor.White);
            toTest = Color.FromKnownColor(KnownColor.Black);
            bool result = white.CompareColor(toTest, 0);

            Assert.AreEqual(false, result, "The compare should not have worked");
        }

        [Test]
        public void ColorCompare_Opposite_FullTol()
        {
            Color white = Color.FromKnownColor(KnownColor.White);
            toTest = Color.FromKnownColor(KnownColor.Black);
            bool result = white.CompareColor(toTest, 255);

            Assert.AreEqual(true, result, "The compare should have worked");
        }

        [Test]
        public void ColorCompare_Normal_Alpha_Valid_Implicit()
        {
            Color white = Color.FromArgb(255, 255, 255, 255);
            toTest = Color.FromArgb(250, 255, 255, 255);
            bool result = white.CompareColor(toTest, 10);

            Assert.AreEqual(true, result, "The compare should have worked");
        }

        [Test]
        public void ColorCompare_Normal_Alpha_Valid_Defined()
        {
            Color white = Color.FromArgb(255, 255, 255, 255);
            toTest = Color.FromArgb(250, 255, 255, 255);
            bool result = white.CompareColor(toTest, 10, true);

            Assert.AreEqual(true, result, "The compare should have worked");
        }

        [Test]
        public void ColorCompare_Normal_Alpha_OffRange_Implicit()
        {
            Color white = Color.FromArgb(255, 255, 255, 255);
            toTest = Color.FromArgb(128, 255, 255, 255);
            bool result = white.CompareColor(toTest, 10);

            Assert.AreEqual(false, result, "The compare should not have worked");
        }

        [Test]
        public void ColorCompare_Normal_Alpha_OffRange_Defined_Active()
        {
            Color white = Color.FromArgb(255, 255, 255, 255);
            toTest = Color.FromArgb(128, 255, 255, 255);
            bool result = white.CompareColor(toTest, 10, true);

            Assert.AreEqual(false, result, "The compare should not have worked");
        }

        [Test]
        public void ColorCompare_Normal_Alpha_OffRange_Defined_NotUsed()
        {
            Color white = Color.FromArgb(255, 255, 255, 255);
            toTest = Color.FromArgb(128, 255, 255, 255);
            bool result = white.CompareColor(toTest, 10, false);

            Assert.AreEqual(true, result, "The compare should have worked");
        }
        #endregion	
	}
}

