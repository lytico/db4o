/* Copyright (C) 2008  Versant Inc.  http://www.db4o.com */

package com.db4o.internal.marshall;

import com.db4o.internal.*;
import com.db4o.typehandlers.*;


/**
 * @exclude
 */
public class SlotFormatCurrent extends SlotFormat {

    protected int handlerVersion() {
        return HandlerRegistry.HANDLER_VERSION;
    }

    public boolean isIndirectedWithinSlot(TypeHandler4 handler){
    	if (Handlers4.isUntyped(handler)) {
    		return false;
    	}
        return Handlers4.isVariableLength(handler) && Handlers4.isValueType(handler);
    }

}
