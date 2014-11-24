/* Copyright (C) 2004   db4objects Inc.   http://www.db4o.com */

package com.db4o.inside.ix;

import com.db4o.*;
import com.db4o.foundation.*;

/**
 * Index root holder for a field and a transaction.
 * @exclude 
 */
public class IndexTransaction implements Visitor4{
	
    final Index4 i_index;
	final Transaction i_trans;
	int i_version;
	private Tree i_root;
	
	IndexTransaction(Transaction a_trans, Index4 a_index){
	    i_trans = a_trans;
	    i_index = a_index;
	}
	
	public boolean equals(Object obj) {
		return i_trans == ((IndexTransaction)obj).i_trans;
    }
    
    /**
     */
    public void add(int id, Object value){
        patch(new IxAdd(this, id, value));
    }
    
    public void remove(int id, Object value){
        patch(new IxRemove(this, id, value));
    }
    
    private void patch(IxPatch patch){
        i_root = Tree.add(i_root,patch);
    }

	
//	public void add(IxPatch a_patch){
//	    i_root = Tree.add(i_root, a_patch);
//	}
	
	public Tree getRoot(){
	    return i_root;
	}
	
	public void commit(){
	    i_index.commit(this);
	}
	
	public void rollback(){
	    i_index.rollback(this);
	}
	
	void merge(IndexTransaction a_ft){
	    Tree otherRoot = a_ft.getRoot();
	    if(otherRoot != null){
	        otherRoot.traverseFromLeaves(this);
	    }
	}
	
	/**
	 * Visitor functionality for merge:<br>
	 * Add 
	 */
	public void visit(Object obj){
	    if(obj instanceof IxPatch){
		    IxPatch tree = (IxPatch)obj;
		    if(tree.hasQueue()){
		        Queue4 queue = tree.detachQueue();
		        while((tree = (IxPatch)queue.next()) != null){
		            tree.detachQueue();
		            addPatchToRoot(tree);
		        }
		    }else{
		        addPatchToRoot(tree);
		    }
	    }
	}
	
	private void addPatchToRoot(IxPatch tree){
	    if(tree._version != i_version){
	        tree.beginMerge();
	        tree.handler().prepareComparison(tree.handler().comparableObject(i_trans, tree._value));
		    if(i_root == null){
		        i_root = tree;
		    } else{
		        i_root = i_root.add(tree);
		    }
	    }
	}
	
	int countLeaves(){
	    if(i_root == null){
	        return 0;
	    }
	    final int[] leaves ={0};
	    i_root.traverse(new Visitor4() {
            public void visit(Object a_object) {
                leaves[0] ++;
            }
        });
	    return leaves[0];
	}

    public void setRoot(Tree a_tree) {
        i_root = a_tree;
    }
    
    public String toString(){
        if(! Debug4.prettyToStrings){
            return super.toString();
        }
        final StringBuffer sb = new StringBuffer();
        sb.append("IxFieldTransaction ");
        sb.append(System.identityHashCode(this));
        if(i_root == null){
            sb.append("\n    Empty");
        }else{
            i_root.traverse(new Visitor4() {
                public void visit(Object a_object) {
                    sb.append("\n");
                    sb.append(a_object.toString());
                }
            });
        }
        return sb.toString();
    }

	
	
	
}
