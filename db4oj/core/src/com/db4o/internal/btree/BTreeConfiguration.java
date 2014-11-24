/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

package com.db4o.internal.btree;

import com.db4o.internal.ids.*;
import com.db4o.internal.slots.*;

/**
 * @exclude
 */
public class BTreeConfiguration {
	
	public static final BTreeConfiguration DEFAULT = new BTreeConfiguration(null, 20, true);
	
	public final TransactionalIdSystem _idSystem;

	public final SlotChangeFactory _slotChangeFactory;
	
	public final boolean _canEnlistWithTransaction;

	public final int _cacheSize;

	public BTreeConfiguration(TransactionalIdSystem idSystem, SlotChangeFactory slotChangeFactory, int cacheSize, boolean canEnlistWithTransaction) {
		_idSystem = idSystem;
		_slotChangeFactory = slotChangeFactory;
		_canEnlistWithTransaction = canEnlistWithTransaction;
		_cacheSize = cacheSize;
	}

	public BTreeConfiguration(TransactionalIdSystem idSystem, int cacheSize, boolean canEnlistWithTransaction){
		this(idSystem, SlotChangeFactory.SYSTEM_OBJECTS, cacheSize, canEnlistWithTransaction);
	}

	
}
