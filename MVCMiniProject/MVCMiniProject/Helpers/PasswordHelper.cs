using System.Security.Cryptography;
using System.Text;

namespace MVCMiniProject.Helpers
{
    public static class PasswordHelper
    {
        private const string LegacySalt = "MVCMiniProject.Auth.v1";
        private const int Iterations = 100_000;
        private const int SaltSize = 16;
        private const int HashSize = 32;

        public static string Hash(string password)
        {
            var salt = RandomNumberGenerator.GetBytes(SaltSize);
            var hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, HashAlgorithmName.SHA256, HashSize);
            return $"PBKDF2.{Iterations}.{Convert.ToBase64String(salt)}.{Convert.ToBase64String(hash)}";
        }

        public static bool Verify(string password, string storedHash)
        {
            if (string.IsNullOrWhiteSpace(storedHash))
            {
                return false;
            }

            if (storedHash.StartsWith("PBKDF2.", StringComparison.Ordinal))
            {
                var parts = storedHash.Split('.');
                if (parts.Length != 4 || !int.TryParse(parts[1], out var iterations))
                {
                    return false;
                }

                var salt = Convert.FromBase64String(parts[2]);
                var expected = Convert.FromBase64String(parts[3]);
                var actual = Rfc2898DeriveBytes.Pbkdf2(password, salt, iterations, HashAlgorithmName.SHA256, expected.Length);
                return CryptographicOperations.FixedTimeEquals(actual, expected);
            }

            var legacy = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(LegacySalt + password)));
            if (legacy.Length == storedHash.Length)
            {
                return CryptographicOperations.FixedTimeEquals(
                    Encoding.UTF8.GetBytes(legacy),
                    Encoding.UTF8.GetBytes(storedHash));
            }

            return false;
        }
    }
}
