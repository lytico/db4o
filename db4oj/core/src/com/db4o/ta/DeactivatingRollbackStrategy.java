/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

package com.db4o.ta;

import com.db4o.*;

/**
 * RollbackStrategy to deactivate all activated objects on rollback.
 * @see TransparentPersistenceSupport
 */
public class DeactivatingRollbackStrategy implements RollbackStrategy {

	/**
	 * deactivates each object.
	 */
	public void rollback(ObjectContainer container, Object obj) {
		container.ext().deactivate(obj);
	}

}
