/* Copyright (C) 2004 - 2005  db4objects Inc.  http://www.db4o.com */

package com.db4o.inside.freespace;

import com.db4o.*;
import com.db4o.foundation.*;
import com.db4o.inside.ix.*;


abstract class FreespaceIx {
    
    Index4 _index;
    
    IndexTransaction _indexTrans;
    
    IxTraverser _traverser;
    
    FreespaceVisitor _visitor;
    
    FreespaceIx(YapFile file, MetaIndex metaIndex){
        _index = new Index4(file.getSystemTransaction(),new YInt(file), metaIndex, false);
        _indexTrans = _index.globalIndexTransaction();
    }
    
    abstract void add(int address, int length);
    
    abstract int address();
    
    public void debug(){
        if(Debug.freespace){
            IxTree tree = (IxTree) _indexTrans.getRoot();
            if(tree != null){
                tree.traverse(new Visitor4(){
                    public void visit(Object obj) {
                        System.out.println(obj);
                    }
                });
            }
        }
    }
    
    void find (int val){
        _traverser = new IxTraverser();
        _traverser.findBoundsExactMatch(new Integer(val), (IxTree)_indexTrans.getRoot());
    }
    
    abstract int length();
    
    boolean match(){
        _visitor = new FreespaceVisitor();
        _traverser.visitMatch(_visitor);
        return _visitor.visited();
    }
    
    boolean preceding(){
        _visitor = new FreespaceVisitor();
        _traverser.visitPreceding(_visitor);
        return _visitor.visited();
    }
    
    abstract void remove(int address, int length);
    
    boolean subsequent(){
        _visitor = new FreespaceVisitor();
        _traverser.visitSubsequent(_visitor);
        return _visitor.visited();
    }

}
