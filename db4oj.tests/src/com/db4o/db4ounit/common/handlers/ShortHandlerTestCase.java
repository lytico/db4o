/* Copyright (C) 2007   Versant Inc.   http://www.db4o.com */

package com.db4o.db4ounit.common.handlers;

import com.db4o.internal.handlers.*;

import db4ounit.*;

public class ShortHandlerTestCase extends TypeHandlerTestCaseBase {
    
    public static void main(String[] args) {
        new ShortHandlerTestCase().runSolo();
    }
    
    public static class Item {
    	public short _short;
    	public Short _shortWrapper;
    	public Item(short s, Short wrapper) {
    		_short = s;
    		_shortWrapper = wrapper;
		}
    	public boolean equals(Object obj) {
        	if(obj == this){
        		return true;
        	}
        	if (!(obj instanceof Item)) {
        		return false;
			}
        	Item other = (Item)obj;
        	return (other._short == this._short) 
        			&& this._shortWrapper.equals(other._shortWrapper);
    	}
    	
    	public String toString() {
    		return "[" + _short + ","+ _shortWrapper + "]";
    	}
    }
    
    private ShortHandler shortHandler() {
        return new ShortHandler();
    }
    
    public void testReadWrite() {
        MockWriteContext writeContext = new MockWriteContext(db());
        Short expected = new Short((short) 0x1020);
        shortHandler().write(writeContext, expected);
        
        MockReadContext readContext = new MockReadContext(writeContext);
        
        Short shortValue = (Short)shortHandler().read(readContext);
        Assert.areEqual(expected, shortValue);
    }
    public void testStoreObject() throws Exception{
        Item storedItem = new Item((short) 0x1020, new Short((short) 0x1122));
        doTestStoreObject(storedItem);
    }

}
