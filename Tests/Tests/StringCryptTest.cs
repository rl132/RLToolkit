using System;

using RLToolkit;
using RLToolkit.Basic;
using NUnit.Framework;

namespace RLToolkit.Tests
{
    [TestFixture]
    public class StringCryptTest : TestHarness, ITestBase
    {
        #region Local Variables
        private string localFolder = ""; // to be initialized later

        #endregion

        #region Interface Override
        public string ModuleName()
        {
            return "StringCrypt";
        }

        public override void SetFolderPaths()
        {
            localFolder = AppDomain.CurrentDomain.BaseDirectory;
            SetPaths (localFolder, ModuleName());
        }

        public override void DataPrepare()
        {

        }

        public override void DataCleanup()
        {
        
        }
        #endregion

        #region Basic
        [Test]
        public void StringCrypt_Basic_Implicit()
        {
            string input = "test 123 %$#";
            string outputExpected = "mEvm2p/kYPyAvzYwf6QREw==";
            
            string outputFunction = StringCrypt.Encrypt(input);
            Assert.AreEqual(outputExpected, outputFunction, "String output should be as exected after encryption.");
            
            outputFunction = StringCrypt.Decrypt(outputFunction);
            Assert.AreEqual(input, outputFunction, "String output should be as exected after decrypt.");
        }

        [Test]
        public void StringCrypt_Basic_RFC2898()
        {
            string input = "test 123 %$#";
            string outputExpected = "mEvm2p/kYPyAvzYwf6QREw==";

            string outputFunction = StringCrypt.EncryptRfc2898(input);
            Assert.AreEqual(outputExpected, outputFunction, "String output should be as exected after encryption.");

            outputFunction = StringCrypt.DecryptRfc2898(outputFunction);
            Assert.AreEqual(input, outputFunction, "String output should be as exected after decrypt.");
        }
        #endregion
    }
}
