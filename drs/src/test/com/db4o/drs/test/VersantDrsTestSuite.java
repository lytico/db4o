package com.db4o.drs.test;

public class VersantDrsTestSuite extends DrsTestSuite {
	
	@Override
	protected Class[] testCases() {
		if(RUN_ONE_SINGLE_TEST){
			return super.testCases();
		}
		return concat(super.testCases(), specificTestcases());
	}

	private Class[] specificTestcases() {
		
		return new Class[] {
			com.db4o.drs.test.ArrayTestSuite.class,
			com.db4o.drs.test.CustomArrayListTestCase.class,
		};
	}

}
