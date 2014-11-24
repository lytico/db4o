package com.db4o.ta.instrumentation.test.data;

import com.db4o.activation.*;
import com.db4o.ta.*;

public class AlreadyInstrumentedSuper implements Activatable {

	private Activator _activator;
	
	public void activate(ActivationPurpose purpose) {
		if(_activator == null) {
			return;
		}
		_activator.activate(purpose);
	}

	public void bind(Activator activator) {
		if(_activator == activator) {
			return;
		}
		if(_activator != null && _activator != activator) {
			throw new IllegalStateException();
		}
		_activator = activator;
	}

}
