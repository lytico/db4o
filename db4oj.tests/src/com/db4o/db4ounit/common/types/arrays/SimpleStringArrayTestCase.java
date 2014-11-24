/* Copyright (C) 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.types.arrays;

import db4ounit.*;
import db4ounit.extensions.*;

public class SimpleStringArrayTestCase extends AbstractDb4oTestCase {

	private static final String[] ARRAY = new String[] {"hi", "babe"};

	public static class Data {
	    public String[] _arr;

		public Data(String[] _arr) {
			this._arr = _arr;
		}
	}

	protected void store() throws Exception {
		db().store(new Data(ARRAY));
	}
	
    public void testRetrieve(){
    	Data data=(Data) retrieveOnlyInstance(Data.class);
    	ArrayAssert.areEqual(ARRAY,data._arr);
    }
}
