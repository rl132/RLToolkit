using System;
using System.Text;

using RLToolkit.Basic;
using NUnit.Framework;
using System.Security.Cryptography;

namespace RLToolkit.Tests
{
    [TestFixture]
    public class StringCryptTest : TestHarness, ITestBase
    {
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
        #endregion

        #region Basic
        [Test]
        public void StringCrypt_Basic_Implicit()
        {
            string input = "test 123 %$#";

            byte[] output = StringCrypt.Encrypt(input);
            string outputFinal = StringCrypt.Decrypt(output);

            Assert.AreEqual(input, outputFinal, "String output should be as exected after decrypt.");
        }

        [Test]
        public void StringCrypt_Basic_RFC2898()
        {
            string input = "test 123 %$#";

            byte[] output = StringCrypt.EncryptRfc2898(input);
            string outputFinal = StringCrypt.DecryptRfc2898(output);

            Assert.AreEqual(input, outputFinal, "String output should be as exected after decrypt.");
        }
            
        [Test]
        public void StringCrypt_Basic_RSA()
        {
            string input = "test 123 %$#";

            byte[] output = StringCrypt.EncryptRSA(input);
            string outputFinal = StringCrypt.DecryptRSA(output);

            Assert.AreEqual(input, outputFinal, "String output should be as exected after decrypt.");
        }

        [Test]
        public void StringCrypt_Basic_TripleDES()
        {
            string input = "test 123 %$#";

            byte[] output = StringCrypt.EncryptTripleDES(input);
            string outputFinal = StringCrypt.DecryptTripleDES(output);

            Assert.AreEqual(input, outputFinal, "String output should be as exected after decrypt.");
        }
        #endregion

        #region Test-Specific-RFC2898
        [Test]
        public void StringCrypt_Specific_RFC2898_Password()
        {
            string input = "foobar and some special characters $%^&*()";
            RFC2898CryptInfo cryptInfo = new RFC2898CryptInfo();
            cryptInfo.password = "test password with space";

            byte[] output = StringCrypt.EncryptRfc2898(input, cryptInfo);
            string outputFinal = StringCrypt.DecryptRfc2898(output, cryptInfo);

            Assert.AreEqual(input, outputFinal, "String output should be as exected after decrypt.");
        }

        [Test]
        public void StringCrypt_Specific_RFC2898_Salt()
        {
            string input = "foobar and some special characters $%^&*()";
            RFC2898CryptInfo cryptInfo = new RFC2898CryptInfo();
            cryptInfo.salt = "RandomSalt12634asdf";

            byte[] output = StringCrypt.EncryptRfc2898(input, cryptInfo);
            string outputFinal = StringCrypt.DecryptRfc2898(output, cryptInfo);

            Assert.AreEqual(input, outputFinal, "String output should be as exected after decrypt.");
        }

        [Test]
        public void StringCrypt_Specific_RFC2898_VIKey()
        {
            string input = "foobar and some special characters $%^&*()";
            RFC2898CryptInfo cryptInfo = new RFC2898CryptInfo();
            cryptInfo.VIKey = "abc12366FFTYYU^d";

            byte[] output = StringCrypt.EncryptRfc2898(input, cryptInfo);
            string outputFinal = StringCrypt.DecryptRfc2898(output, cryptInfo);

            Assert.AreEqual(input, outputFinal, "String output should be as exected after decrypt.");
        }

        [Test]
        public void StringCrypt_Specific_RFC2898_WrongPassword()
        {
            string input = "foobar and some special characters $%^&*()";
            RFC2898CryptInfo cryptInfo1 = new RFC2898CryptInfo();
            cryptInfo1.password = "test password with space";
            RFC2898CryptInfo cryptInfo2 = new RFC2898CryptInfo();
            cryptInfo2.password = "AnotherPassword";

            byte[] output = StringCrypt.EncryptRfc2898(input, cryptInfo1);
            string outputFinal = StringCrypt.DecryptRfc2898(output, cryptInfo2);

            Assert.AreNotEqual(input, outputFinal, "String output should not be decryptable.");
        }

        [Test]
        public void StringCrypt_Specific_RFC2898_WrongSalt()
        {
            string input = "foobar and some special characters $%^&*()";
            RFC2898CryptInfo cryptInfo1 = new RFC2898CryptInfo();
            cryptInfo1.salt = "salt1ABCD";
            RFC2898CryptInfo cryptInfo2 = new RFC2898CryptInfo();
            cryptInfo2.salt = "salt2ABCD";

            byte[] output = StringCrypt.EncryptRfc2898(input, cryptInfo1);
            string outputFinal = StringCrypt.DecryptRfc2898(output, cryptInfo2);

            Assert.AreNotEqual(input, outputFinal, "String output should not be decryptable.");
        }

        [Test]
        public void StringCrypt_Specific_RFC2898_WrongVIKey()
        {
            string input = "foobar and some special characters $%^&*()";
            RFC2898CryptInfo cryptInfo1 = new RFC2898CryptInfo();
            cryptInfo1.VIKey = "983GET$%I8876824";
            RFC2898CryptInfo cryptInfo2 = new RFC2898CryptInfo();
            cryptInfo2.VIKey = "Sdfert@$#G53df$#";

            byte[] output = StringCrypt.EncryptRfc2898(input, cryptInfo1);
            string outputFinal = StringCrypt.DecryptRfc2898(output, cryptInfo2);

            Assert.AreNotEqual(input, outputFinal, "String output should not be decryptable.");
        }
        #endregion

        #region Test-Specific-RSA
        [Test]
        public void StringCrypt_Specific_RSA_KeySize()
        {
            string input = "foobar and some special characters $%^&*()";
            RSACryptInfo cryptInfo = new RSACryptInfo(1024);

            byte[] output = StringCrypt.EncryptRSA(input, cryptInfo);
            string outputFinal = StringCrypt.DecryptRSA(output, cryptInfo);

            Assert.AreEqual(input, outputFinal, "String output should be as exected after decrypt.");
        }

        [Test]
        public void StringCrypt_Specific_RSA_CustomKeys()
        {
            string input = "foobar and some special characters $%^&*()";
            RSACryptInfo cryptInfo = new RSACryptInfo(512);
            RSACryptInfo cryptInfoKey = new RSACryptInfo(512);
            cryptInfo.publicKey = cryptInfoKey.publicKey;
            cryptInfo.privateKey = cryptInfoKey.privateKey;

            byte[] output = StringCrypt.EncryptRSA(input, cryptInfo);
            string outputFinal = StringCrypt.DecryptRSA(output, cryptInfo);

            Assert.AreEqual(input, outputFinal, "String output should be as exected after decrypt.");
        }

        [Test]
        public void StringCrypt_Specific_RSA_WrongPublicKey()
        {
            string input = "foobar and some special characters $%^&*()";
            RSACryptInfo cryptInfo1 = new RSACryptInfo(512);
            RSACryptInfo cryptInfo2 = new RSACryptInfo(512);
            RSACryptInfo cryptInfoKey = new RSACryptInfo(512);
            cryptInfo2.publicKey = cryptInfoKey.publicKey;

            byte[] output = StringCrypt.EncryptRSA(input, cryptInfo1);
            string outputFinal = StringCrypt.DecryptRSA(output, cryptInfo2);

            Assert.AreNotEqual(input, outputFinal, "String output should not be decryptable.");
        }

        [Test]
        public void StringCrypt_Specific_RSA_WrongPrivatKey()
        {
            string input = "foobar and some special characters $%^&*()";
            RSACryptInfo cryptInfo1 = new RSACryptInfo(512);
            RSACryptInfo cryptInfo2 = new RSACryptInfo(512);
            RSACryptInfo cryptInfoKey = new RSACryptInfo(512);
            cryptInfo2.privateKey = cryptInfoKey.privateKey;

            byte[] output = StringCrypt.EncryptRSA(input, cryptInfo1);
            string outputFinal = StringCrypt.DecryptRSA(output, cryptInfo2);

            Assert.AreNotEqual(input, outputFinal, "String output should not be decryptable.");
        }
        #endregion

        #region Test-Specific-TripleDES
        [Test]
        public void StringCrypt_Specific_TripleDES_NewAlg()
        {
            string input = "foobar and some special characters $%^&*()";
            TripleDES alg = TripleDES.Create("TripleDES");

            byte[] output = StringCrypt.EncryptTripleDES(input, alg);
            string outputFinal = StringCrypt.DecryptTripleDES(output, alg);

            Assert.AreEqual(input, outputFinal, "String output should be as exected after decrypt.");
        }

        [Test]
        public void StringCrypt_Specific_TripleDES_NewKey()
        {
            string input = "foobar and some special characters $%^&*()";
            TripleDES alg = TripleDES.Create("TripleDES");
            TripleDES algKey = TripleDES.Create("TripleDES");
            alg.Key = algKey.Key;

            byte[] output = StringCrypt.EncryptTripleDES(input, alg);
            string outputFinal = StringCrypt.DecryptTripleDES(output, alg);

            Assert.AreEqual(input, outputFinal, "String output should be as exected after decrypt.");
        }

        [Test]
        public void StringCrypt_Specific_TripleDES_NewIV()
        {
            string input = "foobar and some special characters $%^&*()";
            TripleDES alg = TripleDES.Create("TripleDES");
            TripleDES algIV = TripleDES.Create("TripleDES");
            alg.IV = algIV.IV;

            byte[] output = StringCrypt.EncryptTripleDES(input, alg);
            string outputFinal = StringCrypt.DecryptTripleDES(output, alg);

            Assert.AreEqual(input, outputFinal, "String output should be as exected after decrypt.");
        }

        [Test]
        public void StringCrypt_Specific_TripleDES_WrongAlg()
        {
            string input = "foobar and some special characters $%^&*()";
            TripleDES alg1 = TripleDES.Create("TripleDES");
            TripleDES alg2 = TripleDES.Create("TripleDES");

            byte[] output = StringCrypt.EncryptTripleDES(input, alg1);
            string outputFinal = StringCrypt.DecryptTripleDES(output, alg2);

            Assert.AreNotEqual(input, outputFinal, "String output should not be decryptable.");
        }

        [Test]
        public void StringCrypt_Specific_TripleDES_WrongKey()
        {
            string input = "foobar and some special characters $%^&*()";
            TripleDES alg1 = TripleDES.Create("TripleDES");
            TripleDES alg2 = TripleDES.Create("TripleDES");
            alg2.IV = alg1.IV;

            byte[] output = StringCrypt.EncryptTripleDES(input, alg1);
            string outputFinal = StringCrypt.DecryptTripleDES(output, alg2);

            Assert.AreNotEqual(input, outputFinal, "String output should not be decryptable.");
        }

        [Test]
        public void StringCrypt_Specific_TripleDES_WrongIV()
        {
            string input = "foobar and some special characters $%^&*()";
            TripleDES alg1 = TripleDES.Create("TripleDES");
            TripleDES alg2 = TripleDES.Create("TripleDES");
            alg2.Key = alg1.Key;

            byte[] output = StringCrypt.EncryptTripleDES(input, alg1);
            string outputFinal = StringCrypt.DecryptTripleDES(output, alg2);

            Assert.AreNotEqual(input, outputFinal, "String output should not be decryptable.");
        }
        #endregion

        #region Base64<->byte[] converter
        [Test]
        public void StringCrypt_Converter()
        {
            string str = "LongString with @#$@# stuff in it!?.";
            byte[] input = Encoding.UTF8.GetBytes(str);

            string base64 = StringCrypt.ConvertByteArrayToBase64(input);
            byte[] output = StringCrypt.ConvertBase64ToByteArray(base64);

            Assert.AreEqual(input, output, "The converted back and forth should be identical");
        }
        #endregion
    }
}
