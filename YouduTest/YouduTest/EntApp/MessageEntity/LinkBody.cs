using System;
using System.Text;
using System.Collections.Generic;

using JsonFx.Json;

namespace YouduTest.EntApp.MessageEntity
{
    public class LinkBody : MessageBody
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="url">链接</param>
        /// <param name="action">是否需要单点登录token</param>
        public LinkBody(String url, String title, int action)
        {
            this.url = url != null ? url : "";
            this.title = title != null ? title : "";
            this.action = action;
        }

        private String title;
        private String url;
        private int action;

        public String Title
        {
            get
            {
                return title;
            }
        }

        public String Url
        {
            get
            {
                return url;
            }
        }

        public int Action
        {
            get
            {
                return action;
            }
        }

        public override String ToJsonString()
        {
            return new JsonWriter().Write(this.ToJsonObject());
        }

        public override object ToJsonObject()
        {
            var jsonObj = new Dictionary<String, object>();
            jsonObj["url"] = url;
            jsonObj["title"] = title;
            jsonObj["action"] = action;
            return jsonObj;
        }

        public override MessageBody FromJsonString(String json)
        {
            throw new NotImplementedException();
        }

        public override MessageBody FromJsonObject(object json)
        {
            throw new NotImplementedException();
        }

        public override String ToString()
        {
            return "ExlinkBody{" +
                "title='" + title + '\'' +
                ", url='" + url + '\'' +
                ", action='" + action + '\'' +
                '}';
        }
    }
}
