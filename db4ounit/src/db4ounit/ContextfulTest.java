/* Copyright (C) 2004 - 2006 Versant Inc. http://www.db4o.com */

package db4ounit;

import com.db4o.foundation.*;

import db4ounit.fixtures.*;

public class ContextfulTest extends Contextful implements Test {
	
	private TestFactory _factory;

	public ContextfulTest(TestFactory factory) {
		_factory = factory;
	}

	public String label() {
		return (String)run(new Closure4() {
			public Object run() {
				return testInstance().label();
			}
		});
	}

	public boolean isLeafTest() {
		return (Boolean)run(new Closure4<Boolean>() {
			public Boolean run() {
				return testInstance().isLeafTest();
			}
		});
	}

	public void run() {
		run(new Runnable() {
			public void run() {
				testInstance().run();
			}
		});
	}

	private Test testInstance() {
		return _factory.newInstance();
	}

	public Test transmogrify(final Function4<Test, Test> fun) {
		return fun.apply(this);
	}
}
