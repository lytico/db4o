/* Copyright (C) 2007   Versant Inc.   http://www.db4o.com */

package com.db4o.db4ounit.common.handlers;

import com.db4o.internal.handlers.*;

import db4ounit.*;

public class CharHandlerTestCase extends TypeHandlerTestCaseBase {
    
    public static void main(String[] args) {
        new CharHandlerTestCase().runSolo();
    }
    
    public static class Item {
    	public char _char;
    	public Character _charWrapper;
    	public Item(char c, Character wrapper) {
    		_char = c;
    		_charWrapper = wrapper;
		}
    	
    	public boolean equals(Object obj) {
        	if(obj == this){
        		return true;
        	}
        	if (!(obj instanceof Item)) {
        		return false;
			}
        	Item other = (Item)obj;
        	return (other._char == this._char) 
        			&& this._charWrapper.equals(other._charWrapper);
    	}
    	
    	public String toString() {
    		return "[" + _char + "," + _charWrapper + "]";
    	}
    }
    
    private CharHandler charHandler() {
        return new CharHandler();
    }
    
    public void testReadWrite() {
        MockWriteContext writeContext = new MockWriteContext(db());
        Character expected = new Character((char)0x4e2d);
        charHandler().write(writeContext, expected);
        
        MockReadContext readContext = new MockReadContext(writeContext);
        Character charValue = (Character)charHandler().read(readContext);
        
        Assert.areEqual(expected, charValue);
    }
    
    public void testStoreObject() throws Exception{
        Item storedItem = new Item((char)0x4e2f, new Character((char)0x4e2d));
        doTestStoreObject(storedItem);
    }
    
}
