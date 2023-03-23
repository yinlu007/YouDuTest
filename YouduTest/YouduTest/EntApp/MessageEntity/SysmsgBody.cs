using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using JsonFx.Json;

namespace YouduTest.EntApp.MessageEntity
{
    public class SysmsgBody : MessageBody
    {
        public SysmsgBody()
        {
            this.msgList = new List<Dictionary<String, MessageBody>>();
        }

        //public SysMsgBody(String title)
        //{
        //    this.title = title != null ? title : "";
        //    this.msgList = new List<Dictionary<String, MessageBody>>();
        //}

       // public SysMsgBody(String title, List<Dictionary<String, MessageBody>> msg)
       // {
       //     this.title = title != null ? title : "";
       //     if (msg == null)
       //     {
       //         msg = new List<Dictionary<String, MessageBody>>();
       //     }
       //     this.msgList = msg;
       // }

        private String title;

        private int popDuration;

        private List<Dictionary<String, MessageBody>> msgList;

        public string Title { get => title; set => title = value; }

        public int PopDuration { get => popDuration; set => popDuration = value;}

        public void addTextBody(String txt)
        {
            if (null == txt || "".Equals(txt))
            {
                return;
            }
            TextBody body = new TextBody(txt);
            Dictionary<String, MessageBody> m = new Dictionary<String, MessageBody>();
            m.Add("text", body);
            this.msgList.Add(m);
        }

        public void addLinkBody(String url, String title, int action)
        {
            if (null == title || "".Equals(title))
            {
                return;
            }
            if (null == url || "".Equals(url))
            {
                return;
            }
            LinkBody body = new LinkBody(url, title, action);
            Dictionary<String, MessageBody> m = new Dictionary<String, MessageBody>();
            m.Add("link", body);
            this.msgList.Add(m);
        }

        public override string ToJsonString()
        {
            return new JsonWriter().Write(this.ToJsonObject());
        }

        public override object ToJsonObject()
        {
            if (this.popDuration < 6) {
                this.popDuration = 6;
            }

            var jsonObj = new Dictionary<string, object>();
            jsonObj["title"] = title;
            jsonObj["popDuration"] = popDuration;

            List<Dictionary<string, object>> array = new List<Dictionary<string, object>>();
            foreach (var msg in msgList)
            {
                foreach (var item in msg)
                {
                    Console.WriteLine(item.Key + item.Value);
                    Dictionary<string, object> d = new Dictionary<string, object>();
                    d.Add(item.Key, item.Value.ToJsonObject());
                    array.Add(d);
                }
            }

            jsonObj["msg"] = array;
            return jsonObj;
        }

        public override MessageBody FromJsonObject(object json)
        {
            throw new NotImplementedException();
        }

        public override MessageBody FromJsonString(string json)
        {
            throw new NotImplementedException();
        }
    }
}
