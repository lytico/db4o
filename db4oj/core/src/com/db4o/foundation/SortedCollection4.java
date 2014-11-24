/* Copyright (C) 2006 Versant Inc. http://www.db4o.com */

package com.db4o.foundation;

/**
 * @exclude
 */
public class SortedCollection4 {
	
	private final Comparison4 _comparison;
	private Tree _tree;

	public SortedCollection4(Comparison4 comparison) {
		if (null == comparison) {
			throw new ArgumentNullException();
		}
		_comparison = comparison;
		_tree = null;
	}
	
	public Object singleElement() {
		if (1 != size()) {
			throw new IllegalStateException();
		}
		return _tree.key();
	}
	
	public void addAll(Iterator4 iterator) {		
		while (iterator.moveNext()) {
			add(iterator.current());
		}		
	}

	public void add(Object element) {
		_tree = Tree.add(_tree, new TreeObject(element, _comparison));
	}	

	public void remove(Object element) {
		_tree = Tree.removeLike(_tree, new TreeObject(element, _comparison));
	}

	public Object[] toArray(final Object[] array) {
		Tree.traverse(_tree, new Visitor4() {
			int i = 0;
			public void visit(Object obj) {
				array[i++] = ((TreeObject)obj).key();
			}
		});
		return array;
	}
	
	public int size() {
		return Tree.size(_tree);
	}
	
}
