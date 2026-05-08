using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace OPS.Obfuscator
{
    //[Obfuscator.Attribute.DoNotObfuscateClass]
    internal static class DecryptReferenceType
    {
        public static String DoNotObfuscateStringPrefix = "##__DoNot__##";

        private static Dictionary<String, String> decryptedDictionary = new Dictionary<string, string>();

        /*private static String Decrypt(String _Key, byte[] _Value)
        {
            byte[] var_Bytes = OPS.Rsa.Process.Decrypt(_Key, OPS.Rsa.ERsaStrength.L_1024, _Value);
            return Encoding.UTF8.GetString(var_Bytes);
        }*/

        [Obfuscator.Attribute.DoNotObfuscateMethodBody]
        public static String DecryptStringFromBytesReferenceMethod(String _EncryptedText, bool _StoreStrings)
        {
            String var_PublicKey = "";

            String var_Value = null;

            if (!decryptedDictionary.TryGetValue(_EncryptedText, out var_Value))
            {
                //Convert base64 string to byte arrray.
                byte[] var_EncryptedBytes = System.Convert.FromBase64String(_EncryptedText);

                //Decrypt bytes to string.
                //string var_DecryptedText = Decrypt(var_PublicKey, var_EncryptedBytes);
                byte[] var_Bytes = OPS.Rsa.Process.Decrypt(var_PublicKey, OPS.Rsa.ERsaStrength.L_1024, var_EncryptedBytes);
                string var_DecryptedText = Encoding.UTF8.GetString(var_Bytes);

                if (_StoreStrings)
                {
                    decryptedDictionary.Add(_EncryptedText, var_DecryptedText);
                }

                var_Value = var_DecryptedText;
            }

            return var_Value;
        }
    }
}
