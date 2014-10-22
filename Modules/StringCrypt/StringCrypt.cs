using System;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.Collections;

namespace RLToolkit.Basic
{
    public static class StringCrypt
    {
        public static TripleDES TripleDESAlg;
        public static RSACryptInfo RSA_cryptInfo;
        public static RFC2898CryptInfo RFC2898_cryptInfo;

        #region implicit encrypt/decrypt
        public static byte[] Encrypt(string input)
        {
            // implicit crypt
            LogManager.Instance.Log().Debug("Calling an implicit Encrypt");
            return EncryptRfc2898(input);
        }

        public static string Decrypt(byte[] input)
        {
            // implicit Decrypt
            LogManager.Instance.Log().Debug("Calling an implicit Decrypt");
            return DecryptRfc2898(input);
        }
        #endregion

        #region RFC2989
        public static byte[] EncryptRfc2898(string input)
        {
            LogManager.Instance.Log().Debug("Calling Encrypt RFC2989 with default crypt info");
            if (RFC2898_cryptInfo == null)
            {
                LogManager.Instance.Log().Debug("Creating a new static instance of RFC2898 Crypt info");
                RFC2898_cryptInfo = new RFC2898CryptInfo();
            }

            return EncryptRfc2898(input, RFC2898_cryptInfo);
        }

        public static byte[] EncryptRfc2898(string input, RFC2898CryptInfo cryptInfo)
        {
            LogManager.Instance.Log().Debug("Calling Encrypt RFC2989");

            byte[] toEncode = new ASCIIEncoding().GetBytes(input);
            byte[] key = new Rfc2898DeriveBytes(cryptInfo.password, new ASCIIEncoding().GetBytes(cryptInfo.salt)).GetBytes(256/8);
            RijndaelManaged symmetricKey = new RijndaelManaged()
            {
                Mode = CipherMode.CBC,
                Padding = PaddingMode.Zeros
            };
            var encryptor = symmetricKey.CreateEncryptor(key, new ASCIIEncoding().GetBytes(cryptInfo.VIKey));

            byte[] encodedBytes;
            MemoryStream mStream = new MemoryStream();
            CryptoStream cStream = new CryptoStream(mStream, encryptor, CryptoStreamMode.Write);

            cStream.Write(toEncode, 0, toEncode.Length);
            cStream.FlushFinalBlock();
            encodedBytes = mStream.ToArray();

            cStream.Close();
            mStream.Close();

            return encodedBytes;
        }

        public static string DecryptRfc2898(byte[] input)
        {
            LogManager.Instance.Log().Debug("Calling Decrypt RFC2989 with default crypt info");
            if (RFC2898_cryptInfo == null)
            {
                LogManager.Instance.Log().Debug("Creating a new static instance of RFC2898 Crypt info");
                RFC2898_cryptInfo = new RFC2898CryptInfo();
            }

            return DecryptRfc2898(input, RFC2898_cryptInfo);
        }

        public static string DecryptRfc2898(byte[] input, RFC2898CryptInfo cryptInfo)
        {
            LogManager.Instance.Log().Debug("Calling Decrypt RFC2989");

            byte[] key = new Rfc2898DeriveBytes(cryptInfo.password, new ASCIIEncoding().GetBytes(cryptInfo.salt)).GetBytes(256/8);
            RijndaelManaged symmetricKey = new RijndaelManaged() 
            {
                Mode = CipherMode.CBC,
                Padding = PaddingMode.None 
            };
            var decryptor = symmetricKey.CreateDecryptor(key, new ASCIIEncoding().GetBytes(cryptInfo.VIKey));

            MemoryStream mStream = new MemoryStream(input);
            CryptoStream cStream = new CryptoStream(mStream, decryptor, CryptoStreamMode.Read);
            byte[] fromEncode = new byte[input.Length];
            cStream.Read(fromEncode, 0, fromEncode.Length);  

            mStream.Close();
            cStream.Close();

            return new ASCIIEncoding().GetString(fromEncode).TrimEnd('\0');
        }
        #endregion

        #region RSA
        public static byte[] EncryptRSA(string input)
        {
            LogManager.Instance.Log().Debug("Calling Decrypt RSA with default crypt info");
            if (RSA_cryptInfo == null)
            {
                LogManager.Instance.Log().Debug("Creating a new static instance of RSA Crypt info");
                RSA_cryptInfo = new RSACryptInfo(512);
            }

            return EncryptRSA(input, RSA_cryptInfo);
        }

        public static byte[] EncryptRSA(string input, RSACryptInfo cryptInfo)
        {
            LogManager.Instance.Log().Debug("Calling Encrypt RSA");

            try
            {
                RSACryptoServiceProvider csp = new RSACryptoServiceProvider(cryptInfo.keySize);
                csp.ImportParameters(cryptInfo.publicKey);
                return csp.Encrypt(new ASCIIEncoding().GetBytes(input), false);
            }
            catch (CryptographicException e)
            {
                LogManager.Instance.Log().Fatal(string.Format("Cryptographic failure occured: {0}", e.Message));
                return null;
            }
        }

        public static string DecryptRSA(byte[] input)
        {
            LogManager.Instance.Log().Debug("Calling Encrypt RSA with default crypt info");
            if (RSA_cryptInfo == null)
            {
                LogManager.Instance.Log().Debug("Creating a new static instance of RSA Crypt info");
                RSA_cryptInfo = new RSACryptInfo(512);
            }

            return DecryptRSA(input, RSA_cryptInfo);
        }

        public static string DecryptRSA( byte[] input, RSACryptInfo cryptInfo)
        {
            LogManager.Instance.Log().Debug("Calling Decrypt RSA");

            try
            {
                RSACryptoServiceProvider csp = new RSACryptoServiceProvider(cryptInfo.keySize);
                csp.ImportParameters(cryptInfo.privateKey);
                return new ASCIIEncoding().GetString(csp.Decrypt(input, false)).Trim('\0');
            }
            catch (CryptographicException e)
            {
                LogManager.Instance.Log().Fatal(string.Format("Cryptographic failure occured: {0}", e.Message));
                return null;
            }
        }
        #endregion

        #region TripleDES
        public static byte[] EncryptTripleDES(string input)
        {
            LogManager.Instance.Log().Debug("Calling Encrypt TripleDES with default crypt info");
            if (TripleDESAlg == null)
            {
                LogManager.Instance.Log().Debug("Creating a new static instance of TripleDES");
                TripleDESAlg = TripleDES.Create("TripleDES");
            }

            return EncryptTripleDES(input, TripleDESAlg);
        }

        public static byte[] EncryptTripleDES(string input, TripleDES alg)
        {
            LogManager.Instance.Log().Debug("Calling Encrypt TripleDES");
            try
            {
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, alg.CreateEncryptor(alg.Key, alg.IV), CryptoStreamMode.Write);

                byte[] toEncode = new ASCIIEncoding().GetBytes(input);
                cStream.Write(toEncode, 0, toEncode.Length);
                cStream.FlushFinalBlock();

                byte[] encodedBytes = mStream.ToArray();

                mStream.Close();
                cStream.Close();

                return encodedBytes;
            } 
            catch (CryptographicException e)
            {
                LogManager.Instance.Log().Fatal(string.Format("Cryptographic failure occured: {0}", e.Message));
                return null;
            }
        }

        public static string DecryptTripleDES(byte[] input)
        {
            LogManager.Instance.Log().Debug("Calling Decrypt TripleDES with default crypt info");
            if (TripleDESAlg == null)
            {
                LogManager.Instance.Log().Debug("Creating a new static instance of TripleDES");
                TripleDESAlg = TripleDES.Create("TripleDES");
            }

            return DecryptTripleDES(input, TripleDESAlg);
        }

        public static string DecryptTripleDES(byte[] input, TripleDES alg)
        {
            LogManager.Instance.Log().Debug("Calling Decrypt TripleDES");
            try
            {
                MemoryStream mStream = new MemoryStream(input);
                CryptoStream cStream = new CryptoStream(mStream, alg.CreateDecryptor(alg.Key, alg.IV), CryptoStreamMode.Read);

                byte[] fromEncode = new byte[input.Length];
                cStream.Read(fromEncode, 0, fromEncode.Length);  

                return new ASCIIEncoding().GetString(fromEncode).TrimEnd('\0');
            } 
            catch (CryptographicException e)
            {
                LogManager.Instance.Log().Fatal(string.Format("Cryptographic failure occured: {0}", e.Message));
                return null;
            }
        }
        #endregion

        #region helper
        public static string ConvertByteArrayToBase64(byte[] input)
        {
            return Convert.ToBase64String(input);
        }

        public static byte[] ConvertBase64ToByteArray(string input)
        {
            return Convert.FromBase64String(input);
        }
        #endregion
    }
}