using System;
using System.Text;
using System.Collections;
using System.Security.Cryptography;

using YouduTest.EntApp.Exceptions;

namespace YouduTest.EntApp.AES
{
    public class Signature
    {
        /// <summary>
        /// 生成回调校验签名
        /// </summary>
        /// <param name="token">回调Token</param>
        /// <param name="timestamp">时间戳，从回调URL参数里取</param>
        /// <param name="nonce">回调随机字符串，从回调URL参数取</param>
        /// <param name="encrypt">加密内容，从回调的json数据中取</param>
        /// <returns>返回签名</returns>
        /// <exception cref="SignatureException">如果出现错误则抛出异常</exception>
        public static string GenerateSignature(string token, string timestamp, string nonce, string encrypt)
        {
            ArrayList AL = new ArrayList();
            AL.Add(token);
            AL.Add(timestamp);
            AL.Add(nonce);
            AL.Add(encrypt);
            AL.Sort(new DictionarySort());
            string raw = "";
            for (int i = 0; i < AL.Count; ++i)
            {
                raw += AL[i];
            }

            SHA1 sha;
            ASCIIEncoding enc;
            string hash = "";
            try
            {
                sha = new SHA1CryptoServiceProvider();
                enc = new ASCIIEncoding();
                byte[] dataToHash = enc.GetBytes(raw);
                byte[] dataHashed = sha.ComputeHash(dataToHash);
                hash = BitConverter.ToString(dataHashed).Replace("-", "");
                hash = hash.ToLower();
            }
            catch (System.Exception e)
            {
                throw new SignatureException(e.Message, e);
            }
            return hash;
        }

        private class DictionarySort : System.Collections.IComparer
        {
            public int Compare(object oLeft, object oRight)
            {
                string sLeft = oLeft as string;
                string sRight = oRight as string;
                int iLeftLength = sLeft.Length;
                int iRightLength = sRight.Length;
                int index = 0;
                while (index < iLeftLength && index < iRightLength)
                {
                    if (sLeft[index] < sRight[index])
                        return -1;
                    else if (sLeft[index] > sRight[index])
                        return 1;
                    else
                        index++;
                }
                return iLeftLength - iRightLength;

            }
        }
    }
}
