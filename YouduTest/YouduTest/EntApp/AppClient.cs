using System;
using System.Collections.Generic;

using YouduTest.EntApp.AES;
using EasyHttp.Http;
using System.Net;
using YouduTest.EntApp.Exceptions;
using JsonFx.Json;
using YouduTest.EntApp.MessageEntity;
using System.IO;
using EasyHttp.Infrastructure;
using YouduTest.EntApp.SessionEntity;

namespace YouduTest.EntApp
{
    public class AppClient
    {
        /// <summary>
        /// 文件类型
        /// </summary>
        public const string FileTypeFile = "file";

        /// <summary>
        /// 图片类型
        /// </summary>
        public const string FileTypeImage = "image";

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="address">目标服务器地址，"ip:port"的形式</param>
        /// <param name="buin">企业号</param>
        /// <param name="appId">AppId</param>
        /// <param name="encodingAesKey">encodingAesKey</param>
        public AppClient(string address, int buin, string appId, string encodingAesKey)
        {
            if (address.Length == 0 || buin == 0 || appId.Length == 0 || encodingAesKey.Length == 0)
            {
                throw new ArgumentException();
            }
            m_addr = address;
            m_buin = buin;
            m_appId = appId;
            m_crypto = new AESCrypto(appId, encodingAesKey);
        }

        private class Token
        {
            public string token;
            public long activeTime;
            public int expire;

            public Token(string token, long activeTime, int expire)
            {
                this.token = token;
                this.activeTime = activeTime;
                this.expire = expire;
            }
        }

        private AESCrypto m_crypto;
        private Token m_tokenInfo;

        private string m_addr;
        private int m_buin;
        private string m_appId;

        public string Addr
        {
            get
            {
                return m_addr;
            }
        }

        public int Buin
        {
            get
            {
                return m_buin;
            }
        }

        public string AppId
        {
            get
            {
                return m_appId;
            }
        }

        private object tokenQuery()
        {
            return new { accessToken = m_tokenInfo.token };
        }

        private string apiGetToken()
        {
            return EntAppApi.SCHEME + m_addr + EntAppApi.API_GET_TOKEN;
        }

        private string apiSendMsg()
        {
            return EntAppApi.SCHEME + m_addr + EntAppApi.API_SEND_MSG;
        }

        private string apiUploadFile()
        {
            return EntAppApi.SCHEME + m_addr + EntAppApi.API_UPLOAD_FILE;
        }

        private string apiSendIMMsg()
        {
            return EntAppApi.SCHEME + m_addr + EntAppApi.API_SEND_IMMSG;
        }

        private string apiDownloadFile()
        {
            return EntAppApi.SCHEME + m_addr + EntAppApi.API_DOWNLOAD_FILE;
        }

        private string apiSearchFile()
        {
            return EntAppApi.SCHEME + m_addr + EntAppApi.API_SEARCH_FILE;
        }

        private string apiGetSession()
        {
            return EntAppApi.SCHEME + m_addr + EntAppApi.API_GET_SESSION;
        }

        private string apiSetNotice()
        {
            return EntAppApi.SCHEME + m_addr + EntAppApi.API_SET_NOTICE;
        }

        private string apiPopWindow()
        {
            return EntAppApi.SCHEME + m_addr + EntAppApi.API_POPWINDOW;
        }

        private Token getToken()
        {
            try
            {
                var now = Helper.GetSecondTimeStamp();
                var timestamp = AESCrypto.ToBytes(string.Format("{0}", now));
                var encryptTime = m_crypto.Encrypt(timestamp);
                var param = new Dictionary<string, object>()
                {
                    { "buin",  m_buin },
                    { "appId", m_appId },
                    { "encrypt" , encryptTime }
                };
                var client = new HttpClient();
                var rsp = client.Post(this.apiGetToken(), param, HttpContentTypes.ApplicationJson);
                Helper.CheckHttpStatus(rsp);
                var body = rsp.StaticBody<Dictionary<string, object>>(overrideContentType: HttpContentTypes.ApplicationJson);
                Helper.CheckApiStatus(body);
                var encrypt = Helper.GetEncryptJsonValue(body);
                var buffer = m_crypto.Decrypt(encrypt);
                var tokenInfo = new JsonReader().Read<Dictionary<string, object>>(AESCrypto.ToString(buffer));
                object accessToken;
                object expireIn;
                if (!tokenInfo.TryGetValue("accessToken", out accessToken)
                    || accessToken is string == false
                    || ((string)accessToken).Length == 0
                    || !tokenInfo.TryGetValue("expireIn", out expireIn)
                    || expireIn is int == false
                    || (int)expireIn <= 0)
                {
                    throw new ParamParserException("invalid token or expireIn", null);
                }
                return new Token((string)accessToken, now, (int)expireIn);
            }
            catch (WebException e)
            {
                throw new HttpRequestException(0, e.Message, e);
            }
            catch (Exception e)
            {
                if (e is GeneralEntAppException)
                {
                    throw e;
                }
                else
                {
                    throw new UnexpectedException(e.Message, e);
                }
            }
        }

        private void checkAndRefreshToken()
        {
            if (m_tokenInfo == null) {
                m_tokenInfo = this.getToken();
            }
            long endTime = m_tokenInfo.activeTime + m_tokenInfo.expire;
            if (endTime <= Helper.GetSecondTimeStamp()) {
                m_tokenInfo = this.getToken();
            }
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="msg">Message对象</param>
        /// <exception cref="AESCryptoException">加解密失败</exception>
        /// <exception cref="ParamParserException">参数解析失败</exception>
        /// <exception cref="HttpRequestException">http请求失败</exception>
        /// <exception cref="UnexpectedException">其它可能的错误</exception>
        public void SendMsg(Message msg)
        {
            this.checkAndRefreshToken();

            try
            {
                Console.WriteLine(msg.ToJson());
                var cipherText = m_crypto.Encrypt(AESCrypto.ToBytes(msg.ToJson()));
                var param = new Dictionary<string, object>()
                {
                    { "buin", m_buin },
                    { "appId", m_appId },
                    { "encrypt", cipherText }
                };
                var client = new HttpClient();
                var rsp = client.Post(this.apiSendMsg(), param, HttpContentTypes.ApplicationJson, query: this.tokenQuery());
                Helper.CheckHttpStatus(rsp);
                var body = rsp.StaticBody<Dictionary<string, object>>(overrideContentType: HttpContentTypes.ApplicationJson);
                Helper.CheckApiStatus(body);
            }
            catch (WebException e)
            {
                throw new HttpRequestException(0, e.Message, e);
            }
            catch (Exception e)
            {
                if (e is GeneralEntAppException)
                {
                    throw e;
                }
                else
                {
                    throw new UnexpectedException(e.Message, e);
                }
            }
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public string Encrypt(string text)
        {
            var cipherText = m_crypto.Encrypt(AESCrypto.ToBytes(text));
            return cipherText;
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public string Decrypt(string text)
        {
            var buffer = m_crypto.Decrypt(text);
            return AESCrypto.ToString(buffer);
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="type">文件类型</param>
        /// <param name="name">文件名称</param>
        /// <param name="path">文件路径</param>
        /// <returns>文件的资源ID</returns>
        /// <exception cref="AESCryptoException">加解密失败</exception>
        /// <exception cref="ParamParserException">参数解析失败</exception>
        /// <exception cref="HttpRequestException">http请求失败</exception>
        /// <exception cref="UnexpectedException">其它可能的错误</exception>
        public string UploadFile(string type, string name, string path)
        {
            this.checkAndRefreshToken();

            try
            {
                var encryptFile = m_crypto.Encrypt(Helper.ReadStream(new FileStream(path, FileMode.Open, FileAccess.Read)));
                var fileInfo = new { type = type, name = name };
                var cipherFileInfo = m_crypto.Encrypt(AESCrypto.ToBytes(new JsonWriter().Write(fileInfo)));

                var tempFileName = Path.GetTempFileName();
                Helper.WriteStream(new FileStream(tempFileName, FileMode.Open, FileAccess.Write), AESCrypto.ToBytes(encryptFile));

                var formData = new Dictionary<string, object>();
                formData["buin"] = String.Format("{0}", m_buin);
                formData["appId"] = m_appId;
                formData["encrypt"] = cipherFileInfo;

                var fileData = new FileData();
                fileData.ContentType = HttpContentTypes.TextPlain;
                fileData.FieldName = "file";
                fileData.Filename = tempFileName;
                var fileList = new List<FileData>();
                fileList.Add(fileData);

                var client = new HttpClient();
                var rsp = client.Post(this.apiUploadFile(), formData, fileList, query: this.tokenQuery());
                File.Delete(tempFileName);

                Helper.CheckHttpStatus(rsp);
                var body = rsp.StaticBody<Dictionary<string, object>>(overrideContentType: HttpContentTypes.ApplicationJson);
                Helper.CheckApiStatus(body);

                var decryptBuffer = m_crypto.Decrypt(Helper.GetEncryptJsonValue(body));
                var mediaInfo = new JsonReader().Read<Dictionary<string, string>>(AESCrypto.ToString(decryptBuffer));
                string mediaId;
                if (!mediaInfo.TryGetValue("mediaId", out mediaId)
                    || mediaId == null
                    || mediaId.Length == 0)
                {
                    throw new ParamParserException("invalid mediaId", null);
                }
                return mediaId;
            }
            catch(IOException e)
            {
                throw new HttpRequestException(0, e.Message, e);
            }
            catch (WebException e)
            {
                throw new HttpRequestException(0, e.Message, e);
            }
            catch (Exception e)
            {
                if (e is GeneralEntAppException)
                {
                    throw e;
                }
                else
                {
                    throw new UnexpectedException(e.Message, e);
                }
            }
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="mediaId">文件的资源ID</param>
        /// <param name="destDir">目标存储目录</param>
        /// <returns>(文件名, 文件字节数大小, 文件内容)</returns>
        /// <exception cref="AESCryptoException">加解密失败</exception>
        /// <exception cref="ParamParserException">参数解析失败</exception>
        /// <exception cref="HttpRequestException">http请求失败</exception>
        /// <exception cref="UnexpectedException">其它可能的错误</exception>
        public Tuple<String, long, byte[]> DownloadFile(string mediaId, string destDir)
        {
            this.checkAndRefreshToken();

            try
            {
                var mediaInfo = new Dictionary<string, string>()
                {
                    { "mediaId", mediaId }
                };
                var cipherMediaInfo = m_crypto.Encrypt(AESCrypto.ToBytes(new JsonWriter().Write(mediaInfo)));
                var param = new Dictionary<string, object>()
                {
                    { "buin", m_buin },
                    { "appId", m_appId },
                    { "encrypt", cipherMediaInfo }
                };
                var client = new HttpClient();
                client.StreamResponse = true;
                var rsp = client.Post(this.apiDownloadFile(), param, HttpContentTypes.ApplicationJson, this.tokenQuery());

                Helper.CheckHttpStatus(rsp);

                var encryptFileInfo = rsp.RawHeaders["encrypt"];
                if (encryptFileInfo == null)
                {
                    var body = new JsonReader().Read<Dictionary<string, object>>(AESCrypto.ToString(Helper.ReadStream(rsp.ResponseStream)));
                    Helper.CheckApiStatus(body);
                    throw new ParamParserException("encrypt content not exists", null);
                }

                var decryptFileInfo = m_crypto.Decrypt(encryptFileInfo);
                var fileInfo = new JsonReader().Read<Dictionary<string, object>>(AESCrypto.ToString(decryptFileInfo));
                object name;
                object size;
                if (!fileInfo.TryGetValue("name", out name)
                    || name is string == false
                    || ((string)name).Length == 0
                    || !fileInfo.TryGetValue("size", out size)
                    || ((size is int && (int)size > 0) == false && (size is long && (long)size > 0) == false))
                {
                    throw new ParamParserException("invalid file name or size", null);
                }

                var encryptBuffer = Helper.ReadStream(rsp.ResponseStream);
                var decryptFile = m_crypto.Decrypt(AESCrypto.ToString(encryptBuffer));
                Helper.WriteStream(new FileStream(Path.Combine(destDir, (string)name), FileMode.OpenOrCreate, FileAccess.Write), decryptFile);
                return new Tuple<string, long, byte[]>((string)name, Convert.ToInt64(size), decryptFile);
            }
            catch (IOException e)
            {
                throw new HttpRequestException(0, e.Message, e);
            }
            catch (WebException e)
            {
                throw new HttpRequestException(0, e.Message, e);
            }
            catch (Exception e)
            {
                if (e is GeneralEntAppException)
                {
                    throw e;
                }
                else
                {
                    throw new UnexpectedException(e.Message, e);
                }
            }
        }

        /// <summary>
        /// 搜索文件信息
        /// </summary>
        /// <param name="mediaId">资源Id</param>
        /// <returns>(文件名, 字节数大小)</returns>
        /// <exception cref="AESCryptoException">加解密失败</exception>
        /// <exception cref="ParamParserException">参数解析失败</exception>
        /// <exception cref="HttpRequestException">http请求失败</exception>
        /// <exception cref="UnexpectedException">其它可能的错误</exception>
        public Tuple<string, long> SearchFile(string mediaId)
        {
            this.checkAndRefreshToken();

            try
            {
                var mediaInfo = new Dictionary<string, string>()
                {
                    { "mediaId", mediaId }
                };
                var cipherMediaInfo = m_crypto.Encrypt(AESCrypto.ToBytes(new JsonWriter().Write(mediaInfo)));
                var param = new Dictionary<string, object>()
                {
                    { "buin", m_buin },
                    { "appId", m_appId },
                    { "encrypt", cipherMediaInfo }
                };
                var client = new HttpClient();
                var rsp = client.Post(this.apiSearchFile(), param, HttpContentTypes.ApplicationJson, this.tokenQuery());

                Helper.CheckHttpStatus(rsp);
                var body = rsp.StaticBody<Dictionary<string, object>>(overrideContentType: HttpContentTypes.ApplicationJson);
                Helper.CheckApiStatus(body);

                var decryptBuffer = m_crypto.Decrypt(Helper.GetEncryptJsonValue(body));
                var existsInfo = new JsonReader().Read<Dictionary<string, object>>(AESCrypto.ToString(decryptBuffer));
                object name;
                object size;
                if (!existsInfo.TryGetValue("name", out name)
                    || name is string == false
                    || ((string)name).Length == 0
                    || !existsInfo.TryGetValue("size", out size)
                    || ((size is int && (int)size > 0) == false && (size is long && (long)size > 0) == false))
                {
                    throw new ParamParserException("invalid file info", null);
                }
                return new Tuple<string, long>((string)name, Convert.ToInt64(size));
            }
            catch (WebException e)
            {
                throw new HttpRequestException(0, e.Message, e);
            }
            catch (Exception e)
            {
                if (e is GeneralEntAppException)
                {
                    throw e;
                }
                else
                {
                    throw new UnexpectedException(e.Message, e);
                }
            }
        }

        /// <summary>
        /// 查询会话信息
        /// </summary>
        /// <param name="sessionId">会话Id</param>
        /// <returns>Session</returns>
        /// <exception cref="AESCryptoException">加解密失败</exception>
        /// <exception cref="ParamParserException">参数解析失败</exception>
        /// <exception cref="HttpRequestException">http请求失败</exception>
        /// <exception cref="UnexpectedException">其它可能的错误</exception>
        public Session GetSession(string sessionId) 
        {
            this.checkAndRefreshToken();

            try
            {
                var client = new HttpClient();
                var rsp = client.Get(this.apiGetSession(), new { accessToken = m_tokenInfo.token, sessionId = sessionId });

                Helper.CheckHttpStatus(rsp);
                var body = rsp.StaticBody<Dictionary<string, object>>(overrideContentType: HttpContentTypes.ApplicationJson);
                Helper.CheckApiStatus(body);

                var decryptBuffer = m_crypto.Decrypt(Helper.GetEncryptJsonValue(body));
                var decryptStr = AESCrypto.ToString(decryptBuffer);
                var sessInfo = new JsonReader().Read<Dictionary<string, object>>(AESCrypto.ToString(decryptBuffer));

                object title;
                object owner;
                object version;
                object type;
                object member;
                object lastMsgId;
                object activeTime;
                if (!sessInfo.TryGetValue("title", out title) || title is string == false
                    || !sessInfo.TryGetValue("owner", out owner) || owner is string == false
                    || !sessInfo.TryGetValue("version", out version) || (version is int == false && version is long == false)
                    || !sessInfo.TryGetValue("type", out type) || type is string == false
                    || !sessInfo.TryGetValue("member", out member) || member is string[] == false
                    || !sessInfo.TryGetValue("lastMsgId", out lastMsgId) || (lastMsgId is int == false && lastMsgId is long == false)
                    || !sessInfo.TryGetValue("activeTime", out activeTime) || (activeTime is int == false && activeTime is long == false))
                {
                    throw new ParamParserException("invalid session info", null);
                }
                var memberList = new List<string>();
                foreach(var m in (string[])member)
                {
                    memberList.Add(m);
                }
                return new Session(sessionId, 
                    (string)title, 
                    (string)owner, 
                    Convert.ToInt64(version), 
                    (string)type, 
                    memberList, 
                    Convert.ToInt64(lastMsgId),
                    Convert.ToInt64(activeTime));
            }
            catch (WebException e)
            {
                throw new HttpRequestException(0, e.Message, e);
            }
            catch (Exception e)
            {
                if (e is GeneralEntAppException)
                {
                    throw e;
                }
                else
                {
                    throw new UnexpectedException(e.Message, e);
                }
            }
        }

        public void SetAppNotice(string account, int count, string tip)
        {
            try
            {
                AppNotice appNotice = new AppNotice(account, count, tip);
                Console.WriteLine(appNotice.ToJsonString());
                var cipherText = m_crypto.Encrypt(AESCrypto.ToBytes(appNotice.ToJsonString()));
                var param = new Dictionary<string, object>()
                {
                    { "app_id", m_appId },
                    { "msg_encrypt", cipherText }
                };
                var client = new HttpClient();
                var rsp = client.Post(this.apiSetNotice(), param, HttpContentTypes.ApplicationJson);
                Helper.CheckHttpStatus(rsp);
                var body = rsp.StaticBody<Dictionary<string, object>>(overrideContentType: HttpContentTypes.ApplicationJson);
                Helper.CheckApiStatus(body);
            }
            catch (WebException e)
            {
                throw new HttpRequestException(0, e.Message, e);
            }
            catch (Exception e)
            {
                if (e is GeneralEntAppException)
                {
                    throw e;
                }
                else
                {
                    throw new UnexpectedException(e.Message, e);
                }
            }
        }

        public void PopWindow(PopWindow wd)
        {
            try
            {
                Console.WriteLine(wd.ToYDSdkJsonString());
                var cipherText = m_crypto.Encrypt(AESCrypto.ToBytes(wd.ToYDSdkJsonString()));
                var param = new Dictionary<string, object>()
                {
                    { "app_id", m_appId },
                    { "msg_encrypt", cipherText }
                };
                var client = new HttpClient();
                var rsp = client.Post(this.apiPopWindow(), param, HttpContentTypes.ApplicationJson);
                Helper.CheckHttpStatus(rsp);
                var body = rsp.StaticBody<Dictionary<string, object>>(overrideContentType: HttpContentTypes.ApplicationJson);
                Helper.CheckApiStatus(body);
            }
            catch (WebException e)
            {
                throw new HttpRequestException(0, e.Message, e);
            }
            catch (Exception e)
            {
                if (e is GeneralEntAppException)
                {
                    throw e;
                }
                else
                {
                    throw new UnexpectedException(e.Message, e);
                }
            }
        }

        /// <summary>
        /// 发送会话消息
        /// </summary>
        /// <param name="msg">Message对象</param>
        /// <exception cref="AESCryptoException">加解密失败</exception>
        /// <exception cref="ParamParserException">参数解析失败</exception>
        /// <exception cref="HttpRequestException">http请求失败</exception>
        /// <exception cref="UnexpectedException">其它可能的错误</exception>
        public void SendIMMsg(IMMessage msg)
        {
            this.checkAndRefreshToken();

            try
            {
                Console.WriteLine(msg.ToJson());
                var cipherText = m_crypto.Encrypt(AESCrypto.ToBytes(msg.ToJson()));
                var param = new Dictionary<string, object>()
                {
                    { "buin", m_buin },
                    { "appId", m_appId },
                    { "encrypt", cipherText }
                };
                var client = new HttpClient();
                var rsp = client.Post(this.apiSendIMMsg(), param, HttpContentTypes.ApplicationJson, query: this.tokenQuery());
                Helper.CheckHttpStatus(rsp);
                var body = rsp.StaticBody<Dictionary<string, object>>(overrideContentType: HttpContentTypes.ApplicationJson);
                Helper.CheckApiStatus(body);
            }
            catch (WebException e)
            {
                throw new HttpRequestException(0, e.Message, e);
            }
            catch (Exception e)
            {
                if (e is GeneralEntAppException)
                {
                    throw e;
                }
                else
                {
                    throw new UnexpectedException(e.Message, e);
                }
            }
        }

        /// <summary>
        /// 发送多人会话消息
        /// </summary>
        /// <param name="msg">Message对象</param>
        /// <exception cref="AESCryptoException">加解密失败</exception>
        /// <exception cref="ParamParserException">参数解析失败</exception>
        /// <exception cref="HttpRequestException">http请求失败</exception>
        /// <exception cref="UnexpectedException">其它可能的错误</exception>
        public string apiCreateSession(EntApp.MessageEntity.SessionMessage msg)
        {

            this.checkAndRefreshToken();

            string sessionid = string.Empty;
            var url = EntAppApi.SCHEME + m_addr + EntAppApi.API_GET_CREATESESSION;
            Console.WriteLine(msg.ToJson());
            var cipherText = m_crypto.Encrypt(AESCrypto.ToBytes(msg.ToJson()));
            var param = new Dictionary<string, object>()
            {
                { "buin", m_buin },
                { "appId", m_appId },
                { "encrypt", cipherText }
            };
            var client = new HttpClient();
            var rsp = client.Post(url, param, HttpContentTypes.ApplicationJson, query: this.tokenQuery());
            Helper.CheckHttpStatus(rsp);
            var body = rsp.StaticBody<Dictionary<string, object>>(overrideContentType: HttpContentTypes.ApplicationJson);
            Helper.CheckApiStatus(body);
            var encrypt = Helper.GetEncryptJsonValue(body);
            var buffer = m_crypto.Decrypt(encrypt);
            var Info = new JsonReader().Read<Dictionary<string, object>>(AESCrypto.ToString(buffer));
            object sessionInfo;
            Info.TryGetValue("sessionId", out sessionInfo);
            if (sessionInfo != null)
            {
                sessionid = sessionInfo.ToString();
                // sessionid = sessionInfo.ToString().Substring(1);
                // sessionid = sessionid.Substring(0, sessionid.Length - 1);
            }
            return sessionid;
        }
    }
}