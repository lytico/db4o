package com.db4o.db4ounit.common.refactor;

import com.db4o.internal.*;

import db4ounit.*;

public class AccessOldFieldVersionsTestCase extends AccessFieldTestCaseBase implements TestLifeCycle {

	private static final Class<Integer> ORIG_TYPE = Integer.TYPE;
	private static final String FIELD_NAME = "_value";
	private static final int ORIG_VALUE = 42;
	
	public void testRetypedField() {
		final Class<RetypedFieldData> targetClazz = RetypedFieldData.class;
		renameClass(OriginalData.class, ReflectPlatform.fullyQualifiedName(targetClazz));
		assertField(targetClazz, FIELD_NAME, ORIG_TYPE, ORIG_VALUE);
	}

	@Override
	protected Object newOriginalData() {
		return new OriginalData(ORIG_VALUE);
	}

	public static class OriginalData {
		public int _value;
			
		public OriginalData(int value) {
			_value = value;
		}
	}
	
	public static class RetypedFieldData {
		public String _value;
		
		public RetypedFieldData(String value) {
			_value = value;
		}
	}

}
