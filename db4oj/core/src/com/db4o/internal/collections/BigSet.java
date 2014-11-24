/* Copyright (C) 2008  Versant Inc.  http://www.db4o.com */

package com.db4o.internal.collections;

import java.util.*;

import com.db4o.foundation.*;
import com.db4o.internal.*;
import com.db4o.internal.activation.*;
import com.db4o.internal.btree.*;
import com.db4o.marshall.*;

/**
 * @exclude
 * @sharpen.partial
 */
@decaf.Ignore(decaf.Platform.JDK11)
public class BigSet<E> implements Set<E>, BigSetPersistence {
	
	private BTree _bTree;
	
	private Transaction _transaction;

	public BigSet(LocalObjectContainer db) {
		if(db == null){
			return;
		}
		_transaction = db.transaction();
		_bTree = bTreeManager().newBTree();
	}
	
	private ObjectContainerBase container(){
		return transaction().container();
	}

	public boolean add(E obj) {
		synchronized(lock()){
			final int id = getID(obj);
			if(id == 0){
				add(store(obj));
				return true;
			}
			if (contains(id)) {
				return false;
			}
			add(id);
			return true;
		}
	}

	private int store(E obj) {
	    return container().store(_transaction, obj, container().updateDepthProvider().unspecified(NullModifiedObjectQuery.INSTANCE));
    }

	private void add(int id) {
	    bTreeForUpdate().add(_transaction, new Integer(id));
    }

	private int getID(Object obj) {
		return (int) container().getID(obj);
	}

	/**
	 * @sharpen.ignore
	 */
	public boolean addAll(Collection<? extends E> collection) {
		final Iterable<? extends E> iterable = collection;
		return addAll(iterable);
	}

	public boolean addAll(final Iterable<? extends E> iterable) {
		boolean result = false;
	    for (E element : iterable) {
			if (add(element)) {
				result = true;
			}
		}
		return result;
    }

	public void clear() {
		synchronized(lock()){
			bTreeForUpdate().clear(transaction());
		}
	}
	
	public boolean contains(Object obj) {
		int id = getID(obj);
		if(id == 0){
			return false;
		}
		return contains(id);
	}

	private boolean contains(int id) {
		synchronized(lock()){
		    BTreeRange range = bTree().searchRange(transaction(), new Integer(id));
			return ! range.isEmpty();
		}
    }
	
	/**
	 * @sharpen.ignore
	 */
	public boolean containsAll(Collection<?> collection) {
		final Iterable<?> iterable = collection;
		for (Object element : iterable) {
			if(! contains(element)){
				return false;
			}
		}
		return true;
	}

	/**
	 * @sharpen.property
	 */
	public boolean isEmpty() {
		return size() == 0;
	}

	/**
	 * @sharpen.ignore
	 */
	public Iterator<E> iterator() {
		return new Iterator4JdkIterator(elements());
	}

	/**
	 * @sharpen.ignore
	 */
	private Iterator4 elements() {
	    return new MappingIterator(bTreeIterator()) {
			protected Object map(Object current) {
				int id = ((Integer)current).intValue();
				return element(id); 
			}
		};
    }

	private Iterator4 bTreeIterator() {
	    return new SynchronizedIterator4(bTree().iterator(transaction()), lock());
    }

	public boolean remove(Object obj) {
		synchronized(lock()){
			if(!contains(obj)){
				return false;
			}
			int id = getID(obj);
			bTreeForUpdate().remove(transaction(), new Integer(id));
			return true;
		}
	}

	/**
	 * @sharpen.ignore
	 */
	public boolean removeAll(Collection<?> collection) {
		boolean res = false;
		for (Object element : collection) {
			if(remove(element)){
				res = true;
			}
		}
		return res;
	}

	/**
	 * @sharpen.ignore
	 */
	public boolean retainAll(Collection<?> c) {
		throw new UnsupportedOperationException();
	}

	public int size() {
		synchronized(lock()){
			return bTree().size(transaction());
		}
	}

	public Object[] toArray() {
		throw new UnsupportedOperationException();
	}

	public <T> T[] toArray(T[] a) {
		throw new UnsupportedOperationException();
	}

	/* (non-Javadoc)
     * @see com.db4o.internal.collections.BigSetPersistence#write(com.db4o.marshall.WriteContext)
     */
	public void write(WriteContext context) {
		int id = bTree().getID();
		if(id == 0){
			bTree().write(systemTransaction());
		}
		context.writeInt(bTree().getID());
	}
	/* (non-Javadoc)
     * @see com.db4o.internal.collections.BigSetPersistence#read(com.db4o.marshall.ReadContext)
     */
	public void read(ReadContext context) {
		int id = context.readInt();
		if(_bTree != null){
			assertCurrentBTreeId(id);
			return;
		}
		_transaction = context.transaction();
		_bTree = bTreeManager().produceBTree(id);
	}

	private BigSetBTreeManager bTreeManager() {
	    return new BigSetBTreeManager(_transaction);
    }

	private void assertCurrentBTreeId(int id) {
	    if (id != _bTree.getID()) {
			throw new IllegalStateException();
		}
    }
	
	private Transaction transaction(){
		return _transaction;
	}
	
	private Transaction systemTransaction(){
		return container().systemTransaction();
	}

	/* (non-Javadoc)
     * @see com.db4o.internal.collections.BigSetPersistence#invalidate()
     */
	public void invalidate() {
		_bTree = null;
	}

	private BTree bTree() {
		if(_bTree == null){
			throw new IllegalStateException();
		}
		return _bTree;
	}
	
	private BTree bTreeForUpdate() {
		final BTree bTree = bTree();
		bTreeManager().ensureIsManaged(bTree);
		return bTree;
	}

	private Object element(int id) {
	    Object obj = container().getByID(transaction(), id);
	    container().activate(obj);
	    return obj;
    }
	
	private Object lock(){
		return container().lock();
	}
}
