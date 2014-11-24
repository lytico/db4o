/* Copyright (C) 2004 - 2006 Versant Inc. http://www.db4o.com */

package com.db4o.internal;

/**
 * @exclude
 */
public interface CommittedCallbackDispatcher {
	
	boolean willDispatchCommitted();

	void dispatchCommitted(CallbackObjectInfoCollections committedInfo);
}
