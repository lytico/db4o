/* Copyright (C) 2008  Versant Inc.  http://www.db4o.com */

package com.db4o.internal.marshall;

import com.db4o.internal.*;
import com.db4o.typehandlers.*;


/**
 * @exclude
 */
public class SlotFormat2 extends SlotFormat {

    protected int handlerVersion() {
        return 2;
    }

    public boolean isIndirectedWithinSlot(TypeHandler4 handler) {
        return Handlers4.isVariableLength(handler);
    }

}
