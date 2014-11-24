/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.internal;

import com.db4o.foundation.*;


class PendingClassInits {
	
    private final Transaction _systemTransaction;
	
	private Collection4 _pending = new Collection4();

	private Queue4<ClassMetadata> _members = new NonblockingQueue();
	private Queue4<ClassMetadata> _statics = new NonblockingQueue();
    private Queue4<ClassMetadata> _writes = new NonblockingQueue();
    private Queue4<ClassMetadata> _inits = new NonblockingQueue();
	
	private boolean _running = false;
	
	PendingClassInits(Transaction systemTransaction){
        _systemTransaction = systemTransaction;
	}
	
	void process(ClassMetadata newClassMetadata) {
		
		if(_pending.contains(newClassMetadata)) {
			return;
		}
		
        final ClassMetadata ancestor = newClassMetadata.getAncestor();
        if (ancestor != null) {
            process(ancestor);
        }
		
		_pending.add(newClassMetadata);
        _members.add(newClassMetadata);
		
		if(_running) {
			return;
		}
		
		_running = true;
		try {
			checkInits();
			_pending = new Collection4();
		} finally {
			_running = false;
		}
	}

	
	private void initializeAspects() {
		while(_members.hasNext()) {
			ClassMetadata classMetadata = _members.next();
			classMetadata.initializeAspects();
            _statics.add(classMetadata);
		}
	}
	
	private void checkStatics() {
		initializeAspects();
		while(_statics.hasNext()) {
			ClassMetadata classMetadata = _statics.next();
			classMetadata.storeStaticFieldValues(_systemTransaction, true);
			_writes.add(classMetadata);
			initializeAspects();
		}
	}
	
	private void checkWrites() {
		checkStatics();
		while(_writes.hasNext()) {
			ClassMetadata classMetadata = _writes.next();
	        classMetadata.setStateDirty();
	        classMetadata.write(_systemTransaction);
            _inits.add(classMetadata);
			checkStatics();
		}
	}
    
    private void checkInits() {
        checkWrites();
        while(_inits.hasNext()) {
            ClassMetadata classMetadata = _inits.next();
            classMetadata.initConfigOnUp(_systemTransaction);
            checkWrites();
        }
    }


}
