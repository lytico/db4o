/* Copyright (C) 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.internal.query.result;

import com.db4o.foundation.*;
import com.db4o.internal.*;
import com.db4o.internal.query.processor.*;


/**
 * @exclude
 */
public class SnapShotQueryResult extends AbstractLateQueryResult {
	
	public SnapShotQueryResult(Transaction transaction) {
		super(transaction);
	}
	
	public void loadFromClassIndex(final ClassMetadata clazz) {
		createSnapshot(classIndexIterable(clazz)); 
	}

	public void loadFromClassIndexes(final ClassMetadataIterator classCollectionIterator) {
		createSnapshot(classIndexesIterable(classCollectionIterator));
	}
	
	public void loadFromQuery(final QQuery query) {
		final Iterator4 _iterator = query.executeSnapshot();
		_iterable = new Iterable4() {
			public Iterator4 iterator() {
				_iterator.reset();
				return _iterator;
			}
		}; 
	}
	
	private void createSnapshot(Iterable4 iterable) {
		final Tree ids = TreeInt.addAll(null, new IntIterator4Adaptor(iterable));
		_iterable = new Iterable4() {
			public Iterator4 iterator() {
				return new TreeKeyIterator(ids);
			}
		
		};
	}

}
