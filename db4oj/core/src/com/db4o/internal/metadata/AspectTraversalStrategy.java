/* Copyright (C) 2009  Versant Corp.  http://www.db4o.com */

package com.db4o.internal.metadata;

/**
 * @exclude
 */
public interface AspectTraversalStrategy {

	void traverseAllAspects(TraverseAspectCommand command);

}