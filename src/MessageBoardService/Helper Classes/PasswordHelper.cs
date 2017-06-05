using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace MessageBoardService.Helper_Classes
{
    public static class PasswordHelper
    {
        #region GetHash
        public static string GetHash(string password, string salt)
        {
            byte[] saltBytes = Encoding.ASCII.GetBytes(salt);


            byte[] plainData = ASCIIEncoding.UTF8.GetBytes(password);
            byte[] plainDataAndSalt = new byte[plainData.Length + saltBytes.Length];

            for (int x = 0; x < plainData.Length; x++)
            {
                plainDataAndSalt[x] = plainData[x];
            }
            for (int n = 0; n < saltBytes.Length; n++)
            {
                plainDataAndSalt[plainData.Length + n] = saltBytes[n];
            }
            byte[] hashValue = null;

            SHA256Managed sha = new SHA256Managed();
            hashValue = sha.ComputeHash(plainDataAndSalt);
            sha.Dispose();

            byte[] result = new byte[hashValue.Length + saltBytes.Length];

            for (int x = 0; x < hashValue.Length; x++)
            {
                result[x] = hashValue[x];
            }
            for (int n = 0; n < saltBytes.Length; n++)
            {
                result[hashValue.Length + n] = saltBytes[n];
            }

            return Convert.ToBase64String(result);
        }
        #endregion

        #region GetSalt
        public static string GetSalt()
        {
            int minSaltLength = 4;
            int maxSaltLength = 16;

            byte[] SaltBytes = null;
            Random r = new Random();
            int SaltLength = r.Next(minSaltLength, maxSaltLength);
            SaltBytes = new byte[SaltLength];
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetNonZeroBytes(SaltBytes);
            string result = Convert.ToBase64String(SaltBytes);
            rng.Dispose();

            return result;
        }
        #endregion
    }
}