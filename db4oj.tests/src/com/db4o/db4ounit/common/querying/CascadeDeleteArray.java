/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.db4ounit.common.querying;

import com.db4o.config.*;

import db4ounit.*;
import db4ounit.extensions.*;

public class CascadeDeleteArray extends AbstractDb4oTestCase {
 
	public static class ArrayElem {

		public String name;
		
		public ArrayElem(String name){
			this.name = name;
		}
	}

	public ArrayElem[] array;
	
	protected void configure(Configuration config) {
		config.objectClass(this).cascadeOnDelete(true);
	}
	
	protected void store() {
		CascadeDeleteArray cda = new CascadeDeleteArray();
		cda.array = new ArrayElem[] {
			new ArrayElem("one"),
			new ArrayElem("two"),
			new ArrayElem("three"),
		};
		db().store(cda);
	}

	public void test(){
		
		CascadeDeleteArray cda = (CascadeDeleteArray)retrieveOnlyInstance(getClass());
		
		Assert.areEqual(3, countOccurences(ArrayElem.class));
		
		db().delete(cda);
		
		Assert.areEqual(0, countOccurences(ArrayElem.class));
		
		db().rollback();
		
		Assert.areEqual(3, countOccurences(ArrayElem.class));
		
		db().delete(cda);

		Assert.areEqual(0, countOccurences(ArrayElem.class));

		db().commit();

		Assert.areEqual(0, countOccurences(ArrayElem.class));
	}
}
