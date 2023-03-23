using domainEntities.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
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
        private readonly IConfiguration configuration;

        public AuthService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

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

        public string GenerateJSONWebToken(Employee emp)
        {
            var strRole = emp.Role switch
            {
                Employee.EmployeeRole.Worker => "Worker",
                Employee.EmployeeRole.Admin => "Admin",
                _ => "None"
            };

            var issuer = configuration["JWT:Issuer"];
            var audience = configuration["JWT:Audience"];
            var key = Encoding.UTF8.GetBytes(configuration["JWT:Key"]!);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, emp.Username),
                    new Claim(ClaimTypes.Role, strRole)
                }),
                Expires = DateTime.Now.AddMinutes(5),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
