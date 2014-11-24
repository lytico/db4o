/* Copyright (C) 2004   db4objects Inc.   http://www.db4o.com */

package com.db4o.inside.ix;

import com.db4o.*;
import com.db4o.foundation.*;
import com.db4o.inside.freespace.*;

/**
 * A range of index entries in the database file. 
 */
class IxFileRange extends IxTree{
    
    final int _address;
    int _addressOffset;
    int _entries;
    private int[] _lowerAndUpperMatches;
    
    public IxFileRange(IndexTransaction a_ft, int a_address, int addressOffset, int a_entries){
        super(a_ft);
        _address = a_address;
        _addressOffset = addressOffset;
        _entries = a_entries;
        _size = a_entries;
    }
    
    public Tree add(final Tree a_new){
        return reader().add(this, a_new);
    }

    public int compare(Tree a_to) {
        _lowerAndUpperMatches = new int[2];
        return reader().compare(this, _lowerAndUpperMatches);
    }
    
    int[] lowerAndUpperMatch(){
        return _lowerAndUpperMatches;
    }
    
    private final IxFileRangeReader reader(){
        return _fieldTransaction.i_index.fileRangeReader();
    }
    
    public void incrementAddress(int length) {
        _addressOffset += length;
    }
    
	public int ownSize(){
	    return _entries;
	}
    
    public String toString(){
        if(! Debug4.prettyToStrings){
            return super.toString();
        }
        YapFile yf = stream();
        Transaction transaction = trans();
        YapReader fileReader = new YapReader(slotLength());
        final StringBuffer sb = new StringBuffer();
        sb.append("IxFileRange");
        visitAll(new IntObjectVisitor() {
            public void visit(int anInt, Object anObject) {
                sb.append("\n  ");
                sb.append("Parent: " + anInt);
                sb.append("\n ");
                sb.append(anObject);
            }
        });
       return sb.toString();
    }
    
    public void visit(Object obj){
        visit((Visitor4)obj, null);
    }

    public void visit(Visitor4 visitor, int[] lowerUpper){
        IxFileRangeReader frr = reader();
        if (lowerUpper == null) {
            lowerUpper = new int[] { 0, _entries - 1};
        }
        int count = lowerUpper[1] - lowerUpper[0] + 1;
        if (count > 0) {
            YapReader fileReader = new YapReader(count * frr._slotLength);
            fileReader.read(stream(), _address, _addressOffset + (lowerUpper[0] * frr._slotLength));
            for (int i = lowerUpper[0]; i <= lowerUpper[1]; i++) {
                fileReader.incrementOffset(frr._linkLegth);
                visitor.visit(new Integer(fileReader.readInt()));
            }
        }
    }

    public int write(Indexable4 a_handler, YapWriter a_writer) {
        YapFile yf = (YapFile)a_writer.getStream();
        int length = _entries * slotLength();
        yf.copy(_address, _addressOffset, a_writer.getAddress(), a_writer.addressOffset(), length);
        a_writer.moveForward(length);
        return _entries;
    }
    
    public void visitAll(IntObjectVisitor visitor) {
        YapFile yf = stream();
        Transaction transaction = trans();
        YapReader fileReader = new YapReader(slotLength());
        for (int i = 0; i < _entries; i++) {
            int address = _address + (i * slotLength());
            fileReader.read(yf, address, _addressOffset);
            fileReader._offset = 0;
            Object obj = handler().comparableObject(transaction, handler().readIndexEntry(fileReader));
            visitor.visit(fileReader.readInt(), obj);
        }
    }
    
    public void visitFirst(FreespaceVisitor visitor){
        if(_preceding != null){
            ((IxTree)_preceding).visitFirst(visitor);
            if(visitor.visited()){
                return;
            }
        }
        freespaceVisit(visitor, 0);
    }
    
    public void visitLast(FreespaceVisitor visitor){
        if(_subsequent != null){
            ((IxTree)_subsequent).visitLast(visitor);
            if(visitor.visited()){
                return;
            }
        }
        int lastIndex = _entries - 1;
        freespaceVisit(visitor, _entries - 1);
    }
    
    public void freespaceVisit(FreespaceVisitor visitor, int index){
        IxFileRangeReader frr = reader();
        YapReader fileReader = new YapReader(frr._slotLength);
        fileReader.read(stream(), _address, _addressOffset + (index * frr._slotLength));
        int val = fileReader.readInt();
        int parentID = fileReader.readInt();
        visitor.visit(parentID, val);
    }

	public Object shallowClone() {
		IxFileRange range=new IxFileRange(_fieldTransaction,_address,_addressOffset,_entries);
		super.shallowCloneInternal(range);
		if(_lowerAndUpperMatches!=null) {
			range._lowerAndUpperMatches=new int[]{_lowerAndUpperMatches[0],_lowerAndUpperMatches[1]};
		}
		return range;
	}
}
