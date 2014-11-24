/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.internal.delete;

import com.db4o.internal.marshall.*;
import com.db4o.internal.slots.*;
import com.db4o.marshall.*;
import com.db4o.typehandlers.*;


/**
 * @exclude
 */
public interface DeleteContext extends Context, ReadBuffer, HandlerVersionContext{
    
    public boolean cascadeDelete();

	public int cascadeDeleteDepth();
	
	public void delete(TypeHandler4 handler);
	
	public void deleteObject();

	boolean isLegacyHandlerVersion();
	
	void defragmentRecommended();

	Slot readSlot();

	public int objectId();
	
}
