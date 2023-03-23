using EasyHttp.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using YouduTest.EntApp.Exceptions;

namespace YouduTest.EntApp
{
    public class Helper
    {
        public static void CheckHttpStatus(HttpResponse rsp)
        {
            if (rsp.StatusCode != HttpStatusCode.OK)
            {
                throw new HttpRequestException((int)rsp.StatusCode, rsp.StatusDescription, null);
            }
        }

        public static void CheckApiStatus(Dictionary<string, object> body)
        {
            object errCode;
            object errmsg;
            if (!body.TryGetValue("errcode", out errCode) 
                || errCode is int == false 
                || !body.TryGetValue("errmsg", out errmsg) 
                || errmsg is string == false)
            {
                throw new ParamParserException("invalid errcode or errmsg", null);
            }
            if ((int)errCode != 0)
            {
                throw new HttpRequestException((int)errCode, (string)errmsg, null);
            }
        }

        public static string GetEncryptJsonValue(Dictionary<string, object> body)
        {
            object encrypt;
            if (!body.TryGetValue("encrypt", out encrypt) 
                || encrypt is string == false
                || ((string)encrypt).Length == 0)
            {
                throw new ParamParserException("encrypt content not exists", null);
            }
            return (string)encrypt;
        }

        public static byte[] ReadStream(Stream stream)
        {
            using (stream)
            {
                using (var mem = new MemoryStream())
                {
                    stream.CopyTo(mem);
                    return mem.ToArray();
                }
            }
        }

        public static void WriteStream(Stream stream, byte[] content)
        {
            using (stream)
            {
                stream.Write(content, 0, content.Length);
            }
        }

        //获取当前时间秒数
        public static long GetSecondTimeStamp()
        {
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); // 当地时区
            long duration = (long)(DateTime.Now - startTime).TotalMilliseconds/1000; // 相差毫秒秒数
            return duration;
        }

        public static bool HasKey(object obj, string key)
        {
            return ((IDictionary<string, object>)obj).ContainsKey(key);
        }
    }
}
