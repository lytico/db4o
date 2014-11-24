/* Copyright (C) 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.foundation;

/**
 * @exclude
 */
public abstract class AbstractTreeIterator implements Iterator4 {
	
	private final Tree	_tree;

	private Stack4 _stack;

	public AbstractTreeIterator(Tree tree) {
		_tree = tree;
	}

	public Object current() {
		if(_stack == null){
			throw new IllegalStateException();
		}
		Tree tree = peek();
		if(tree == null){
			return null;
		}
		return currentValue(tree);
	}
	
	private Tree peek(){
		return (Tree) _stack.peek();
	}

	public void reset() {
		_stack = null;
	}

	public boolean moveNext() {
		if(_stack == null){
			initStack();
			return _stack != null;
		}
		
		Tree current = peek();
		if(current == null){
			return false;
		}
		
		if(pushPreceding(current._subsequent)){
			return true;
		}
		
		while(true){
			_stack.pop();
			Tree parent = peek();
			if(parent == null){
				return false;
			}
			if(current == parent._preceding){
				return true;
			}
			current = parent;
		}
	}

	private void initStack() {
		if(_tree == null){
			return;
		}
		_stack = new Stack4();
		pushPreceding(_tree);
	}

	private boolean pushPreceding(Tree node) {
		if(node == null){
			return false;
		}
		while (node != null) {
			_stack.push(node);
			node = node._preceding;
		}
		return true;
	}

	protected abstract Object currentValue(Tree tree);
}
