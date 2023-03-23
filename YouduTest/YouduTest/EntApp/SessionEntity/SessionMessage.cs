
using JsonFx.Json;
using System;
using System.Collections.Generic;
using YouduTest.EntApp.Exceptions;
namespace YouduTest.EntApp.SessionEntity
{
    public class SessionMessage
    {
        public SessionMessage()
        {

        }

        public string SessionId = "";
        public string FromUser = "";
        public string Receiver = "";
        public List<string> Receivers = new List<string>();
        public long CreateTime = 0;
        public long MsgId = 0;
        public string MsgType = "";
        public long Version = 0;
        public string PackageId = "";
        public object Content = null;

        public SessionMessage FromJson(string json)
        {
            dynamic jsonObj = new JsonReader().Read(json);
            if (jsonObj == null)
            {
                throw new ParamParserException("invalid json object", null);
            }
            if (Helper.HasKey(jsonObj, "fromUser"))
            {
                var fromUser = jsonObj.fromUser;
                if (fromUser is string)
                {
                    this.FromUser = (string)fromUser;
                }
            }
            var createTime = jsonObj.createTime;
            if (createTime is int || createTime is long)
            {
                this.CreateTime = Convert.ToInt64(createTime);
            }
            var msgType = jsonObj.msgType;
            if (msgType is string)
            {
                this.MsgType = (string)msgType;
            }
            if (Helper.HasKey(jsonObj, "sessionId"))
            {
                var sessionId = jsonObj.sessionId;
                if (sessionId is string)
                {
                    this.SessionId = (string)sessionId;
                }
            }
            if (Helper.HasKey(jsonObj, "receiver"))
            {
                var receiver = jsonObj.receiver;
                if (receiver is string)
                {
                    this.Receiver = (string)receiver;
                }
            }
            if (Helper.HasKey(jsonObj, "msgId"))
            {
                var msgId = jsonObj.msgId;
                if (msgId is int || msgId is long)
                {
                    this.MsgId = Convert.ToInt64(msgId);
                }
            }
            if (Helper.HasKey(jsonObj, "version"))
            {
                var version = jsonObj.version;
                if (version is int || version is long)
                {
                    this.Version = Convert.ToInt64(version);
                }
            }
            if (Helper.HasKey(jsonObj, "receivers"))
            {
                var receivers = jsonObj.receivers;
                if (receivers is string[])
                {
                    foreach (var r in (string[])receivers)
                    {
                        this.Receivers.Add(r);
                    }
                }
            }
            var packageId = jsonObj.packageId;
            if (packageId is string)
            {
                this.PackageId = (string)packageId;
            }
            switch(this.MsgType)
            {
                case "session_create":
                    this.Content = jsonObj.session_create;
                    break;
                case "session_update":
                    this.Content = jsonObj.session_update;
                    break;
                case "text":
                    this.Content = jsonObj.text;
                    break;
                case "image":
                    this.Content = jsonObj.image;
                    break;
                case "file":
                    this.Content = jsonObj.file;
                    break;
                case "complex":
                    this.Content = jsonObj.complex;
                    break;
                case "broadcast":
                    this.Content = jsonObj.broadcast;
                    break;
                case "system":
                    this.Content = jsonObj.system;
                    break;
            }
            return this;
        }

        public override string ToString()
        {
            return "SessionMessage{" + "\n" +
                "SessionId=" + this.SessionId + "\n" +
                ", FromUser=" + this.FromUser + "\n" +
                ", Receiver=" + this.Receiver + "\n" +
                ", CreateTime=" + this.CreateTime + "\n" +
                ", MsgId=" + this.MsgId + "\n" +
                ", MsgType=" + this.MsgType + "\n" +
                ", Version=" + this.Version + "\n" +
                ", PackageId=" + this.PackageId + "\n" +
                ", Content=" + this.Content.ToString() + "\n" +
                "}";
        }
    }
}
