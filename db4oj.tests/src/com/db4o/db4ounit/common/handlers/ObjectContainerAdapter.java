/* Copyright (C) 2010 Versant Inc. http://www.db4o.com */
package com.db4o.db4ounit.common.handlers;

import com.db4o.ext.*;
import com.db4o.query.*;

public interface ObjectContainerAdapter {
	
	void store(Object obj);
	
	void store(Object obj, int depth);
	
	void commit();
	
	void delete(Object obj);
	
	Query query();
	
	ObjectContainerAdapter forContainer(ExtObjectContainer db);
	
	Object objectContainer();
}