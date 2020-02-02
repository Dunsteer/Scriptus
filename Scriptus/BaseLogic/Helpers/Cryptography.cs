using System;
using System.Collections.Generic;
using System.Text;

namespace BaseLogic.Helpers
{
    public class Cryptography
    {
        public static string GetSHA256Hash(string strToHash)
        {
            System.Security.Cryptography.SHA256Managed sha1Obj = new System.Security.Cryptography.SHA256Managed();
            byte[] bytesToHash = System.Text.Encoding.ASCII.GetBytes(strToHash);
            string strResult = "";

            bytesToHash = sha1Obj.ComputeHash(bytesToHash);

            foreach (byte b in bytesToHash)
            {
                strResult += b.ToString("x2");
            }

            return strResult;
        }
    }
}
