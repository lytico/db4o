/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.internal.marshall;

import com.db4o.internal.*;
import com.db4o.marshall.*;
import com.db4o.typehandlers.*;


/**
 * @exclude
 */
public interface InternalReadContext extends ReadContext, HandlerVersionContext{
    
    public ReadBuffer buffer(ReadBuffer buffer);
    
    public ReadBuffer buffer();
    
    public ObjectContainerBase container();

    public int offset();

    public Object read(TypeHandler4 handler);
    
    public Object readAtCurrentSeekPosition(TypeHandler4 handler);
    
    public ReadWriteBuffer readIndirectedBuffer();

    public void seek(int offset);
    
    public int handlerVersion();
    
    public void notifyNullReferenceSkipped();

}
