using Adspro.Contract.Models;
using Adspro.Contract.Providers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using SpanJson;
using System.Text;

namespace Adspro.Providers
{
    internal class TokenProvider(IConfiguration configuration, ILogger<TokenProvider> logger) : ITokenProvider
    {
        private readonly byte[] _key = Convert.FromHexString(configuration["TOKEN_ENCRYPTION_KEY"] ?? throw new ApplicationException("EncryptionKey is undefined"));

        public Guid? GetUserIdFromToken(string token)
        {
            try
            {
                var decrypted = DecryptBytes(Convert.FromHexString(token));
                return new Guid(decrypted);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "");
                return null;
            }
        }

        public string GetUserIdToken(Guid userId)
        {
            var encrypted = EncryptBytes(userId.ToByteArray());
            return Convert.ToHexString(encrypted);
        }

        private byte[] EncryptBytes(byte[] data)
        {
            using var aes = CreateAes();
            using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            return PerformCryptography(data, encryptor);
        }

        private byte[] DecryptBytes(byte[] data)
        {
            using var aes = CreateAes();
            using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            return PerformCryptography(data, decryptor);
        }

        private byte[] PerformCryptography(byte[] data, ICryptoTransform cryptoTransform)
        {
            using var ms = new MemoryStream();
            using var cryptoStream = new CryptoStream(ms, cryptoTransform, CryptoStreamMode.Write);
            cryptoStream.Write(data, 0, data.Length);
            cryptoStream.FlushFinalBlock();
            return ms.ToArray();
        }
        private Aes CreateAes()
        {
            var aes = Aes.Create();
            aes.KeySize = 128;
            aes.BlockSize = 128;
            aes.Mode = CipherMode.ECB;
            aes.Padding = PaddingMode.ISO10126;
            aes.Key = _key;
            return aes;
        }
    }
}
