/* Copyright (C) 2004   db4objects Inc.   http://www.db4o.com */
package com.db4o;

import com.db4o.foundation.*;

/**
 * @exclude
 */
public class TreeIntWeakObject extends TreeIntObject {

	public TreeIntWeakObject(int key) {
		super(key);
	}

	public TreeIntWeakObject(int key, Object obj) {
		super(key, Platform4.createWeakReference(obj));
	}

	public Object shallowClone() {
        return shallowCloneInternal(new TreeIntWeakObject(_key)); 
	}
    
    protected Tree shallowCloneInternal(Tree tree) {
        TreeIntWeakObject tiwo = (TreeIntWeakObject) super.shallowCloneInternal(tree);
        tiwo.setObject(getObject());
        return tiwo;
    }

	public Object getObject() {
		return Platform4.weakReferenceTarget(_object);
	}

	public void setObject(Object obj) {
		_object = Platform4.createWeakReference(obj);
	}

	public final TreeIntWeakObject traverseRemoveEmpty(final Visitor4 visitor) {
		if (_preceding != null) {
			_preceding = ((TreeIntWeakObject) _preceding)
					.traverseRemoveEmpty(visitor);
		}
		if (_subsequent != null) {
			_subsequent = ((TreeIntWeakObject) _subsequent)
					.traverseRemoveEmpty(visitor);
		}
		Object referent = Platform4.weakReferenceTarget(_object);
		if (referent == null) {
			return (TreeIntWeakObject) remove();
		}
		visitor.visit(referent);
		calculateSize();
		return this;
	}

}
