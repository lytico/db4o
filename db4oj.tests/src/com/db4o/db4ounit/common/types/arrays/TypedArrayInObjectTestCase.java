/* Copyright (C) 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.types.arrays;

import com.db4o.db4ounit.common.sampledata.*;

import db4ounit.*;
import db4ounit.extensions.*;

public class TypedArrayInObjectTestCase extends AbstractDb4oTestCase {
	
	private final static AtomData[] ARRAY = {new AtomData("TypedArrayInObject")};

	public static class Data {
		public Object _obj;
		public Object[] _objArr;

		public Data(Object obj, Object[] obj2) {
			this._obj = obj;
			this._objArr = obj2;
		}
	}
	
	protected void store(){
		Data data = new Data(ARRAY,ARRAY);
		db().store(data);
	}
	
	public void testRetrieve(){
		Data data=(Data)retrieveOnlyInstance(Data.class);
		Assert.isTrue(data._obj instanceof AtomData[],"Expected instance of "+AtomData[].class+", but got "+data._obj);
		Assert.isTrue(data._objArr instanceof AtomData[],"Expected instance of "+AtomData[].class+", but got "+data._objArr);
		ArrayAssert.areEqual(ARRAY,data._objArr);
		ArrayAssert.areEqual(ARRAY,(AtomData[])data._obj);
	}
}
