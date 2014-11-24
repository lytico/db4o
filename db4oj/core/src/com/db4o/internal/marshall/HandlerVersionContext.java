/* Copyright (C) 2008  Versant Inc.  http://www.db4o.com */

package com.db4o.internal.marshall;

import com.db4o.marshall.*;

/**
 * @exclude
 */
public interface HandlerVersionContext extends Context{
    
    public int handlerVersion();
    
    public SlotFormat slotFormat();

}
