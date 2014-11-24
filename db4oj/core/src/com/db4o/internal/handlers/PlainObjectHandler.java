package com.db4o.internal.handlers;

import com.db4o.ext.*;
import com.db4o.internal.*;
import com.db4o.internal.delete.*;
import com.db4o.marshall.*;
import com.db4o.typehandlers.*;


/**
 * Tyehandler for naked plain objects (java.lang.Object).
 */
public class PlainObjectHandler implements ReferenceTypeHandler {

    public void defragment(DefragmentContext context) {
    }

    public void delete(DeleteContext context) throws Db4oIOException {
    }
    
    public void activate(ReferenceActivationContext context) {
    }
    
    public void write(WriteContext context, Object obj) {
    }
}
