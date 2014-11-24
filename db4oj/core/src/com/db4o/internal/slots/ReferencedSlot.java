/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.internal.slots;

import com.db4o.foundation.*;
import com.db4o.internal.*;

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

	public Tree free(LocalObjectContainer file, Tree treeRoot, Slot slot) {
		file.free(_slot.address(), _slot.length());
		if (removeReferenceIsLast()) {
			if(treeRoot != null){
				return treeRoot.removeNode(this);
			}
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

    public Slot slot() {
        return _slot;
    }

}
