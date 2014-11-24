/* Copyright (C) 2007   Versant Inc.   http://www.db4o.com */
package com.db4o.db4ounit.common.handlers;

import com.db4o.internal.handlers.*;

import db4ounit.*;

public class BooleanHandlerTestCase extends TypeHandlerTestCaseBase {
	
    public static void main(String[] arguments) {
        new BooleanHandlerTestCase().runSolo();
    }
    
    public static class Item {
    	public Boolean _boolWrapper;
    	public boolean _bool;
    	
    	public Item(Boolean boolWrapper, boolean bool){
    		_boolWrapper = boolWrapper;
    		_bool = bool;
    	}
    	
    	public boolean equals(Object obj) {
        	if(obj == this){
        		return true;
        	}
        	if (!(obj instanceof Item)) {
        		return false;
			}
        	Item other = (Item)obj;
        	return (other._bool == this._bool) 
        			&& this._boolWrapper.equals(other._boolWrapper);
    	}
    	
    	public String toString() {
    		return "[" + _bool + "," + _boolWrapper + "]";
    	}
    }
    
    private BooleanHandler booleanHandler() {
        return new BooleanHandler();
    }

	public void testReadWriteTrue(){
		doTestReadWrite(Boolean.TRUE);
	}
	
	public void testReadWriteFalse(){
		doTestReadWrite(Boolean.FALSE);
	}
	
	public void doTestReadWrite(Boolean b){
	    MockWriteContext writeContext = new MockWriteContext(db());
	    booleanHandler().write(writeContext, b);
	    
	    MockReadContext readContext = new MockReadContext(writeContext);
	    Boolean res = (Boolean)booleanHandler().read(readContext);
	    
	    Assert.areEqual(b, res);
	}
	
    public void testStoreObject() throws Exception{
        Item storedItem = new Item(Boolean.FALSE, true);
        doTestStoreObject(storedItem);
    }


}
