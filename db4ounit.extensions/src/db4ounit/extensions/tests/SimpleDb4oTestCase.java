/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package db4ounit.extensions.tests;

import com.db4o.config.*;
import com.db4o.foundation.*;

import db4ounit.*;
import db4ounit.extensions.*;
import db4ounit.mocking.*;

public class SimpleDb4oTestCase extends AbstractDb4oTestCase {
	
	public static final DynamicVariable<MethodCallRecorder> RECORDER_VARIABLE = DynamicVariable.newInstance();
	
	public static class Data {}
	
	protected void configure(Configuration config) {
		record(new MethodCall("fixture", fixture()));
		record(new MethodCall("configure", config));
	}

	private void record(final MethodCall call) {
	    recorder().record(call);
    }

	private MethodCallRecorder recorder() {
		return RECORDER_VARIABLE.value();
	}
	
	protected void store() {
		record(new MethodCall("store"));
		fixture().db().store(new Data());
	}
	
	public void testResultSize() {
		record(new MethodCall("testResultSize"));
		Assert.areEqual(1, fixture().db().queryByExample(Data.class).size());
	}
}
