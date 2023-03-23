using System;
using System.Text;
using System.Collections.Generic;

using JsonFx.Json;
using YouduTest.EntApp.Exceptions;
using System.Dynamic;

namespace YouduTest.EntApp.MessageEntity
{
    public class ReceiveMessage
    {
        public const string MessageTypeText = "text";
        public const string MessageTypeImage = "image";
        public const string MessageTypeFile = "file";

        public ReceiveMessage()
        {

        }

        private string m_fromUser = "";
        private long m_createTime;
        private string m_packageId = "";
        private string m_msgType = "";
        private MessageBody m_msgBody;

        /// <summary>
        /// 发送消息的用户
        /// </summary>
        public string FromUser
        {
            get
            {
                return m_fromUser;
            }
        }

        /// <summary>
        /// 消息的发送时间
        /// </summary>
        public long CreateTime
        {
            get
            {
                return m_createTime;
            }
        }

        /// <summary>
        /// packageId，需要返回给有度服务表示接收成功
        /// </summary>
        public string PackageId
        {
            get
            {
                return m_packageId;
            }
        }

        /// <summary>
        /// 消息类型
        /// </summary>
        public string MsgType
        {
            get
            {
                return m_msgType;
            }
        }

        /// <summary>
        /// 消息体
        /// </summary>
        public MessageBody MsgBody
        {
            get
            {
                return m_msgBody;
            }
        }

        public ReceiveMessage FromJson(string json)
        {
            dynamic jsonObj = new JsonReader().Read(json);
            if (jsonObj == null)
            {
                throw new ParamParserException("invalid json object", null);
            }
            var fromUser = jsonObj.fromUser;
            var createTime = jsonObj.createTime;
            var packageId = jsonObj.packageId;
            var msgType = jsonObj.msgType;
            if (fromUser is string 
                && (createTime is int || createTime is long) 
                && packageId is string
                && msgType is string)
            {
                string type = msgType;
                switch (type)
                {
                    case MessageTypeText:
                        m_msgBody = new TextBody().FromJsonObject(jsonObj.text);
                        break;
                    case MessageTypeImage:
                        m_msgBody = new ImageBody().FromJsonObject(jsonObj.image);
                        break;
                    case MessageTypeFile:
                        m_msgBody = new FileBody().FromJsonObject(jsonObj.file);
                        break;
                    default:
                        throw new ParamParserException(String.Format("unknown message type {0}", type), null);
                }
                m_fromUser = Convert.ToString(fromUser);
                m_createTime = Convert.ToInt64(createTime);
                m_packageId = Convert.ToString(packageId);
                m_msgType = type;
                return this;
            }
            throw new ParamParserException(String.Format("invalid json content: {0}", jsonObj.ToString()), null);
        }

        public override string ToString()
        {
            return "ReMessage{" +
                    "FromUser='" + m_fromUser + '\'' +
                    ", CreateTime=" + m_createTime +
                    ", PackageId=" + m_packageId +
                    ", MsgType='" + m_msgType + '\'' +
                    ", MsgBody=" + m_msgBody.ToString() +
                    '}';
        }
    }
}
