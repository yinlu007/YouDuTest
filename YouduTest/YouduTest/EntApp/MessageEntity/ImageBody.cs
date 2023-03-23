using System;
using System.Collections.Generic;

using YouduTest.EntApp.Exceptions;
using JsonFx.Json;

namespace YouduTest.EntApp.MessageEntity
{
    public class ImageBody : MessageBody
    {
        public ImageBody()
        {

        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="mediaId">资源Id</param>
        public ImageBody(string mediaId)
        {
            m_mediaId = mediaId != null ? mediaId : "";
        }

        private string m_mediaId;

        public string MediaId
        {
            get
            {
                return m_mediaId;
            }
        }

        public override MessageBody FromJsonString(string json)
        {
            return this.FromJsonObject(new JsonReader().Read(json));
        }

        public override MessageBody FromJsonObject(object json)
        {
            if (json == null)
            {
                throw new ParamParserException("empty message body", null);
            }
            dynamic jsonObj = json;
            var mediaId = jsonObj.media_id as string;
            if (mediaId == null || mediaId.Length == 0)
            {
                throw new ParamParserException("media_id not exists", null);
            }
            m_mediaId = mediaId;
            return this;
        }

        public override string ToJsonString()
        {
            return new JsonWriter().Write(this.ToJsonObject());
        }

        public override object ToJsonObject()
        {
            var jsonObj = new Dictionary<string, string>();
            jsonObj["media_id"] = m_mediaId;
            return jsonObj;
        }

        public override string ToString()
        {
            return "ImageBody{" +
                "mediaId='" + m_mediaId + '\'' +
                '}';
        }
    }
}
