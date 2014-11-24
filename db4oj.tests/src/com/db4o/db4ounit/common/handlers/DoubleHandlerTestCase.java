/* Copyright (C) 2006 - 2007 Versant Inc. http://www.db4o.com */

package com.db4o.db4ounit.common.handlers;

import com.db4o.foundation.*;
import com.db4o.internal.*;
import com.db4o.internal.handlers.*;

import db4ounit.*;

/**
 * @exclude
 */
public class DoubleHandlerTestCase extends TypeHandlerTestCaseBase {
	
	private Indexable4 _handler;
	
	public static void main(String[] args) {
	    new DoubleHandlerTestCase().runSolo();
    }
	
	protected void db4oSetupBeforeStore() throws Exception {
		_handler = new DoubleHandler();
	}
	
	public void testMarshalling() {
		final Double expected = new Double(1.1);
		
		ByteArrayBuffer buffer = new ByteArrayBuffer(_handler.linkLength());		
		_handler.writeIndexEntry(context(), buffer, expected);
		
		buffer.seek(0);
		final Object actual = _handler.readIndexEntry(context(), buffer);
		Assert.areEqual(expected, actual);
	}

	public void testComparison() {		
		assertComparison(0, 1.1, 1.1);
		assertComparison(-1, 1.0, 1.1);
		assertComparison(1, 1.1, 0.5);
	}

	private void assertComparison(final int expected, final double prepareWith, final double compareTo) {
		PreparedComparison preparedComparison = _handler.prepareComparison(stream().transaction().context(), new Double(prepareWith));
		final Double doubleCompareTo = new Double(compareTo);
		Assert.areEqual(expected, preparedComparison.compareTo(doubleCompareTo));
	}
	
	public void testReadWrite() {
	    MockWriteContext writeContext = new MockWriteContext(db());
	    DoubleHandler doubleHandler = (DoubleHandler)_handler;
	    Double expected = new Double(1.23456789);
	    doubleHandler.write(writeContext, expected);
	    
	    MockReadContext readContext = new MockReadContext(writeContext);
	    Double d = (Double) doubleHandler.read(readContext);
	    
	    Assert.areEqual(expected, d);
	}
	
	public void testStoreObject() {
        Item storedItem = new Item(1.023456789, new Double(1.023456789));
        doTestStoreObject(storedItem);
    }
    
    public static class Item {
        public double _double;
        public Double _doubleWrapper;
        public Item(double d, Double wrapper) {
            _double = d;
            _doubleWrapper = wrapper;
        }
        public boolean equals(Object obj) {
            if(obj == this){
                return true;
            }
            if (!(obj instanceof Item)) {
                return false;
            }
            Item other = (Item)obj;
            return (other._double == this._double) 
                    && this._doubleWrapper.equals(other._doubleWrapper);
        }
        
        public String toString() {
            return "[" + _double + ","+ _doubleWrapper + "]";
        }
    }
}
