/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

package com.db4o.internal.qlin;

import com.db4o.qlin.*;
import com.db4o.query.*;

/**
 * @exclude
 */
public class QLinField<T> extends QLinSubNode<T>{
	
	private final Query _node;
	
	public QLinField(QLinRoot<T> root, Object expression){
		super(root);
		_node = root.descend(expression);
	}
	
	@Override
	public QLin<T> equal(Object obj) {
		Constraint constraint = _node.constrain(obj);
		constraint.equal();
		return new QLinConstraint<T>(_root, constraint);
	}
	
	@Override
	public QLin<T> startsWith(String string) {
		Constraint constraint = _node.constrain(string);
		constraint.startsWith(true);
		return new QLinConstraint<T>(_root, constraint);
	}
	
	@Override
	public QLin<T> smaller(Object obj) {
		Constraint constraint = _node.constrain(obj);
		constraint.smaller();
		return new QLinConstraint<T>(_root, constraint);
	}
	
	@Override
	public QLin<T> greater(Object obj) {
		Constraint constraint = _node.constrain(obj);
		constraint.greater();
		return new QLinConstraint<T>(_root, constraint);
	}


}
