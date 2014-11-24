/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.exceptions;

import com.db4o.db4ounit.common.assorted.*;
import com.db4o.db4ounit.common.exceptions.ExceptionSimulatingStorage.*;
import com.db4o.foundation.*;
import com.db4o.internal.*;
import com.db4o.internal.ids.*;
import com.db4o.internal.slots.*;

public class ExceptionSimulatingIdSystem extends DelegatingIdSystem{
	
	private final ExceptionFactory _exceptionFactory;
	
	private final ExceptionTriggerCondition _triggerCondition = new ExceptionTriggerCondition();

	public ExceptionSimulatingIdSystem(LocalObjectContainer container, ExceptionFactory exceptionFactory) {
		super(container);
		_exceptionFactory = exceptionFactory;
	}
	
	private void resetShutdownState() {
		_triggerCondition._isClosed = false;
	}

	public void triggerException(boolean exception) {
		resetShutdownState();
		_triggerCondition._triggersException = exception;
	}

	public boolean triggersException() {
		return this._triggerCondition._triggersException;
	}

	public boolean isClosed() {
		return _triggerCondition._isClosed;
	}
	
	@Override
	public Slot committedSlot(int id) {
		if (triggersException()) {
			_exceptionFactory.throwException();
		}
		return super.committedSlot(id);
	}
	
	@Override
	public int newId() {
		if (triggersException()) {
			_exceptionFactory.throwException();
		}
		return super.newId();
	}
	
	@Override
	public void close() {
		super.close();
		if(triggersException()) {
			_exceptionFactory.throwOnClose();
		}
	}
	
	@Override
	public void completeInterruptedTransaction(int transactionId1,
			int transactionId2) {
		if (triggersException()) {
			_exceptionFactory.throwException();
		}
		super.completeInterruptedTransaction(transactionId1, transactionId2);
	}
	
	@Override
	public void commit(Visitable<SlotChange> slotChanges, FreespaceCommitter freespaceCommitter) {
		if (triggersException()) {
			_exceptionFactory.throwException();
		}
		super.commit(slotChanges, freespaceCommitter);
	}
	

}
