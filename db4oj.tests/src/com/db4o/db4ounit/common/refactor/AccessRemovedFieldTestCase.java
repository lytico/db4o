package com.db4o.db4ounit.common.refactor;

import com.db4o.internal.*;

import db4ounit.*;

public class AccessRemovedFieldTestCase extends AccessFieldTestCaseBase implements TestLifeCycle {

	private static final Class<Integer> FIELD_TYPE = Integer.TYPE;
	private static final String FIELD_NAME = "_value";
	private static final int FIELD_VALUE = 42;
	
	public void testRemovedField() {
		final Class<RemovedFieldData> targetClazz = RemovedFieldData.class;
		renameClass(OriginalData.class, ReflectPlatform.fullyQualifiedName(targetClazz));
		assertField(targetClazz, FIELD_NAME, FIELD_TYPE, FIELD_VALUE);
	}

	@Override
	protected Object newOriginalData() {
		return new OriginalData(FIELD_VALUE);
	}

	public static class OriginalData {
		public int _value;
		public String _name;
			
		public OriginalData(int value) {
			_value = value;
		}
	}
	
	public static class RemovedFieldData {
		public String _name;
	}

}
