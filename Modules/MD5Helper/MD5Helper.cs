using System;
using System.Security.Cryptography;
using System.Text;

namespace RLToolkit.Basic
{
    public static class MD5Helper
    {
        public static string ComputeHash(string input)
        {
            LogManager.Instance.Log().Debug("Computing implicit hash for:" + Environment.NewLine + input);
            return ComputeHash(input, false);
        }

        public static string ComputeHash(string input, bool upperCase)
        {
            string casing = upperCase ? "uppercase" : "lowercase";
            LogManager.Instance.Log().Debug(string.Format("Computing {0} hash for:" + Environment.NewLine + input, casing));
            MD5 hash = MD5.Create();
            byte[] output = hash.ComputeHash(Encoding.ASCII.GetBytes(input));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < output.Length; i++)
            {
                string uc = "x2";
                if (upperCase)
                {
                    uc = "X2";
                }

                sBuilder.Append(output[i].ToString(uc));
            }
            return sBuilder.ToString();
        }
        
        public static bool VerifyMd5Hash(string input, string reference)
        {
            LogManager.Instance.Log().Debug("verifying hash for:" + Environment.NewLine + input + Environment.NewLine + "Should be:" + Environment.NewLine + reference);
            string hashOfInput = ComputeHash(input, false);
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;
            if (0 == comparer.Compare(hashOfInput, reference))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}