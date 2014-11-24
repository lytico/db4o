/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.db4ounit.common.soda.classes.simple;
import com.db4o.query.*;


public class STBooleanTestCase extends com.db4o.db4ounit.common.soda.util.SodaBaseTestCase{
	
	public boolean i_boolean;
	
	public STBooleanTestCase(){
	}
	
	private STBooleanTestCase(boolean a_boolean){
		i_boolean = a_boolean;
	}
	
	public Object[] createData() {
		return new Object[]{
			new STBooleanTestCase(false),
			new STBooleanTestCase(true),
			new STBooleanTestCase(false),
			new STBooleanTestCase(false)
		};
	}
	
	public void testEqualsTrue(){
		Query q = newQuery();
		q.constrain(new STBooleanTestCase(true));  
		
		com.db4o.db4ounit.common.soda.util.SodaTestUtil.expectOne(q, new STBooleanTestCase(true));
	}
	
	public void testEqualsFalse(){
		Query q = newQuery();
		q.constrain(new STBooleanTestCase(false));
		q.descend("i_boolean").constrain(new Boolean(false));
		
		expect(q, new int[] {0, 2, 3});
	}
	
	
	
}

