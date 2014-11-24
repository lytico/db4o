/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.internal;

import com.db4o.foundation.*;
import com.db4o.internal.query.processor.*;


/**
 * Base class for balanced trees.
 * 
 * @exclude
 */
public class TreeInt extends Tree<Integer> implements ReadWriteable {
	
	public static TreeInt add(TreeInt tree, int value) {
		return (TreeInt) Tree.add(tree, new TreeInt(value));
	}
	
	public static TreeInt removeLike(TreeInt tree, int value) {
		return (TreeInt) Tree.removeLike(tree, new TreeInt(value));
	}
	
	public static Tree addAll(Tree tree, IntIterator4 iter){
		if(! iter.moveNext()){
			return tree;
		}
		TreeInt firstAdded = new TreeInt(iter.currentInt());
		tree = Tree.add(tree, firstAdded);
		while(iter.moveNext()){
			tree = tree.add( new TreeInt(iter.currentInt()));
		}
		return tree;
	}

	public int _key;

	public TreeInt(int a_key) {
		this._key = a_key;
	}

	public int compare(Tree a_to) {
		return _key - ((TreeInt) a_to)._key;
	}

	Tree deepClone() {
		return new TreeInt(_key);
	}

	public boolean duplicates() {
		return false;
	}

	public static final TreeInt find(Tree a_in, int a_key) {
		if (a_in == null) {
			return null;
		}
		return ((TreeInt) a_in).find(a_key);
	}

	public final TreeInt find(int a_key) {
		int cmp = _key - a_key;
		if (cmp < 0) {
			if (_subsequent != null) {
				return ((TreeInt) _subsequent).find(a_key);
			}
		} else {
			if (cmp > 0) {
				if (_preceding != null) {
					return ((TreeInt) _preceding).find(a_key);
				}
			} else {
				return this;
			}
		}
		return null;
	}

	public Object read(ByteArrayBuffer buffer) {
		return new TreeInt(buffer.readInt());
	}

	public void write(ByteArrayBuffer buffer) {
		buffer.writeInt(_key);
	}
	
	public static void write(final ByteArrayBuffer buffer, TreeInt tree){
        write(buffer, tree, tree == null ? 0 : tree.size());
	}
    
    public static void write(final ByteArrayBuffer buffer, TreeInt tree, int size){
        if(tree == null){
            buffer.writeInt(0);
            return;
        }
        buffer.writeInt(size);
        tree.traverse(new Visitor4() {
            public void visit(Object a_object) {
                ((TreeInt)a_object).write(buffer);
            }
        });
    }

	public int ownLength() {
		return Const4.INT_LENGTH;
	}

	boolean variableLength() {
		return false;
	}

	public String toString() {
		return "" + _key;
	}

	protected Tree shallowCloneInternal(Tree tree) {
		TreeInt treeint=(TreeInt)super.shallowCloneInternal(tree);
		treeint._key=_key;
		return treeint;
	}

	public Object shallowClone() {
		TreeInt treeint= new TreeInt(_key);
		return shallowCloneInternal(treeint);
	}
	
	public static int marshalledLength(TreeInt a_tree){
		if(a_tree == null){
			return Const4.INT_LENGTH;
		}
		return a_tree.marshalledLength();
	}
	
	public final int marshalledLength(){
		if(variableLength()){
			final IntByRef length = new IntByRef(Const4.INT_LENGTH);
			traverse(new Visitor4(){
				public void visit(Object obj){
					length.value += ((TreeInt)obj).ownLength();
				}
			});
			return length.value;
		}
		return marshalledLength(size());
	}
	
	public final int marshalledLength(int size){
		return Const4.INT_LENGTH + (size * ownLength());
	}
	
	@Override
    public Integer key(){
    	return new Integer(_key);
    }
	
	@Override
	public boolean equals(Object obj) {
		TreeInt other = (TreeInt) obj;
		return other._key == _key;
	}

}
