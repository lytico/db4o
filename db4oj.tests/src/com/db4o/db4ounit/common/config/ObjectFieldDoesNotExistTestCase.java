/* Copyright (C) 2009  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.config;

import com.db4o.config.*;
import com.db4o.diagnostic.*;

import db4ounit.*;
import db4ounit.extensions.*;

public class ObjectFieldDoesNotExistTestCase extends AbstractDb4oTestCase{
	
	private static final String BOGUS_FIELD_NAME = "bogusField";
	
	private boolean _diagnosticCalled = false; 
	
	public static class Item{
		
		public String _name;
		
	}
	
	@Override
	protected void configure(final Configuration config) throws Exception {
		config.diagnostic().addListener(new DiagnosticListener(){
			public void onDiagnostic(Diagnostic d) {
				if(d instanceof ObjectFieldDoesNotExist){
					ObjectFieldDoesNotExist message = (ObjectFieldDoesNotExist) d;
					Assert.areEqual(BOGUS_FIELD_NAME, message._fieldName);
					_diagnosticCalled = true;
				}
			}
		});
		config.objectClass(Item.class).objectField(BOGUS_FIELD_NAME).indexed(true);
		config.objectClass(Item.class).objectField("_name").indexed(true);
	}
	
	public void test(){
		store(new Item());
		Assert.isTrue(_diagnosticCalled);
	}
	
	public static void main(String[] args) {
		new ObjectFieldDoesNotExistTestCase().runNetworking();
	}

}
