/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.internal;

import com.db4o.ext.*;
import com.db4o.foundation.*;
import com.db4o.reflect.*;


/**
 * @exclude
 */
public class StoredFieldImpl implements StoredField {
    
    private final Transaction _transaction;
    
    private final FieldMetadata _fieldMetadata;

    public StoredFieldImpl(Transaction transaction, FieldMetadata fieldMetadata) {
        _transaction = transaction;
        _fieldMetadata = fieldMetadata;
    }

    public void createIndex() {
    	synchronized(lock()){
    		_fieldMetadata.createIndex();
    	}
    }
    
    public void dropIndex() {
    	synchronized(lock()){
    		_fieldMetadata.dropIndex();
    	}
    }

    private Object lock() {
		return _transaction.container().lock();
	}

	public FieldMetadata fieldMetadata(){
        return _fieldMetadata;
    }
    
    public Object get(Object onObject) {
        return _fieldMetadata.get(_transaction, onObject);
    }

    public String getName() {
        return _fieldMetadata.getName();
    }

    public ReflectClass getStoredType() {
        return _fieldMetadata.getStoredType();
    }

    public boolean hasIndex() {
        return _fieldMetadata.hasIndex();
    }

    public boolean isArray() {
        return _fieldMetadata.isArray();
    }

    public void rename(String name) {
    	synchronized(lock()){
    		_fieldMetadata.rename(name);
    	}
    }

    public void traverseValues(Visitor4 visitor) {
        _fieldMetadata.traverseValues(_transaction, visitor);
    }
    public int hashCode() {
        return _fieldMetadata.hashCode();
    }

    public boolean equals(Object obj) {
        if (obj == null){
            return false;
        }
        if (getClass() != obj.getClass()) {
            return false;
        }
        return _fieldMetadata.equals(((StoredFieldImpl) obj)._fieldMetadata);
    }
    
}
