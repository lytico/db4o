/* Copyright (C) 2009  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.diagnostics;

import java.util.*;

import com.db4o.config.*;
import com.db4o.diagnostic.*;
import com.db4o.query.*;

import db4ounit.*;
import db4ounit.extensions.*;

public class IndexFieldDiagnosticTestCase extends AbstractDb4oTestCase {
	
	private boolean _diagnosticsCalled;
	
	public static class Car {
	    public String model;
	    public List history;

	    public Car(String model) {
	        this(model,new ArrayList());
	    }

	    public Car(String model,List history) {
	        this.model=model;
	        this.history=history;
	    }

	    public String getModel() {
	        return model;
	    }

	    public List getHistory() {
	        return history;
	    }
	    
	    
	    public String toString() {
	        return model;
	    }
	}
	
	@Override
	protected void store() throws Exception {
		Car car = new Car("BMW");
		store(car);
	}
	
	@Override
	protected void configure(Configuration config) throws Exception {
		config.diagnostic().addListener(new DiagnosticListener(){
			public void onDiagnostic(Diagnostic d) {
				if ( d instanceof LoadedFromClassIndex){
					_diagnosticsCalled = true;	
				}
			}
		});
	}
	
	public void testNonIndexedFieldQuery(){
		Query query = newQuery(Car.class);
		query.descend("model").constrain("BMW");
		query.execute();
		Assert.isTrue(_diagnosticsCalled);
	}

	public void testClassQuery(){
		db().query(Car.class);
		Assert.isFalse(_diagnosticsCalled);
	}

}
