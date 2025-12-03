using System.Security.Cryptography;

namespace SmartSchool.Infrastructure.Services
{
    public static class PasswordHasher
    {
        //PKBDF2 with HMAC-SHA256, 100,000 iterations
        private const int Iterations = 100000;
        private const int SaltSize = 16; // 128 bits
        private const int SaltLength = 32;  //256 bits

        public static string Hash(string password)
        {
            var salt = RandomNumberGenerator.GetBytes(SaltSize);
            var hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, HashAlgorithmName.SHA256, SaltLength);
            var hashBytes = new byte[SaltSize + SaltLength];
            Array.Copy(salt, 0, hashBytes, 0, SaltSize);
            Array.Copy(hash, 0, hashBytes, SaltSize, SaltLength);
            return Convert.ToBase64String(hashBytes);
        }

        public static bool Verify(string password, string hashedPassword)
        {
            var hashBytes = Convert.FromBase64String(hashedPassword);
            var salt = new byte[SaltSize];
            Array.Copy(hashBytes, 0, salt, 0, SaltSize);
            var storedHash = new byte[SaltLength];
            Array.Copy(hashBytes, SaltSize, storedHash, 0, SaltLength);
            var computedHash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, HashAlgorithmName.SHA256, SaltLength);
            return CryptographicOperations.FixedTimeEquals(storedHash, computedHash);
        }
    }
}
