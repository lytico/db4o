/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.db4ounit.jre11.soda.wrapper.typed;
import com.db4o.query.*;


public class STDoubleWTTestCase extends com.db4o.db4ounit.common.soda.util.SodaBaseTestCase{
	
	public Double i_double;
	
	public STDoubleWTTestCase(){
	}
	
	private STDoubleWTTestCase(double a_double){
		i_double = new Double(a_double);
	}
	
	public Object[] createData() {
		return new Object[]{
			new STDoubleWTTestCase(0),
			new STDoubleWTTestCase(0),
			new STDoubleWTTestCase(1.01),
			new STDoubleWTTestCase(99.99),
			new STDoubleWTTestCase(909.00)
		};
	}
	
	public void testEquals(){
		Query q = newQuery();
		q.constrain(new STDoubleWTTestCase(0));  
		
		// Primitive default values are ignored, so we need an 
		// additional constraint:
		q.descend("i_double").constrain(new Double(0));
		
		expect(q, new int[] {0, 1});
	}
	
	public void testGreater(){
		Query q = newQuery();
		q.constrain(new STDoubleWTTestCase(1));
		q.descend("i_double").constraints().greater();
		
		expect(q, new int[] {2, 3, 4});
	}
	
	public void testSmaller(){
		Query q = newQuery();
		q.constrain(new STDoubleWTTestCase(1));
		q.descend("i_double").constraints().smaller();
		
		expect(q, new int[] {0, 1});
	}
	
	public void testGreaterOrEqual(){
		Query q = newQuery();
		q.constrain(_array[2]);
		q.descend("i_double").constraints().greater().equal();
		
		expect(q, new int[] {2, 3, 4});
	}
	
	public void testGreaterAndNot(){
		Query q = newQuery();
		q.constrain(new STDoubleWTTestCase());
		Query val = q.descend("i_double");
		val.constrain(new Double(0)).greater();
		val.constrain(new Double(99.99)).not();
		
		expect(q, new int[] {2, 4});
	}
	
}

