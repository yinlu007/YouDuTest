using System;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Net;

using YouduTest.EntApp.Exceptions;

namespace YouduTest.EntApp.AES
{
    public class AESCrypto
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="appId">企业应用AppId</param>
        /// <param name="encodingAESKey">企业应用AppKey</param>
        public AESCrypto(string appId, string encodingAESKey)
        {
            if (appId.Length == 0 || encodingAESKey.Length == 0)
            {
                throw new ArgumentException();
            }
            m_appId = appId;
            m_AESKey = encodingAESKey;
        }


        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="input">输入明文</param>
        /// <returns>输出密文</returns>
        /// <exception cref="AESCryptoException">加密失败抛出异常</exception>
        public string Encrypt(byte[] input)
        {
            return AES_encrypt(input, m_AESKey, m_appId);
        }


        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="input">输入密文</param>
        /// <returns>输出明文</returns>
        /// <exception cref="AESCryptoException">解密失败抛出异常</exception>
        public byte[] Decrypt(string input)
        {
            return AES_decrypt(input, m_AESKey, m_appId);
        }

        public static byte[] ToBytes(string input)
        {
            return Encoding.UTF8.GetBytes(input);
        }

        public static string ToString(byte[] input)
        {
            return Encoding.UTF8.GetString(input);
        }

        private string m_appId;
        private string m_AESKey;

        private static UInt32 HostToNetworkOrder(UInt32 inval)
        {
            UInt32 outval = 0;
            for (int i = 0; i < 4; i++)
                outval = (outval << 8) + ((inval >> (i * 8)) & 255);
            return outval;
        }

        private static Int32 HostToNetworkOrder(Int32 inval)
        {
            Int32 outval = 0;
            for (int i = 0; i < 4; i++)
                outval = (outval << 8) + ((inval >> (i * 8)) & 255);
            return outval;
        }

        private static byte[] AES_decrypt(String input, string encodingAESKey, string appId)
        {
            try
            {
                byte[] Key = Convert.FromBase64String(encodingAESKey);
                byte[] Iv = new byte[16];
                Array.Copy(Key, Iv, 16);
                byte[] btmpMsg = AES_decrypt(input, Iv, Key);

                int len = BitConverter.ToInt32(btmpMsg, 16);
                len = IPAddress.NetworkToHostOrder(len);

                byte[] bMsg = new byte[len];
                byte[] bAppId = new byte[btmpMsg.Length - 20 - len];
                Array.Copy(btmpMsg, 20, bMsg, 0, len);
                Array.Copy(btmpMsg, 20 + len, bAppId, 0, btmpMsg.Length - 20 - len);
                string decryptAppId = Encoding.UTF8.GetString(bAppId);
                if (!decryptAppId.Equals(appId))
                {
                    throw new AESCryptoException("appId not match", null);
                }
                return bMsg;
            }
            catch (System.Exception e)
            {
                throw new AESCryptoException(e.Message, e);
            }
        }

        private static String AES_encrypt(byte[] input, string encodingAESKey, string appId)
        {
            try
            {
                byte[] Key = Convert.FromBase64String(encodingAESKey);
                byte[] Iv = new byte[16];
                Array.Copy(Key, Iv, 16);
                string Randcode = CreateRandCode(16);
                byte[] bRand = Encoding.UTF8.GetBytes(Randcode);
                byte[] bAppId = Encoding.UTF8.GetBytes(appId);
                byte[] bMsgLen = BitConverter.GetBytes(HostToNetworkOrder(input.Length));
                byte[] bMsg = new byte[bRand.Length + bMsgLen.Length + bAppId.Length + input.Length];

                Array.Copy(bRand, bMsg, bRand.Length);
                Array.Copy(bMsgLen, 0, bMsg, bRand.Length, bMsgLen.Length);
                Array.Copy(input, 0, bMsg, bRand.Length + bMsgLen.Length, input.Length);
                Array.Copy(bAppId, 0, bMsg, bRand.Length + bMsgLen.Length + input.Length, bAppId.Length);

                return AES_encrypt(bMsg, Iv, Key);
            }
            catch (System.Exception e)
            {
                throw new AESCryptoException(e.Message, e);
            }
        }

        private static string CreateRandCode(int codeLen)
        {
            string codeSerial = "2,3,4,5,6,7,a,c,d,e,f,h,i,j,k,m,n,p,r,s,t,A,C,D,E,F,G,H,J,K,M,N,P,Q,R,S,U,V,W,X,Y,Z";
            if (codeLen == 0)
            {
                codeLen = 16;
            }
            string[] arr = codeSerial.Split(',');
            string code = "";
            int randValue = -1;
            Random rand = new Random(unchecked((int)DateTime.Now.Ticks));
            for (int i = 0; i < codeLen; i++)
            {
                randValue = rand.Next(0, arr.Length - 1);
                code += arr[randValue];
            }
            return code;
        }

        private static String AES_encrypt(byte[] Input, byte[] Iv, byte[] Key)
        {
            var aes = new RijndaelManaged();
            aes.KeySize = Key.Length * 8;
            aes.BlockSize = 128;
            aes.Padding = PaddingMode.None;
            aes.Mode = CipherMode.CBC;
            aes.Key = Key;
            aes.IV = Iv;
            var encrypt = aes.CreateEncryptor(aes.Key, aes.IV);
            byte[] xBuff = null;

            byte[] msg = new byte[Input.Length + 32 - Input.Length % 32];
            Array.Copy(Input, msg, Input.Length);
            byte[] pad = KCS7Encoder(Input.Length);
            Array.Copy(pad, 0, msg, Input.Length, pad.Length);

            using (var ms = new MemoryStream())
            {
                using (var cs = new CryptoStream(ms, encrypt, CryptoStreamMode.Write))
                {
                    cs.Write(msg, 0, msg.Length);
                }
                xBuff = ms.ToArray();
            }

            String Output = Convert.ToBase64String(xBuff);
            return Output;
        }

        private static byte[] KCS7Encoder(int text_length)
        {
            int block_size = 32;
            int amount_to_pad = block_size - (text_length % block_size);
            if (amount_to_pad == 0)
            {
                amount_to_pad = block_size;
            }
            char pad_chr = chr(amount_to_pad);
            string tmp = "";
            for (int index = 0; index < amount_to_pad; index++)
            {
                tmp += pad_chr;
            }
            return Encoding.UTF8.GetBytes(tmp);
        }

        static char chr(int a)
        {

            byte target = (byte)(a & 0xFF);
            return (char)target;
        }

        private static byte[] AES_decrypt(String Input, byte[] Iv, byte[] Key)
        {
            RijndaelManaged aes = new RijndaelManaged();
            aes.KeySize = Key.Length * 8;
            aes.BlockSize = 128;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.None;
            aes.Key = Key;
            aes.IV = Iv;
            var decrypt = aes.CreateDecryptor(aes.Key, aes.IV);
            byte[] xBuff = null;
            using (var ms = new MemoryStream())
            {
                using (var cs = new CryptoStream(ms, decrypt, CryptoStreamMode.Write))
                {
                    byte[] rawInput = Convert.FromBase64String(Input);
                    byte[] msg = new byte[rawInput.Length + 32 - rawInput.Length % 32];
                    Array.Copy(rawInput, msg, rawInput.Length);
                    cs.Write(rawInput, 0, rawInput.Length);
                }
                xBuff = decode(ms.ToArray());
            }
            return xBuff;
        }

        private static byte[] decode(byte[] decrypted)
        {
            int pad = (int)decrypted[decrypted.Length - 1];
            if (pad < 1 || pad > 32)
            {
                pad = 0;
            }
            byte[] res = new byte[decrypted.Length - pad];
            Array.Copy(decrypted, 0, res, 0, decrypted.Length - pad);
            return res;
        }
    }
}
