using JsonFx.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YouduTest.EntApp.MessageEntity
{
    public class AppNotice
    {
        public AppNotice()
        {}

        private string account;

        public string Account
        {
            get { return account; }
            set { account = value; }
        }

        private int count;

        public int Count
        {
            get { return count; }
            set { count = value; }
        }

        private string tip;

        public string Tip
        {
            get { return tip; }
            set { tip = value; }
        }



        public AppNotice(string account, int count, string tip)
        {
            this.account = account;
            this.count = count;
            this.tip = tip;
        }

        public string ToJsonString()
        {
            return new JsonWriter().Write(this.ToJsonObject());
        }

        public object ToJsonObject()
        {
            var jsonObj = new Dictionary<string, Object>();
            jsonObj["tip"] = tip;
            jsonObj["count"] = count;
            jsonObj["account"] = account;
            return jsonObj;
        }

        public override string ToString()
        {
            return "AppNotice{" +
                "account=" + account+
                "tip=" + tip +
                "count=" + count +
                '}';
        }

    }
}
