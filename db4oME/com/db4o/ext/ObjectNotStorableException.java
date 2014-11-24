/* Copyright (C) 2004   db4objects Inc.   http://www.db4o.com */

package com.db4o.ext;

import com.db4o.*;
import com.db4o.reflect.*;


/**
 * this Exception is thrown, if objects can not be stored and if
 * db4o is configured to throw Exceptions on storage failures.
 * @see com.db4o.config.Configuration#exceptionsOnNotStorable
 */
public class ObjectNotStorableException extends RuntimeException{
	
	public ObjectNotStorableException(ReflectClass a_class){
	    super(Messages.get(a_class.isPrimitive() ? 59: 45, a_class.getName()));
	}
}
