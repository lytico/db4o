/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.db4ounit.common.soda.ordered;
import com.db4o.query.*;


public class STOIntegerTestCase extends com.db4o.db4ounit.common.soda.util.SodaBaseTestCase{
	
	public int i_int;
	
	public STOIntegerTestCase(){
	}
	
	private STOIntegerTestCase(int a_int){
		i_int = a_int;
	}
	
	public String toString(){
		return "STInteger: " + i_int;
	}
	
	public Object[] createData() {
		return new Object[]{
			new STOIntegerTestCase(1001),
			new STOIntegerTestCase(99),
			new STOIntegerTestCase(1),
			new STOIntegerTestCase(909),
			new STOIntegerTestCase(1001),
			new STOIntegerTestCase(0),
			new STOIntegerTestCase(1010),
		};
	}
	
	public void testAscending() {
		Query q = newQuery();
		q.constrain(STOIntegerTestCase.class);
		q.descend("i_int").orderAscending();
		
		expectOrdered(q, new int[] { 5, 2,  1, 3, 0, 4, 6 });
	}
	
	public void testDescending() {
		Query q = newQuery();
		q.constrain(STOIntegerTestCase.class);
		q.descend("i_int").orderDescending();
		
		expectOrdered(q, new int[] { 6, 4,  0, 3, 1, 2, 5 });
	}
	
	public void testAscendingGreater(){
		Query q = newQuery();
		q.constrain(STOIntegerTestCase.class);
		Query qInt = q.descend("i_int");
		qInt.constrain(new Integer(100)).greater();
		qInt.orderAscending();
		
		expectOrdered(q, new int[] {3, 0, 4, 6 });
	}
	
}

