/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

package com.db4o.drs.versant.cobra.qlin;

import com.db4o.internal.qlin.*;
import com.db4o.qlin.*;


public abstract class CLinCobraNode <T> extends QLinNode<T> {
	
	protected abstract CLinRoot<T> root();
	
	public QLin<T> where(Object expression) {
		return new CLinField<T>(root(), expression);
	}

	public QLin<T> orderBy(Object expression, QLinOrderByDirection direction) {
		return new CLinOrderBy<T>(root(), expression, direction);
	}

}
