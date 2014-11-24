/* Copyright (C) 2004 - 2006 Versant Inc. http://www.db4o.com */

package com.db4o.db4ounit.common.ta;

import com.db4o.activation.*;

import db4ounit.mocking.*;

public class MockActivatable implements com.db4o.ta.Activatable {
	
	private transient MethodCallRecorder _recorder;
	
	public MethodCallRecorder recorder() {
		if (null == _recorder) {
			_recorder = new MethodCallRecorder();
		}
		return _recorder;
	}

	public void bind(Activator activator) {
		record(new MethodCall("bind", activator));
	}
	
	public void activate(ActivationPurpose purpose) {
		/* need to call with explicit Object[] so it sharpens correctly */
		record(new MethodCall("activate", new Object[] { purpose }));
	}
	
	private void record(MethodCall methodCall) {
		recorder().record(methodCall);
	}
}
