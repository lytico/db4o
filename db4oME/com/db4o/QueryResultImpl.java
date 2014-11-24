/* Copyright (C) 2004   db4objects Inc.   http://www.db4o.com */

package com.db4o;

import com.db4o.foundation.*;
import com.db4o.inside.query.*;
import com.db4o.query.*;

/**
 * @exclude
 */
class QueryResultImpl extends IntArrayList implements Visitor4, QueryResult {
    
	Tree i_candidates;
	boolean i_checkDuplicates;
    
	final Transaction i_trans;

	QueryResultImpl(Transaction a_trans) {
		i_trans = a_trans;
	}
    
    QueryResultImpl(Transaction trans, int initialSize){
        super(initialSize);
        i_trans = trans;
    }

	final Object activate(Object obj){
		YapStream stream = i_trans.i_stream;
		stream.beginEndActivation();
		stream.activate2(i_trans, obj, stream.i_config.activationDepth());
		stream.beginEndActivation();
		return obj;
	}
    
    /* (non-Javadoc)
     * @see com.db4o.QueryResult#get(int)
     */
    public Object get(int index) {
        synchronized (streamLock()) {
            if (index < 0 || index >= size()) {
                throw new IndexOutOfBoundsException();
            }
            int id = i_content[index];
            YapStream stream = i_trans.i_stream;
            Object obj = stream.getByID(id);
            if(obj == null){
                return null;
            }
            return activate(obj);
        }
    }

	final void checkDuplicates(){
		i_checkDuplicates = true;
	}

	/* (non-Javadoc)
     * @see com.db4o.QueryResult#getIDs()
     */
	public long[] getIDs() {
		synchronized (streamLock()) {
		    return asLong();
		}
	}

	/* (non-Javadoc)
     * @see com.db4o.QueryResult#hasNext()
     */
	public boolean hasNext() {
		synchronized (streamLock()) {
			return super.hasNext();
		}
	}

	/* (non-Javadoc)
     * @see com.db4o.QueryResult#next()
     */
	public Object next() {
		synchronized (streamLock()) {
			YapStream stream = i_trans.i_stream;
			stream.checkClosed();
			if (super.hasNext()) {
				Object ret = stream.getByID2(i_trans, nextInt());
				if (ret == null) {
					return next();
				}
				return activate(ret);
			}
			return null;
		}
	}

	/* (non-Javadoc)
     * @see com.db4o.QueryResult#reset()
     */
	public void reset() {
		synchronized (streamLock()) {
		    super.reset();
		}
	}

	public void visit(Object a_tree) {
		QCandidate candidate = (QCandidate) a_tree;
		if (candidate.include()) {
		    addKeyCheckDuplicates(candidate._key);
		}
	}
	
	void addKeyCheckDuplicates(int a_key){
	    if(i_checkDuplicates){
	        TreeInt newNode = new TreeInt(a_key);
	        i_candidates = Tree.add(i_candidates, newNode);
	        if(newNode._size == 0){
	            return;
	        }
	    }
	    
	    // TODO: It would be more efficient to hold TreeInts
	    // here only but it won't work, in case an ordering
	    // is applied. Modify to hold a tree here, in case
	    // there is no ordering.
	    
	    add(a_key);
	    
	}
	
	public Object streamLock(){
		return i_trans.i_stream.i_lock;
	}

    public ObjectContainer objectContainer() {
        return i_trans.i_stream;
    }

	public void sort(QueryComparator cmp) {
		sort(cmp,0,size()-1);
		reset();
	}

	private void sort(QueryComparator cmp,int from,int to) {
		if(to-from<1) {
			return;
		}
		Object pivot=get(to);
		int left=from;
		int right=to;
		while(left<right) {
			while(left<right&&cmp.compare(pivot,get(left))<0) {
				left++;
			}
			while(left<right&&cmp.compare(pivot,get(right))>=0) {
				right--;
			}
			swap(left, right);
		}
		swap(to, right);
		sort(cmp,from,right-1);
		sort(cmp,right+1,to);
	}

	private void swap(int left, int right) {
		if(left!=right) {
			int swap=i_content[left];
			i_content[left]=i_content[right];
			i_content[right]=swap;
		}
	}
}
