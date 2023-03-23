using System;
using System.Collections.Generic;

using YouduTest.EntApp.Exceptions;
using JsonFx.Json;

namespace YouduTest.EntApp.MessageEntity
{
    public class MpnewsBodyCell : MessageBody
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="mediaId">封面图片Id</param>
        /// <param name="digest">摘要</param>
        /// <param name="content">正文</param>
        public MpnewsBodyCell(string title, string mediaId, string digest, string content)
        {
            m_title = title != null ? title : "";
            m_mediaId = mediaId != null ? mediaId : "";
            m_digest = digest != null ? digest : "";
            m_content = content != null ? content : "";
        }

        private string m_title;
        private string m_mediaId;
        private string m_digest;
        private string m_content;

        public string Title
        {
            get
            {
                return m_title;
            }
        }

        public string MediaId
        {
            get
            {
                return m_mediaId;
            }
        }

        public string Digest
        {
            get
            {
                return m_digest;
            }
        }

        public string Content
        {
            get
            {
                return m_content;
            }
        }

        public override MessageBody FromJsonString(string json)
        {
            throw new NotImplementedException();
        }

        public override MessageBody FromJsonObject(object json)
        {
            throw new NotImplementedException();
        }

        public override string ToJsonString()
        {
            return new JsonWriter().Write(this.ToJsonObject());
        }

        public override object ToJsonObject()
        {
            var jsonObj = new Dictionary<string, string>();
            jsonObj["title"] = m_title;
            jsonObj["media_id"] = m_mediaId;
            jsonObj["digest"] = m_digest;
            jsonObj["content"] = m_content;
            return jsonObj;
        }
    }
}
