using JsonFx.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YouduTest.EntApp.MessageEntity
{
    public class PopWindow
    {
        private int width;
        public int Width
        {
            get { return width; }
            set { width = value; }
        }

        private int height;
        public int Height
        {
            get { return height; }
            set { height = value; }
        }

        private int position;
        public int Position
        {
            get { return position; }
            set { position = value; }
        }

        private int duration;
        public int Duration
        {
            get { return duration; }
            set { duration = value; }
        }

        private int popMode;
        public int PopMode
        {
            get { return popMode; }
            set { popMode = value; }
        }

        private string url;
        public string Url
        {
            get { return url; }
            set { url = value; }
        }

        private string tip;
        public string Tip
        {
            get { return tip; }
            set { tip = value; }
        }

        private string title;
        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        private string toUser;
        public string ToUser
        {
            get { return toUser; }
            set { toUser = value; }
        }

        private string toDept;
        public string ToDept
        {
            get { return toDept; }
            set { toDept = value; }
        }

        private string noticeId;
        public string NoticeId
        {
            get { return noticeId; }
            set { noticeId = value; }
        }

        public string ToYDSdkJsonString()
        {
            return new JsonWriter().Write(this.ToYDSdkJsonObject());
        }

        public object ToYDSdkJsonObject()
        {
            var jsonObj = new Dictionary<string, Object>();
            if (null != toUser && !"".Equals(toUser))
            {
                jsonObj["toUser"] = toUser;
            }
            else {
                jsonObj["toUser"] = "";
            }

            if (null != toDept && !"".Equals(toDept))
            {
                jsonObj["toDept"] = toDept;
            }
            else {
                jsonObj["toDept"] = "";
            }
            
            var winObj = new Dictionary<string, Object>();
            if (null != url) {
                winObj["url"] = url;
            }
            if (null != tip) {
                winObj["tip"] = tip;
            }
            if (null != title)
            {
                winObj["title"] = title;
            }
          
            winObj["width"] = width;
            winObj["height"] = height;
            winObj["duration"] = duration;
            winObj["position"] = position;
            if (null != noticeId)
            {
                winObj["notice_id"] = noticeId;
            }
            winObj["pop_mode"] = popMode;

            jsonObj["popWindow"] = winObj;
            return jsonObj;
        }
    }
}
