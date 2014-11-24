/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

/**
 * 
 */
package db4ounit.extensions;

import java.lang.reflect.*;

import com.db4o.foundation.*;

import db4ounit.*;

public class Db4oTestSuiteBuilder extends ReflectionTestSuiteBuilder {
		
	private Db4oFixture _fixture;
    
	public Db4oTestSuiteBuilder(Db4oFixture fixture, Class clazz) {		
		this(fixture, new Class[] { clazz });
	}
    
    public Db4oTestSuiteBuilder(Db4oFixture fixture, Class[] classes) {     
        super(classes);
        fixture(fixture);
    }
    
    private void fixture(Db4oFixture fixture){
        if (null == fixture) throw new ArgumentNullException("fixture");     
        _fixture = fixture;
    }

    protected boolean isApplicable(Class clazz) {
    	return _fixture.accept(clazz);
    }
    
    protected Test createTest(Object instance, Method method) {
    	final Test test = super.createTest(instance, method);
    	return new TestDecorationAdapter(test) {
			public String label() {
				return "(" + Db4oFixtureVariable.fixture().label() + ") " + test.label();
			}
		};
    }
    
    protected Object withContext(Closure4 closure) {
    	return Db4oFixtureVariable.FIXTURE_VARIABLE.with(_fixture, closure);
    }
}