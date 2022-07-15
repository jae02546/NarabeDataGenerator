using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography;

namespace NarabeDataGenerator
{
    class EncryptDecrypt
    {
        //// 初期化ベクトル"<半角16文字（1byte=8bit, 8bit*16=128bit>"
        //private const string AES_IV_256 = @"pf69DL6GrWFyZcMK";
        //// 暗号化鍵<半角32文字（8bit*32文字=256bit）>
        //private const string AES_Key_256 = @"5TGB&YHN7UJM(IK<5TGB&YHN7UJM(IK<";

        /// <summary>
        /// 対称鍵暗号を使って文字列を暗号化する
        /// </summary>
        /// <param name="iv">対称アルゴリズムの初期ベクター</param>
        /// <param name="key">対称アルゴリズムの共有鍵</param>
        /// <param name="text">暗号化する文字列</param>
        /// <returns>暗号化された文字列</returns>
        public string Encrypt(string iv, string key, string text)
        {

            using (RijndaelManaged myRijndael = new RijndaelManaged())
            {
                // ブロックサイズ（何文字単位で処理するか）
                myRijndael.BlockSize = 128;
                // 暗号化方式はAES-256を採用
                myRijndael.KeySize = 128; //256;
                // 暗号利用モード
                myRijndael.Mode = CipherMode.CBC;
                // パディング
                myRijndael.Padding = PaddingMode.PKCS7;

                myRijndael.IV = Encoding.UTF8.GetBytes(iv);
                myRijndael.Key = Encoding.UTF8.GetBytes(key);

                // 暗号化
                ICryptoTransform encryptor = myRijndael.CreateEncryptor(myRijndael.Key, myRijndael.IV);

                byte[] encrypted;
                using (MemoryStream mStream = new MemoryStream())
                {
                    using (CryptoStream ctStream = new CryptoStream(mStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter sw = new StreamWriter(ctStream))
                        {
                            sw.Write(text);
                        }
                        encrypted = mStream.ToArray();
                    }
                }
                // Base64形式（64種類の英数字で表現）で返す
                return (System.Convert.ToBase64String(encrypted));
            }
        }

        /// <summary>
        /// 対称鍵暗号を使って暗号文を復号する
        /// </summary>
        /// <param name="iv">対称アルゴリズムの初期ベクター</param>
        /// <param name="key">対称アルゴリズムの共有鍵</param>
        /// <param name="cipher">暗号化された文字列</param>
        /// <returns>復号された文字列</returns>
        public string Decrypt(string iv, string key, string cipher)
        {
            using (RijndaelManaged rijndael = new RijndaelManaged())
            {
                // ブロックサイズ（何文字単位で処理するか）
                rijndael.BlockSize = 128;
                // 暗号化方式はAES-256を採用
                rijndael.KeySize = 128; //256;
                // 暗号利用モード
                rijndael.Mode = CipherMode.CBC;
                // パディング
                rijndael.Padding = PaddingMode.PKCS7;

                rijndael.IV = Encoding.UTF8.GetBytes(iv);
                rijndael.Key = Encoding.UTF8.GetBytes(key);

                ICryptoTransform decryptor = rijndael.CreateDecryptor(rijndael.Key, rijndael.IV);

                string plain = string.Empty;
                using (MemoryStream mStream = new MemoryStream(System.Convert.FromBase64String(cipher)))
                {
                    using (CryptoStream ctStream = new CryptoStream(mStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader sr = new StreamReader(ctStream))
                        {
                            plain = sr.ReadLine();
                        }
                    }
                }
                return plain;
            }
        }

        public string ToBase64(string text)
        {
            var foo = System.Text.Encoding.UTF8.GetBytes(text);
            var bar = System.Convert.ToBase64String(foo);

            return bar;
        }

        public string FromBase64(string text)
        {
            var foo = System.Convert.FromBase64String(text);
            var bar = System.Text.Encoding.UTF8.GetString(foo);

            return bar;
        }



    }
}
