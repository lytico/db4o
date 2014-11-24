/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.ext;

import com.db4o.internal.*;
import com.db4o.reflect.*;


/**
 * this Exception is thrown, if objects can not be stored and if
 * db4o is configured to throw Exceptions on storage failures.
 * @see com.db4o.config.Configuration#exceptionsOnNotStorable
 */
public class ObjectNotStorableException extends Db4oRecoverableException{
	
	public ObjectNotStorableException(ReflectClass clazz){
	    super(Messages.get(clazz.isSimple() ? 59: 45, clazz.getName()));
	}
    
    public ObjectNotStorableException(String message){
        super(message);
    }
    
    public ObjectNotStorableException(ReflectClass clazz, String message){
        super(clazz.getName() + ": " + message);
    }

}
