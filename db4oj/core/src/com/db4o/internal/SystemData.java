/* Copyright (C) 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.internal;

import com.db4o.ext.*;
import com.db4o.internal.ids.*;
import com.db4o.internal.slots.*;

/**
 * @exclude
 */
public class SystemData {
    
    private int _classCollectionID;
    
    private int _converterVersion;
    
    private Slot _inMemoryFreespaceSlot;
    
    private int _bTreeFreespaceId;
    
    private byte _freespaceSystem;
    
    private Db4oDatabase _identity;
    
    private int _identityId;
    
    private long _lastTimeStampID;
    
    private byte _stringEncoding;

    private int _uuidIndexId;
    
    private byte _idSystemType;
    
    private int _transactionPointer1;
    
    private int _transactionPointer2;
    
    private Slot _idSystemSlot;
    
    private int _idToTimestampIndexId;
    private int _timestampToIdIndexId;
    
    public Slot idSystemSlot() {
		return _idSystemSlot;
	}

	public void idSystemSlot(Slot slot) {
		_idSystemSlot = slot;
	}

	private TransactionalIdSystem _freespaceIdSystem;

    public void idSystemType(byte idSystem) {
		_idSystemType = idSystem;
	}

	public byte idSystemType() {
		return _idSystemType;
	}

	public int classCollectionID() {
        return _classCollectionID;
    }
    
    public void classCollectionID(int id) {
        _classCollectionID = id;
    }
    
    public int converterVersion(){
        return _converterVersion;
    }

    public void converterVersion(int version){
        _converterVersion = version;
    }
    
    public int bTreeFreespaceId(){
        return _bTreeFreespaceId;
    }
    
    public void bTreeFreespaceId(int id){
        _bTreeFreespaceId = id;
    }
    
    public Slot inMemoryFreespaceSlot() {
        return _inMemoryFreespaceSlot;
    }

    public void inMemoryFreespaceSlot(Slot slot) {
        _inMemoryFreespaceSlot = slot;
    }
    
    public byte freespaceSystem() {
        return _freespaceSystem;
    }
    
    public void freespaceSystem(byte freespaceSystemtype){
        _freespaceSystem = freespaceSystemtype;
    }
    
    public Db4oDatabase identity(){
        return _identity;
    }
    
    public void identity(Db4oDatabase identityObject) {
        _identity = identityObject;
    }

    public long lastTimeStampID(){
        return _lastTimeStampID;
    }
    
    public void lastTimeStampID(long id) {
        _lastTimeStampID = id;
    }
    
    public byte stringEncoding(){
        return _stringEncoding;
    }
    
    public void stringEncoding(byte encodingByte){
        _stringEncoding = encodingByte; 
    }
    
    public int uuidIndexId(){
        return _uuidIndexId;
    }
    
    public void uuidIndexId(int id){
        _uuidIndexId = id;
    }

	public void identityId(int id) {
		_identityId = id;
	}
	
	public int identityId(){
		return _identityId;
	}
	
	public void transactionPointer1(int pointer){
		_transactionPointer1 = pointer;
	}
	
	public void transactionPointer2(int pointer){
		_transactionPointer2 = pointer;
	}
	
	public int transactionPointer1(){
		return _transactionPointer1;
	}
	
	public int transactionPointer2(){
		return _transactionPointer2;
	}
	
	public void freespaceIdSystem(TransactionalIdSystem transactionalIdSystem){
		_freespaceIdSystem = transactionalIdSystem;
	}
	

	public TransactionalIdSystem freespaceIdSystem() {
		return _freespaceIdSystem;
	}

	public void idToTimestampIndexId(int idToTimestampIndexId) {
		_idToTimestampIndexId = idToTimestampIndexId;
	}

	public int idToTimestampIndexId() {
		return _idToTimestampIndexId;
	}

	public void timestampToIdIndexId(int timestampToIdIndexId) {
		_timestampToIdIndexId = timestampToIdIndexId;
	}

	public int timestampToIdIndexId() {
		return _timestampToIdIndexId;
	}

	
	
}
