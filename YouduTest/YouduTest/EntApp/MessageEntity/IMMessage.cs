using System;
using System.Text;
using System.Collections.Generic;

using JsonFx.Json;
using YouduTest.EntApp.SessionEntity;

namespace YouduTest.EntApp.MessageEntity
{
    public class IMMessage
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
        public IMMessage(string toUser,  string FromUsers, string SessionId, string msgType, MessageBody msgBody)
        {
            m_toUser = toUser != null ? toUser : "";
            m_msgType = msgType != null ? msgType : "";
            m_msgBody = msgBody;
            m_sessionId = SessionId;
            m_sender = FromUsers;
        }

        private string m_toUser;
        private string m_msgType;
        private string m_sessionId;
        private string m_sender;
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
            //如果sessionid不为空则表示多人会话
            if (string.IsNullOrEmpty(m_sessionId))
            {
                jsonObj["receiver"] = m_toUser;
            }
            else
            {
                jsonObj["sessionId"] = m_sessionId;
            }
            jsonObj["sender"] = m_sender;
            jsonObj["msgType"] = m_msgType;
            jsonObj[m_msgType] = m_msgBody.ToJsonObject();
            return new JsonWriter().Write(jsonObj);
        }
    }
}
