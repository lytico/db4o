/* Copyright (C) 2004 - 2005  db4objects Inc.  http://www.db4o.com */

package com.db4o.inside;

import com.db4o.*;
import com.db4o.ext.*;

/**
 * @exclude
 */
public class Exceptions4 {

    public static final void throwRuntimeException (int code) {
        throwRuntimeException(code, null, null);
    }

    public static final void throwRuntimeException (int code, Throwable cause) {
    	throwRuntimeException(code, null, cause);
    }

    public static final void throwRuntimeException (int code, String msg) {
        throwRuntimeException(code, msg, null);
    }

    public static final void throwRuntimeException (int code, String msg, Throwable cause) {
    	Messages.logErr(Db4o.configure(), code,msg, cause);
        throw new Db4oException(Messages.get(code, msg));
    }
    
    public static final void notSupported(){
		throwRuntimeException(53);
    }
    
    public static final void catchAll(Throwable exc) throws Db4oException {
    	if(exc instanceof Db4oException) {
    		throw (Db4oException)exc;
    	}
    }
}
