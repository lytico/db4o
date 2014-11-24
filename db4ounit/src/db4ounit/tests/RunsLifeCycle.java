package db4ounit.tests;

import com.db4o.foundation.*;

import db4ounit.*;

public class RunsLifeCycle implements TestCase, TestLifeCycle {

	public static DynamicVariable<ByRef<Boolean>> _tearDownCalled = DynamicVariable.newInstance();
	private boolean _setupCalled=false;
	
	public void setUp() {
		_setupCalled=true;
	}
	
	public void tearDown() {
		tearDownCalled().value = true;
	}
	
	public boolean setupCalled() {
		return _setupCalled;
	}

	public void testMethod() throws Exception {
		Assert.isTrue(_setupCalled);
		Assert.isTrue(!tearDownCalled().value);
		throw FrameworkTestCase.EXCEPTION;
	}

	private ByRef<Boolean> tearDownCalled() {
	    return _tearDownCalled.value();
    }
}
