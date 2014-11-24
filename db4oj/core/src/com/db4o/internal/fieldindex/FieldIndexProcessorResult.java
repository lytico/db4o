/* Copyright (C) 2006   Versant Inc.   http://www.db4o.com */

package com.db4o.internal.fieldindex;

import com.db4o.foundation.*;
import com.db4o.internal.btree.*;

public class FieldIndexProcessorResult implements IntVisitable{
	
	public static final FieldIndexProcessorResult NO_INDEX_FOUND = new FieldIndexProcessorResult(null);

	public static final FieldIndexProcessorResult FOUND_INDEX_BUT_NO_MATCH = new FieldIndexProcessorResult(null);
	
	private final IndexedNode _indexedNode;
	
	public FieldIndexProcessorResult(IndexedNode indexedNode) {
		_indexedNode = indexedNode;
	}
	
	public void traverse(IntVisitor visitor) {
		if(! foundMatch()){
			return;
		}
		_indexedNode.traverse(visitor);
	}
	
	public boolean foundMatch(){
		return foundIndex() && ! noMatch();
	}
	
	public boolean foundIndex(){
		return this != NO_INDEX_FOUND;
	}
	
	public boolean noMatch(){
		return this == FOUND_INDEX_BUT_NO_MATCH;
	}
	
	public Iterator4 iterateIDs(){
		return new MappingIterator(_indexedNode.iterator()) {
			protected Object map(Object current) {
			    FieldIndexKey composite = (FieldIndexKey)current;
				return new Integer(composite.parentID());
			}
		};
	}

	
}