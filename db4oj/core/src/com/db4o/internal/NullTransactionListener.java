/* Copyright (C) 2004 - 2006 Versant Inc. http://www.db4o.com */

package com.db4o.internal;

import com.db4o.*;

public class NullTransactionListener implements TransactionListener {
	
	public static final TransactionListener INSTANCE = new NullTransactionListener();
	
	private NullTransactionListener() {
	}

	public void postRollback() {
	}

	public void preCommit() {
	}
}
