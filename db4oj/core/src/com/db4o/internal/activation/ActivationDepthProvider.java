/* Copyright (C) 2004 - 2006 Versant Inc. http://www.db4o.com */

package com.db4o.internal.activation;

import com.db4o.internal.*;

/**
 * Factory for ActivationDepth strategies.
 */
public interface ActivationDepthProvider {

	/**
	 * Returns an ActivationDepth suitable for the specified class and activation mode.
	 * 
	 * @param classMetadata root class that's being activated
	 * @param mode activation mode
	 * @return an appropriate ActivationDepth for the class and activation mode
	 */
	ActivationDepth activationDepthFor(ClassMetadata classMetadata, ActivationMode mode);

	/**
	 * Returns an ActivationDepth that will activate at most *depth* levels.
	 * 
	 * A special case is Integer.MAX_VALUE (int.MaxValue for .net) for which a
	 * FullActivationDepth object must be returned.
	 * 
	 * @param depth 
	 * @param mode
	 * 
	 * @return
	 */
	ActivationDepth activationDepth(int depth, ActivationMode mode);
}
