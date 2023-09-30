using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BlockStorm.Utils
{
    public class Crypto
    {
        private static readonly string AESKey = Config.GetAesKey();

       public static string ComputeSHA256Hash(string data)
        {
            using SHA256 sha256 = SHA256.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(data);
            byte[] hashBytes = sha256.ComputeHash(bytes);
            var hashBuilder = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                hashBuilder.Append(hashBytes[i].ToString("x2")); // 将每个字节转换为十六进制字符串
            }
            return hashBuilder.ToString();

        }

        public static string? RST_AesEncrypt_Base64(string Data)
        {
            if (string.IsNullOrEmpty(Data))
            {
                return null;
            }
            if (string.IsNullOrEmpty(AESKey))
            {
                return null;
            }
            string Vector = new(AESKey[..16].Reverse().ToArray());
            byte[] plainBytes = Encoding.UTF8.GetBytes(Data);
            byte[] bKey = new Byte[32];
            Array.Copy(Encoding.UTF8.GetBytes(AESKey.PadRight(bKey.Length)), bKey, bKey.Length);
            byte[] bVector = new Byte[16];
            Array.Copy(Encoding.UTF8.GetBytes(Vector.PadRight(bVector.Length)), bVector, bVector.Length);
            byte[]? Cryptograph = null; // 加密后的密文  
            Aes aes = Aes.Create();
            //add 
            aes.Mode = CipherMode.CBC;//兼任其他语言的des
            aes.BlockSize = 128;
            aes.Padding = PaddingMode.PKCS7;
            //add end
            try
            {
                // 开辟一块内存流  
                using var Memory = new MemoryStream();
                // 把内存流对象包装成加密流对象  
                using var Encryptor = new CryptoStream(Memory,
                 aes.CreateEncryptor(bKey, bVector),
                 CryptoStreamMode.Write);
                // 明文数据写入加密流  
                Encryptor.Write(plainBytes, 0, plainBytes.Length);
                Encryptor.FlushFinalBlock();

                Cryptograph = Memory.ToArray();
            }
            catch
            {
                Cryptograph = null;
            }
            return Convert.ToBase64String(Cryptograph);
        }

        /// <summary>
        ///  AES base64 解密算法；Key为16位
        /// </summary>
        /// <param name="Data">需要解密的字符串</param>
        /// <returns></returns>
        public static string? RST_AesDecrypt_Base64(string Data)
        {
            try
            {
                if (string.IsNullOrEmpty(Data))
                {
                    return null;
                }
                if (string.IsNullOrEmpty(AESKey))
                {
                    return null;
                }
                string Vector = new(AESKey[..16].Reverse().ToArray());
                byte[] encryptedBytes = Convert.FromBase64String(Data);
                byte[] bKey = new Byte[32];
                Array.Copy(Encoding.UTF8.GetBytes(AESKey.PadRight(bKey.Length)), bKey, bKey.Length);
                byte[] bVector = new Byte[16];
                Array.Copy(Encoding.UTF8.GetBytes(Vector.PadRight(bVector.Length)), bVector, bVector.Length);
                byte[]? original = null; // 解密后的明文  
                Aes aes = Aes.Create();
                //add 
                aes.Mode = CipherMode.CBC;//兼任其他语言的des
                aes.BlockSize = 128;
                aes.Padding = PaddingMode.PKCS7;
                //add end
                try
                {
                    // 开辟一块内存流，存储密文  
                    using var Memory = new MemoryStream(encryptedBytes);
                    //把内存流对象包装成加密流对象  
                    using var Decryptor = new CryptoStream(Memory,
                    aes.CreateDecryptor(bKey, bVector),
                    CryptoStreamMode.Read);
                    // 明文存储区  
                    using var originalMemory = new MemoryStream();
                    byte[] Buffer = new Byte[1024];
                    Int32 readBytes = 0;
                    while ((readBytes = Decryptor.Read(Buffer, 0, Buffer.Length)) > 0)
                    {
                        originalMemory.Write(Buffer, 0, readBytes);
                    }
                    original = originalMemory.ToArray();
                }
                catch
                {
                    original = null;
                }
                return Encoding.UTF8.GetString(original);
            }
            catch { return null; }
        }

    }
}
