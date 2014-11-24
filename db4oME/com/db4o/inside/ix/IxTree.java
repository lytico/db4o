/* Copyright (C) 2004   db4objects Inc.   http://www.db4o.com */

package com.db4o.inside.ix;

import com.db4o.*;
import com.db4o.foundation.*;
import com.db4o.inside.freespace.*;

/**
 * @exclude
 */
public abstract class IxTree extends Tree implements Visitor4{
    
    IndexTransaction _fieldTransaction;
    
    int _version;
    
    int _nodes = 1;
    
    IxTree(IndexTransaction a_ft){
        _fieldTransaction = a_ft;
        _version = a_ft.i_version;
    }
    
    public Tree add(final Tree a_new, final int a_cmp){
        if(a_cmp < 0){
            if(_subsequent == null){
                _subsequent = a_new;
            }else{
                _subsequent = _subsequent.add(a_new);
            }
        }else {
            if(_preceding == null){
                _preceding = a_new;
            }else{
                _preceding = _preceding.add(a_new);
            }
        }
        return balanceCheckNulls();
    }
    
    void beginMerge(){
        _preceding = null;
        _subsequent = null;
        setSizeOwn();
    }
    
    public Tree deepClone(Object a_param) {
		IxTree tree = (IxTree) this.shallowClone();
		tree._fieldTransaction = (IndexTransaction) a_param;
		return tree;
	}
    
    final Indexable4 handler(){
        return _fieldTransaction.i_index._handler;
    }
    
    final Index4 index(){
        return _fieldTransaction.i_index;
    }
    
    /**
     * Overridden in IxFileRange
     * Only call directly after compare() 
     */
    int[] lowerAndUpperMatch(){
        return null;
    }
    
    public final int nodes(){
        return _nodes;
    }
    
    public final void nodes(int count){
       _nodes = count;
    }
    
    public void setSizeOwn(){
        super.setSizeOwn();
        _nodes = 1;
    }
    
    public void setSizeOwnPrecedingSubsequent(){
        super.setSizeOwnPrecedingSubsequent();
        _nodes = 1 + _preceding.nodes() + _subsequent.nodes();
    }
    
    public void setSizeOwnPreceding(){
        super.setSizeOwnPreceding();
        _nodes = 1 + _preceding.nodes();
    }
    
    public void setSizeOwnSubsequent(){
        super.setSizeOwnSubsequent();
        _nodes = 1 + _subsequent.nodes();
    }
    
    public final void setSizeOwnPlus(Tree tree){
        super.setSizeOwnPlus(tree);
        _nodes = 1 + tree.nodes();
    }
    
    public final void setSizeOwnPlus(Tree tree1, Tree tree2){
        super.setSizeOwnPlus(tree1, tree2);
        _nodes = 1 + tree1.nodes() + tree2.nodes();
    }
    
    int slotLength(){
        return handler().linkLength() + YapConst.YAPINT_LENGTH;
    }
    
    final YapFile stream(){
        return trans().i_file;
    }
    
    final Transaction trans(){
        return _fieldTransaction.i_trans;
    }
    
    public abstract void visit(Object obj);
    
    public abstract void visit(Visitor4 visitor, int[] a_lowerAndUpperMatch);
    
    public abstract void visitAll(IntObjectVisitor visitor);
    
    public abstract void freespaceVisit(FreespaceVisitor visitor, int index);
    
    public abstract int write(Indexable4 a_handler, YapWriter a_writer);
    
    public void visitFirst(FreespaceVisitor visitor){
        if(_preceding != null){
            ((IxTree)_preceding).visitFirst(visitor);
            if(visitor.visited()){
                return;
            }
        }
        freespaceVisit(visitor, 0);
        if(visitor.visited()){
            return;
        }
        if(_subsequent != null){
            ((IxTree)_subsequent).visitFirst(visitor);
            if(visitor.visited()){
                return;
            }
        }
    }
    
    public void visitLast(FreespaceVisitor visitor){
        if(_subsequent != null){
            ((IxTree)_subsequent).visitLast(visitor);
            if(visitor.visited()){
                return;
            }
        }
        freespaceVisit(visitor, 0);
        if(visitor.visited()){
            return;
        }
        if(_preceding != null){
            ((IxTree)_preceding).visitLast(visitor);
            if(visitor.visited()){
                return;
            }
        }
    }
    
    protected Tree shallowCloneInternal(Tree tree) {
    	IxTree ixTree=(IxTree)super.shallowCloneInternal(tree);
    	ixTree._fieldTransaction=_fieldTransaction;
    	ixTree._version=_version;
    	ixTree._nodes=_nodes;
    	return ixTree;
    }
}
