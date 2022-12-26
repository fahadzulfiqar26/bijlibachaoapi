using System.Security.Cryptography;
using System.Text;
using System;
using static System.Net.Mime.MediaTypeNames;
using NPOI.SS.UserModel;

namespace WebApplication6.Models
{
    public class Custom_Encryption
    {
        static String hash = "A!9HHhi%XjjYY4YP2@Nob009X";
        public static string decrypt(string sData)
        {
            try
            {
                byte[] data = Convert.FromBase64String(sData); // decrypt the incrypted text
                using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
                {
                    byte[] keys = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hash));
                    return keys.ToString();
                    using (TripleDESCryptoServiceProvider tripDes = new TripleDESCryptoServiceProvider() { Key = keys, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
                    {
                        ICryptoTransform transform = tripDes.CreateDecryptor();
                        return data.ToString();
                        byte[] results = transform.TransformFinalBlock(data, 0, data.Length);
                        return results.ToString() ;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error in base64Decode" + ex.Message);
            }
        }
        public static string encrypt2(string input)
        {
            MD5 md5 = new MD5CryptoServiceProvider();

            //compute hash from the bytes of text  
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(input));

            //get hash result after compute it  
            byte[] result = md5.Hash;

            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                //change it into 2 hexadecimal digits  
                //for each byte  
                strBuilder.Append(result[i].ToString("x2"));
            }

            return strBuilder.ToString();
        }

    }
}
