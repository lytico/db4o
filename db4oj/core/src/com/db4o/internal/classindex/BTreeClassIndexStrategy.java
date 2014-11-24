/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.internal.classindex;

import com.db4o.foundation.*;
import com.db4o.internal.*;
import com.db4o.internal.btree.*;
import com.db4o.internal.references.*;

/**
 * @exclude
 */
public class BTreeClassIndexStrategy extends AbstractClassIndexStrategy {
	
	private BTree _btreeIndex;
	
	public BTreeClassIndexStrategy(ClassMetadata classMetadata) {
		super(classMetadata);
	}	
	
	public BTree btree() {
		return _btreeIndex;
	}

	public int entryCount(Transaction ta) {
		return _btreeIndex != null
			? _btreeIndex.size(ta)
			: 0;
	}

	public void initialize(ObjectContainerBase stream) {
		createBTreeIndex(stream, 0);
	}

	public void purge() {
	}

	public void read(ObjectContainerBase stream, int indexID) {
		readBTreeIndex(stream, indexID);
	}

	public int write(Transaction trans) {
		if (_btreeIndex == null){
            return 0;
        } 
        _btreeIndex.write(trans);
        return _btreeIndex.getID();
	}
	
	public void traverseIds(Transaction ta, Visitor4 command) {
		if(_btreeIndex!=null) {
			_btreeIndex.traverseKeys(ta,command);
		}
	}

	private void createBTreeIndex(final ObjectContainerBase stream, int btreeID){
        if (stream.isClient()) {
        	return;
        }
        _btreeIndex = ((LocalObjectContainer)stream).createBTreeClassIndex(btreeID);
        _btreeIndex.setRemoveListener(new Visitor4() {
            public void visit(Object obj) {
            	removeId((TransactionContext)obj);
            }
        });
    }
	
    private void removeId(TransactionContext context) {
    	ReferenceSystem referenceSystem = context._transaction.referenceSystem();
        ObjectReference reference = referenceSystem.referenceForId((Integer)context._object);
        if(reference != null){
            referenceSystem.removeReference(reference);
        }
    }

	private void readBTreeIndex(ObjectContainerBase stream, int indexId) {
		if(! stream.isClient() && _btreeIndex == null){
            createBTreeIndex(stream, indexId);
		}
	}

	protected void internalAdd(Transaction trans, int id) {
		_btreeIndex.add(trans, new Integer(id));
	}

	protected void internalRemove(Transaction ta, int id) {
		_btreeIndex.remove(ta, new Integer(id));
	}

	public void dontDelete(Transaction transaction, int id) {
	}
	
	public void defragReference(ClassMetadata classMetadata, DefragmentContextImpl context,int classIndexID) {
		int newID = -classIndexID;
		context.writeInt(newID);
	}
	
	public int id() {
		return _btreeIndex.getID();
	}

	public Iterator4 allSlotIDs(Transaction trans) {
        return _btreeIndex.allNodeIds(trans);
	}

	public void defragIndex(DefragmentContextImpl context) {
		_btreeIndex.defragIndex(context);
	}
	
	public static BTree btree(ClassMetadata clazz) {
		ClassIndexStrategy index = clazz.index();
		if(! (index instanceof BTreeClassIndexStrategy)){
			throw new IllegalStateException();
		}
		return ((BTreeClassIndexStrategy)index).btree();
	}
	
	public static Iterator4 iterate(ClassMetadata clazz, Transaction trans) {
		return btree(clazz).asRange(trans).keys();
	}

	public IntVisitable idVisitable(final Transaction trans){
		return new IntVisitable() {
			public void traverse(final IntVisitor visitor) {
				traverseIds(trans, new Visitor4<Integer>() {
					public void visit(Integer i) {
						visitor.visit(i);
					}
				});				
			}
		};
	}
	
	
}
