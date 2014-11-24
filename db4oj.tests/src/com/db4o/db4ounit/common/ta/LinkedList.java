/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.ta;

/**
 * @exclude
 */
public class LinkedList {
	
	public static LinkedList newList(int depth) {
		if (depth == 0) {
			return null;
		}
		LinkedList head = new LinkedList(depth);
		head.next = newList(depth - 1);
		return head;
	}

	public LinkedList next;
	
	public int value;

	public LinkedList(int v) {
		value = v;
	}

	public LinkedList nextN(int depth) {
		LinkedList node = this;
		for (int i = 0; i < depth; ++i) {
			node = node.next;
		}
		return node;
	}
	
	public boolean equals(Object other) {
		LinkedList otherList = (LinkedList) other;
		if( value != otherList.value) {
			return false;
		}
		if(next == otherList.next) {
			return true;
		}
		if(otherList.next == null) {
			return false;
		}
		return next.equals(otherList.next);
	}
}
