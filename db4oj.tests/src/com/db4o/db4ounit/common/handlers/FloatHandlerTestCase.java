/* Copyright (C) 2007   Versant Inc.   http://www.db4o.com */

package com.db4o.db4ounit.common.handlers;

import com.db4o.internal.handlers.*;

import db4ounit.*;

public class FloatHandlerTestCase extends TypeHandlerTestCaseBase {
    
    public static void main(String[] args) {
        new FloatHandlerTestCase().runSolo();
    }

    private FloatHandler floatHandler() {
        return new FloatHandler();
    }
    
    public void testReadWrite() {
        MockWriteContext writeContext = new MockWriteContext(db());
        Float expected = new Float(Float.MAX_VALUE);
        floatHandler().write(writeContext, expected);
        
        MockReadContext readContext = new MockReadContext(writeContext);
        Float f = (Float) floatHandler().read(readContext);
        
        Assert.areEqual(expected, f);
    }
    
    public void testStoreObject() {
        Item storedItem = new Item(1.23456789f, new Float(1.23456789f));
        doTestStoreObject(storedItem);
    }
    
    public static class Item {
        public float _float;
        public Float _floatWrapper;
        public Item(float f, Float wrapper) {
            _float = f;
            _floatWrapper = wrapper;
        }
        public boolean equals(Object obj) {
            if(obj == this){
                return true;
            }
            if (!(obj instanceof Item)) {
                return false;
            }
            Item other = (Item)obj;
            return (other._float == this._float) 
                    && this._floatWrapper.equals(other._floatWrapper);
        }
        
        public String toString() {
            return "[" + _float + ","+ _floatWrapper + "]";
        }
    }
}
