/* Copyright (C) 2008  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.btree;

import com.db4o.foundation.*;

import db4ounit.*;
import db4ounit.data.*;

/**
 * @exclude
 */
public class BTreeIteratorTestCase extends BTreeTestCaseBase {
	
	public void testEmpty(){
		Iterator4 iterator = _btree.iterator(trans());
		Assert.isNotNull(iterator);
		Assert.isFalse(iterator.moveNext());
	}
	
	public void testOneKey(){
		_btree.add(trans(), new Integer(1));
		Iterator4 iterator = _btree.iterator(trans());
		Assert.isTrue(iterator.moveNext());
		Assert.areEqual(new Integer(1), iterator.current());
		Assert.isFalse(iterator.moveNext());
	}
	
	public void testManyKeys(){
		for (int keyCount = 50; keyCount < 70; keyCount++) {
			_btree = newBTree();
			Iterable4 keys = randomPositiveIntegersWithoutDuplicates(keyCount);
			Iterator4 keyIterator = keys.iterator();
			while(keyIterator.moveNext()){
				Integer currentKey = (Integer) keyIterator.current();
				_btree.add(trans(), currentKey);
			}
			Iterator4Assert.sameContent(keys.iterator(), _btree.iterator(trans()));
		}
	}

	private Iterable4 randomPositiveIntegersWithoutDuplicates(int keyCount) {
		Iterable4 generator = Generators.take(keyCount, Streams.randomIntegers());
		Collection4 res = new Collection4();
		Iterator4 i = generator.iterator();
		while(i.moveNext()){
			Integer currentInteger = (Integer) i.current();
			if(currentInteger.intValue() < 0){
				currentInteger = new Integer(- currentInteger.intValue());
			}
			if(! res.contains(currentInteger)){
				res.add(currentInteger);
			}
		}
		return res;
	}

}
