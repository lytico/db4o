/* Copyright (C) 2010  Versant Inc.   http://www.db4o.com */
package com.db4o.internal.activation;

import com.db4o.internal.*;

public interface UpdateDepth {
	boolean sufficientDepth();
	boolean negative();
	UpdateDepth adjust(ClassMetadata clazz);
	UpdateDepth adjustUpdateDepthForCascade(boolean isCollection);
	UpdateDepth descend();
	boolean canSkip(ObjectReference ref);
}
