using System;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.Collections;

namespace RLToolkit.Basic
{
    public static class StringCrypt
    {
        public static string Encrypt(string input)
        {
            // implicit crypt
            LogManager.Instance.Log().Debug("Calling an implicit Encrypt");
            return EncryptRfc2898(input);
        }

        public static string Decrypt(string input)
        {
            // implicit Decrypt
            LogManager.Instance.Log().Debug("Calling an implicit Decrypt");
            return DecryptRfc2898(input);
        }

        public static string EncryptRfc2898(string input)
        {
            // todo: refactor this to pass the parameters in arguments
            string PasswordHash = "P@@Sw0rd";
            string SaltKey = "S@LT&KEY";
            string VIKey = "@1B2c3D4e5F6g7H8";

            LogManager.Instance.Log().Debug("Calling Encrypt RFC2989");
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(input);

            byte[] keyBytes = new Rfc2898DeriveBytes(PasswordHash, Encoding.ASCII.GetBytes(SaltKey)).GetBytes(256 / 8);
            var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.Zeros };
            var encryptor = symmetricKey.CreateEncryptor(keyBytes, Encoding.ASCII.GetBytes(VIKey));
            
            byte[] cipherTextBytes;

            using (var memoryStream = new MemoryStream())
            {
                using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                    cryptoStream.FlushFinalBlock();
                    cipherTextBytes = memoryStream.ToArray();
                    cryptoStream.Close();
                }
                memoryStream.Close();
            }
            return Convert.ToBase64String(cipherTextBytes);
        }

        public static string DecryptRfc2898(string input)
        {
            // todo: refactor this to pass the parameters in arguments
            string PasswordHash = "P@@Sw0rd";
            string SaltKey = "S@LT&KEY";
            string VIKey = "@1B2c3D4e5F6g7H8";

            LogManager.Instance.Log().Debug("Calling Decrypt RFC2989");
            byte[] cipherTextBytes = Convert.FromBase64String(input);
            byte[] keyBytes = new Rfc2898DeriveBytes(PasswordHash, Encoding.ASCII.GetBytes(SaltKey)).GetBytes(256 / 8);
            var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.None };

            var decryptor = symmetricKey.CreateDecryptor(keyBytes, Encoding.ASCII.GetBytes(VIKey));
            var memoryStream = new MemoryStream(cipherTextBytes);
            var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            byte[] plainTextBytes = new byte[cipherTextBytes.Length];

            int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
            memoryStream.Close();
            cryptoStream.Close();
            return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount).TrimEnd("\0".ToCharArray());
        }
    }
}