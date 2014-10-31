using System;
using System.Security.Cryptography;
using System.IO;
using System.Xml.Serialization;

namespace RLToolkit.Basic
{
    /// <summary>RSA crypt info used to define the parameters of a RSA crypt</summary>
    public class RSACryptInfo
    {
        ///<summary>the size of the key</summary>
        public int keySize { get; set; }

        ///<summary>the private RSA Key</summary>
        public RSAParameters privateKey { get; set; }

        ///<summary>the public RSA key</summary>
        public RSAParameters publicKey { get; set; }

        /// <summary>constructor with the default parameters</summary>
        /// <param name="keySize">Key size.</param>
        public RSACryptInfo(int keySize)
        {
            this.keySize = keySize;
            RSACryptoServiceProvider csp = new RSACryptoServiceProvider(keySize);
            privateKey = csp.ExportParameters(true);
            publicKey = csp.ExportParameters(false);
        }

        /// <summary>
        /// Converts the key to string.
        /// </summary>
        /// <returns>The key to string.</returns>
        /// <param name="input">RSA Key</param>
        public string ConvertKeyToString(RSAParameters input)
        {
            StringWriter sWriter = new StringWriter();
            XmlSerializer serializer = new XmlSerializer(typeof(RSAParameters));
            serializer.Serialize(sWriter, input);
            return sWriter.ToString();
        }

        /// <summary>
        /// Converts the string to key.
        /// </summary>
        /// <returns>RSA Key.</returns>
        /// <param name="input">The key to string</param>
        public RSAParameters ConvertStringToKey(string input)
        {
            StringReader sReader = new StringReader(input);
            XmlSerializer serializer = new XmlSerializer(typeof(RSAParameters));
            return (RSAParameters)serializer.Deserialize(sReader);
        }
    }
}

