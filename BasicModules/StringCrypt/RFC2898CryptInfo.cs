using System;

namespace RLToolkit.Basic
{
    /// <summary>RFC2898 crypt info used to define the parameters of a RFC2898 crypt</summary>
    public class RFC2898CryptInfo
    {
        /// <summary>Gets or sets the password.</summary>
        public string password { get; set; }

        /// <summary>Gets or sets the salt.</summary>
        public string salt { get; set; }

        /// <summary>Gets or sets the VIKey.</summary>
        public string VIKey { get; set; }

        /// <summary>constructor with the default parameters</summary>
        public RFC2898CryptInfo()
        {
            password = "Password1";
            salt = "S@lt_K3Y";
            VIKey = "@1B2c3D4e5F6g7H8";
        }
    }
}

