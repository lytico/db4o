/* Copyright (C) 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.stored;

import com.db4o.ext.*;

import db4ounit.*;
import db4ounit.extensions.*;
import db4ounit.extensions.util.*;

public class ArrayStoredTypeTestCase extends AbstractDb4oTestCase {

	public static class Data {
		public boolean[] _primitiveBoolean;
		public Boolean[] _wrapperBoolean;
		public int[] _primitiveInt;
		public Integer[] _wrapperInteger;

		public Data(boolean[] primitiveBoolean, Boolean[] wrapperBoolean, int[] primitiveInteger, Integer[] wrapperInteger) {
			this._primitiveBoolean = primitiveBoolean;
			this._wrapperBoolean = wrapperBoolean;
			this._primitiveInt = primitiveInteger;
			this._wrapperInteger = wrapperInteger;
		}
	}
	
	protected void store() throws Exception {
		Data data=new Data(
				new boolean[] { true, false },
				new Boolean[] { Boolean.TRUE, Boolean.FALSE },
				new int[] { 0, 1, 2 },
				new Integer[] { new Integer(4),new Integer(5), new Integer(6)}
		);
		store(data);
	}
	
	public void testArrayStoredTypes() {
		StoredClass clazz = db().storedClass(Data.class);
		assertStoredType(clazz, "_primitiveBoolean", boolean.class);
		assertStoredType(clazz, "_wrapperBoolean", Boolean.class);
		assertStoredType(clazz, "_primitiveInt", int.class);
		assertStoredType(clazz, "_wrapperInteger", Integer.class);
	}

	private void assertStoredType(StoredClass clazz, String fieldName,
			Class type) {
		StoredField field = clazz.storedField(fieldName,null);
		
		Assert.areEqual(
				type.getName(),
				// getName() also contains the assembly name in .net
				// so we better remove it for comparison
				CrossPlatformServices.simpleName(field.getStoredType().getName()));
	}
}
