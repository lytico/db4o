/* Copyright (C) 2004 - 2006 Versant Inc. http://www.db4o.com */

package com.db4o.internal.activation;

public abstract class ActivationDepthImpl implements ActivationDepth {
	
	protected final ActivationMode _mode;

	protected ActivationDepthImpl(ActivationMode mode) {
		_mode = mode;
	}
	
	public ActivationMode mode() {
		return _mode;
	}
}
