using System;
using System.Text;
using System.Collections.Generic;

using JsonFx.Json;

namespace YouduTest.EntApp.MessageEntity
{
    public class Message
    {
        public const string MessageTypeText = "text";
        public const string MessageTypeImage = "image";
        public const string MessageTypeMpnews = "mpnews";
        public const string MessageTypeFile = "file";
        public const string MessageTypeExlink = "exlink";
        public const string MessageTypeSysmsg = "sysMsg";

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="toUser">发送目标用户，格式为"user1 | user2 | user3"</param>
        /// <param name="msgType">消息类型</param>
        /// <param name="msgBody">消息体</param>
        public Message(string toUser, string msgType, MessageBody msgBody)
        {
            m_toUser = toUser != null ? toUser : "";
            m_msgType = msgType != null ? msgType : "";
            m_msgBody = msgBody;
        }

        private string m_toUser;
        private string m_msgType;
        private MessageBody m_msgBody;

        public string ToUser
        {
            get
            {
                return m_toUser;
            }
        }

        public string MsgType
        {
            get
            {
                return m_msgType;
            }
        }

        public MessageBody MsgBody
        {
            get
            {
                return m_msgBody;
            }
        }

        public string ToJson()
        {
            var jsonObj = new Dictionary<string, object>();
            jsonObj["toUser"] = m_toUser;
            jsonObj["msgType"] = m_msgType;
            jsonObj[m_msgType] = m_msgBody.ToJsonObject();
            return new JsonWriter().Write(jsonObj);
        }

        public override string ToString()
        {
            return "Message{" +
                "ToUser='" + m_toUser + '\'' +
                ", MsgType='" + m_msgType + '\'' +
                ", MsgBody=" + m_msgBody.ToString() +
                '}';
        }
    }
}
