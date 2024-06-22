using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Underdog.Common.Helper
{
    public static class EncryptHelper
    {
        public static string EncryptDES(string plainText, string key)
        {
            using var desCrypto = DES.Create();
            byte[] inputBytes = Encoding.UTF8.GetBytes(plainText);
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);

            desCrypto.Key = keyBytes;
            desCrypto.Mode = CipherMode.ECB;
            desCrypto.Padding = PaddingMode.PKCS7;

            using MemoryStream memoryStream = new();
            using (CryptoStream cryptoStream = new(memoryStream, desCrypto.CreateEncryptor(), CryptoStreamMode.Write))
            {
                cryptoStream.Write(inputBytes, 0, inputBytes.Length);
                cryptoStream.FlushFinalBlock();
            }

            return Convert.ToBase64String(memoryStream.ToArray());
        }

        public static string DecryptDES(string encryptedText, string key)
        {
            using var desCrypto = DES.Create();
            byte[] inputBytes = Convert.FromBase64String(encryptedText);
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);

            desCrypto.Key = keyBytes;
            desCrypto.Mode = CipherMode.ECB;
            using MemoryStream memoryStream = new();
            using (CryptoStream cryptoStream = new(memoryStream, desCrypto.CreateDecryptor(), CryptoStreamMode.Write))
            {
                cryptoStream.Write(inputBytes, 0, inputBytes.Length);
                cryptoStream.FlushFinalBlock();
            }

            return Encoding.UTF8.GetString(memoryStream.ToArray());
        }

        public static string EncryptAES(string plainText, string key)
        {
            return string.Empty;
        }

        public static string DecryptAES(string encryptedText, string key)
        {
            return string.Empty;
        }
    }
}
