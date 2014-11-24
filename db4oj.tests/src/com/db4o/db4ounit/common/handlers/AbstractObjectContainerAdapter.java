/* Copyright (C) 2010 Versant Inc. http://www.db4o.com */
package com.db4o.db4ounit.common.handlers;

import com.db4o.ext.*;
import com.db4o.query.*;

public abstract class AbstractObjectContainerAdapter implements ObjectContainerAdapter {

	protected ExtObjectContainer db;

	public ObjectContainerAdapter forContainer(ExtObjectContainer db) {
		this.db = db;
		
		return this;
	}

	public void commit() {
		db.commit();
	}

	public void delete(Object obj) {
		db.delete(obj);
	}

	public Query query() { 
		return db.query();
	}

	public AbstractObjectContainerAdapter() {
		super();
	}
	
	@Override
	public Object objectContainer() {
		return db;
	}


}