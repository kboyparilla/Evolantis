using System;
using System.Collections;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Evolantis.Lib
{
    public class Cipher
    {
        private static string passPhrase = null;
        private static string saltValue = null;
        private static string hashAlgorithm = null;
        private static int passwordIterations = 0;
        private static string initVector = null;
        private static int keySize = 0;

        static Cipher()
        {
            passPhrase = "kboyparilla";
            saltValue = "LLC";
            hashAlgorithm = "SHA1";
            passwordIterations = 2;
            initVector = "@1B2c3D4e5F6g7H8";
            keySize = 128;
        }

        public static string EncryptBase64(string str)
        {
            byte[] initVectorBytes = null;
            initVectorBytes = Encoding.ASCII.GetBytes(initVector);

            byte[] saltValueBytes = null;
            saltValueBytes = Encoding.ASCII.GetBytes(saltValue);
            
            byte[] plainTextBytes = null;
            plainTextBytes = Encoding.UTF8.GetBytes(str);
            
            DeriveBytes password = default(DeriveBytes);
            password = new PasswordDeriveBytes(passPhrase, saltValueBytes, hashAlgorithm, passwordIterations);
            
            byte[] keyBytes = null;
            keyBytes = password.GetBytes(keySize / 8);

            RijndaelManaged symmetricKey = default(RijndaelManaged);
            symmetricKey = new RijndaelManaged();
            
            symmetricKey.Mode = CipherMode.CBC;
          
            ICryptoTransform encryptor = default(ICryptoTransform);
            encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);

            MemoryStream memoryStream = default(MemoryStream);
            memoryStream = new MemoryStream();

            CryptoStream cryptoStream = default(CryptoStream);
            cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
            cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);

            cryptoStream.FlushFinalBlock();

            byte[] cipherTextBytes = null;
            cipherTextBytes = memoryStream.ToArray();

            memoryStream.Close();
            cryptoStream.Close();

            string cipherText = null;
            cipherText = Convert.ToBase64String(cipherTextBytes);

            return cipherText;
        }

        public static string Encrypt(string str)
        {
            return HashEncrypt(EncryptBase64(str));
        }

        public static string Decrypt(string str)
        {
            return DecryptBase64(HashDecrypt(str));
        }

        public static string DecryptBase64(string str)
        {
            byte[] initVectorBytes = null;
            initVectorBytes = Encoding.ASCII.GetBytes(initVector);

            byte[] saltValueBytes = null;
            saltValueBytes = Encoding.ASCII.GetBytes(saltValue);

            byte[] cipherTextBytes = null;
            str = str.Replace(" ", "+");
            cipherTextBytes = Convert.FromBase64String(str);
            
            DeriveBytes password = default(DeriveBytes);
            password = new PasswordDeriveBytes(passPhrase, saltValueBytes, hashAlgorithm, passwordIterations);
            
            byte[] keyBytes = null;
            keyBytes = password.GetBytes(keySize / 8);

            RijndaelManaged symmetricKey = default(RijndaelManaged);
            symmetricKey = new RijndaelManaged();
            
            symmetricKey.Mode = CipherMode.CBC;
            
            ICryptoTransform decryptor = default(ICryptoTransform);
            decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes);

            MemoryStream memoryStream = default(MemoryStream);
            memoryStream = new MemoryStream(cipherTextBytes);

            CryptoStream cryptoStream = default(CryptoStream);
            cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            
            byte[] plainTextBytes = null;
            plainTextBytes = new byte[cipherTextBytes.Length + 1];

            int decryptedByteCount = 0;
            decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);

            memoryStream.Close();
            cryptoStream.Close();
            
            string plainText = null;
            plainText = Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);

            return plainText;
        }

        public static string HashEncrypt(string str)
        {
            try
            {
                string strRet = null;
                string strSub = null;
                ArrayList arrOffsets = new ArrayList();
                int intCounter = 0;
                int intMod = 0;
                int intVal = 0;
                int intNewVal = 0;

                arrOffsets.Insert(0, 73);
                arrOffsets.Insert(1, 56);
                arrOffsets.Insert(2, 31);
                arrOffsets.Insert(3, 58);
                arrOffsets.Insert(4, 77);
                arrOffsets.Insert(5, 75);

                strRet = "";

                for (intCounter = 0; intCounter <= str.Length - 1;
                intCounter++)
                {
                    strSub = str.Substring(intCounter, 1);
                    intVal =
                    (int)System.Text.Encoding.ASCII.GetBytes(strSub)[0];
                    intMod = intCounter % arrOffsets.Count;
                    intNewVal = intVal +
                    Convert.ToInt32(arrOffsets[intMod]);
                    intNewVal = intNewVal % 256;
                    strRet = strRet + intNewVal.ToString("X2");
                }
                return strRet.ToLower();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string HashDecrypt(string str)
        {
            try
            {
                ArrayList arrOffsets = new ArrayList();
                int intCounter = 0;
                int intMod = 0;
                int intVal = 0;
                int intNewVal = 0;
                string strOut = null;
                string strSub = null;
                string strSub1 = null;
                string strDecimal = null;

                arrOffsets.Insert(0, 73);
                arrOffsets.Insert(1, 56);
                arrOffsets.Insert(2, 31);
                arrOffsets.Insert(3, 58);
                arrOffsets.Insert(4, 77);
                arrOffsets.Insert(5, 75);

                strOut = "";
                for (intCounter = 0; intCounter <= str.Length - 1;
                intCounter += 2)
                {
                    strSub = str.Substring(intCounter, 1);
                    strSub1 = str.Substring((intCounter + 1), 1);
                    intVal = int.Parse(strSub,
                    System.Globalization.NumberStyles.HexNumber) * 16 + int.Parse(strSub1,
                    System.Globalization.NumberStyles.HexNumber);
                    intMod = (intCounter / 2) % arrOffsets.Count;
                    intNewVal = intVal -
                    Convert.ToInt32(arrOffsets[intMod]) + 256;
                    intNewVal = intNewVal % 256;
                    strDecimal = ((char)intNewVal).ToString();
                    strOut = strOut + strDecimal;
                }
                return strOut;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
