using domainEntities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace service
{
    public class AuthService
    {
        private const int keySize = 64;
        private const int iterations = 350000;
        private readonly HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA512;

        public string GetHashAndSalt(string password)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(keySize);
            var hash = Rfc2898DeriveBytes.Pbkdf2(Encoding.UTF8.GetBytes(password), salt, iterations, hashAlgorithm, keySize);
            return Convert.ToHexString(hash) + Convert.ToHexString(salt);
        }

        public bool VerifyHash(string password, string hashSalt)
        {
            var newHash = Rfc2898DeriveBytes.Pbkdf2(Encoding.UTF8.GetBytes(password),
                    Convert.FromHexString(hashSalt.Substring(128)), iterations, hashAlgorithm, keySize);
            return newHash.SequenceEqual(Convert.FromHexString(hashSalt.Substring(0, 128)));
        }
    }
}
