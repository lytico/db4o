package com.db4o.internal.activation;

import com.db4o.internal.*;

/**
 * Activates the full object graph.
 */
public class FullActivationDepth extends ActivationDepthImpl {

	public FullActivationDepth(ActivationMode mode) {
		super(mode);
	}
	
	public FullActivationDepth() {
		this(ActivationMode.ACTIVATE);
	}

	public ActivationDepth descend(ClassMetadata metadata) {
		return this;
	}

	public boolean requiresActivation() {
		return true;
	}

}
