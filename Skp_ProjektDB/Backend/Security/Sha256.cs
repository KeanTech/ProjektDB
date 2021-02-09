using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace SkpDbLib.Security
{
    internal class Sha256
    {
        public string Sha2Hash(byte[] data)
        {
            SHA256 sha = SHA256.Create();
            return Encoding.UTF8.GetString(sha.ComputeHash(data));
        }
    }
}
