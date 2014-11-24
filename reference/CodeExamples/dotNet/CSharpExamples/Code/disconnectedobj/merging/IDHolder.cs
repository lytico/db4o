using System;

namespace Db4oDoc.Code.DisconnectedObj.Merging
{
    public abstract class IDHolder
    {
        private readonly Guid guid = Guid.NewGuid();

        public Guid ObjectId
        {
            get { return guid; }
        }
    }
}