using System.Security.Cryptography;

namespace MVCMiniProject.Helpers
{
    public static class VerificationTokenHelper
    {
        public static (string PlainToken, string Hash) Create()
        {
            var bytes = RandomNumberGenerator.GetBytes(32);
            var plain = Convert.ToHexString(bytes);
            return (plain, Hash(plain));
        }

        public static string Hash(string plainToken)
        {
            var bytes = SHA256.HashData(Convert.FromHexString(plainToken.Trim()));
            return Convert.ToHexString(bytes);
        }
    }
}
