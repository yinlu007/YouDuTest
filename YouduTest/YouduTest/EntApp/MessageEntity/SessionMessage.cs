using System;
using System.Text;
using System.Collections.Generic;

using JsonFx.Json;
using YouduTest.EntApp.SessionEntity;

namespace YouduTest.EntApp.MessageEntity
{
    public class SessionMessage
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="title"></param>
        /// <param name="creator"></param>
        /// <param name="type"></param>
        /// <param name="member"></param>
        public SessionMessage(string title,  string creator, string type, string [] member)
        {
            m_title = title != null ? title : "";
            m_creator = creator != null ? creator : "";
            m_type = type;
            m_member = member;
        }

        private string m_title;
        private string m_creator;
        private string m_type;
        private string [] m_member;

        public string ToJson()
        {
            var jsonObj = new Dictionary<string, object>();
            jsonObj["title"] = m_title;
            jsonObj["creator"] = m_creator;
            jsonObj["type"] = m_type;
            jsonObj["member"] = m_member;
            return new JsonWriter().Write(jsonObj);
        }
    }
}
