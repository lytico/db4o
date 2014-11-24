/* Copyright (C) 2006   Versant Inc.   http://www.db4o.com */

package com.db4o.internal.fieldindex;

import com.db4o.foundation.*;
import com.db4o.internal.*;
import com.db4o.internal.query.processor.*;

public class IndexedPath extends IndexedNodeBase {
	
	public static IndexedNode newParentPath(IndexedNode next, QCon constraint) {
		if (!canFollowParent(constraint)) {
			return null;
		}
		return new IndexedPath((QConObject) constraint.parent(), next);
	}	
	
	private static boolean canFollowParent(QCon con) {
		final QCon parent = con.parent();
		final FieldMetadata parentField = getYapField(parent);
		if (null == parentField) return false;
		final FieldMetadata conField = getYapField(con);
		if (null == conField) return false;
		return parentField.hasIndex() &&
		    parentField.fieldType().isAssignableFrom(conField.containingClass());
	}
	
	private static FieldMetadata getYapField(QCon con) {
		QField field = con.getField();
		if (null == field) return null;
		return field.getFieldMetadata();
	}
	
	private IndexedNode _next;

	public IndexedPath(QConObject parent, IndexedNode next) {
		super(parent);
		_next = next;
	}
	
	public Iterator4 iterator() {		
		return new IndexedPathIterator(this, _next.iterator());
	}

	public int resultSize() {
		throw new NotSupportedException();
	}
	
	public void markAsBestIndex(QCandidates candidates) {
		_constraint.setProcessedByIndex(candidates);
		_next.markAsBestIndex(candidates);
	}
	
	public boolean isEmpty(){
		throw new NotSupportedException();
	}


}
