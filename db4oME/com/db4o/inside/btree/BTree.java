/* Copyright (C) 2004 - 2006  db4objects Inc.  http://www.db4o.com */

package com.db4o.inside.btree;

import com.db4o.*;
import com.db4o.foundation.*;
import com.db4o.inside.ix.*;

/**
 * @exclude
 */
public class BTree extends YapMeta{
    
    final Indexable4 _keyHandler;
    
    final Indexable4 _valueHandler;
    
    BTreeNode _root;
   
    /**
     * All instantiated nodes are held in this tree. From here the nodes
     * are only referred to by weak references, so they can be garbage
     * collected automatically, as soon as they are no longer referenced
     * from the hard references in the BTreeNode#_children array.
     */
    private TreeIntWeakObject _nodes;  
    
    private int _size;
    
    public BTree(int id, Indexable4 keyHandler, Indexable4 valueHandler){
        _keyHandler = keyHandler;
        _valueHandler = (valueHandler == null) ? Null.INSTANCE : valueHandler;
        if(id > 0){
            setID(id);
            setStateDeactivated();
        }else{
            _root = new BTreeNode(this);
            setStateDirty();
        }
    }
    
    public void add(Transaction trans, Object value){
        trans.dirtyBTree(this);
        _keyHandler.prepareComparison(value);
        ensureActive(trans);
        Object addResult = _root.add(trans);
        if(addResult instanceof BTreeNode){
            _root = _root.newRoot(trans, (BTreeNode)addResult);
            setStateDirty();
        }
        _size ++;
    }
    
    public void commit(final Transaction trans){
        if(_nodes != null){
            _nodes = _nodes.traverseRemoveEmpty(new Visitor4() {
                public void visit(Object obj) {
                    ((BTreeNode)obj).commit(trans);
                }
            });
        }
        write(trans);
    }
    
    public void rollback(final Transaction trans){
        if(_nodes == null){
            return;
        }
        _nodes = _nodes.traverseRemoveEmpty(new Visitor4() {
            public void visit(Object obj) {
                ((BTreeNode)obj).rollback(trans);
            }
        });
    }
    
    private void ensureActive(Transaction trans){
        if(! isActive()){
            read(trans.systemTransaction());
        }
    }

    public byte getIdentifier() {
        return YapConst.BTREE;
    }
    
    public int ownLength() {
        return YapConst.OBJECT_LENGTH + YapConst.YAPINT_LENGTH + YapConst.YAPID_LENGTH;
    }
    
    BTreeNode produceNode(int id){
        TreeIntWeakObject tio = new TreeIntWeakObject(id);
        _nodes = (TreeIntWeakObject)Tree.add(_nodes, tio);
        tio = (TreeIntWeakObject)tio.duplicateOrThis();
        BTreeNode node = (BTreeNode)tio.getObject();
        if(node == null){
            node = new BTreeNode(this, id);
            tio.setObject(node);
        }
        return node;
    }
    
    void addNode(int id, BTreeNode node){
        _nodes = (TreeIntWeakObject)Tree.add(_nodes, new TreeIntWeakObject(id, node));
    }

    public void readThis(Transaction a_trans, YapReader a_reader) {
        _size = a_reader.readInt();
        _root = produceNode(a_reader.readInt());
    }
    
    public void writeThis(Transaction trans, YapReader a_writer) {
        a_writer.writeInt(_size);
        a_writer.writeIDOf(trans, _root);
    }
    
    public int size(){
        return _size;
    }
    
    public void traverseKeys(Transaction trans, Visitor4 visitor){
        if(_root == null){
            return;
        }
        _root.traverseKeys(trans, visitor);
    }


}

