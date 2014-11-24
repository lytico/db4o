/* Copyright (C) 2008  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.migration;

import com.db4o.*;
import com.db4o.db4ounit.common.handlers.*;
import com.db4o.ext.*;
import com.db4o.query.*;

import db4ounit.*;

public class QueryingMigrationTestCase extends HandlerUpdateTestCaseBase{
	
	private static final int OBJECT_COUNT = 5;
	
	public static class Car{
		
		public String _name;
		
		public Pilot _pilot;
		
	}
	
	public static class Pilot {
		
		public String _name;
		
	}

	
	protected Object[] createValues() {
		Object[] cars = new Object[OBJECT_COUNT];
		for (int i = 0; i < cars.length; i++) {
			Car car = new Car();
			car._name = "Car " + i;
			Pilot pilot = new Pilot(); 
			car._pilot = pilot;
			pilot._name = "Pilot " + i;
			cars[i] = car;
		}
		return cars;
	}

	protected void assertValues(ExtObjectContainer objectContainer,
			Object[] values) {
		
	}

	protected Object createArrays() {
		return null;
	}
	
	protected void assertArrays(ExtObjectContainer objectContainer, Object obj) {
		// do nothing
	}
	
    protected void assertQueries(ExtObjectContainer objectContainer) {
    	for (int i = 0; i < OBJECT_COUNT; i++) {
            Query query = objectContainer.query();
            query.constrain(Car.class);
            query.descend("_pilot").descend("_name").constrain("Pilot " + i);
            ObjectSet objectSet = query.execute();
            Assert.areEqual(1, objectSet.size());
            Car car = (Car) objectSet.next();
            Assert.areEqual("Car " + i, car._name);
            Assert.areEqual("Pilot " + i, car._pilot._name);
		}
    }

	protected String typeName() {
		return "querying";
	}
	

}
