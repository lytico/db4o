package com.db4o.typehandlers;

import com.db4o.marshall.*;

public interface ValueTypeHandler extends TypeHandler4 {
	
	/**
	 * gets called when an value type is to be read from the database.
	 * 
	 * @param context
	 * @return the read value type
	 */
	Object read(ReadContext context);

}
