using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace Ezz_Helper.Security.Cryptography
{
   public class Encrypt_N_Decrypt
    {

        public static byte[] CreateKey(string strP)
        {
            //Convert strPassword to an array and store in chrData.
            char[] chrData = strP.ToCharArray();
            //Use intLength to get strPassword size.
            int intLength = chrData.GetUpperBound(0);
            //Declare bytDataToHash and make it the same size as chrData.
            byte[] bytDataToHash = new byte[intLength + 1];

            //Use For Next to convert and store chrData into bytDataToHash.
            for (int i = 0; i <= intLength; i++)
            {
                bytDataToHash[i] = Convert.ToByte(chrData[i]);//(byte)Asc(chrData(i));
            }

            //Declare what hash to use.
            System.Security.Cryptography.SHA512Managed SHA512 = new System.Security.Cryptography.SHA512Managed();
            //Declare bytResult, Hash bytDataToHash and store it in bytResult.
            byte[] bytResult = SHA512.ComputeHash(bytDataToHash);
            //Declare bytKey(31).  It will hold 256 bits.
            byte[] bytKey = new byte[32];

            //Use For Next to put a specific size (256 bits) of
            //bytResult into bytKey. The 0 To 31 will put the first 256 bits
            //of 512 bits into bytKey.
            for (int i = 0; i <= 31; i++)
            {
                bytKey[i] = bytResult[i];
            }

            return bytKey;
            //Return the key.
        }
        public static byte[] CreateIV(string strP)
        {

            //Convert strP to an array and store in chrData.
            char[] chrData = strP.ToCharArray();
            //Use intLength to get strP size.
            int intLength = chrData.GetUpperBound(0);
            //Declare bytDataToHash and make it the same size as chrData.
            byte[] bytDataToHash = new byte[intLength + 1];

            //Use For Next to convert and store chrData into bytDataToHash.
            for (int i = 0; i <= intLength; i++)
            {
                bytDataToHash[i] = Convert.ToByte(chrData[i]);//byte)Asc(chrData[i]);
            }

            //Declare what hash to use.
            System.Security.Cryptography.SHA512Managed SHA512 = new System.Security.Cryptography.SHA512Managed();
            //Declare bytResult, Hash bytDataToHash and store it in bytResult.
            byte[] bytResult = SHA512.ComputeHash(bytDataToHash);
            //Declare bytIV(15).  It will hold 128 bits.
            byte[] bytIV = new byte[16];

            //Use For Next to put a specific size (128 bits) of
            //bytResult into bytIV. The 0 To 30 for bytKey used the first 256 bits.
            //of the hashed password. The 32 To 47 will put the next 128 bits into bytIV.
            for (int i = 32; i <= 47; i++)
            {
                bytIV[i - 32] = bytResult[i];
            }

            return bytIV;
            //return the IV
        }
        //
     
        #region Encrypt

        public static string EncryptString(string strValue, string strKey, string strIV)
        {
            return EncryptString(strValue, CreateKey(strKey), CreateIV(strIV));
        }
        public static string EncryptString(string strValue, byte[] Key, byte[] IV)
        {
            byte[] clearBytes = System.Text.Encoding.Unicode.GetBytes(strValue);
            byte[] encryptedData = Encrypt(clearBytes, Key, IV);
            return Convert.ToBase64String(encryptedData);
        }
        public static byte[] Encrypt(byte[] _Data, string strKey, string strIV)
        {
            return Encrypt(_Data, CreateKey(strKey), CreateIV(strIV));
        }
        public static byte[] Encrypt(byte[] _Data, byte[] Key, byte[] IV)
        {
            CryptoStream cs = null;
            byte[] res = null;
            try
            {
                MemoryStream ms = new MemoryStream();
                Rijndael alg = Rijndael.Create();
                cs = new CryptoStream(ms, alg.CreateEncryptor(Key, IV), CryptoStreamMode.Write);
                cs.Write(_Data, 0, _Data.Length);
                cs.FlushFinalBlock();
                res = ms.ToArray();
            }
            catch (Exception Ex)
            {
                res = _Data;
                throw Ex;
            }
            finally
            {
                try
                {
                    cs.Close();
                }
                catch { }
            }
            return res;
        }

        #endregion  //Encrypt

        #region Decrypt

        public static string DecryptSting(string _Text, string strKey, string strIV)
        {
            return DecryptSting(_Text, CreateKey(strKey), CreateIV(strIV));
        }
        public static string DecryptSting(string _Text, byte[] Key, byte[] IV)
        {
            try
            {
                byte[] cipherBytes = Convert.FromBase64String(_Text);
                byte[] decryptedData = Decrypt(cipherBytes, Key, IV);
                return System.Text.Encoding.Unicode.GetString(decryptedData);
            }
            catch
            {
                return _Text;
            }
        }
        public static byte[] Decrypt(byte[] _Data, string strKey, string strIV)
        {
            return Decrypt(_Data, CreateKey(strKey), CreateIV(strIV));
        }
        public static byte[] Decrypt(byte[] _Data, byte[] Key, byte[] IV)
        {
            CryptoStream cs = null;
            byte[] res = null;
            try
            {
                MemoryStream ms = new MemoryStream();
                Rijndael alg = Rijndael.Create();
                cs = new CryptoStream(ms, alg.CreateDecryptor(Key, IV), CryptoStreamMode.Write);
                cs.Write(_Data, 0, _Data.Length);
                cs.FlushFinalBlock();
                res = ms.ToArray();
            }
            catch
            {
                res = _Data;
            }
            finally
            {
                try
                {
                    cs.Close();
                }
                catch { }
            }
            return res;
        }

        #endregion  //Decrypt

        //File
        //File_Encrypt
        public static void File_Encrypt(string FilePath, string strKey, string strIV)
        {
            File_Encrypt(FilePath, CreateKey(strKey), CreateIV(strIV));
        }
        public static void File_Encrypt(string FilePath, byte[] Key, byte[] IV)
        {
            File.WriteAllBytes(FilePath, Encrypt(File.ReadAllBytes(FilePath), Key, IV));
        }

        //File_Decrypt
        public static void File_Decrypt(string FilePath, string strKey, string strIV)
        {
            File_Decrypt(FilePath, CreateKey(strKey), CreateIV(strIV));
        }
        public static void File_Decrypt(string FilePath, byte[] Key, byte[] IV)
        {
            File.WriteAllBytes(FilePath, Decrypt(File.ReadAllBytes(FilePath), Key, IV));
        }

        //
        public static string FileDecryptToString(string FilePath, byte[] Key, byte[] IV)
        {
            var DRes = Decrypt(File.ReadAllBytes(FilePath), Key, IV);
            System.Text.Encoding FileEnc = OtherH.DetectFileEncoding(FilePath);
            return FileEnc.GetString(DRes);
        }

    }
}
