using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Security.Cryptography;

namespace KE_PDC.Services
{
    public class Hasher
    {
        private string ApiKey;

        public Hasher(string apiKey)
        {
            ApiKey = apiKey;
        }

        public byte[] GetSalt()
        {
            // generate a 256-bit salt using a secure PRNG
            byte[] salt = new byte[256 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            //derive a 256 - bit subkey(use HMACSHA1 with 10, 000 iterations)
            return salt;
        }

        public string Make(string value)
        {
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: value,
                salt: Convert.FromBase64String(ApiKey),
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));
        }

        public bool Check(string value, string hashedValue)
        {
            if (string.IsNullOrEmpty(hashedValue))
            {
                return false;
            }

            return KeyDerivation.Equals(Make(value), hashedValue);
        }
    }
}
