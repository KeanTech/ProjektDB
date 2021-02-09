using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace SkpDbLib.Security
{
    internal class Encryption
    {
        public string Encrypt(byte[] data, byte[] salt, int itterations = 50000)
        {
            Rijndael rijndael = Rijndael.Create();
            Rfc2898DeriveBytes rfc = new Rfc2898DeriveBytes("SkpDbPass", salt, itterations);
            rijndael.Key = rfc.GetBytes(32);
            rijndael.IV = rfc.GetBytes(16);

            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, rijndael.CreateEncryptor(), CryptoStreamMode.Write);

            cs.Write(data, 0, data.Length);
            cs.FlushFinalBlock();
            cs.Close();

            return Convert.ToBase64String(ms.ToArray());
        }
    }
}
