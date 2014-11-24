using System;
using Db4objects.Db4o.Config.Attributes;

namespace Db4oDoc.WebApp.Models
{
    // #example: using the GUID as id
    public abstract class IDHolder
    {
        [Indexed]
        private readonly Guid id;

        protected IDHolder()
        {
            id = Guid.NewGuid();
        }

        public Guid ID
        {
            get { return id; }
        }
    }
    // #end example
}