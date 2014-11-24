/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.events;

import com.db4o.*;
import com.db4o.internal.*;

public class TransactionalEventArgs extends EventArgs {
	
	private final Transaction _transaction;

	public TransactionalEventArgs(Transaction transaction) {
		_transaction = transaction;
	}
	
	public Object transaction() {
		return _transaction;
	}
	
	public ObjectContainer objectContainer(){
		return _transaction.objectContainer();
	}
	
}
