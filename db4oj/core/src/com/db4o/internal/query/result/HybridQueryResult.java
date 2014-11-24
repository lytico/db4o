/* Copyright (C) 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.internal.query.result;

import com.db4o.config.*;
import com.db4o.foundation.*;
import com.db4o.internal.*;
import com.db4o.internal.query.processor.*;
import com.db4o.query.*;

/**
 * @exclude
 */
public class HybridQueryResult extends AbstractQueryResult {
	
	private AbstractQueryResult _delegate;
	
	public HybridQueryResult(Transaction transaction, QueryEvaluationMode mode) {
		super(transaction);
		_delegate = forMode(transaction, mode);
	}
	
	private static AbstractQueryResult forMode(Transaction transaction, QueryEvaluationMode mode){
		if(mode == QueryEvaluationMode.LAZY){
			return new LazyQueryResult(transaction); 
		}
		if(mode == QueryEvaluationMode.SNAPSHOT){
			return new SnapShotQueryResult(transaction); 
		}
		return new IdListQueryResult(transaction);
	}

	public Object get(int index) {
		_delegate = _delegate.supportElementAccess();
		return _delegate.get(index);
	}
	
	public int getId(int index) {
		_delegate = _delegate.supportElementAccess();
		return _delegate.getId(index);
	}

	public int indexOf(int id) {
		_delegate = _delegate.supportElementAccess();
		return _delegate.indexOf(id);
	}

	public IntIterator4 iterateIDs() {
		return _delegate.iterateIDs();
	}
	
	public Iterator4 iterator() {
		return _delegate.iterator();
	}

	public void loadFromClassIndex(ClassMetadata clazz) {
		_delegate.loadFromClassIndex(clazz);
	}

	public void loadFromClassIndexes(ClassMetadataIterator iterator) {
		_delegate.loadFromClassIndexes(iterator);
	}

	public void loadFromIdReader(Iterator4 reader) {
		_delegate.loadFromIdReader(reader);
	}

	public void loadFromQuery(QQuery query) {
		if(query.requiresSort()){
			_delegate = new IdListQueryResult(transaction());
		}
		_delegate.loadFromQuery(query);
	}

	public int size() {
		_delegate = _delegate.supportSize();
		return _delegate.size();
	}

	public void sort(QueryComparator cmp) {
		_delegate = _delegate.supportSort();
		_delegate.sort(cmp);
	}
	
	public void sortIds(IntComparator cmp) {
		_delegate = _delegate.supportSort();
		_delegate.sortIds(cmp);
	}

	public void skip(int count) {
		_delegate.skip(count);
	}
}
