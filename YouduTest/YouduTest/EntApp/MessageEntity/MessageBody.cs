using System;

namespace YouduTest.EntApp.MessageEntity
{
    public abstract class MessageBody
    {
        public abstract MessageBody FromJsonString(string json);

        public abstract MessageBody FromJsonObject(object json);

        public abstract string ToJsonString();

        public abstract object ToJsonObject();

        public TextBody ToTextBody()
        {
            return this as TextBody;
        }

        public ImageBody ToImageBody()
        {
            return this as ImageBody;
        }

        public FileBody ToFileBody()
        {
            return this as FileBody;
        }

        public MpnewsBody ToMpnewsBody()
        {
            return this as MpnewsBody;
        }

        public ExlinkBody ToExlinkBody()
        {
            return this as ExlinkBody;
        }
    }
}
