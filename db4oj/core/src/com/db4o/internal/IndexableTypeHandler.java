/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.internal;

import com.db4o.*;
import com.db4o.ext.*;
import com.db4o.internal.marshall.*;
import com.db4o.marshall.*;
import com.db4o.typehandlers.*;


/**
 * @exclude
 */
public interface IndexableTypeHandler extends Indexable4, TypeHandler4{
    
    Object indexEntryToObject(Context context, Object indexEntry);
    
    Object readIndexEntryFromObjectSlot(MarshallerFamily mf, StatefulBuffer writer) throws CorruptionException, Db4oIOException;
    
    Object readIndexEntry(ObjectIdContext context) throws CorruptionException, Db4oIOException;

}
