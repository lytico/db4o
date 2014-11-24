/* Copyright (C) 2007   Versant Inc.   http://www.db4o.com */
package com.db4o.db4ounit.common.ta;

import com.db4o.activation.*;
import com.db4o.ta.*;

public class MockActivator implements Activator {
	private int _readCount;
	private int _writeCount;
	
	public MockActivator() {
	}
	
	public int count() {
		return _readCount + _writeCount;
	}

	public void activate(ActivationPurpose purpose)  {
		if (purpose == ActivationPurpose.READ) {
			++_readCount;
		} else {
			++_writeCount;
		}
	}

	public int writeCount() {
		return _writeCount;
	}
	
	public int readCount() {
		return _readCount;
	}

	public static MockActivator activatorFor(final Activatable obj) {
		MockActivator activator = new MockActivator();
		obj.bind(activator);
		return activator;
	}

}

