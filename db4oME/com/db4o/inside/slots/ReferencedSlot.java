/* Copyright (C) 2004 - 2006  db4objects Inc.  http://www.db4o.com */

package com.db4o.inside.slots;

import com.db4o.*;

/**
 * @exclude
 */
public class ReferencedSlot extends TreeInt {

	private Slot _slot;

	private int _references;

	public ReferencedSlot(int a_key) {
		super(a_key);
	}

	public Object shallowClone() {
		ReferencedSlot rs = new ReferencedSlot(_key);
		rs._slot = _slot;
		rs._references = _references;
		return super.shallowCloneInternal(rs);
	}

	public void pointTo(Slot slot) {
		_slot = slot;
	}

	public Tree free(YapFile file, Tree treeRoot, Slot slot) {
		file.free(_slot._address, _slot._length);
		if (removeReferenceIsLast()) {
			return treeRoot.removeNode(this);
		}
		pointTo(slot);
		return treeRoot;
	}

	public boolean addReferenceIsFirst() {
		_references++;
		return (_references == 1);
	}

	public boolean removeReferenceIsLast() {
		_references--;
		return _references < 1;
	}

}
