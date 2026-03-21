using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

using Microsoft.Extensions.Configuration;

namespace MARN_API.Services
{
    public class EncryptionService : IEncryptionService
    {
        private readonly byte[] _key;

        public EncryptionService(IConfiguration configuration)
        {
            var keyString = configuration["EncryptionSettings:SecretKey"];
            if (string.IsNullOrEmpty(keyString))
            {
                throw new InvalidOperationException("EncryptionSettings:SecretKey is missing from appsettings.json.");
            }

            // AES-256 requires exactly a 32-byte key
            _key = Encoding.UTF8.GetBytes(keyString);
            if (_key.Length != 32)
            {
                throw new InvalidOperationException("EncryptionSettings:SecretKey must be exactly 32 characters long!");
            }
        }

        public string Encrypt(string plainText)
        {
            if (string.IsNullOrEmpty(plainText)) return plainText;

            using var aes = Aes.Create();
            aes.Key = _key;
            aes.GenerateIV(); // Produce a random cryptographically secure Initialization Vector

            using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            using var ms = new MemoryStream();
            
            // Prepend the randomly generated IV to the stream so we can extract it during Decryption
            ms.Write(aes.IV, 0, aes.IV.Length);

            using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
            using (var sw = new StreamWriter(cs))
            {
                sw.Write(plainText);
            }

            return Convert.ToBase64String(ms.ToArray());
        }

        public string Decrypt(string cipherText)
        {
            if (string.IsNullOrEmpty(cipherText)) return cipherText;

            try 
            {
                var fullCipher = Convert.FromBase64String(cipherText);

                using var aes = Aes.Create();
                aes.Key = _key;

                // Extract the IV (first 16 bytes stored during Encryption)
                var iv = new byte[aes.BlockSize / 8];
                Array.Copy(fullCipher, 0, iv, 0, iv.Length);
                aes.IV = iv;

                using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                using var ms = new MemoryStream(fullCipher, iv.Length, fullCipher.Length - iv.Length);
                using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
                using var sr = new StreamReader(cs);

                return sr.ReadToEnd();
            }
            catch
            {
                // Fallback: If decryption fails, it usually means the string was saved 
                // in the database as Plain Text before encryption was implemented.
                return cipherText; 
            }
        }
    }
}
