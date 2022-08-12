using System;
using System.Security.Cryptography;
using System.Text;

namespace Tools
{
    public static class HashingUtility
    {
        public static string GetHash(string value)
        {
            MD5 md5 = MD5.Create();
            byte[] hash = md5.ComputeHash(Encoding.UTF8.GetBytes(value));
            return Convert.ToBase64String(hash);
        }
    }
}