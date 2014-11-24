/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.internal;

import com.db4o.*;
import com.db4o.foundation.*;
import com.db4o.internal.marshall.*;
import com.db4o.marshall.*;

/**
 * @exclude
 */
public abstract class AbstractBufferContext implements BufferContext, HandlerVersionContext{
	
	private ReadBuffer _buffer;
	
	private final Transaction _transaction;
	
	public AbstractBufferContext(Transaction transaction, ReadBuffer buffer) {
		_transaction = transaction;
        _buffer = buffer;
	}

	public ReadBuffer buffer(ReadBuffer buffer) {
	    ReadBuffer temp = _buffer;
	    _buffer = buffer;
	    return temp;
	}

	public ReadBuffer buffer() {
	    return _buffer;
	}

	public byte readByte() {
	    return _buffer.readByte();
	}

	public void readBytes(byte[] bytes) {
	    _buffer.readBytes(bytes);
	}

	public int readInt() {
	    return _buffer.readInt();
	}

	public long readLong() {
	    return _buffer.readLong();
	}

	public int offset() {
	    return _buffer.offset();
	}

	public void seek(int offset) {
	    _buffer.seek(offset);
	}

	public ObjectContainerBase container() {
	    return _transaction.container();
	}

	public ObjectContainer objectContainer() {
	    return container();
	}

	public Transaction transaction() {
	    return _transaction;
	}

	public abstract int handlerVersion();
	
	public boolean isLegacyHandlerVersion() {
		return handlerVersion() == 0;
	}
	
    public BitMap4 readBitMap(int bitCount){
        return _buffer.readBitMap(bitCount);
    }
    
	public SlotFormat slotFormat() {
		return SlotFormat.forHandlerVersion(handlerVersion());
	}

}
