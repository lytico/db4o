/* Copyright (C) 2004   db4objects Inc.   http://www.db4o.com */

package com.db4o;

import com.db4o.foundation.*;
import com.db4o.inside.slots.*;

/**
 * representation to collect and hold all IDs of one class
 */
 class ClassIndex extends YapMeta implements ReadWriteable, UseSystemTransaction {
     
     
    private final YapClass _yapClass;
     
	/**
	 * contains TreeInt with object IDs 
	 */
	private Tree i_root;
    
    ClassIndex(YapClass yapClass){
        _yapClass = yapClass;
    }
	
	void add(int a_id){
		i_root = Tree.add(i_root, new TreeInt(a_id));
	}

    public final int byteCount() {
    	return YapConst.YAPINT_LENGTH * (Tree.size(i_root) + 1);
    }

    public final void clear() {
        i_root = null;
    }

    final Tree cloneForYapClass(Transaction a_trans, int a_yapClassID) {
    	final Tree[] tree = new Tree[]{Tree.deepClone(i_root, null)}; 
        a_trans.traverseAddedClassIDs(a_yapClassID, new Visitor4() {
            public void visit(Object obj) {
				tree[0] = Tree.add(tree[0], new TreeInt(((TreeInt) obj)._key));
            }
        });
        a_trans.traverseRemovedClassIDs(a_yapClassID, new Visitor4() {
            public void visit(Object obj) {
				tree[0] = Tree.removeLike(tree[0], (TreeInt) obj);
            }
        });
        return tree[0];
    }
    
    void ensureActive(){
        if (!isActive()) {
            setStateDirty();
            read(getStream().getSystemTransaction());
        }
    }
    
    int entryCount(Transaction ta){
        if(isActive()){
            return Tree.size(i_root);
        }
        Slot slot = ta.getSlotInformation(i_id);
        int length = YapConst.YAPINT_LENGTH;
        if(Deploy.debug){
            length += YapConst.LEADING_LENGTH;
        }
        YapReader reader = new YapReader(length);
        reader.readEncrypt(ta.i_stream, slot._address);
        if (reader == null) {
            return 0;
        }
        if (Deploy.debug) {
            reader.readBegin(getID(), getIdentifier());
        }
        return reader.readInt();
    }
    
    public final byte getIdentifier() {
        return YapConst.YAPINDEX;
    }


    long[] getInternalIDs(Transaction a_trans, int a_yapClassID) {
    	Tree tree = cloneForYapClass(a_trans, a_yapClassID);
    	if(tree == null){
    		return new long[0];
    	}
        final long[] ids = new long[tree.size()];
        final int[] i = new int[] { 0 };
        tree.traverse(new Visitor4() {
            public void visit(Object obj) {
                ids[i[0]++] = ((TreeInt) obj)._key;
            }
        });
        return ids;
    }
    
    TreeInt getRoot(){
        ensureActive();
        return (TreeInt)i_root;
    }
    
    YapStream getStream(){
        return _yapClass.getStream();
    }

    public final int ownLength() {
        return YapConst.OBJECT_LENGTH + byteCount();
    }

    public final Object read(YapReader a_reader) {
    	throw YapConst.virtualException();
    }

    public final void readThis(Transaction a_trans, YapReader a_reader) {
    	i_root = new TreeReader(a_reader, new TreeInt(0)).read();
    }

	void remove(int a_id){
		i_root = Tree.removeLike(i_root, new TreeInt(a_id));
	}

    void setDirty(YapStream a_stream) {
        a_stream.setDirty(this);
    }

    public void write(YapReader a_writer) {
        writeThis(null, a_writer);
    }

    public final void writeThis(Transaction trans, final YapReader a_writer) {
    	Tree.write(a_writer, i_root);
    }
    
    public String toString(){
        if(! Debug4.prettyToStrings){
            return super.toString();
        }
        return _yapClass + " index";  
    }
}