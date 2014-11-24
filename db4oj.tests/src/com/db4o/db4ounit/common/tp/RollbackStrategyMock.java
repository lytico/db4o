/* Copyright (C) 2004 - 2006 Versant Inc. http://www.db4o.com */

package com.db4o.db4ounit.common.tp;

import com.db4o.*;
import com.db4o.ta.*;

import db4ounit.mocking.*;

public class RollbackStrategyMock implements RollbackStrategy {
	
	private MethodCallRecorder _recorder = new MethodCallRecorder();

	public void rollback(ObjectContainer container, Object obj) {
		_recorder.record(new MethodCall("rollback", container, obj));
	}
	
	public void verify(MethodCall[] expectedCalls) {
		_recorder.verify(expectedCalls);
	}

	public void verifyUnordered(MethodCall[] methodCalls) {
		_recorder.verifyUnordered(methodCalls);
    }
}
