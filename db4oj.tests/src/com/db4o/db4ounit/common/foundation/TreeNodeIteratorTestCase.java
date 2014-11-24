/* Copyright (C) 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.foundation;

import com.db4o.foundation.*;
import com.db4o.internal.*;

import db4ounit.*;


public class TreeNodeIteratorTestCase implements TestCase {

	public static void main(String[] args) {
		new ConsoleTestRunner(TreeNodeIteratorTestCase.class).run(); 
	}
	
	private static int[] VALUES = new int[]{1, 3, 5, 7, 9, 10, 11, 13, 24, 76};
	
	public void testIterate(){
		for (int i = 1; i <= VALUES.length; i++) {
			assertIterateValues(VALUES, i);
		}
	}
	
	public void testMoveNextAfterCompletion(){
		Iterator4 i = new TreeNodeIterator(createTree(VALUES));
		while(i.moveNext()){
			
		}
		Assert.isFalse(i.moveNext());
	}
	
	private void assertIterateValues(int[] values, int count) {
		int[] testValues = new int[count];
		System.arraycopy(values, 0, testValues, 0, count);
		assertIterateValues(testValues);
	}

	private void assertIterateValues(int[] values) {
		Tree tree = createTree(VALUES);
		final Iterator4 i = new TreeNodeIterator(tree); 
		tree.traverse(new Visitor4() {
			public void visit(Object obj) {
				i.moveNext();
				Assert.areSame(obj, i.current());
			}
		});
	}
	
	private Tree createTree(int[] values){
		Tree tree = new TreeInt(values[0]);
		for (int i = 1; i < values.length; i++) {
			tree = tree.add(new TreeInt(values[i]));
		}
		return tree;
	}
	
	public void testEmpty(){
		Iterator4 i = new TreeNodeIterator(null);
		Assert.isFalse(i.moveNext());
	}


}
