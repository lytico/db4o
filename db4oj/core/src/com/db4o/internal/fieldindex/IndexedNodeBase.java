/* Copyright (C) 2006   Versant Inc.   http://www.db4o.com */

package com.db4o.internal.fieldindex;

import com.db4o.foundation.*;
import com.db4o.internal.*;
import com.db4o.internal.btree.*;
import com.db4o.internal.query.processor.*;

public abstract class IndexedNodeBase  implements IndexedNode {
	
	protected final QConObject _constraint;

	public IndexedNodeBase(QConObject qcon) {
		if (null == qcon) {
			throw new ArgumentNullException();
		}
		if (null == qcon.getField()) {
			throw new IllegalArgumentException();
		}
        _constraint = qcon;
	}

	public final BTree getIndex() {
	    return fieldMetadata().getIndex(transaction());
	}

	private FieldMetadata fieldMetadata() {
	    return _constraint.getField().getFieldMetadata();
	}

	public QCon constraint() {
	    return _constraint;
	}

	public boolean isResolved() {
		final QCon parent = constraint().parent();
		return null == parent || !parent.hasParent();
	}

	public BTreeRange search(final Object value) {
		return fieldMetadata().search(transaction(), value);
	}

	public static void traverse(final IndexedNode node, IntVisitor visitor) {
	    Iterator4 i = node.iterator();
		while (i.moveNext()) {
		    FieldIndexKey composite = (FieldIndexKey)i.current();
		    visitor.visit(composite.parentID());
		}
	}
	
	public void traverse(IntVisitor visitor){
		traverse(this, visitor);
	}

	public IndexedNode resolve() {
		if (isResolved()) {
			return null;
		}
		return IndexedPath.newParentPath(this, constraint());
	}

	private Transaction transaction() {
		return constraint().transaction();
	}
	
	@Override
	public String toString() {
		return "IndexedNode " + _constraint.toString(); 
	}

	
	

}