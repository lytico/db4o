/* Copyright (C) 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.types.arrays;

import db4ounit.*;
import db4ounit.extensions.*;

public class SimpleTypeArrayInUntypedVariableTestCase extends AbstractDb4oTestCase {
    
	private static final int[] ARRAY = {1, 2, 3};

	public static class Data {
		public Object _arr;

		public Data(Object arr) {
			this._arr = arr;
		}
	}
	
    protected void store(){
    	db().store(new Data(ARRAY));
    }
    
    public void testRetrieval(){
    	Data data=(Data)retrieveOnlyInstance(Data.class);
    	Assert.isTrue(data._arr instanceof int[]);
        int[] arri = (int[])data._arr;
        ArrayAssert.areEqual(ARRAY,arri);
    }
}
