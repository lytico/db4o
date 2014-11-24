using System;

namespace ObjectActivationSample
{
    public class SupportObjectId
    {
        [NonSerialized]
        private long objectId = -1;

        public long ObjectId
        {
            get { return objectId; }
            set { objectId = value; }
        }

        public override string ToString()
        {
            return String.Format(" (Id: {0}) ", objectId);
        }
    }
}
