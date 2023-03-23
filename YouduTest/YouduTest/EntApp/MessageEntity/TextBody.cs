using System;
using System.Collections.Generic;

using YouduTest.EntApp.Exceptions;
using JsonFx.Json;

namespace YouduTest.EntApp.MessageEntity
{
    public class TextBody : MessageBody
    {
        public TextBody()
        {

        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="content">消息内容</param>
        public TextBody(string content)
        {
            m_content = content != null ? content : "";
        }

        private string m_content;

        public string Content
        {
            get
            {
                return m_content;
            }
        }

        public override MessageBody FromJsonObject(object json)
        {
            if (json == null)
            {
                throw new ParamParserException("empty message body", null);
            }
            dynamic jsonObj = json;
            var content = jsonObj.content as string;
            if (content == null)
            {
                throw new ParamParserException("content not exists", null);
            }
            m_content = content;
            return this;
        }

        public override MessageBody FromJsonString(string json)
        {
            return this.FromJsonObject(new JsonReader().Read(json));
        }

        public override object ToJsonObject()
        {
            var jsonObj = new Dictionary<string, string>();
            jsonObj["content"] = m_content;
            return jsonObj;
        }

        public override string ToJsonString()
        {
            var writer = new JsonWriter();
            return writer.Write(this.ToJsonObject());
        }

        public override string ToString()
        {
            return "TextBody{" + 
                "content='" + m_content + '\'' +
                '}';
        }
    }
}
