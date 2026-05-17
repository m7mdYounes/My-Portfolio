using MyPortfolio.Helpers.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace MyPortfolio.Helpers.Implementations
{
    public class HashHelper : IHashHelper
    {
        public string Sha256Hash(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return string.Empty;

            var bytes = Encoding.UTF8.GetBytes(value);

            var hashBytes = SHA256.HashData(bytes);

            return Convert.ToHexString(hashBytes).ToLowerInvariant();
        }
    }
}
