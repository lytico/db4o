/* Copyright (C) 2008  Versant Inc.  http://www.db4o.com */

package com.db4o.internal.handlers.versions;

import com.db4o.internal.*;
import com.db4o.marshall.*;
import com.db4o.typehandlers.*;


/**
 * @exclude
 */
public class OpenTypeHandler2 extends OpenTypeHandler7 {
    
    public OpenTypeHandler2(ObjectContainerBase container) {
        super(container);
    }
    
    protected void seekSecondaryOffset(ReadBuffer buffer, TypeHandler4 typeHandler) {
        if(Handlers4.handlesPrimitiveArray(typeHandler)){
            buffer.seek(buffer.readInt());
        }
    }


}
