/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

package com.db4o.internal.qlin;

import com.db4o.query.*;

/**
 * @exclude
 */
public class QLinConstraint<T> extends QLinSubNode<T>{
	
	private final Constraint _constraint;
	
	public QLinConstraint(QLinRoot<T> root, Constraint constraint) {
		super(root);
		_constraint = constraint;
	}
	
}
