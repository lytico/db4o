/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package com.db4o.db4ounit.common.exceptions.propagation;

import com.db4o.*;
import com.db4o.db4ounit.common.exceptions.*;

public class DatabaseContext {
	public final ObjectContainer _db;
	public final Object _unactivated;

	public DatabaseContext(ObjectContainer db, Object unactivated) {
		_db = db;
		_unactivated = unactivated;
	}
	
	public boolean storageIsClosed() {
		return ((ExceptionSimulatingStorage)_db.ext().configure().storage()).isClosed();
	}
}
