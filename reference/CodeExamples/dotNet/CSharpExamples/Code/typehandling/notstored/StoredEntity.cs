using System;
using Db4objects.Db4o;

namespace Db4oDoc.Code.TypeHandling.NotStored
{
    // #example: Mark a field as transient
    public class StoredEntity {
        // Fields marked with NonSerialized won't be stored
        [NonSerialized]
        private int someCachedValue;

        // Fields marked with Transient won't be stored
        [Transient]
        private int someOtherCachedValue;

        // ..
    }
    // #end example
}