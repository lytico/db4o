/* Copyright (C) 2004 - 2006  db4objects Inc.  http://www.db4o.com */

package com.db4o.inside.btree;

import com.db4o.*;
import com.db4o.foundation.*;
import com.db4o.inside.ix.*;

/**
 * We work with BTreeNode in two states:
 * 
 * - deactivated: never read, no valid members, ID correct or 0 if new
 * - write: real representation of keys, values and children in arrays
 * The write state can be detected with canWrite(). States can be changed
 * as needed with prepareRead() and prepareWrite().
 * 
 * @exclude
 */
public class BTreeNode extends YapMeta{
    
    private static final int MAX_ENTRIES = 4;

    private static final int HALF_ENTRIES = MAX_ENTRIES / 2;
    
    private static final int SLOT_LEADING_LENGTH = YapConst.OBJECT_LENGTH +  YapConst.YAPINT_LENGTH * 2; 

    
    final BTree _btree;
    
    
    private int _count;
    
    private int _height;
    
    
    private Object[] _keys;
    
    /**
     * Can contain BTreeNode or Integer for ID of BTreeNode 
     */
    private Object[] _children;  
    
    /**
     * Only used for leafs where _height == 0
     */
    private Object[] _values;
    
    
    /* Constructor for new nodes */
    public BTreeNode(BTree btree){
        _btree = btree;
        setStateClean();
    }
    
    /* Constructor for existing nodes, requires valid ID */
    public BTreeNode(BTree btree, int id){
        _btree = btree;
        setID(id);
        setStateDeactivated();
    }
    
    
    /**
     * @return a split node if the node is split
     * or the first key, if the first key has changed
     */
    public Object add(Transaction trans){
        
        YapReader reader = prepareRead(trans);
        
        Searcher s = search(trans, reader);
        if(s._cursor < 0){
            s._cursor = 0;
        }
        
        if(isLeaf()){
            
            prepareWrite(trans);
            

            // TODO: Anything special on exact match?  Possibly compare value part also?
            
//            if(s._cmp == 0){
//                
//            }
                
            
            // Check last comparison result and position beyond last
            // if added is greater.
            if(s._cmp < 0){
                s._cursor ++;
            }
            insert(trans, s._cursor);
            _keys[s._cursor] = new BTreeAdd(trans, keyHandler().current());
            if(handlesValues()){
                _values[s._cursor] = valueHandler().current();
            }
            
        }else{
            
            Object addResult = child(reader, s._cursor).add(trans);
            if(addResult == null){
                return null;
            }
            prepareWrite(trans);
            if(addResult instanceof BTreeNode){
                BTreeNode splitChild = (BTreeNode)addResult;
                s._cursor ++;
                insert(trans, s._cursor);
                _keys[s._cursor] = splitChild._keys[0];
                _children[s._cursor] = splitChild;
            } else{
                _keys[s._cursor] = addResult;
            }
        }
        
        setStateDirty();
        if(_count == MAX_ENTRIES){
            return split(trans);
        }
        
        if(s._cursor == 0){
            return _keys[0];
        }
        
        return null;
    }
    
    private boolean canWrite(){
        return _keys != null;
    }
    
    private BTreeNode child(YapReader reader, int index){
        if( childLoaded(index) ){
            return (BTreeNode)_children[index];
        }
        BTreeNode child = _btree.produceNode(childID(reader, index));
        if(_children != null){
            _children[index] = child; 
        }
        return child;
    }
    
    private int childID(YapReader reader, int index){
        if(_children == null){
            seekChild(reader, index);
            return reader.readInt();
        }
        if(childLoaded(index)){
            return ((BTreeNode)_children[index]).getID();
        }
        return ((Integer)_children[index]).intValue();
    }
    
    
    private boolean childLoaded(int index){
        if(_children == null){
            return false;
        }
        return _children[index] instanceof BTreeNode;
    }
    
    void commit(Transaction trans){
        
        
    }
    
    private void compare(Searcher s, YapReader reader){
        Indexable4 handler = keyHandler();
        if(_keys != null){
            s.resultIs(handler.compareTo(key(s._cursor)));
        }else{
            seekKey(reader, s._cursor);
            s.resultIs(handler.compareTo(handler.readIndexEntry(reader)));
        }
    }
    
    private int entryLength(){
        int len = keyHandler().linkLength();
        if(isLeaf()){
            if(handlesValues()){
                len += valueHandler().linkLength();
            }
        }else{
            len += YapConst.YAPID_LENGTH;
        }
        return len;
    }
    
    private Object firstKey(Transaction trans){
        for (int ix = 0; ix < _count; ix++) {
            BTreePatch patch = keyPatch(ix);
            if(patch == null){
                return _keys[ix];
            }
            Object obj = patch.getObject(trans);
            if(obj != Null.INSTANCE){
                return obj;
            }
        }
        return Null.INSTANCE;
    }
    
    public byte getIdentifier() {
        return YapConst.BTREE_NODE;
    }
    
    private boolean handlesValues(){
        return _btree._valueHandler != Null.INSTANCE; 
    }
    
    private void insert(Transaction trans, int pos){
        prepareWrite(trans);
        if(pos < 0){
            pos = 0;
        }
        if(pos > _count -1){
            _count ++;
            return;
        }
        int len = _count - pos;
        System.arraycopy(_keys, pos, _keys, pos + 1, len);
        if(_values != null){
            System.arraycopy(_values, pos, _values, pos + 1, len);
        }
        if(_children != null){
            System.arraycopy(_children, pos, _children, pos + 1, len);
        }
        _count++;
    }
    
    private boolean isLeaf(){
        return _height == 0;
    }
    
    private Object key(int index){
        BTreePatch patch = keyPatch(index);
        if(patch == null){
            return _keys[index];
        }
        return patch._object;
    }
    
    private Object key(Transaction trans, YapReader reader, int index){
        if( _keys != null ){
            return key(trans, index);
        }
        seekKey(reader, index);
        return keyHandler().readIndexEntry(reader);
    }
    
    private Object key(Transaction trans, int index){
        BTreePatch patch = keyPatch(index);
        if(patch == null){
            return _keys[index];
        }
        return patch.getObject(trans);
    }
    
    private BTreePatch keyPatch(int index){
        if( _keys[index] instanceof BTreePatch){
            return (BTreePatch)_keys[index];
        }
        return null;
    }
    
    private Indexable4 keyHandler(){
        return _btree._keyHandler;
    }
    
    BTreeNode newRoot(Transaction trans, BTreeNode peer){
        BTreeNode res = new BTreeNode(_btree);
        res._height = _height + 1;
        res._count = 2;
        res.prepareWrite(trans);
        res._keys[0] = _keys[0];
        res._children[0] = this;
        res._keys[1] = peer._keys[0];
        res._children[1] = peer;
        return res;
    }
    
    public int ownLength() {
        return YapConst.OBJECT_LENGTH 
          + YapConst.YAPINT_LENGTH * 2  // height, count
          + _count * entryLength();
    }
    
    private YapReader prepareRead(Transaction trans){
        if(canWrite()){
            return null;
        }
        if(isNew()){
            return null;
        }
        
        YapReader reader = trans.i_file.readReaderByID(trans, getID());
        _count = reader.readInt();
        _height = reader.readInt();
        
        return reader;
    }

    private void prepareWrite(Transaction trans){
        if(canWrite()){
            return;
        }
        if(isNew()){
            prepareArrays();
            return;
        }
        if(! isActive()){
            prepareArrays();
            read(trans);
        }
    }
    
    private void prepareArrays(){
        _keys = new Object[MAX_ENTRIES];
        if(isLeaf()){
            if(handlesValues()){
                _values = new Object[MAX_ENTRIES];
            }
        }else{
            _children = new Object[MAX_ENTRIES];
        }
    }
    
    public void readThis(Transaction a_trans, YapReader a_reader) {
        _count = a_reader.readInt();
        _height = a_reader.readInt();
        boolean isInner = ! isLeaf();
        boolean vals = handlesValues() && isLeaf();
        for (int i = 0; i < _count; i++) {
            _keys[i] = keyHandler().readIndexEntry(a_reader);
            if(vals){
                _values[i] = valueHandler().readIndexEntry(a_reader);
            }else{
                if(isInner){
                    _children[i] = new Integer(a_reader.readInt());
                }
            }
        }
    }
    
    public BTreeNode remove(Transaction trans){
        YapReader reader = prepareRead(trans);
        Searcher s = search(trans, reader);
        if(s._cursor < 0){
            return this;
        }
        if(isLeaf()){
            if(s._cmp == 0){
                prepareWrite(trans);
                
                Object obj = _keys[s._cursor];
                
                if(obj instanceof BTreePatch){
                    
                }
                
                
                
                
            }else{
                // Check last comparison result and position beyond last
                // if added is greater.
                if(s._cmp < 0){
                    s._cursor ++;
                }
                insert(trans, s._cursor);
                _keys[s._cursor] = new BTreeAdd(trans, keyHandler().current());
                if(handlesValues()){
                    _values[s._cursor] = valueHandler().current();
                }
            }
        }else{
            child(reader, s._cursor).remove(trans);
            
        }
        return this;
    }

    
    void rollback(Transaction trans){
        
        int xxx = 1;
        
        
        
    }
    
    private Searcher search(Transaction trans, YapReader reader){
        Searcher s = new Searcher(_count);
        while(s.incomplete()){
            compare(s, reader);
        }
        return s;
    }
    
    private void seekAfterKey(YapReader reader, int ix){
        seekKey(reader, ix);
        reader._offset += keyHandler().linkLength();
    }
    
    private void seekChild(YapReader reader, int ix){
        seekAfterKey(reader, ix);
    }
    
    private void seekKey(YapReader reader, int ix){
        reader._offset = SLOT_LEADING_LENGTH + (entryLength() * ix);
    }
    
    private void seekValue(YapReader reader, int ix){
        if(handlesValues()){
            seekAfterKey(reader, ix);
        }else{
            seekKey(reader, ix);
        }
    }

    
    
    

    
    public void setID(int a_id) {
        if(getID() == 0){
            _btree.addNode(a_id, this);
        }
        super.setID(a_id);
    }

    private BTreeNode split(Transaction trans){
        BTreeNode res = new BTreeNode(_btree);
        res.prepareWrite(trans);
        System.arraycopy(_keys, HALF_ENTRIES, res._keys, 0, HALF_ENTRIES);
        if(_values != null){
            res._values = new Object[MAX_ENTRIES];
            System.arraycopy(_values, HALF_ENTRIES, res._values, 0, HALF_ENTRIES);
        }
        if(_children != null){
            res._children = new Object[MAX_ENTRIES];
            System.arraycopy(_children, HALF_ENTRIES, res._children, 0, HALF_ENTRIES);
        }
        res._count = HALF_ENTRIES;
        
        _count = HALF_ENTRIES;
        
        return res;
    }
    
    public void traverseKeys(Transaction trans, Visitor4 visitor){
        YapReader reader = prepareRead(trans);
        if(isLeaf()){
            for (int i = 0; i < _count; i++) {
                Object obj = key(trans,reader, i);
                if(obj != Null.INSTANCE){
                    visitor.visit(obj);
                }
            }
        }else{
            for (int i = 0; i < _count; i++) {
                child(reader,i).traverseKeys(trans, visitor);
            }
        }
    }
    
    public void traverseValues(Transaction trans, Visitor4 visitor){
        if(! handlesValues()){
            traverseKeys(trans, visitor);
            return;
        }
        YapReader reader = prepareRead(trans);
        if(isLeaf()){
            for (int i = 0; i < _count; i++) {
                if(key(trans,reader, i) != Null.INSTANCE){
                    visitor.visit(value(reader, i));
                }
            }
        }else{
            for (int i = 0; i < _count; i++) {
                child(reader,i).traverseValues(trans, visitor);
            }
        }
    }
    
    private Object value(YapReader reader, int index){
        if( _values != null ){
            return _values[index];
        }
        seekValue(reader, index);
        return valueHandler().readIndexEntry(reader);
    }

    
    private Indexable4 valueHandler(){
        return _btree._valueHandler;
    }
    
    
    public void writeThis(Transaction trans, YapReader a_writer) {
        
        int count = 0;
        int startOffset = a_writer._offset;
        a_writer.incrementOffset(YapConst.YAPINT_LENGTH * 2);

        if(isLeaf()){
            boolean vals = handlesValues();
            for (int i = 0; i < _count; i++) {
                Object obj = key(trans, i);
                if(obj != Null.INSTANCE){
                    count ++;
                    keyHandler().writeIndexEntry(a_writer, obj);
                    if(vals){
                        valueHandler().writeIndexEntry(a_writer, _values[i]);
                    }
                }
            }
        }else{
            for (int i = 0; i < _count; i++) {
                if(childLoaded(i)){
                    BTreeNode child = (BTreeNode)_children[i];
                    Object childKey = child.firstKey(trans);
                    if(childKey != Null.INSTANCE){
                        count ++;
                        keyHandler().writeIndexEntry(a_writer, childKey);
                        a_writer.writeIDOf(trans, child);
                    }
                }else{
                    count ++;
                    keyHandler().writeIndexEntry(a_writer, _keys[i]);
                    a_writer.writeIDOf(trans, _children[i]);
                }
            }
        }
        
        int endOffset = a_writer._offset;
        a_writer._offset = startOffset;
        a_writer.writeInt(count);
        a_writer.writeInt(_height);
        a_writer._offset = endOffset;

    }
    

}
