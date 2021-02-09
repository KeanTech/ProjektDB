using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace SkpDbLib.Security
{
    internal class SaltGenerator
    {
        public string GenerateSalt()
        {
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] salt = new byte[1024];
            rng.GetBytes(salt);

            return Convert.ToBase64String(salt);
        }
    }
}
