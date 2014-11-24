/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

package com.db4o.internal.fileheader;

import com.db4o.*;
import com.db4o.ext.*;
import com.db4o.internal.*;
import com.db4o.internal.activation.*;
import com.db4o.internal.slots.*;

/**
 * @exclude
 */
public abstract class FileHeaderVariablePart {
	
    protected final LocalObjectContainer _container;

	public abstract Runnable commit(boolean shuttingDown);

	public abstract void read(int variablePartAddress, int variablePartLength);
	
	protected FileHeaderVariablePart(LocalObjectContainer container){
		_container = container;
	}
	
    public final byte getIdentifier() {
        return Const4.HEADER;
    }
    
	protected final SystemData systemData() {
		return _container.systemData();
	}
	
	protected final Slot allocateSlot(int length) {
		Slot reusedSlot = _container.freespaceManager().allocateSafeSlot(length);
		if(reusedSlot != null){
			return reusedSlot;
		}
		return _container.appendBytes(length);
	}
	
    public void readIdentity(LocalTransaction trans) {
        LocalObjectContainer file = trans.localContainer();
        Db4oDatabase identity = Debug4.staticIdentity ? 
        		Db4oDatabase.STATIC_IDENTITY : 
        		(Db4oDatabase) file.getByID(trans, systemData().identityId());
        if (null != identity) {
        	file.activate(trans, identity, new FixedActivationDepth(2));
        	systemData().identity(identity);
        } else{
        	// TODO: What now?
        	// Apparently we get this state after defragment
        	// and defragment then sets the identity.
        	// If we blindly generate a new identity here,
        	// ObjectUpdateFileSizeTestCase reports trouble.
        }
    }
    
    public abstract int marshalledLength();

}
