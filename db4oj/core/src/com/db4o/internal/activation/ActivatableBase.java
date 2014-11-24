package com.db4o.internal.activation;

import com.db4o.activation.*;
import com.db4o.ta.*;

public abstract class ActivatableBase implements Activatable {

	private transient Activator _activator;

	public void bind(Activator activator) {
    	if (_activator == activator) {
    		return;
    	}
    	if (activator != null && _activator != null) {
            throw new IllegalStateException();
        }

		_activator = activator;
	}
	
	public void activate(ActivationPurpose purpose) {
		if (_activator == null) return;
		_activator.activate(purpose);
	}

	protected void activateForRead() {
		activate(ActivationPurpose.READ);
	}

	protected void activateForWrite() {
		activate(ActivationPurpose.WRITE);
	}
}
