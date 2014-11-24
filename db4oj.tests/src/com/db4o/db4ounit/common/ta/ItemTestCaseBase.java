/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.ta;

import com.db4o.ext.*;
import com.db4o.reflect.*;

import db4ounit.*;
import db4ounit.extensions.fixtures.*;


public abstract class ItemTestCaseBase
	extends TransparentActivationTestCaseBase
	implements OptOutDefragSolo {
    
	private Class _clazz;
	protected long id;
	protected Db4oUUID uuid;
	
    protected void store() throws Exception {
        Object value = createItem();
        _clazz = value.getClass();
        store(value);
        id = db().ext().getID(value);
        uuid = db().ext().getObjectInfo(value).getUUID();
    }
    
    public void testQuery() throws Exception {
        Object item = retrieveOnlyInstance();
        assertRetrievedItem(item);
        assertItemValue(item);
    }
    
    public void testDeactivate() throws Exception {	
    	Object item = retrieveOnlyInstance();
    	db().deactivate(item, 1);
    	assertNullItem(item);  
    	
    	db().activate(item, 42);
    	db().deactivate(item, 1);
    	assertNullItem(item);
	}

	protected Object retrieveOnlyInstance() {
		return retrieveOnlyInstance(_clazz);
	}
    
    protected void assertNullItem(Object obj) throws Exception {
    	ReflectClass claxx = reflector().forObject(obj);
        ReflectField[] fields = claxx.getDeclaredFields();
    	for(int i = 0; i < fields.length; ++i) {
    		ReflectField field = fields[i];
    		if(field.isStatic() || field.isTransient()) {
    			continue;
    		}
    		ReflectClass type = field.getFieldType();
    		if(container().classMetadataForReflectClass(type).isValueType()) {
    			continue;
    		}
    		Object value = field.get(obj);
    		Assert.isNull(value);
    			
    	}
    }

	protected abstract void assertItemValue(Object obj) throws Exception;
     
    protected abstract Object createItem() throws Exception;
    
    protected abstract void assertRetrievedItem(Object obj) throws Exception;
}
