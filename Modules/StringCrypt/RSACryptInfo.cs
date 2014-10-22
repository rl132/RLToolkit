using System;
using System.Security.Cryptography;
using System.IO;
using System.Xml.Serialization;

namespace RLToolkit.Basic
{
    public class RSACryptInfo
    {
        public int keySize { get; set; }
        public RSAParameters privateKey { get; set; }
        public RSAParameters publicKey { get; set; }

        public RSACryptInfo(int keySize)
        {
            this.keySize = keySize;
            RSACryptoServiceProvider csp = new RSACryptoServiceProvider(keySize);
            privateKey = csp.ExportParameters(true);
            publicKey = csp.ExportParameters(false);
        }

        public string ConvertKeyToString(RSAParameters input)
        {
            StringWriter sWriter = new StringWriter();
            XmlSerializer serializer = new XmlSerializer(typeof(RSAParameters));
            serializer.Serialize(sWriter, input);
            return sWriter.ToString();
        }

        public RSAParameters ConvertStringToKey(string input)
        {
            StringReader sReader = new StringReader(input);
            XmlSerializer serializer = new XmlSerializer(typeof(RSAParameters));
            return (RSAParameters)serializer.Deserialize(sReader);
        }
    }
}

