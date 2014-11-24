/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.ta.ta;

import com.db4o.activation.*;
import com.db4o.db4ounit.common.ta.*;

/**
 * @exclude
 */
public class TALinkedList extends ActivatableImpl {
	
	public static TALinkedList newList(int depth) {
		if (depth == 0) {
			return null;
		}
		TALinkedList head = new TALinkedList(depth);
		head.next = newList(depth - 1);
		return head;
	}

	public TALinkedList nextN(int depth) {
		TALinkedList node = this;
		for (int i = 0; i < depth; ++i) {
			node = node.next();
		}
		return node;
	}
	
	public TALinkedList next;
	
	public int value;

	public TALinkedList(int v) {
		value = v;
	}

	public int value() {
		activate(ActivationPurpose.READ);
		return value;
	}
	
	public TALinkedList next() {
		activate(ActivationPurpose.READ);
		return next;
	}
	
	public boolean equals(Object other) {
		activate(ActivationPurpose.READ);
		TALinkedList otherList = (TALinkedList) other;
		if( value != otherList.value()) {
			return false;
		}
		if(next == otherList.next()) {
			return true;
		}
		if(otherList.next() == null) {
			return false;
		}
		return next.equals(otherList.next());
	}
}
