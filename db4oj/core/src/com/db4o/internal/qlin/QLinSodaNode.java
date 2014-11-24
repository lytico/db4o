/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

package com.db4o.internal.qlin;

import com.db4o.qlin.*;

/**
 * @exclude
 */
public abstract class QLinSodaNode <T> extends QLinNode<T> {
	
	protected abstract QLinRoot<T> root();
	
	public QLin<T> where(Object expression) {
		return new QLinField<T>(root(), expression);
	}
	
	public QLin<T> orderBy(Object expression, QLinOrderByDirection direction) {
		return new QLinOrderBy<T>(root(), expression, direction);
	}

}
