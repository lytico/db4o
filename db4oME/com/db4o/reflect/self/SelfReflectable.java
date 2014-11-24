/* Copyright (C) 2004 - 2005  db4objects Inc.  http://www.db4o.com */

package com.db4o.reflect.self;


public interface SelfReflectable {
    
	Object self_get(String fieldName);
    
	void self_set(String fieldName,Object value);
    
}
