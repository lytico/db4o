/* Copyright (C) 2008  Versant Inc.  http://www.db4o.com */

package com.db4o.internal.btree;

import com.db4o.foundation.*;
import com.db4o.internal.*;

/**
 * @exclude
 */
public class BTreeIterator implements Iterator4{
	
	private final Transaction _transaction;
	
	private final BTree _bTree;
	
	private BTreePointer _currentPointer;
	
	private boolean _beyondEnd;
	
	public BTreeIterator(Transaction trans, BTree bTree){
		_transaction = trans;
		_bTree = bTree;
	}

	public Object current() {
		if(_currentPointer == null){
			throw new IllegalStateException();
		}
		return _currentPointer.key();
	}

	public boolean moveNext() {
		if(_beyondEnd){
			return false;
		}
		if(beforeFirst()){
			_currentPointer = _bTree.firstPointer(_transaction);
		} else {
			_currentPointer = _currentPointer.next();	
		}
		_beyondEnd = (_currentPointer == null);
		return ! _beyondEnd;
	}

	private boolean beforeFirst() {
		return _currentPointer == null;
	}

	public void reset() {
		throw new UnsupportedOperationException();
	}

}
