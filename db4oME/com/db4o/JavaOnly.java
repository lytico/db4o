/* Copyright (C) 2004   db4objects Inc.   http://www.db4o.com */

package com.db4o;

import com.db4o.config.*;

class JavaOnly {
    
    public static void link(){
        Object obj = new TClass();
        obj = new TVector();
        obj = new THashtable();
        obj = new TNull();
    }
    
    public static void runFinalizersOnExit(){
    }
    
	static final Class[] SIMPLE_CLASSES ={
		Integer.class,
		Long.class,
		Float.class,
		Boolean.class,
		Double.class,
		Byte.class,
		Character.class,
		Short.class,
		String.class,
		java.util.Date.class
	};


}
