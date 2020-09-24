using System;
using System.Security.Cryptography;
using System.Text;

namespace InstitutionService.Helper
{
    public class CryptographyHelper
    {
        private static bool _optimalAsymmetricEncryptionPadding = false;

        //These keys are of 512byte
        private readonly static string PublicKey = "NTEyITxSU0FLZXlWYWx1ZT48TW9kdWx1cz56bEFkZ1Z6bW1WM2dGVFRWbzh5NW5iTGxhTnNiVWRtQmNVRWExN0tIakZpT0Erck5QR0g1VEcrdXduS3oyVmd4T2kzQ1R0R2NvTWZzMEYzQlViVkZWUT09PC9Nb2R1bHVzPjxFeHBvbmVudD5BUUFCPC9FeHBvbmVudD48L1JTQUtleVZhbHVlPg==";
        private readonly static string PrivateKey = "NTEyITxSU0FLZXlWYWx1ZT48TW9kdWx1cz56bEFkZ1Z6bW1WM2dGVFRWbzh5NW5iTGxhTnNiVWRtQmNVRWExN0tIakZpT0Erck5QR0g1VEcrdXduS3oyVmd4T2kzQ1R0R2NvTWZzMEYzQlViVkZWUT09PC9Nb2R1bHVzPjxFeHBvbmVudD5BUUFCPC9FeHBvbmVudD48UD4xWmUzVytuTGpWOGFPZWltMEZ3OXM3MzM5ZjFQSTd2QUZvbVFzQlZ5MTNNPTwvUD48UT45MFprem53SG1FZ201dGNwOVRzcVEwdE9xRlJyNEpZU2pvT3JoMFBON2hjPTwvUT48RFA+UnNaakI1M015U2QrTlRvV0xHeWtOVzc1UHhXVXA2NkswK3o1ZUo5aTBtaz08L0RQPjxEUT5NMDFpaXNpRzF0eFpYZGxVTmpIUlFlYjA2YzJpR0JWcXdqQ2xQTW9maG44PTwvRFE+PEludmVyc2VRPkozalNieWhYeGxqZ0tBOEpnQjR2K2xTNmNMdXJRUVlSaG0vQmhJbWtHNm89PC9JbnZlcnNlUT48RD5kVlBPVThFWFN1UytDSkZSWmFPSmM0dEhONGhUVXdyVXVDbUVNR0pRLzVoZW96ZU9GMktHTGd2NXFybVlHMXVZWERNMm5qbDVTSDNvMzZsMUl4eHRpUT09PC9EPjwvUlNBS2V5VmFsdWU+";

        public static string Encrypt(string plainText)
        {
            int keySize = 0;
            string publicKeyXml = "";

            GetKeyFromEncryptionString(PublicKey, out keySize, out publicKeyXml);

            var encrypted = Encrypt(Encoding.UTF8.GetBytes(plainText), keySize, publicKeyXml);

            return Convert.ToBase64String(encrypted);
        }

        private static byte[] Encrypt(byte[] data, int keySize, string publicKeyXml)
        {
            if (data == null || data.Length == 0) throw new ArgumentException("Data are empty", "data");
            int maxLength = GetMaxDataLength(keySize);
            if (data.Length > maxLength) throw new ArgumentException(String.Format("Maximum data length is {0}", maxLength), "data");
            if (!IsKeySizeValid(keySize)) throw new ArgumentException("Key size is not valid", "keySize");
            if (String.IsNullOrEmpty(publicKeyXml)) throw new ArgumentException("Key is null or empty", "publicKeyXml");

            using (var provider = new RSACryptoServiceProvider(keySize))
            {
                provider.FromXmlString(publicKeyXml);
                return provider.Encrypt(data, _optimalAsymmetricEncryptionPadding);
            }
        }

        public static string Decrypt(string encryptedText)
        {
            try
            {
                int keySize = 0;
                string publicAndPrivateKeyXml = "";

                GetKeyFromEncryptionString(PrivateKey, out keySize, out publicAndPrivateKeyXml);

                var decrypted = Decrypt(Convert.FromBase64String(encryptedText), keySize, publicAndPrivateKeyXml);

                return Encoding.UTF8.GetString(decrypted);
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                return null;
            }

        }

        private static byte[] Decrypt(byte[] data, int keySize, string publicAndPrivateKeyXml)
        {
            if (data == null || data.Length == 0) throw new ArgumentException("Data are empty", "data");
            if (!IsKeySizeValid(keySize)) throw new ArgumentException("Key size is not valid", "keySize");
            if (String.IsNullOrEmpty(publicAndPrivateKeyXml)) throw new ArgumentException("Key is null or empty", "publicAndPrivateKeyXml");

            using (var provider = new RSACryptoServiceProvider(keySize))
            {
                provider.FromXmlString(publicAndPrivateKeyXml);
                return provider.Decrypt(data, _optimalAsymmetricEncryptionPadding);
            }
        }

        private static int GetMaxDataLength(int keySize)
        {
            if (_optimalAsymmetricEncryptionPadding)
            {
                return ((keySize - 384) / 8) + 7;
            }
            return ((keySize - 384) / 8) + 37;
        }

        private static bool IsKeySizeValid(int keySize)
        {
            return keySize >= 384 && keySize <= 16384 && keySize % 8 == 0;
        }

        private static void GetKeyFromEncryptionString(string rawkey, out int keySize, out string xmlKey)
        {
            keySize = 0;
            xmlKey = "";

            if (rawkey != null && rawkey.Length > 0)
            {
                byte[] keyBytes = Convert.FromBase64String(rawkey);
                var stringKey = Encoding.UTF8.GetString(keyBytes);

                if (stringKey.Contains("!"))
                {
                    var splittedValues = stringKey.Split(new char[] { '!' }, 2);

                    try
                    {
                        keySize = int.Parse(splittedValues[0]);
                        xmlKey = splittedValues[1];
                    }
                    catch (Exception ex) { }
                }
            }
        }
    }
}
