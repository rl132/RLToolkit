using System;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.Collections;

using RLToolkit.Logger;

namespace RLToolkit.Basic
{
    /// <summary>
    /// String crypt.
    /// </summary>
    public static class StringCrypt
    {
        /// <summary>Static TripleDES info to use if none are supplied</summary>
        public static TripleDES TripleDESAlg;

        /// <summary>Static RSA crypt info info to use if none are supplied</summary>
        public static RSACryptInfo RSA_cryptInfo;

        /// <summary>Static RFC2898 info to use if none are supplied</summary>
        public static RFC2898CryptInfo RFC2898_cryptInfo;

        #region implicit encrypt/decrypt
        /// <summary>
        /// Method to encrypt with the default technique
        /// </summary>
        /// <param name="input">the string to encrypt</param>
        /// <returns>a byte array of the encrypted data</returns>
        public static byte[] Encrypt(string input)
        {
            // implicit crypt
            LogManager.Instance.Log().Debug("Calling an implicit Encrypt");
            return EncryptRfc2898(input);
        }

        /// <summary>
        /// Method to decryt with the default technique
        /// </summary>
        /// <param name="input">the byte array to decrypt</param>
        /// <returns>decrypted string data</returns>
        public static string Decrypt(byte[] input)
        {
            // implicit Decrypt
            LogManager.Instance.Log().Debug("Calling an implicit Decrypt");
            return DecryptRfc2898(input);
        }
        #endregion

        #region RFC2989
        /// <summary>
        /// Method to encrypt with the RFC2898 technique, with the static RFC2898 info
        /// </summary>
        /// <param name="input">the string to encrypt</param>
        /// <returns>a byte array of the encrypted data</returns>
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

        /// <summary>
        /// Method to encrypt with the RFC2898 technique, with a supplied RFC2898 info
        /// </summary>
        /// <param name="input">the string to encrypt</param>
        /// <param name="cryptInfo">the RFC2898 crypt info</param>
        /// <returns>a byte array of the encrypted data</returns>
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

        /// <summary>
        /// Method to decrypt with the RFC2898 technique, with the static RFC2898 info
        /// </summary>
        /// <param name="input">the byte array to decrypt</param>
        /// <returns>decrypted string data</returns>
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

        /// <summary>
        /// Method to decrypt with the RFC2898 technique, with a supplied RFC2898 info
        /// </summary>
        /// <param name="input">the byte array to decrypt</param>
        /// <param name="cryptInfo">the RFC2898 crypt info</param>
        /// <returns>decrypted string data</returns>
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
        /// <summary>
        /// Method to encrypt with the RSA technique, with the static RSA info
        /// </summary>
        /// <param name="input">the string to encrypt</param>
        /// <returns>a byte array of the encrypted data</returns>
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

        /// <summary>
        /// Method to encrypt with the RSA technique, with a supplied RSA info
        /// </summary>
        /// <param name="input">the string to encrypt</param>
        /// <param name="cryptInfo">the RSA crypt info</param>
        /// <returns>a byte array of the encrypted data</returns>
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

        /// <summary>
        /// Method to decrypt with the RSA technique, with the static RSA info
        /// </summary>
        /// <param name="input">the byte array to decrypt</param>
        /// <returns>decrypted string data</returns>
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

        /// <summary>
        /// Method to decrypt with the RSA technique, with a supplied RSA info
        /// </summary>
        /// <param name="input">the byte array to decrypt</param>
        /// <param name="cryptInfo">the RSA crypt info</param>
        /// <returns>decrypted string data</returns>
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
        /// <summary>
        /// Method to encrypt with the TripleDES technique, with the static TripleDES instance
        /// </summary>
        /// <param name="input">the string to encrypt</param>
        /// <returns>a byte array of the encrypted data</returns>
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

        /// <summary>
        /// Method to encrypt with the TripleDES technique, with a supplied TripleDES instance
        /// </summary>
        /// <param name="input">the string to encrypt</param>
        /// <param name="alg">the TripleDES instance</param>
        /// <returns>a byte array of the encrypted data</returns>
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

        /// <summary>
        /// Method to decrypt with the TripleDES technique, with a supplied TripleDES instance
        /// </summary>
        /// <param name="input">the byte array to decrypt</param>
        /// <returns>decrypted string data</returns>
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

        /// <summary>
        /// Method to decrypt with the TripleDES technique, with a supplied TripleDES instance
        /// </summary>
        /// <param name="input">the byte array to decrypt</param>
        /// <param name="alg">the TripleDES instance</param>
        /// <returns>decrypted string data</returns>
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
        /// <summary>
        /// Converts the byte array to base64 string.
        /// </summary>
        /// <returns>The string representation of the array in base64</returns>
        /// <param name="input">the byte array</param>
        public static string ConvertByteArrayToBase64(byte[] input)
        {
            return Convert.ToBase64String(input);
        }

        /// <summary>
        /// Converts the base64 string to byte array.
        /// </summary>
        /// <returns>The byte array</returns>
        /// <param name="input">The string representation of the array in base64.</param>
        public static byte[] ConvertBase64ToByteArray(string input)
        {
            return Convert.FromBase64String(input);
        }
        #endregion
    }
}