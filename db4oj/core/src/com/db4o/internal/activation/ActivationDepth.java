package com.db4o.internal.activation;

import com.db4o.internal.*;

/**
 * Controls how deep an object graph is activated.
 */
public interface ActivationDepth {
	
	ActivationMode mode();
	
	boolean requiresActivation();

	ActivationDepth descend(ClassMetadata metadata);

}
