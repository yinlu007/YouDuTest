using System;
using System.Text;
using System.Collections.Generic;

using JsonFx.Json;

namespace YouduTest.EntApp.MessageEntity
{
    public class MpnewsBody : MessageBody
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="msgList">MpnewsBodyCell的列表</param>
        public MpnewsBody(List<MpnewsBodyCell> msgList)
        {
            m_msgList = msgList != null ? msgList : new List<MpnewsBodyCell>();
        }

        private List<MpnewsBodyCell> m_msgList;

        public List<MpnewsBodyCell> MsgList
        {
            get
            {
                return m_msgList;
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
            var jsonObj = new List<object>();
            foreach(var cell in m_msgList)
            {
                jsonObj.Add(cell.ToJsonObject());
            }
            return jsonObj;
        }

        public override string ToString()
        {
            return "MpnewsBody{" +
                "msgList=" + m_msgList.ToString() +
                '}';
        }
    }
}
