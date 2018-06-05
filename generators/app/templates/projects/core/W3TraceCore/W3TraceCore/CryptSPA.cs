using System;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace W3TraceCore
{
    public class CryptSPA
    {
        private string fraseSecretaWSPA = "@biro agora .net";
        private string valorSalt = "3|r0";
        private string hashAlgorithm = "SHA1";                // SHA1 ou MD5
        private int passwordIterations = 5;                   // qq valor
        private string initVector = "03|R0#&M#D0T#N&T";       // must be 16 bytes
        private int keySize = 256;

        private string convStrToHex(string sAsciiString)
        {
            string hex = "";

            foreach (char c in sAsciiString)
            {
                int tmp = c;
                hex += String.Format("{0:x2}", (uint)System.Convert.ToUInt32(tmp.ToString()));
            }
            return hex.ToUpper();
        }

        private string convHexToStr(string sHexValue)
        {
            string StrValue = "";

            while (sHexValue.Length > 0)
            {
                StrValue += System.Convert.ToChar(System.Convert.ToUInt32(sHexValue.Substring(0, 2), 16)).ToString();
                sHexValue = sHexValue.Substring(2, sHexValue.Length - 2);
            }
            return StrValue;
        }

        public string geraContraSenha(string sOperador, string sUsuario)
        {
            string strRet = "";
            string sInput = "@" + sOperador + "@" + sOperador;
            strRet = encryptAES(sInput, fraseSecretaWSPA, valorSalt, hashAlgorithm, passwordIterations, initVector, keySize);

            return strRet;
        }

        public string geraChavePublica(string sContraSenha)
        {
            string strRet = "";
            string strChave = "";
            strRet = encryptAES(sContraSenha, fraseSecretaWSPA, valorSalt, hashAlgorithm, passwordIterations, initVector, 128);
            strChave = strRet.Substring(0, 4);
            strRet = convStrToHex(strChave);

            return strRet;
        }

        internal string encryptAES(string sDados, string fraseSecreta, string saltValue, string hashAlgorithm, int passwordIterations, string initVector, int keySize)
        {
            // Converte strings em byte arrays e aceita somente caracteres ASCII
            byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
            byte[] saltValueBytes = Encoding.ASCII.GetBytes(saltValue);

            // Converte dados em byte array.
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(sDados);

            // Criar um password para criar a chave derivada
            PasswordDeriveBytes password = new PasswordDeriveBytes(fraseSecreta, saltValueBytes, hashAlgorithm, passwordIterations);

            // Usar o password para gerar o pseudo-random bytes para encripitar
            byte[] keyBytes = password.GetBytes(keySize / 8);

            // Criar o objeto de encriptação Rijndael
            RijndaelManaged symmetricKey = new RijndaelManaged();
            symmetricKey.Mode = CipherMode.CBC;

            // Gerar encryptor com base na chave criada
            ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);

            // Encriptando.
            cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
            cryptoStream.FlushFinalBlock();

            // Convert our encrypted data from a memory stream into a byte array.
            byte[] cipherTextBytes = memoryStream.ToArray();

            // Close both streams.
            memoryStream.Close();
            cryptoStream.Close();

            // Converter dado encripitado para string base64-encoded.
            string cipherText = Convert.ToBase64String(cipherTextBytes);

            return cipherText;
        }

        internal string decryptAES(string sDadosEncriptados, string fraseSecreta, string saltValue, string hashAlgorithm, int passwordIterations, string initVector, int keySize)
        {
            // Converte strings em byte arrays e aceita somente caracteres ASCII
            byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
            byte[] saltValueBytes = Encoding.ASCII.GetBytes(saltValue);

            // Converte dados encripitados em byte array.
            byte[] cipherTextBytes = Convert.FromBase64String(sDadosEncriptados);

            PasswordDeriveBytes password = new PasswordDeriveBytes(fraseSecreta, saltValueBytes, hashAlgorithm, passwordIterations);
            byte[] keyBytes = password.GetBytes(keySize / 8);

            RijndaelManaged symmetricKey = new RijndaelManaged();
            symmetricKey.Mode = CipherMode.CBC;

            ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes);

            MemoryStream memoryStream = new MemoryStream(cipherTextBytes);
            CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);

            byte[] plainTextBytes = new byte[cipherTextBytes.Length];

            int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);

            memoryStream.Close();
            cryptoStream.Close();

            string plainText = Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);

            return plainText;
        }

        public string encryptDES(string sDados, string sChave)
        {
            DESCryptoServiceProvider DES = new DESCryptoServiceProvider();

            DES.Mode = CipherMode.ECB;
            //DES.Key = GetKey(password);
            DES.Key = Encoding.UTF8.GetBytes(sChave);
            DES.Padding = PaddingMode.PKCS7;

            ICryptoTransform DESEncrypt = DES.CreateEncryptor();
            Byte[] Buffer = ASCIIEncoding.ASCII.GetBytes(sDados);

            return Convert.ToBase64String(DESEncrypt.TransformFinalBlock(Buffer, 0, Buffer.Length));
        }

        public string decryptDES(string sDados, string sChave)
        {
            DESCryptoServiceProvider DES = new DESCryptoServiceProvider();

            DES.Mode = CipherMode.ECB;
            DES.Key = Encoding.UTF8.GetBytes(sChave);
            DES.Padding = PaddingMode.PKCS7;

            ICryptoTransform DESEncrypt = DES.CreateDecryptor();
            Byte[] Buffer = Convert.FromBase64String(sDados);

            return Encoding.UTF8.GetString(DESEncrypt.TransformFinalBlock(Buffer, 0, Buffer.Length));
        }

        public string encryptTripleDES(string source, string key)
        {
            TripleDESCryptoServiceProvider desCryptoProvider = new TripleDESCryptoServiceProvider();
            MD5CryptoServiceProvider hashMD5Provider = new MD5CryptoServiceProvider();

            byte[] byteHash;
            byte[] byteBuff;

            byteHash = hashMD5Provider.ComputeHash(Encoding.UTF8.GetBytes(key));
            desCryptoProvider.Key = byteHash;
            desCryptoProvider.Mode = CipherMode.ECB; //CBC, CFB
            byteBuff = Encoding.UTF8.GetBytes(source);

            string encoded =
                Convert.ToBase64String(desCryptoProvider.CreateEncryptor().TransformFinalBlock(byteBuff, 0, byteBuff.Length));
            return encoded;
        }

        public string decryptTripleDES(string encodedText, string key)
        {
            TripleDESCryptoServiceProvider desCryptoProvider = new TripleDESCryptoServiceProvider();
            MD5CryptoServiceProvider hashMD5Provider = new MD5CryptoServiceProvider();

            byte[] byteHash;
            byte[] byteBuff;

            byteHash = hashMD5Provider.ComputeHash(Encoding.UTF8.GetBytes(key));
            desCryptoProvider.Key = byteHash;
            desCryptoProvider.Mode = CipherMode.ECB; //CBC, CFB
            byteBuff = Convert.FromBase64String(encodedText);

            string plaintext = Encoding.UTF8.GetString(desCryptoProvider.CreateDecryptor().TransformFinalBlock(byteBuff, 0, byteBuff.Length));
            return plaintext;

        }
    }
}