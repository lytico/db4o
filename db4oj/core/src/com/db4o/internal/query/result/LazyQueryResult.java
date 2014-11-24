/* Copyright (C) 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.internal.query.result;

import com.db4o.foundation.*;
import com.db4o.internal.*;
import com.db4o.internal.query.processor.*;


/**
 * @exclude
 */
public class LazyQueryResult extends AbstractLateQueryResult {
	
	public LazyQueryResult(Transaction trans) {
		super(trans);
	}

	public void loadFromClassIndex(final ClassMetadata clazz) {
		_iterable = classIndexIterable(clazz);
	}
	
	public void loadFromClassIndexes(final ClassMetadataIterator classCollectionIterator) {
		_iterable = classIndexesIterable(classCollectionIterator);
	}
	
	public void loadFromQuery(final QQuery query) {
		_iterable = new Iterable4(){
			public Iterator4 iterator() {
				return query.executeLazy();
			}
		};
	}

}
