/* Copyright (C) 2010  Versant Inc.   http://www.db4o.com */
package com.db4o.internal.activation;

import com.db4o.internal.*;

public abstract class UnspecifiedUpdateDepth implements UpdateDepth {

	protected UnspecifiedUpdateDepth() {
	}

	public boolean sufficientDepth() {
		return true;
	}
	
	public boolean negative() {
		return true;
	}

	@Override
	public String toString() {
		return getClass().getName();
	}

	public UpdateDepth adjust(ClassMetadata clazz) {
        FixedUpdateDepth depth = (FixedUpdateDepth) forDepth(clazz.updateDepthFromConfig()).descend();
		return depth;
	}
	
	public UpdateDepth adjustUpdateDepthForCascade(boolean isCollection) {
		throw new IllegalStateException();
	}

	public UpdateDepth descend() {
		throw new IllegalStateException();
	}
	
	protected abstract FixedUpdateDepth forDepth(int depth);
}
