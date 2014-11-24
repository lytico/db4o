/* Copyright (C) 2009  Versant Corp.  http://www.db4o.com */

package com.db4o.internal.metadata;

import com.db4o.internal.*;

/**
 * @exclude
 */
public interface TraverseAspectCommand {

	int declaredAspectCount(ClassMetadata classMetadata);

	boolean cancelled();

	void processAspectOnMissingClass(ClassAspect aspect, int currentSlot);

	void processAspect(ClassAspect aspect, int currentSlot);

}