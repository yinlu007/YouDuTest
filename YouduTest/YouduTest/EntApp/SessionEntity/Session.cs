using System.Collections.Generic;

namespace YouduTest.EntApp.SessionEntity
{
    public class Session
    {
        public Session(string sessionId, 
            string title, 
            string owner, 
            long version, 
            string type, 
            List<string> member, 
            long lastMsgId, 
            long activeTime)
        {
            m_sessionId = sessionId;
            m_title = title;
            m_owner = owner;
            m_version = version;
            m_type = type;
            m_member = member;
            m_lastMsgId = lastMsgId;
            m_activeTime = activeTime;
        }

        private string m_sessionId = "";

        public string SessionId
        {
            get
            {
                return m_sessionId;
            }
        }

        private string m_title = "";

        public string Title
        {
            get
            {
                return m_title;
            }
        }

        private string m_owner = "";

        public string Owner
        {
            get
            {
                return m_owner;
            }
        }

        private long m_version = 0;

        public long Version
        {
            get
            {
                return m_version;
            }
        }

        private string m_type = "";

        public string Type
        {
            get
            {
                return m_type;
            }
        }

        private List<string> m_member = new List<string>();

        public List<string> Member
        {
            get
            {
                return m_member;
            }
        }

        private long m_lastMsgId = 0;

        public long LastMsgId
        {
            get
            {
                return m_lastMsgId;
            }
        }

        private long m_activeTime = 0;

        public long ActiveTime
        {
            get
            {
                return m_activeTime;
            }
        }

        public override string ToString()
        {
            return "Session{" +
                "sessionId=" + m_sessionId + "\n" +
                ", title=" + m_title + "\n" +
                ", owner=" + m_owner + "\n" +
                ", version=" + m_version + "\n" +
                ", type=" + m_type + "\n" +
                ", lastMsgId=" + m_lastMsgId + "\n" +
                ", activeTime=" + m_activeTime + "\n" +
                ", member=" + m_member.ToString() + "\n" +
                "}";
        }
    }
}
