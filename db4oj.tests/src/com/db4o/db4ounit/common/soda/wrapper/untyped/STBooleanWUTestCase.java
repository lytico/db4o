/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.db4ounit.common.soda.wrapper.untyped;
import com.db4o.query.*;


public class STBooleanWUTestCase extends com.db4o.db4ounit.common.soda.util.SodaBaseTestCase{
	
	final static String DESCENDANT = "i_boolean";

	public Object i_boolean;
	
	public STBooleanWUTestCase(){
	}
	
	private STBooleanWUTestCase(boolean a_boolean){
		i_boolean = new Boolean(a_boolean);
	}
	
	public Object[] createData() {
		return new Object[]{
			new STBooleanWUTestCase(false),
			new STBooleanWUTestCase(true),
			new STBooleanWUTestCase(false),
			new STBooleanWUTestCase(false),
			new STBooleanWUTestCase()
		};
	}
	
	public void testEqualsTrue(){
		Query q = newQuery();
		q.constrain(new STBooleanWUTestCase(true));  
		
		com.db4o.db4ounit.common.soda.util.SodaTestUtil.expectOne(q, new STBooleanWUTestCase(true));
	}
	
	public void testEqualsFalse(){
		Query q = newQuery();
		q.constrain(new STBooleanWUTestCase(false));
		q.descend(DESCENDANT).constrain(new Boolean(false));
		
		expect(q, new int[] {0, 2, 3});
	}
	
	public void testNull(){
		Query q = newQuery();
		q.constrain(new STBooleanWUTestCase());
		q.descend(DESCENDANT).constrain(null);
		
		com.db4o.db4ounit.common.soda.util.SodaTestUtil.expectOne(q, new STBooleanWUTestCase());
	}
	
	public void testNullOrTrue(){
		Query q = newQuery();
		q.constrain(new STBooleanWUTestCase());
		Query qd = q.descend(DESCENDANT);
		qd.constrain(null).or(qd.constrain(new Boolean(true)));
		
		expect(q, new int[] {1, 4});
	}
	
	public void testNotNullAndFalse(){
		Query q = newQuery();
		q.constrain(new STBooleanWUTestCase());
		Query qd = q.descend(DESCENDANT);
		qd.constrain(null).not().and(qd.constrain(new Boolean(false)));
		
		expect(q, new int[] {0, 2, 3});
	}
	
}

