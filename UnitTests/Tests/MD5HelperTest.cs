using System;

using RLToolkit;
using RLToolkit.Basic;
using NUnit.Framework;

namespace RLToolkit.UnitTests
{
    [TestFixture]
    public class MD5HelperTest : TestHarness, ITestBase
    {
        #region Interface Override
        public string ModuleName()
        {
            return "MD5Helper";
        }

        public override void SetFolderPaths()
        {
            localFolder = AppDomain.CurrentDomain.BaseDirectory;
            SetPaths (localFolder, ModuleName());
        }
        #endregion

        #region Basic
        [Test]
        public void MD5_Basic_Implicit()
        {
            string input = "MD5 test ABC _ 123 $%^";
            string outputExpected = "13d3cd6193eddbb721d0d19513be210f";
            
            string outputFunction = MD5Helper.ComputeHash(input);
            Assert.AreEqual(outputExpected, outputFunction, "MD5 hash should be as expected.");
        }

        [Test]
        public void MD5_Basic_lowercase()
        {
            string input = "MD5 test ABC _ 123 $%^";
            string outputExpected = "13d3cd6193eddbb721d0d19513be210f";
            
            string outputFunction = MD5Helper.ComputeHash(input, false);
            Assert.AreEqual(outputExpected, outputFunction, "MD5 hash should be as expected.");
        }

        [Test]
        public void MD5_Basic_uppercase()
        {
            string input = "MD5 test ABC _ 123 $%^";
            string outputExpected = "13D3CD6193EDDBB721D0D19513BE210F";
            
            string outputFunction = MD5Helper.ComputeHash(input, true);
            Assert.AreEqual(outputExpected, outputFunction, "MD5 hash should be as expected.");
        }

        [Test]
        public void MD5_ValidateHash_Valid()
        {
            string input = "MD5 test ABC _ 123 $%^";
            string outputExpected = "13d3cd6193eddbb721d0d19513be210f";
            
            bool output = MD5Helper.VerifyMd5Hash(input, outputExpected);
            Assert.IsTrue(output, "MD5 hash should be the same.");
        }

        [Test]
        public void MD5_ValidateHash_Invalid()
        {
            string input = "MD5 test - 1234 foobar Mismatch";
            string outputExpected = "13d3cd6193eddbb721d0d19513be210f";
            
            bool output = MD5Helper.VerifyMd5Hash(input, outputExpected);
            Assert.IsFalse(output, "MD5 hash should be different.");
        }
        #endregion
    }
}
