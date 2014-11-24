package com.db4o.internal.activation;

import com.db4o.internal.*;

public class UnknownActivationDepth implements ActivationDepth {
	
	public static final ActivationDepth INSTANCE = new UnknownActivationDepth();
	
	private UnknownActivationDepth() {
	}
	
	public ActivationMode mode() {
		throw new IllegalStateException();
	}

	public ActivationDepth descend(ClassMetadata metadata) {
		throw new IllegalStateException();
	}

	public boolean requiresActivation() {
		throw new IllegalStateException();
	}

}
