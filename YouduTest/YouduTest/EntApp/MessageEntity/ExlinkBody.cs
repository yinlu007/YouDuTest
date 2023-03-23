using System;
using System.Text;
using System.Collections.Generic;

using JsonFx.Json;

namespace YouduTest.EntApp.MessageEntity
{
    public class ExlinkBody : MessageBody
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="msgList">ExlinkBodyCell的列表</param>
        public ExlinkBody(List<ExlinkBodyCell> msgList)
        {
            m_msgList = msgList != null ? msgList : new List<ExlinkBodyCell>();
        }

        private List<ExlinkBodyCell> m_msgList;

        public List<ExlinkBodyCell> MsgList
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
            foreach (var cell in m_msgList)
            {
                jsonObj.Add(cell.ToJsonObject());
            }
            return jsonObj;
        }

        public override string ToString()
        {
            return "ExlinkBody{" +
                "msgList=" + m_msgList.ToString() +
                '}';
        }
    }
}
