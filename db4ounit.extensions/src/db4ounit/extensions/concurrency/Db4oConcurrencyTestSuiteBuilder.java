/* Copyright (C) 2004 - 2007  Versant Inc.   http://www.db4o.com */
package db4ounit.extensions.concurrency;

import java.lang.reflect.*;

import com.db4o.ext.*;

import db4ounit.*;
import db4ounit.extensions.*;

public class Db4oConcurrencyTestSuiteBuilder extends Db4oTestSuiteBuilder {	

	public Db4oConcurrencyTestSuiteBuilder(Db4oFixture fixture, Class clazz) {
		super(fixture, clazz);
	}

	public Db4oConcurrencyTestSuiteBuilder(Db4oFixture fixture, Class[] classes) {
		super(fixture, classes);
	}

	protected Test createTest(Object instance, Method method) {
		return new ConcurrencyTestMethod(instance, method);
	}

	protected boolean isTestMethod(Method method) {
		String name = method.getName();
		return startsWithIgnoreCase(name, ConcurrencyConventions.testPrefix())
				&& TestPlatform.isPublic(method)
				&& !TestPlatform.isStatic(method) && hasValidParameter(method);
	}

	static boolean hasValidParameter(Method method) {
		Class[] parameters = method.getParameterTypes();
		if (parameters.length == 1 && parameters[0] == ExtObjectContainer.class)
			return true;

		if (parameters.length == 2 && parameters[0] == ExtObjectContainer.class
				&& parameters[1] == Integer.TYPE)
			return true;

		return false;
	}
}
