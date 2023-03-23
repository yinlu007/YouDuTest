using System;
using System.Text;
using System.Collections.Generic;

using JsonFx.Json;

namespace YouduTest.EntApp.MessageEntity
{
    public class ExlinkBodyCell : MessageBody
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="url">链接</param>
        /// <param name="digest">摘要</param>
        /// <param name="mediaId">封面图片的Id</param>
        public ExlinkBodyCell(string title, string url, string digest, string mediaId)
        {
            m_title = title != null ? title : "";
            m_url = url != null ? url : "";
            m_digest = digest != null ? digest : "";
            m_mediaId = mediaId != null ? mediaId : "";
        }

        private string m_title;
        private string m_url;
        private string m_digest;
        private string m_mediaId;

        public string Title
        {
            get
            {
                return m_title;
            }
        }

        public string Url
        {
            get
            {
                return m_url;
            }
        }

        public string Digest
        {
            get
            {
                return m_digest;
            }
        }

        public string MediaId
        {
            get
            {
                return m_mediaId;
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
            jsonObj["url"] = m_url;
            jsonObj["digest"] = m_digest;
            jsonObj["media_id"] = m_mediaId;
            return jsonObj;
        }

        public override string ToString()
        {
            return "ExlinkBody{" +
                "title='" + m_title + '\'' +
                ", url='" + m_url + '\'' +
                ", digest='" + m_digest + '\'' +
                ", mediaId='" + m_mediaId + '\'' +
                '}';
        }
    }
}
