using System;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace Controllers
{
    public static class SecurityController
    {
        private static ConsoleColor classColor = ConsoleColor.DarkGreen;

        public static string error = string.Empty;


        public static string GetHashSha256(string text)
        {
            try
            {
                var crypt = new SHA256Managed();
                var hash = new StringBuilder();
                byte[] buffer = crypt.ComputeHash(Encoding.UTF8.GetBytes(text));

                for (int i = 0; i < buffer.Length; i++)
                    hash.Append(buffer[i].ToString("x2"));

                Sistem.WriteLog(Sistem.GetLogTag(Sistem.EnumLogTags.CRYPTOGRAPHY), "SHA256(" + text + ") = " + hash.ToString().ToLower(), false, false, true, classColor);
                return hash.ToString();
            }
            catch (Exception ex)
            {
                Sistem.WriteLog(ex, Sistem.GetLogTag(Sistem.EnumLogTags.CRYPTOGRAPHYCERROR) + "GetHashSha256(string text)", true);
                error += ex.Message + "\n";
                return null;
            }
        }
        public static string GetHashSha256(byte[] text)
        {
            try
            {
                var crypt = new SHA256Managed();
                var hash = new StringBuilder();
                byte[] buffer = crypt.ComputeHash(text);

                for (int i = 0; i < buffer.Length; i++)
                    hash.Append(buffer[i].ToString("x2"));

                Sistem.WriteLog(Sistem.GetLogTag(Sistem.EnumLogTags.CRYPTOGRAPHY), "SHA256(" + Encoding.UTF8.GetString(text) + ") = " + hash.ToString().ToLower(), false, false, true, classColor);
                return hash.ToString();
            }
            catch (Exception ex)
            {
                Sistem.WriteLog(ex, Sistem.GetLogTag(Sistem.EnumLogTags.CRYPTOGRAPHYCERROR) + "GetHashSha256(string text)", true);
                error += ex.Message + "\n";
                return null;
            }
        }
        public static byte[] GetHashSha256(string text, bool returnBytes = true)
        {
            try
            {
                var crypt = new SHA256Managed();
                var hash = new StringBuilder();
                byte[] buffer = crypt.ComputeHash(Encoding.UTF8.GetBytes(text));
                Sistem.WriteLog(Sistem.GetLogTag(Sistem.EnumLogTags.CRYPTOGRAPHY), "SHA256(" + text + ") = " + hash.ToString().ToLower(), false, false, true, classColor);
                return buffer;
            }
            catch (Exception ex)
            {
                Sistem.WriteLog(ex, Sistem.GetLogTag(Sistem.EnumLogTags.CRYPTOGRAPHYCERROR) + "GetHashSha256(string text)", true);
                error += ex.Message + "\n";
                return null;
            }
        }
        public static byte[] GetHashSha256(byte[] text, bool returnBytesAndTextAsBytes = true)
        {
            try
            {
                var crypt = new SHA256Managed();
                var hash = new StringBuilder();
                byte[] buffer = crypt.ComputeHash(text);
                Sistem.WriteLog(Sistem.GetLogTag(Sistem.EnumLogTags.CRYPTOGRAPHY), "SHA256(" + Encoding.UTF8.GetString(text) + ") = " + hash.ToString().ToLower(), false, false, true, classColor);
                return buffer;
            }
            catch (Exception ex)
            {
                Sistem.WriteLog(ex, Sistem.GetLogTag(Sistem.EnumLogTags.CRYPTOGRAPHYCERROR) + "GetHashSha256(string text)", true);
                error += ex.Message + "\n";
                return null;
            }
        }

        public static string MD5Hash(string text)
        {
            try
            {
                StringBuilder hash = new StringBuilder();
                MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
                byte[] buffer = md5.ComputeHash(Encoding.UTF8.GetBytes(text));

                for (int i = 0; i < buffer.Length; i++)
                    hash.Append(buffer[i].ToString("X2"));

                Sistem.WriteLog(Sistem.GetLogTag(Sistem.EnumLogTags.CRYPTOGRAPHY), "MD5(" + text + ") = " + hash.ToString().ToLower(), false, false, true, classColor);
                return hash.ToString().ToLower();
            }
            catch (Exception ex)
            {
                Sistem.WriteLog(ex, Sistem.GetLogTag(Sistem.EnumLogTags.CRYPTOGRAPHYCERROR) + "MD5Hash(string text)", true);
                error += ex.Message + "\n";
                return null;
            }
        }
        public static string MD5Hash(byte[] text)
        {
            try
            {
                StringBuilder hash = new StringBuilder();
                MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
                byte[] buffer = md5.ComputeHash(text);

                for (int i = 0; i < buffer.Length; i++)
                    hash.Append(buffer[i].ToString("X2"));

                Sistem.WriteLog(Sistem.GetLogTag(Sistem.EnumLogTags.CRYPTOGRAPHY), "MD5(" + Encoding.UTF8.GetString(text) + ") = " + hash.ToString().ToLower(), false, false, true, classColor);
                return hash.ToString().ToLower();
            }
            catch (Exception ex)
            {
                Sistem.WriteLog(ex, Sistem.GetLogTag(Sistem.EnumLogTags.CRYPTOGRAPHYCERROR) + "MD5Hash(string text)", true);
                error += ex.Message + "\n";
                return null;
            }
        }
        public static byte[] MD5Hash(string text, bool returnBytes = true)
        {
            try
            {
                StringBuilder hash = new StringBuilder();
                MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
                byte[] buffer = md5.ComputeHash(Encoding.UTF8.GetBytes(text));
                for (int i = 0; i < buffer.Length; i++)
                    hash.Append(buffer[i].ToString("X2"));
                Sistem.WriteLog(Sistem.GetLogTag(Sistem.EnumLogTags.CRYPTOGRAPHY), "MD5(" + text + ") = " + hash.ToString().ToLower(), false, false, true, classColor);
                return buffer;
            }
            catch (Exception ex)
            {
                Sistem.WriteLog(ex, Sistem.GetLogTag(Sistem.EnumLogTags.CRYPTOGRAPHYCERROR) + "MD5Hash(string text)", true);
                error += ex.Message + "\n";
                return null;
            }
        }
        public static byte[] MD5Hash(byte[] text, bool returnBytes = true)
        {
            try
            {
                StringBuilder hash = new StringBuilder();
                MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
                byte[] buffer = md5.ComputeHash(text);
                for (int i = 0; i < buffer.Length; i++)
                    hash.Append(buffer[i].ToString("X2"));
                Sistem.WriteLog(Sistem.GetLogTag(Sistem.EnumLogTags.CRYPTOGRAPHY), "MD5(" + Encoding.UTF8.GetString(text) + ") = " + hash.ToString().ToLower(), false, false, true, classColor);
                return buffer;
            }
            catch (Exception ex)
            {
                Sistem.WriteLog(ex, Sistem.GetLogTag(Sistem.EnumLogTags.CRYPTOGRAPHYCERROR) + "MD5Hash(string text)", true);
                error += ex.Message + "\n";
                return null;
            }
        }

        public static string DESEncrypt(string message, string password)
        {
            try
            {
                // Encode message and password
                byte[] messageBytes = Encoding.UTF8.GetBytes(message);
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

                // Set encryption settings -- Use password for both key and init. vector
                DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
                ICryptoTransform transform = provider.CreateEncryptor(passwordBytes, passwordBytes);
                CryptoStreamMode mode = CryptoStreamMode.Write;

                // Set up streams and encrypt
                MemoryStream memStream = new MemoryStream();
                CryptoStream cryptoStream = new CryptoStream(memStream, transform, mode);
                cryptoStream.Write(messageBytes, 0, messageBytes.Length);
                cryptoStream.FlushFinalBlock();

                // Read the encrypted message from the memory stream
                byte[] encryptedMessageBytes = new byte[memStream.Length];
                memStream.Position = 0;
                memStream.Read(encryptedMessageBytes, 0, encryptedMessageBytes.Length);

                Sistem.WriteLog(Sistem.GetLogTag(Sistem.EnumLogTags.CRYPTOGRAPHY), "DESEncrypt(" + message + ") = " + Encoding.UTF8.GetString(encryptedMessageBytes), false, false, true, classColor);
                // Encode the encrypted message as base64 string
                return Convert.ToBase64String(encryptedMessageBytes);
            }
            catch (Exception ex)
            {
                Sistem.WriteLog(ex, Sistem.GetLogTag(Sistem.EnumLogTags.CRYPTOGRAPHYCERROR) + "DESEncrypt(string message, string password)", true);
                error += ex.Message + "\n";
                return null;
            }
        }
        public static byte[] DESEncrypt(byte[] message, string password)
        {
            try
            {
                // Encode message and password
                byte[] messageBytes = message;
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

                // Set encryption settings -- Use password for both key and init. vector
                DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
                ICryptoTransform transform = provider.CreateEncryptor(passwordBytes, passwordBytes);
                CryptoStreamMode mode = CryptoStreamMode.Write;

                // Set up streams and encrypt
                MemoryStream memStream = new MemoryStream();
                CryptoStream cryptoStream = new CryptoStream(memStream, transform, mode);
                cryptoStream.Write(messageBytes, 0, messageBytes.Length);
                cryptoStream.FlushFinalBlock();

                // Read the encrypted message from the memory stream
                byte[] encryptedMessageBytes = new byte[memStream.Length];
                memStream.Position = 0;
                memStream.Read(encryptedMessageBytes, 0, encryptedMessageBytes.Length);

                Sistem.WriteLog(Sistem.GetLogTag(Sistem.EnumLogTags.CRYPTOGRAPHY), "DESEncrypt(" + message + ") = " + Encoding.UTF8.GetString(encryptedMessageBytes), false, false, false, classColor);
                // Encode the encrypted message as base64 string
                return encryptedMessageBytes;
            }
            catch (Exception ex)
            {
                Sistem.WriteLog(ex, Sistem.GetLogTag(Sistem.EnumLogTags.CRYPTOGRAPHYCERROR) + "DESEncrypt(string message, string password)", true);
                error += ex.Message + "\n";
                return null;
            }
        }


        public static string DESDecrypt(string encryptedMessage, string password, bool toBase64 = false)
        {
            try
            {
                // Convert encrypted message and password to bytes
                byte[] encryptedMessageBytes = Convert.FromBase64String(encryptedMessage);
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

                // Set encryption settings -- Use password for both key and init. vector
                DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
                ICryptoTransform transform = provider.CreateDecryptor(passwordBytes, passwordBytes);
                CryptoStreamMode mode = CryptoStreamMode.Write;

                // Set up streams and decrypt
                MemoryStream memStream = new MemoryStream();
                CryptoStream cryptoStream = new CryptoStream(memStream, transform, mode);
                cryptoStream.Write(encryptedMessageBytes, 0, encryptedMessageBytes.Length);
                cryptoStream.FlushFinalBlock();

                // Read decrypted message from memory stream
                byte[] decryptedMessageBytes = new byte[memStream.Length];
                memStream.Position = 0;
                memStream.Read(decryptedMessageBytes, 0, decryptedMessageBytes.Length);

                // Encode deencrypted binary data to base64 string
                string result = string.Empty;
                if (toBase64)
                    result = Convert.ToBase64String(decryptedMessageBytes);
                else
                    result = Encoding.UTF8.GetString(decryptedMessageBytes);

                Sistem.WriteLog(Sistem.GetLogTag(Sistem.EnumLogTags.CRYPTOGRAPHY), "DESDecrypt(" + encryptedMessage + ") = " + result, false, false, true, classColor);
                return result;
            }
            catch (Exception ex)
            {
                Sistem.WriteLog(ex, Sistem.GetLogTag(Sistem.EnumLogTags.CRYPTOGRAPHYCERROR) + "DESDecrypt(string encryptedMessage, string password, bool toBase64 = false)", true);
                error += ex.Message + "\n";
                return null;
            }
        }
        public static string DESDecrypt(byte[] encryptedMessage, string password, bool toBase64 = false)
        {
            try
            {
                // Convert encrypted message and password to bytes
                byte[] encryptedMessageBytes = encryptedMessage;
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

                // Set encryption settings -- Use password for both key and init. vector
                DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
                ICryptoTransform transform = provider.CreateDecryptor(passwordBytes, passwordBytes);
                CryptoStreamMode mode = CryptoStreamMode.Write;

                // Set up streams and decrypt
                MemoryStream memStream = new MemoryStream();
                CryptoStream cryptoStream = new CryptoStream(memStream, transform, mode);
                cryptoStream.Write(encryptedMessageBytes, 0, encryptedMessageBytes.Length);
                cryptoStream.FlushFinalBlock();

                // Read decrypted message from memory stream
                byte[] decryptedMessageBytes = new byte[memStream.Length];
                memStream.Position = 0;
                memStream.Read(decryptedMessageBytes, 0, decryptedMessageBytes.Length);

                // Encode deencrypted binary data to base64 string
                string result = string.Empty;
                if (toBase64)
                    result = Convert.ToBase64String(decryptedMessageBytes);
                else
                    result = Encoding.UTF8.GetString(decryptedMessageBytes);

                Sistem.WriteLog(Sistem.GetLogTag(Sistem.EnumLogTags.CRYPTOGRAPHY), "DESDecrypt(" + encryptedMessage + ") = " + result, false, false, true, classColor);
                return result;
            }
            catch (Exception ex)
            {
                Sistem.WriteLog(ex, Sistem.GetLogTag(Sistem.EnumLogTags.CRYPTOGRAPHYCERROR) + "DESDecrypt(string encryptedMessage, string password, bool toBase64 = false)", true);
                error += ex.Message + "\n";
                return null;
            }
        }
        public static byte[] DESDecrypt(string encryptedMessage, string password, bool returnBytes, bool toBase64 = false)
        {
            try
            {
                // Convert encrypted message and password to bytes
                byte[] encryptedMessageBytes = Convert.FromBase64String(encryptedMessage);
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

                // Set encryption settings -- Use password for both key and init. vector
                DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
                ICryptoTransform transform = provider.CreateDecryptor(passwordBytes, passwordBytes);
                CryptoStreamMode mode = CryptoStreamMode.Write;

                // Set up streams and decrypt
                MemoryStream memStream = new MemoryStream();
                CryptoStream cryptoStream = new CryptoStream(memStream, transform, mode);
                cryptoStream.Write(encryptedMessageBytes, 0, encryptedMessageBytes.Length);
                cryptoStream.FlushFinalBlock();

                // Read decrypted message from memory stream
                byte[] decryptedMessageBytes = new byte[memStream.Length];
                memStream.Position = 0;
                memStream.Read(decryptedMessageBytes, 0, decryptedMessageBytes.Length);

                // Encode deencrypted binary data to base64 string
                byte[] result = null;
                if (toBase64)
                    result = Encoding.UTF8.GetBytes(Convert.ToBase64String(decryptedMessageBytes));
                else
                    result = decryptedMessageBytes;

                Sistem.WriteLog(Sistem.GetLogTag(Sistem.EnumLogTags.CRYPTOGRAPHY), "DESDecrypt(" + encryptedMessage + ") = " + result, false, false, true, classColor);
                return result;
            }
            catch (Exception ex)
            {
                Sistem.WriteLog(ex, Sistem.GetLogTag(Sistem.EnumLogTags.CRYPTOGRAPHYCERROR) + "DESDecrypt(string encryptedMessage, string password, bool toBase64 = false)", true);
                error += ex.Message + "\n";
                return null;
            }
        }
        public static byte[] DESDecrypt(byte[] encryptedMessage, string password, bool returnBytes, bool toBase64 = false)
        {
            try
            {
                // Convert encrypted message and password to bytes
                byte[] encryptedMessageBytes = encryptedMessage;
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

                // Set encryption settings -- Use password for both key and init. vector
                DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
                ICryptoTransform transform = provider.CreateDecryptor(passwordBytes, passwordBytes);
                CryptoStreamMode mode = CryptoStreamMode.Write;

                // Set up streams and decrypt
                MemoryStream memStream = new MemoryStream();
                CryptoStream cryptoStream = new CryptoStream(memStream, transform, mode);
                cryptoStream.Write(encryptedMessageBytes, 0, encryptedMessageBytes.Length);
                cryptoStream.FlushFinalBlock();

                // Read decrypted message from memory stream
                byte[] decryptedMessageBytes = new byte[memStream.Length];
                memStream.Position = 0;
                memStream.Read(decryptedMessageBytes, 0, decryptedMessageBytes.Length);

                // Encode deencrypted binary data to base64 string
                byte[] result = null;
                if (toBase64)
                    result = Encoding.UTF8.GetBytes(Convert.ToBase64String(decryptedMessageBytes));
                else
                    result = decryptedMessageBytes;

                Sistem.WriteLog(Sistem.GetLogTag(Sistem.EnumLogTags.CRYPTOGRAPHY), "DESDecrypt(" + encryptedMessage + ") = " + result, false, false, true, classColor);
                return result;
            }
            catch (Exception ex)
            {
                Sistem.WriteLog(ex, Sistem.GetLogTag(Sistem.EnumLogTags.CRYPTOGRAPHYCERROR) + "DESDecrypt(string encryptedMessage, string password, bool toBase64 = false)", true);
                error += ex.Message + "\n";
                return null;
            }
        }


        public static byte[] RSAController(byte[] data, bool encryptTrueDecryptFalse)
        {
            try
            {
                //Create a UnicodeEncoder to convert between byte array and string.
                UnicodeEncoding ByteConverter = new UnicodeEncoding();

                //Create byte arrays to hold original, encrypted, and decrypted data.
                byte[] dataToEncryptOrDecrypt = data;
                byte[] encryptedData = null;
                byte[] decryptedData = null;

                //Create a new instance of RSACryptoServiceProvider to generate
                //public and private key data.
                using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
                {
                    //Pass the data to ENCRYPT, the public key information 
                    //(using RSACryptoServiceProvider.ExportParameters(false),and a boolean flag specifying no OAEP padding.
                    //(encryptTrueDecryptFalse = true) Encrypt Method, else Decrypt Method
                    if (encryptTrueDecryptFalse)
                        encryptedData = RSAEncrypt(dataToEncryptOrDecrypt, RSA.ExportParameters(false), false);
                    else
                    {
                        //Pass the data to DECRYPT, the private key information 
                        //(using RSACryptoServiceProvider.ExportParameters(true),and a boolean flag specifying no OAEP padding.
                        decryptedData = RSADecrypt(dataToEncryptOrDecrypt, RSA.ExportParameters(true), false);
                        //Display the decrypted plaintext to the console. 
                        Console.WriteLine("Decrypted plaintext: {0}", ByteConverter.GetString(decryptedData));
                    }
                }

                if (encryptedData != null)
                    return encryptedData;
                else if (decryptedData != null)
                    return decryptedData;
                else return null;
            }
            catch (Exception e)
            {
                //Catch this exception in case the encryption did not succeed.
                error += e.Message + "\n";
                return null;
            }
        }
        public static string RSAController(string data, bool encryptTrueDecryptFalse)
        {
            try
            {
                //Create a UnicodeEncoder to convert between byte array and string.
                UnicodeEncoding ByteConverter = new UnicodeEncoding();

                //Create byte arrays to hold original, encrypted, and decrypted data.
                byte[] dataToEncryptOrDecrypt = ByteConverter.GetBytes(data);
                byte[] encryptedData = null;
                byte[] decryptedData = null;

                //Create a new instance of RSACryptoServiceProvider to generate
                //public and private key data.
                using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
                {
                    //Pass the data to ENCRYPT, the public key information 
                    //(using RSACryptoServiceProvider.ExportParameters(false),and a boolean flag specifying no OAEP padding.
                    //(encryptTrueDecryptFalse = true) Encrypt Method, else Decrypt Method
                    if (encryptTrueDecryptFalse)
                        encryptedData = RSAEncrypt(dataToEncryptOrDecrypt, RSA.ExportParameters(false), false);
                    else
                    {
                        //Pass the data to DECRYPT, the private key information 
                        //(using RSACryptoServiceProvider.ExportParameters(true),and a boolean flag specifying no OAEP padding.
                        decryptedData = RSADecrypt(dataToEncryptOrDecrypt, RSA.ExportParameters(true), false);
                        //Display the decrypted plaintext to the console. 
                        Console.WriteLine("Decrypted plaintext: {0}", ByteConverter.GetString(decryptedData));
                    }
                }

                if (encryptedData != null)
                    return Encoding.UTF8.GetString(encryptedData);
                else if (decryptedData != null)
                    return Encoding.UTF8.GetString(decryptedData);
                else return null;
            }
            catch (Exception e)
            {
                //Catch this exception in case the encryption did not succeed.
                error += e.Message + "\n";
                return null;
            }
        }

        public static byte[] RSAEncrypt(byte[] DataToEncrypt, RSAParameters RSAKeyInfo, bool DoOAEPPadding)
        {
            try
            {
                byte[] encryptedData;
                //Create a new instance of RSACryptoServiceProvider.
                using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
                {
                    //Import the RSA Key information. This only needs toinclude the public key information.
                    RSA.ImportParameters(RSAKeyInfo);
                    //Encrypt the passed byte array and specify OAEP padding.  
                    //OAEP padding is only available on Microsoft Windows XP or later.  
                    encryptedData = RSA.Encrypt(DataToEncrypt, DoOAEPPadding);
                }
                Sistem.WriteLog(Sistem.GetLogTag(Sistem.EnumLogTags.CRYPTOGRAPHY), "RSADecrypt(" + Encoding.UTF8.GetString(DataToEncrypt) + ") = " + Encoding.UTF8.GetString(encryptedData), false, false, true, classColor);
                return encryptedData;
            }
            //Catch and display a CryptographicException  
            //to the console.
            catch (CryptographicException e)
            {
                Sistem.WriteLog(e, Sistem.GetLogTag(Sistem.EnumLogTags.CRYPTOGRAPHYCERROR) + "RSAEncrypt(byte[] DataToEncrypt, RSAParameters RSAKeyInfo, bool DoOAEPPadding)", true);
                error += e.Message + "\n";
                return null;
            }
        }
        public static byte[] RSADecrypt(byte[] DataToDecrypt, RSAParameters RSAKeyInfo, bool DoOAEPPadding)
        {
            try
            {
                byte[] decryptedData;

                //Create a new instance of RSACryptoServiceProvider.
                using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
                {
                    //Import the RSA Key information. This needs to include the private key information.
                    RSA.ImportParameters(RSAKeyInfo);

                    //Decrypt the passed byte array and specify OAEP padding.  
                    //OAEP padding is only available on Microsoft Windows XP or later.  
                    decryptedData = RSA.Decrypt(DataToDecrypt, DoOAEPPadding);
                }
                Sistem.WriteLog(Sistem.GetLogTag(Sistem.EnumLogTags.CRYPTOGRAPHY), "RSADecrypt(" + Encoding.UTF8.GetString(DataToDecrypt) + ") = " + Encoding.UTF8.GetString(decryptedData), false, false, true, classColor);
                return decryptedData;
            }
            //Catch and display a CryptographicException to the console.
            catch (CryptographicException e)
            {
                Sistem.WriteLog(e, Sistem.GetLogTag(Sistem.EnumLogTags.CRYPTOGRAPHYCERROR) + "RSADecrypt(byte[] DataToDecrypt, RSAParameters RSAKeyInfo, bool DoOAEPPadding)", true);
                error += e.Message + "\n";
                return null;
            }
        }

        public static string RSAEncryption(string strText)
        {
            string base64Encrypted = string.Empty;
            try
            {
                var publicKey = "<RSAKeyValue><Modulus>21wEnTU+mcD2w0Lfo1Gv4rtcSWsQJQTNa6gio05AOkV/Er9w3Y13Ddo5wGtjJ19402S71HUeN0vbKILLJdRSES5MHSdJPSVrOqdrll/vLXxDxWs/U0UT1c8u6k/Ogx9hTtZxYwoeYqdhDblof3E75d9n2F0Zvf6iTb4cI7j6fMs=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";

                var testData = Encoding.UTF8.GetBytes(strText);

                using (var rsa = new RSACryptoServiceProvider(1024))
                {
                    try
                    {
                        // client encrypting data with public key issued by server                    
                        rsa.FromXmlString(publicKey.ToString());

                        var encryptedData = rsa.Encrypt(testData, true);

                        base64Encrypted = Convert.ToBase64String(encryptedData);

                        Sistem.WriteLog(Sistem.GetLogTag(Sistem.EnumLogTags.CRYPTOGRAPHY), "Public Key RSAEncryption(" + strText + ") = " + base64Encrypted, false, false, true, classColor);

                    }
                    finally
                    {
                        rsa.PersistKeyInCsp = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Sistem.WriteLog(ex, Sistem.GetLogTag(Sistem.EnumLogTags.CRYPTOGRAPHYCERROR) + "RSAEncryption(string strText)", true);
                error += ex.Message + "\n";
                return null;
            }
            return base64Encrypted;
        }
        public static string RSADecryption(string strText)
        {
            string decryptedData = string.Empty;
            try
            {
                var privateKey = "<RSAKeyValue><Modulus>21wEnTU+mcD2w0Lfo1Gv4rtcSWsQJQTNa6gio05AOkV/Er9w3Y13Ddo5wGtjJ19402S71HUeN0vbKILLJdRSES5MHSdJPSVrOqdrll/vLXxDxWs/U0UT1c8u6k/Ogx9hTtZxYwoeYqdhDblof3E75d9n2F0Zvf6iTb4cI7j6fMs=</Modulus><Exponent>AQAB</Exponent><P>/aULPE6jd5IkwtWXmReyMUhmI/nfwfkQSyl7tsg2PKdpcxk4mpPZUdEQhHQLvE84w2DhTyYkPHCtq/mMKE3MHw==</P><Q>3WV46X9Arg2l9cxb67KVlNVXyCqc/w+LWt/tbhLJvV2xCF/0rWKPsBJ9MC6cquaqNPxWWEav8RAVbmmGrJt51Q==</Q><DP>8TuZFgBMpBoQcGUoS2goB4st6aVq1FcG0hVgHhUI0GMAfYFNPmbDV3cY2IBt8Oj/uYJYhyhlaj5YTqmGTYbATQ==</DP><DQ>FIoVbZQgrAUYIHWVEYi/187zFd7eMct/Yi7kGBImJStMATrluDAspGkStCWe4zwDDmdam1XzfKnBUzz3AYxrAQ==</DQ><InverseQ>QPU3Tmt8nznSgYZ+5jUo9E0SfjiTu435ihANiHqqjasaUNvOHKumqzuBZ8NRtkUhS6dsOEb8A2ODvy7KswUxyA==</InverseQ><D>cgoRoAUpSVfHMdYXW9nA3dfX75dIamZnwPtFHq80ttagbIe4ToYYCcyUz5NElhiNQSESgS5uCgNWqWXt5PnPu4XmCXx6utco1UVH8HGLahzbAnSy6Cj3iUIQ7Gj+9gQ7PkC434HTtHazmxVgIR5l56ZjoQ8yGNCPZnsdYEmhJWk=</D></RSAKeyValue>";

                var testData = Encoding.UTF8.GetBytes(strText);

                using (var rsa = new RSACryptoServiceProvider(1024))
                {
                    try
                    {
                        var base64Encrypted = strText;

                        // server decrypting data with private key                    
                        rsa.FromXmlString(privateKey);

                        var resultBytes = Convert.FromBase64String(base64Encrypted);
                        var decryptedBytes = rsa.Decrypt(resultBytes, true);
                        decryptedData = Encoding.UTF8.GetString(decryptedBytes);
                        Sistem.WriteLog(Sistem.GetLogTag(Sistem.EnumLogTags.CRYPTOGRAPHY), "Public Key RSADecryption(" + strText + ") = " + decryptedData, false, false, true, classColor);
                    }
                    finally
                    {
                        rsa.PersistKeyInCsp = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Sistem.WriteLog(ex, Sistem.GetLogTag(Sistem.EnumLogTags.CRYPTOGRAPHYCERROR) + "RSADecryption(string strText)", true);
                error += ex.Message + "\n";
                return null;
            }

            return decryptedData;
        }

        public static bool FileEncryptDES(string filePath, string password)
        {
            bool result;
            string fileEncryptedPath = filePath + ".crp";

            try
            {
                if (File.Exists(filePath))
                {
                    byte[] buffer = File.ReadAllBytes(filePath);
                    byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

                    // Set encryption settings -- Use password for both key and init. vector
                    DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
                    ICryptoTransform transform = provider.CreateEncryptor(passwordBytes, passwordBytes);
                    CryptoStreamMode mode = CryptoStreamMode.Write;

                    // Set up streams and encrypt
                    MemoryStream memStream = new MemoryStream();
                    CryptoStream cryptoStream = new CryptoStream(memStream, transform, mode);
                    cryptoStream.Write(buffer, 0, buffer.Length);
                    cryptoStream.FlushFinalBlock();

                    // Read the encrypted message from the memory stream
                    byte[] encryptedFile = new byte[memStream.Length];
                    memStream.Position = 0;
                    memStream.Read(encryptedFile, 0, encryptedFile.Length);

                    File.WriteAllBytes(fileEncryptedPath, encryptedFile);

                    if (File.Exists(fileEncryptedPath))
                        result = true;
                    else
                        result = false;
                }
                else
                    result = false;

                if (result)
                    Sistem.WriteLog(Sistem.GetLogTag(Sistem.EnumLogTags.CRYPTOGRAPHY), "Create File Encrypt DES in " + fileEncryptedPath, false, false, true, classColor);
                else
                    Sistem.WriteLog(Sistem.GetLogTag(Sistem.EnumLogTags.CRYPTOGRAPHY), "File  " + filePath + " does not exist", false, false, true, classColor);
            }
            catch (Exception ex)
            {
                Sistem.WriteLog(ex, Sistem.GetLogTag(Sistem.EnumLogTags.CRYPTOGRAPHYCERROR) + "FileEncryptDES(string filePath, string password)", true);
                error += ex.Message + "\n";
                result = false;
            }
            return result;
        }
        public static bool FileDecryptDES(string encryptedFilePath, string password)
        {
            bool result;
            string fileDecryptedPath = encryptedFilePath.Substring(0, (encryptedFilePath.LastIndexOf(".")));
            try
            {
                byte[] buffer = File.ReadAllBytes(encryptedFilePath);
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

                // Set encryption settings -- Use password for both key and init. vector
                DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
                ICryptoTransform transform = provider.CreateDecryptor(passwordBytes, passwordBytes);
                CryptoStreamMode mode = CryptoStreamMode.Write;

                // Set up streams and decrypt
                MemoryStream memStream = new MemoryStream();
                CryptoStream cryptoStream = new CryptoStream(memStream, transform, mode);
                cryptoStream.Write(buffer, 0, buffer.Length);
                cryptoStream.FlushFinalBlock();

                // Read decrypted message from memory stream
                byte[] decryptedFile = new byte[memStream.Length];
                memStream.Position = 0;
                memStream.Read(decryptedFile, 0, decryptedFile.Length);

                File.WriteAllBytes(fileDecryptedPath, decryptedFile);

                if (File.Exists(fileDecryptedPath))
                    result = true;
                else
                    result = false;

                if (result)
                    Sistem.WriteLog(Sistem.GetLogTag(Sistem.EnumLogTags.CRYPTOGRAPHY), "Create DES Decrypted File in " + fileDecryptedPath, false, false, true, classColor);
                else
                    Sistem.WriteLog(Sistem.GetLogTag(Sistem.EnumLogTags.CRYPTOGRAPHY), "File  " + encryptedFilePath + " does not exist", false, false, true, classColor);
            }
            catch (Exception ex)
            {
                Sistem.WriteLog(ex, Sistem.GetLogTag(Sistem.EnumLogTags.CRYPTOGRAPHYCERROR) + "FileDecryptDES(string encryptedFilePath, string password)", true);
                error += ex.Message + "\n";
                result = false;
            }
            return result;
        }

        public static string ToBase64Loop(string dataToEncode, int loop)
        {
            string result = dataToEncode;
            try
            {
                for (int i = 0; i < loop; i++)
                    result = Convert.ToBase64String(Encoding.UTF8.GetBytes(result));

                Sistem.WriteLog(Sistem.GetLogTag(Sistem.EnumLogTags.CRYPTOGRAPHY) + "ToBase64Loop(" + dataToEncode + ") x" + loop.ToString() + " = " + result, "", false, false, true, classColor);
            }
            catch (Exception ex)
            {
                Sistem.WriteLog(ex, Sistem.GetLogTag(Sistem.EnumLogTags.CRYPTOGRAPHYCERROR) + "ToBase64Loop(string dataToEncode, int loop)", true);
            }
            return result;
        }
        public static string FromBase64Loop(string dataEncoded, int loop)
        {
            string result = dataEncoded;
            try
            {
                byte[] buffer = Encoding.UTF8.GetBytes(dataEncoded);
                string charArray = string.Empty;
                for (int i = 0; i < loop; i++)
                {
                    charArray = Encoding.UTF8.GetString(buffer);
                    buffer = Convert.FromBase64CharArray(charArray.ToCharArray(), 0, charArray.Length);
                }

                result = Encoding.UTF8.GetString(buffer);
                Sistem.WriteLog(Sistem.GetLogTag(Sistem.EnumLogTags.CRYPTOGRAPHY) + "FromBase64Loop(" + dataEncoded + ") x" + loop.ToString() + " = " + result, "", false, false, true, classColor);
            }
            catch (Exception ex)
            {
                Sistem.WriteLog(ex, Sistem.GetLogTag(Sistem.EnumLogTags.CRYPTOGRAPHYCERROR) + "FromBase64Loop(string dataEncoded, int loop)", true);
            }
            return result;
        }

    }
}
