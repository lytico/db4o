/* Copyright (C) 2007   Versant Inc.   http://www.db4o.com */

package com.db4o.db4ounit.common.handlers;

import com.db4o.internal.handlers.*;

import db4ounit.*;

public class IntHandlerTestCase extends TypeHandlerTestCaseBase {

    public static void main(String[] args) {
        new IntHandlerTestCase().runSolo();
    }
    
    public static class Item  {
    	public int _int;
    	public Integer _intWrapper;
    	public Item(int i, Integer wrapper) {
    		_int = i;
    		_intWrapper = wrapper;
		}
    	public boolean equals(Object obj) {
        	if(obj == this){
        		return true;
        	}
        	if (!(obj instanceof Item)) {
        		return false;
			}
        	Item other = (Item)obj;
        	return (other._int == this._int) 
        			&& this._intWrapper.equals(other._intWrapper);
    	}
    	
    	public String toString() {
    		return "[" + _int + ","+ _intWrapper + "]";
    	}
    	
    }
    
    private IntHandler intHandler() {
        return new IntHandler();
    }
    
    public void testReadWrite() {
        MockWriteContext writeContext = new MockWriteContext(db());
        Integer expected = new Integer(100);
        intHandler().write(writeContext, expected);
        
        MockReadContext readContext = new MockReadContext(writeContext);
        
        Integer intValue = (Integer)intHandler().read(readContext);
        Assert.areEqual(expected, intValue);
    }
    public void testStoreObject() throws Exception{
        Item storedItem = new Item(100, new Integer(200));
        doTestStoreObject(storedItem);
    }
}
