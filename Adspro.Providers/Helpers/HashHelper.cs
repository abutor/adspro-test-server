using System.Security.Cryptography;
using System.Text;

namespace Adspro.Providers.Helpers
{
    internal static class HashHelper
    {
        public static string ComputeHash(string input)
        {
            var bytes = Encoding.UTF8.GetBytes(input);
            var hash = SHA256.HashData(bytes);
            return Convert.ToHexString(hash);
        }
    }
}
