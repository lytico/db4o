/* Copyright (C) 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.types.arrays;

import com.db4o.db4ounit.common.sampledata.*;

import db4ounit.*;
import db4ounit.extensions.*;

public class TypedDerivedArrayTestCase extends AbstractDb4oTestCase {

	private static final MoleculeData[] ARRAY = {new MoleculeData("TypedDerivedArray")};

	public static class Data {
		public AtomData[] _array;

		public Data(AtomData[] AtomDatas) {
			this._array = AtomDatas;
		}
	}
	
	protected void store(){
		db().store(new Data(ARRAY));
	}
	
	public void test(){
		Data data=(Data) retrieveOnlyInstance(Data.class);
		Assert.isTrue(data._array instanceof MoleculeData[],"Expected instance of "+MoleculeData[].class+", but got "+data._array);
		ArrayAssert.areEqual(ARRAY,data._array);
	}
}
