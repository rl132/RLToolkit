using System;
using System.Security.Cryptography;
using System.Text;

namespace RLToolkit.Basic
{
    public static class MD5Helper
    {
        /// <summary>
        /// Implicit method that will call the ComputeHash method with lower case
        /// </summary>
        /// <returns>The hash</returns>
        /// <param name="input">the input string</param>
        public static string ComputeHash(string input)
        {
            LogManager.Instance.Log().Debug("Computing implicit hash for:" + Environment.NewLine + input);
            return ComputeHash(input, false);
        }

        /// <summary>
        /// Method that will compute a MD5 hash.
        /// </summary>
        /// <returns>The hash</returns>
        /// <param name="input">the input string</param>
        /// <param name="upperCase">If set to <c>true</c> the hash will be all upper case.</param>
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

        /// <summary>
        /// Method that validate that the input string computes as the reference hash
        /// </summary>
        /// <returns><c>true</c>, if the input computes to was reference, <c>false</c> otherwise.</returns>
        /// <param name="input">Input string</param>
        /// <param name="reference">Reference hash</param>
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