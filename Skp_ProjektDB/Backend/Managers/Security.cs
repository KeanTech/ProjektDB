using SkpDbLib.Security;
using System;
using System.Collections.Generic;
using System.Text;

namespace SkpDbLib.Managers
{
    public class Security : IDisposable
    {
        private Encryption _encryption = new Encryption();
        private Decryption _decryption = new Decryption();
        private Sha256 _sha = new Sha256();
        private SaltGenerator _saltGenerator = new SaltGenerator();

        public string GenerateSalt()
        {
            return _saltGenerator.GenerateSalt();
        }
        public string Hash(byte[] data)
        {
            return _sha.Sha2Hash(data);
        }

        public string Encrypt(byte[] data, byte[] salt, int itterations = 50000)
        {
            return _encryption.Encrypt(data, salt, itterations);
        }

        public string Decrypt(byte[] data, byte[] salt, int itterations = 50000)
        {
            return _decryption.Decrypt(data, salt, itterations);
        }

        public void Dispose()
        {
            _encryption = null;
            _decryption = null;
            _sha = null;
            _saltGenerator = null;
        }
    }
}
