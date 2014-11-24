using System;

namespace Db4oDoc.Code.DisconnectedObj.IdExamples
{
    
    /// Id holder. For the example code it supports both, uuid and an int-id.
    /// For a project you normally choose one or the other.
    public abstract class IDHolder
    {
        // #example: generate the id
        private readonly Guid guid = Guid.NewGuid();

        public Guid ObjectId
        {
            get { return guid; }
        }

        // #end example

        // #example: id holder
        private int id;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        // #end example
    }
}