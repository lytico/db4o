/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.typehandlers;

import com.db4o.internal.marshall.*;

/**
 * TypeHandler for objects with members.
 */
public interface CascadingTypeHandler extends TypeHandler4{
    
	/**
	 * will be called during activation if the handled
	 * object is already active 
	 * @param context
	 */
    void cascadeActivation(ActivationContext context);
    
    /**
     * will be called during querying to ask for the handler
     * to be used to collect children of the handled object
     * @param context
     * @return
     */
    TypeHandler4 readCandidateHandler(QueryingReadContext context);
    
    /**
     * will be called during querying to ask for IDs of member
     * objects of the handled object.
     * @param context
     */
    void collectIDs(QueryingReadContext context);

}
