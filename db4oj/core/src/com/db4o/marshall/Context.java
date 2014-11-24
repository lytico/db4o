/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.marshall;

import com.db4o.*;
import com.db4o.internal.*;
import com.db4o.internal.delete.*;

/**
 * common functionality for {@link ReadContext} and 
 * {@link WriteContext} and {@link DeleteContext}  
 */
public interface Context {
	
    ObjectContainer objectContainer();
    
    Transaction transaction();

}
