/* Copyright (C) 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.internal.fileheader;

import com.db4o.*;
import com.db4o.internal.*;
import com.db4o.internal.slots.*;


/**
 * @exclude
 */
public class FileHeaderVariablePart1 extends FileHeaderVariablePart {
    
    // The variable part format is:

    // (int) converter version
    // (byte) freespace system used
    // (int)  freespace address
    // (int) identity ID
    // (long) versionGenerator
	// (int) uuid index ID
	
    
    private static final int LENGTH = 1 + (Const4.INT_LENGTH * 4) + Const4.LONG_LENGTH + Const4.ADDED_LENGTH;
    
    private int _id;
    
    
    public FileHeaderVariablePart1(LocalObjectContainer container, int id) {
    	super(container);
        _id = id;
    }
    
    public FileHeaderVariablePart1(LocalObjectContainer container) {
    	this(container, 0);
    }

    public int ownLength() {
        return LENGTH;
    }

    public void readThis(ByteArrayBuffer buffer) {
		if (Deploy.debug) {
		    buffer.readBegin(getIdentifier());
		}
        systemData().converterVersion(buffer.readInt());
        systemData().freespaceSystem(buffer.readByte());
        buffer.readInt();  // was BTreeFreespaceId, converted to slot, can no longer be used
        systemData().identityId(buffer.readInt());
        systemData().lastTimeStampID(buffer.readLong());
        systemData().uuidIndexId(buffer.readInt());
    }

    public void writeThis(ByteArrayBuffer buffer) {
    	throw new IllegalStateException();
    }

	public Runnable commit(boolean shuttingDown) {
		throw new IllegalStateException();
	}

	public int id() {
		return _id;
	}

	@Override
	public void read(int variablePartID, int unused) {
		_id = variablePartID;
    	Slot slot = _container.readPointerSlot(_id);
    	ByteArrayBuffer buffer = _container.readBufferBySlot(slot);
    	readThis(buffer);
	}

	@Override
	public int marshalledLength() {
		return ownLength();
	}

}
