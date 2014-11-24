/* Copyright (C) 2004 - 2006 Versant Inc. http://www.db4o.com */

package com.db4o.ta;

import com.db4o.*;

/**
 * Interface defining rollback behavior when Transparent Persistence mode is on.
 * @see TransparentPersistenceSupport
 */
public interface RollbackStrategy {
	
	/**
	 * Method to be called per TP-enabled object when the transaction is rolled back.
	 * @param container current ObjectContainer
	 * @param obj TP-enabled object
	 */
	void rollback(ObjectContainer container, Object obj);

}
