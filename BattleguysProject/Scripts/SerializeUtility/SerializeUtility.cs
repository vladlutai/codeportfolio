using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using UnityEngine;

namespace SerializeUtility
{
    public static class SerializeUtility
    {
        private static readonly byte[] Key =
        {
        4, 20, 161, 43, 142, 12, 215, 213, 95, 136, 235, 156, 17, 32, 232, 88, 55, 198, 79, 66, 133, 105, 57, 8, 192,
        67, 144, 220, 155, 128, 79, 240
        };
        private static readonly byte[] Iv = { 167, 234, 172, 115, 51, 159, 137, 253, 187, 196, 78, 179, 54, 191, 170, 78 };
        public static void Serialize<T>(T saves, string savesPath)
        {
            {
                using (var rijndaelManaged = new RijndaelManaged())
                {
                    rijndaelManaged.Padding = PaddingMode.PKCS7;
                    var encryptor = rijndaelManaged.CreateEncryptor(Key, Iv);

                    using (var fileStream = new FileStream(savesPath, FileMode.Create))
                    {
                        using (var cryptoStream = new CryptoStream(fileStream, encryptor, CryptoStreamMode.Write))
                        {
                            var formatter = new BinaryFormatter();
                            formatter.Serialize(cryptoStream, saves);
                            cryptoStream.FlushFinalBlock();
                            fileStream.Flush(true);
                        }
                    }
                }
            }
        }  

        public static T Deserialize<T>(string savesPath)
        {
            if (!File.Exists(savesPath))
                return default;

            T saves;
            using (var rijndaelManaged = new RijndaelManaged())
            {
                rijndaelManaged.Padding = PaddingMode.PKCS7;
                var decryptor = rijndaelManaged.CreateDecryptor(Key, Iv);

                using (var fileStream = new FileStream(savesPath, FileMode.Open, FileAccess.Read))
                {
                    try
                    {
                        using (var cryptoStream = new CryptoStream(fileStream, decryptor, CryptoStreamMode.Read))
                        {
                            var formatter = new BinaryFormatter();
                            saves = (T)formatter.Deserialize(cryptoStream);
                        }
                    }
                    catch (Exception)
                    {
                        saves = default;
                    }
                }
            }
            return saves;
        }

        public static string SerializeToJSon<T>(T serializeObject)
        {
            return JsonUtility.ToJson(serializeObject);
        }

        public static T DeserializeJSon<T>(string serializedObject)
        {
            return JsonUtility.FromJson<T>(serializedObject);
        }
    }
}