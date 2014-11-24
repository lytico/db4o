package com.db4o.typehandlers;

import com.db4o.marshall.*;

public interface ReferenceTypeHandler extends TypeHandler4 {
	
	/**
	 * gets called when an object is to be activated.
	 * 
	 * @param context
	 */
	void activate(ReferenceActivationContext context);
}
