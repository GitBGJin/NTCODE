using System;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections;

namespace SmartEP.Utilities.Encryption
{
    /// <summary>
    /// MySecurity(安全类) 的摘要说明。
    /// </summary>
    public class MySecurity
    {
        /// <summary>
        /// 初始化安全类
        /// </summary>
        public MySecurity()
        {
            ///默认密码
            key = "0123456789";
        }
        private string key; //默认密钥

        private byte[] sKey;
        private byte[] sIV;

        #region 加密字符串
        /// <summary>
        /// 加密字符串
        /// </summary>
        /// <param name="inputStr">输入字符串</param>
        /// <param name="keyStr">密码，可以为“”</param>
        /// <returns>输出加密后字符串</returns>
        static public string SEncryptString(string inputStr, string keyStr)
        {
            MySecurity ws = new MySecurity();
            return ws.EncryptString(inputStr, keyStr);
        }
        /// <summary>
        /// 加密字符串
        /// </summary>
        /// <param name="inputStr">输入字符串</param>
        /// <param name="keyStr">密码，可以为“”</param>
        /// <returns>输出加密后字符串</returns>
        public string EncryptString(string inputStr, string keyStr)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            if (keyStr == "")
                keyStr = key;
            byte[] inputByteArray = Encoding.Default.GetBytes(inputStr);
            byte[] keyByteArray = Encoding.Default.GetBytes(keyStr);
            SHA1 ha = new SHA1Managed();
            byte[] hb = ha.ComputeHash(keyByteArray);
            sKey = new byte[8];
            sIV = new byte[8];
            for (int i = 0; i < 8; i++)
                sKey[i] = hb[i];
            for (int i = 8; i < 16; i++)
                sIV[i - 8] = hb[i];
            des.Key = sKey;
            des.IV = sIV;
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            StringBuilder ret = new StringBuilder();
            foreach (byte b in ms.ToArray())
            {
                ret.AppendFormat("{0:X2}", b);
            }
            cs.Close();
            ms.Close();
            return ret.ToString();
        }
        #endregion

        #region 加密字符串 密钥为系统默认 0123456789
        /// <summary>
        /// 加密字符串 密钥为系统默认
        /// </summary>
        /// <param name="inputStr">输入字符串</param>
        /// <returns>输出加密后字符串</returns>
        static public string SEncryptString(string inputStr)
        {
            MySecurity ws = new MySecurity();
            return ws.EncryptString(inputStr, "");
        }
        #endregion


        #region 加密文件
        /// <summary>
        /// 加密文件
        /// </summary>
        /// <param name="filePath">输入文件路径</param>
        /// <param name="savePath">加密后输出文件路径</param>
        /// <param name="keyStr">密码，可以为“”</param>
        /// <returns></returns>  
        public bool EncryptFile(string filePath, string savePath, string keyStr)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            if (keyStr == "")
                keyStr = key;
            FileStream fs = File.OpenRead(filePath);
            byte[] inputByteArray = new byte[fs.Length];
            fs.Read(inputByteArray, 0, (int)fs.Length);
            fs.Close();
            byte[] keyByteArray = Encoding.Default.GetBytes(keyStr);
            SHA1 ha = new SHA1Managed();
            byte[] hb = ha.ComputeHash(keyByteArray);
            sKey = new byte[8];
            sIV = new byte[8];
            for (int i = 0; i < 8; i++)
                sKey[i] = hb[i];
            for (int i = 8; i < 16; i++)
                sIV[i - 8] = hb[i];
            des.Key = sKey;
            des.IV = sIV;
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            fs = File.OpenWrite(savePath);
            foreach (byte b in ms.ToArray())
            {
                fs.WriteByte(b);
            }
            fs.Close();
            cs.Close();
            ms.Close();
            return true;
        }
        #endregion

        #region 解密字符串
        /// <summary>
        /// 解密字符串
        /// </summary>
        /// <param name="inputStr">要解密的字符串</param>
        /// <param name="keyStr">密钥</param>
        /// <returns>解密后的结果</returns>
        static public string SDecryptString(string inputStr, string keyStr)
        {
            MySecurity ws = new MySecurity();
            return ws.DecryptString(inputStr, keyStr);
        }
        /// <summary>
        ///  解密字符串 密钥为系统默认
        /// </summary>
        /// <param name="inputStr">要解密的字符串</param>
        /// <returns>解密后的结果</returns>
        static public string SDecryptString(string inputStr)
        {
            MySecurity ws = new MySecurity();
            return ws.DecryptString(inputStr, "");
        }
        /// <summary>
        /// 解密字符串
        /// </summary>
        /// <param name="inputStr">要解密的字符串</param>
        /// <param name="keyStr">密钥</param>
        /// <returns>解密后的结果</returns>
        public string DecryptString(string inputStr, string keyStr)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            if (keyStr == "")
                keyStr = key;
            byte[] inputByteArray = new byte[inputStr.Length / 2];
            for (int x = 0; x < inputStr.Length / 2; x++)
            {
                int i = (Convert.ToInt32(inputStr.Substring(x * 2, 2), 16));
                inputByteArray[x] = (byte)i;
            }
            byte[] keyByteArray = Encoding.Default.GetBytes(keyStr);
            SHA1 ha = new SHA1Managed();
            byte[] hb = ha.ComputeHash(keyByteArray);
            sKey = new byte[8];
            sIV = new byte[8];
            for (int i = 0; i < 8; i++)
                sKey[i] = hb[i];
            for (int i = 8; i < 16; i++)
                sIV[i - 8] = hb[i];
            des.Key = sKey;
            des.IV = sIV;
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            StringBuilder ret = new StringBuilder();
            return System.Text.Encoding.Default.GetString(ms.ToArray());
        }
        #endregion

        #region 解密文件
        /// <summary>
        /// 解密文件
        /// </summary>
        /// <param name="filePath">输入文件路径</param>
        /// <param name="savePath">解密后输出文件路径</param>
        /// <param name="keyStr">密码，可以为“”</param>
        /// <returns></returns>    
        public bool DecryptFile(string filePath, string savePath, string keyStr)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            if (keyStr == "")
                keyStr = key;
            FileStream fs = File.OpenRead(filePath);
            byte[] inputByteArray = new byte[fs.Length];
            fs.Read(inputByteArray, 0, (int)fs.Length);
            fs.Close();
            byte[] keyByteArray = Encoding.Default.GetBytes(keyStr);
            SHA1 ha = new SHA1Managed();
            byte[] hb = ha.ComputeHash(keyByteArray);
            sKey = new byte[8];
            sIV = new byte[8];
            for (int i = 0; i < 8; i++)
                sKey[i] = hb[i];
            for (int i = 8; i < 16; i++)
                sIV[i - 8] = hb[i];
            des.Key = sKey;
            des.IV = sIV;
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            fs = File.OpenWrite(savePath);
            foreach (byte b in ms.ToArray())
            {
                fs.WriteByte(b);
            }
            fs.Close();
            cs.Close();
            ms.Close();
            return true;
        }
        #endregion
    }
}
