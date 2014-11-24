/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.db4ounit.common.soda.classes.simple;
import com.db4o.*;
import com.db4o.query.*;


public class STIntegerTestCase extends com.db4o.db4ounit.common.soda.util.SodaBaseTestCase{
	
	public int i_int;
	
	public STIntegerTestCase(){
	}
	
	private STIntegerTestCase(int a_int){
		i_int = a_int;
	}
	
	public Object[] createData() {
		return new Object[]{
			new STIntegerTestCase(0),
			new STIntegerTestCase(1),
			new STIntegerTestCase(99),
			new STIntegerTestCase(909)
		};
	}
	
	public void testEquals(){
		Query q = newQuery();
		q.constrain(new STIntegerTestCase(0));  
		
		// Primitive default values are ignored, so we need an 
		// additional constraint:
		q.descend("i_int").constrain(new Integer(0));
		com.db4o.db4ounit.common.soda.util.SodaTestUtil.expectOne(q, _array[0]);
	}
	
	public void testNotEquals(){
		Query q = newQuery();
		
		q.constrain(_array[0]);
		q.descend("i_int").constrain(new Integer(0)).not();
		expect(q, new int[] {1, 2, 3});
	}
	
	public void testGreater(){
		Query q = newQuery();
		q.constrain(new STIntegerTestCase(9));
		q.descend("i_int").constraints().greater();
		
		expect(q, new int[] {2, 3});
	}
	
	public void testSmaller(){
		Query q = newQuery();
		q.constrain(new STIntegerTestCase(1));
		q.descend("i_int").constraints().smaller();
		com.db4o.db4ounit.common.soda.util.SodaTestUtil.expectOne(q, _array[0]);
	}
	
	public void testContains(){
		Query q = newQuery();
		q.constrain(new STIntegerTestCase(9));
		q.descend("i_int").constraints().contains();
		
		expect(q, new int[] {2, 3});
	}
	
	public void testNotContains(){
		Query q = newQuery();
		q.constrain(new STIntegerTestCase(0));
		q.descend("i_int").constrain(new Integer(0)).contains().not();
		
		expect(q, new int[] {1, 2});
	}
	
	public void testLike(){
		Query q = newQuery();
		q.constrain(new STIntegerTestCase(90));
		q.descend("i_int").constraints().like();
		com.db4o.db4ounit.common.soda.util.SodaTestUtil.expectOne(q, new STIntegerTestCase(909));
		q = newQuery();
		q.constrain(new STIntegerTestCase(10));
		q.descend("i_int").constraints().like();
		expect(q, new int[] {});
	}
	
	public void testNotLike(){
		Query q = newQuery();
		q.constrain(new STIntegerTestCase(1));
		q.descend("i_int").constraints().like().not();
		
		expect(q, new int[] {0, 2, 3});
	}
	
	public void testIdentity(){
		Query q = newQuery();
		q.constrain(new STIntegerTestCase(1));
		ObjectSet set = q.execute();
		STIntegerTestCase identityConstraint = (STIntegerTestCase)set.next();
		identityConstraint.i_int = 9999;
		q = newQuery();
		q.constrain(identityConstraint).identity();
		identityConstraint.i_int = 1;
		com.db4o.db4ounit.common.soda.util.SodaTestUtil.expectOne(q,_array[1]);
	}
	
	public void testNotIdentity(){
		Query q = newQuery();
		q.constrain(new STIntegerTestCase(1));
		ObjectSet set = q.execute();
		STIntegerTestCase identityConstraint = (STIntegerTestCase)set.next();
		identityConstraint.i_int = 9080;
		q = newQuery();
		q.constrain(identityConstraint).identity().not();
		identityConstraint.i_int = 1;
		
		expect(q, new int[] {0, 2, 3});
	}
	
	public void testConstraints(){
		Query q = newQuery();
		q.constrain(new STIntegerTestCase(1));
		q.constrain(new STIntegerTestCase(0));
		Constraints cs = q.constraints();
		Constraint[] csa = cs.toArray();
		if(csa.length != 2){
			db4ounit.Assert.fail("Constraints not returned");
		}
	}
	
}

