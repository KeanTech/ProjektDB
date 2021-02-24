using System;
using System.Security.Cryptography;
using System.Text;

namespace Skp_ProjektDB.Backend.Security
{
    internal class Sha256
    {
        public string Sha2Hash(byte[] data)
        {
            SHA256 sha = SHA256.Create();
            return Convert.ToBase64String(sha.ComputeHash(data));
        }
    }
}
