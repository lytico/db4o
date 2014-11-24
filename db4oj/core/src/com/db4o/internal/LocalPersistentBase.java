/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

package com.db4o.internal;

import com.db4o.*;
import com.db4o.internal.ids.*;
import com.db4o.internal.slots.*;

/**
 * @exclude
 */
public abstract class LocalPersistentBase extends PersistentBase {
	
	private final TransactionalIdSystem _idSystem;
	
	public LocalPersistentBase(TransactionalIdSystem idSystem){
		_idSystem = idSystem;
	}
	
	public LocalPersistentBase(){
		this(null);
	}
	
	public TransactionalIdSystem idSystem(Transaction trans) {
		if(_idSystem != null){
			return _idSystem;
		}
		return super.idSystem(trans);
	}
	
	@Override
	protected ByteArrayBuffer readBufferById(Transaction trans) {
		Slot slot = idSystem(trans).currentSlot(getID());
		if(DTrace.enabled){
			DTrace.SLOT_READ.logLength(getID(), slot);
		}
		return ((LocalObjectContainer)trans.container()).readBufferBySlot(slot);
	}

}
