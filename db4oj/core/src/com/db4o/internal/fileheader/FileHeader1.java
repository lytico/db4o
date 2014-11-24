/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.internal.fileheader;

import com.db4o.internal.*;

/**
 * @exclude
 */
public class FileHeader1 extends NewFileHeaderBase {
    
    // The header format is:

    // (byte) 'd'
    // (byte) 'b'
    // (byte) '4'
    // (byte) 'o'
    // (byte) headerVersion
    // (int) headerLock
    // (long) openTime
    // (long) accessTime
    // (int) Transaction pointer 1
    // (int) Transaction pointer 2
    // (int) blockSize
    // (int) classCollectionID
    // (int) freespaceID
    // (int) variablePartID
	
    
    private static final int TRANSACTION_POINTER_OFFSET = ACCESS_TIME_OFFSET + Const4.LONG_LENGTH;
    private static final int BLOCKSIZE_OFFSET = TRANSACTION_POINTER_OFFSET + (Const4.INT_LENGTH * 2);
    
    public static final int HEADER_LENGTH = TRANSACTION_POINTER_OFFSET + (Const4.INT_LENGTH * 6);
    
    public int length() {
        return HEADER_LENGTH;
    }

    protected void read(LocalObjectContainer file, ByteArrayBuffer reader) {
        newTimerFileLock(file);
		oldEncryptionOff(file);
        checkThreadFileLock(file, reader);
        reader.seek(TRANSACTION_POINTER_OFFSET);
        file.systemData().transactionPointer1(reader.readInt());
        file.systemData().transactionPointer2(reader.readInt());
        reader.seek(BLOCKSIZE_OFFSET);
        file.blockSizeReadFromFile(reader.readInt());
        SystemData systemData = file.systemData();
		systemData.classCollectionID(reader.readInt());
		reader.readInt();  // was freespace ID, can no longer be read
        _variablePart =  createVariablePart(file);
        int variablePartId = reader.readInt();
        _variablePart.read(variablePartId, 0);
    }
    
    public void writeFixedPart(
        LocalObjectContainer file, boolean startFileLockingThread, boolean shuttingDown, StatefulBuffer writer, int blockSize) {
    	throw new IllegalStateException();
    }

    public void writeTransactionPointer(Transaction systemTransaction, int transactionPointer) {
    	throw new IllegalStateException();
    }

    protected NewFileHeaderBase createNew(){
		return new FileHeader1();
	}
	
    protected byte version() {
		return (byte) 1;
	}

	@Override
	public FileHeaderVariablePart createVariablePart(
			LocalObjectContainer file) {
		return new FileHeaderVariablePart1(file);
	}

}
