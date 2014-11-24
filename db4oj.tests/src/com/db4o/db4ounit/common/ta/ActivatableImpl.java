/* Copyright (C) 2004 - 2006 Versant Inc. http://www.db4o.com */

package com.db4o.db4ounit.common.ta;

import com.db4o.activation.*;

public class ActivatableImpl /* TA BEGIN */ implements com.db4o.ta.Activatable /* TA END */{

	// TA BEGIN
	private transient Activator _activator;
	// TA END

	//	 TA BEGIN
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
	
	/**
	 * @sharpen.ignore 
	 */
	protected Object clone() throws CloneNotSupportedException {
		// clone must remember to reset the _activator field
		final ActivatableImpl clone = (ActivatableImpl)super.clone();
		clone._activator = null;
		return clone;
	}
	// TA END
}
