using System;
using System.Security.Cryptography;

namespace Skp_ProjektDB.Backend.Security
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
