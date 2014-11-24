/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

package com.db4o.drs.foundation;

import com.db4o.ext.*;

public class DrsUUIDImpl implements DrsUUID {
	
	private final Signature _signature;
	
	private final long _longPart;
	
	public DrsUUIDImpl(Signature signature, long longPart){
		_signature = signature;
		_longPart = longPart;  
	}
	
	public DrsUUIDImpl(byte[] signature, long longPart){
		this(new Signature(signature), longPart);  
	}
	
	public DrsUUIDImpl(Db4oUUID db4oUUID){
		this(db4oUUID.getSignaturePart(), db4oUUID.getLongPart());
	}

	public long getLongPart() {
		return _longPart;
	}

	public byte[] getSignaturePart() {
		return _signature.bytes;
	}

	@Override
	public boolean equals(Object obj) {
		if( ! (obj instanceof DrsUUIDImpl)){
			return false;
		}
		DrsUUIDImpl other = (DrsUUIDImpl) obj;
		return _longPart == other._longPart && _signature.equals(other._signature);
	}
	
	@Override
	public int hashCode() {
		return ((int)_longPart) ^ _signature.hashCode() ;
	}
	
	@Override
	public String toString() {
		return "longpart " + _longPart + " signature " + _signature.asString();
	}
	
}
