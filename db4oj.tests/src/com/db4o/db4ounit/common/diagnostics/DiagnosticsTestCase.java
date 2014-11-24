/* Copyright (C) 2009  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.diagnostics;

import com.db4o.config.*;
import com.db4o.diagnostic.*;
import com.db4o.ta.*;

import db4ounit.*;
import db4ounit.extensions.*;

public class DiagnosticsTestCase extends AbstractDb4oTestCase{
	
	@Override
	protected void configure(Configuration config) throws Exception {
		config.diagnostic().addListener(new DiagnosticListener() {
			public void onDiagnostic(Diagnostic d) {
				if (!(d instanceof NotTransparentActivationEnabled))
				{
					Assert.fail("no diagnostic message expected but was " + d);
				}
			}
		});
	}
	
	public static class Item {
		public String _name;
		
		public Item(String name){
			_name = name;
		}
	}
	
	public void testNoDiagnosticsForInternalClasses(){
		store(new Item("one"));
		retrieveOnlyInstance(Item.class);
	}

}
