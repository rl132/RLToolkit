using System;

namespace RLToolkit.Basic
{
    public class RFC2898CryptInfo
    {
        public string password { get; set; }
        public string salt { get; set; }
        public string VIKey { get; set; }

        public RFC2898CryptInfo()
        {
            password = "Password1";
            salt = "S@lt_K3Y";
            VIKey = "@1B2c3D4e5F6g7H8";
        }
    }
}

