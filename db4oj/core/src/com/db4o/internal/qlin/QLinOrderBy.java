/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

package com.db4o.internal.qlin;

import static com.db4o.qlin.QLinSupport.*;

import com.db4o.qlin.*;
import com.db4o.query.*;

/**
 * @exclude
 */
public class QLinOrderBy<T> extends QLinSubNode<T>{
	
	private final Query _node;

	public QLinOrderBy(QLinRoot<T> root, Object expression, QLinOrderByDirection direction) {
		super(root);
		_node = root.descend(expression);
		if(direction == ascending()){
			_node.orderAscending();	
		} else {
			_node.orderDescending();
		}
	}

}
