using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Timesheet
{
    public class Crypto
    {
        public static string Hash(string password)
        {
            try
            {
                return Convert.ToBase64String(
                    System.Security.Cryptography.SHA256.Create()
                    .ComputeHash(Encoding.UTF8.GetBytes(password))
                    );
            } 
            catch (Exception e) 
            {
                throw new Exception(e.Message);
            }
        }
        //public static string Decode(string encryptedpassword)
        //{
        //    try
        //    {
        //        System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
        //        System.Text.Decoder Ut8Decode = encoder.GetDecoder();
        //        byte[] DecodeByte = Convert.FromBase64String(encryptedpassword);
        //        int CharCount = Ut8Decode.GetCharCount(DecodeByte, 0, DecodeByte.Length);
        //        char[] DecodeChar = new char[CharCount];
        //        Ut8Decode.GetChars(DecodeByte, 0, DecodeByte.Length, DecodeChar, 0);
        //        string DecryptedData = new string(DecodeChar);
        //        return DecryptedData;
        //    }
        //    catch (Exception e)
        //    {
        //        throw new Exception(e.Message);
        //    }
        //}
    }
}