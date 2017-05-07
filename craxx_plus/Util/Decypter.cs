using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace craxx_plus.Util
{
    public static class Decypter
    {
        public static byte[] cryKey = new byte[] { 0x76, 0x61, 0x6D, 0x70, 0x73 };
        public static string Decrypt(string txt)
        {
            string ret;
            //文字列を暗号化されたデータに戻す
            byte[] encryptedData = System.Convert.FromBase64String(txt);

            //復号化する
            byte[] userData = System.Security.Cryptography.ProtectedData.Unprotect(
                encryptedData, cryKey,
                System.Security.Cryptography.DataProtectionScope.CurrentUser);

            //復号化されたデータを文字列に変換
           ret = System.Text.Encoding.UTF8.GetString(userData);

            return ret;
        }

        public static string Crypt(string txt)
        {
            string ret;

            //文字列をバイト型配列に変換
            byte[] userData = System.Text.Encoding.UTF8.GetBytes(txt);

            //暗号化する
            byte[] encryptedData = System.Security.Cryptography.ProtectedData.Protect(
                userData, cryKey,
                System.Security.Cryptography.DataProtectionScope.CurrentUser);

            //暗号化されたデータを文字列に変換
            ret = System.Convert.ToBase64String(encryptedData);

            return ret;
        }

    }
}
