using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Licesing
{
    public static class SymmetricEncryptor
    {
        private const int AesBlockByteSize = 128 / 8;

        private const int PasswordSaltByteSize = 128 / 8;
        private const int PasswordByteSize = 256 / 8;
        private const int PasswordIterationCount = 100_000;

        private static readonly Encoding StringEncoding = Encoding.UTF8;
        private static readonly RandomNumberGenerator Random = RandomNumberGenerator.Create();

        public static byte[] EncryptString(string toEncrypt, string password)
        {
            using (var aes = Aes.Create())
            {
                var keySalt = GenerateRandomBytes(PasswordSaltByteSize);
                var key = GetKey(password, keySalt);
                var iv = GenerateRandomBytes(AesBlockByteSize);

                using (var encryptor = aes.CreateEncryptor(key, iv))
                {
                    var plainText = StringEncoding.GetBytes(toEncrypt);
                    var cipherText = encryptor
                        .TransformFinalBlock(plainText, 0, plainText.Length);

                    var result = MergeArrays(keySalt, iv, cipherText);
                    return result;
                }
            }
        }

        public static string DecryptToString(byte[] encryptedData, string password)
        {
            using (var aes = Aes.Create())
            {
                var keySalt = encryptedData.Take(PasswordSaltByteSize).ToArray();
                var key = GetKey(password, keySalt);
                var iv = encryptedData
                    .Skip(PasswordSaltByteSize).Take(AesBlockByteSize).ToArray();
                var cipherText = encryptedData
                    .Skip(PasswordSaltByteSize + AesBlockByteSize).ToArray();

                using (var encryptor = aes.CreateDecryptor(key, iv))
                {
                    var decryptedBytes = encryptor
                        .TransformFinalBlock(cipherText, 0, cipherText.Length);
                    return StringEncoding.GetString(decryptedBytes);
                }
            }
        }

        private static byte[] GetKey(string password, byte[] passwordSalt)
        {
            var keyBytes = StringEncoding.GetBytes(password);

            using (var derivator = new Rfc2898DeriveBytes(
                keyBytes, passwordSalt,
                PasswordIterationCount, HashAlgorithmName.SHA256))
            {
                return derivator.GetBytes(PasswordByteSize);
            }
        }

        private static byte[] GenerateRandomBytes(int numberOfBytes)
        {
            var randomBytes = new byte[numberOfBytes];
            Random.GetBytes(randomBytes);
            return randomBytes;
        }

        private static byte[] MergeArrays(params byte[][] arrays)
        {
            var merged = new byte[arrays.Sum(a => a.Length)];
            var mergeIndex = 0;
            for (int i = 0; i < arrays.GetLength(0); i++)
            {
                arrays[i].CopyTo(merged, mergeIndex);
                mergeIndex += arrays[i].Length;
            }

            return merged;
        }
    }
}
