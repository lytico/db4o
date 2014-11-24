/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.db4ounit.common.soda.classes.simple;
import com.db4o.*;
import com.db4o.query.*;


public class STShortTestCase extends com.db4o.db4ounit.common.soda.util.SodaBaseTestCase{
	
	final static String DESCENDANT = "i_short";
	
	public short i_short;
	
	public STShortTestCase(){
	}
	
	private STShortTestCase(short a_short){
		i_short = a_short;
	}
	
	public Object[] createData() {
		return new Object[]{
			new STShortTestCase((short)0),
			new STShortTestCase((short)1),
			new STShortTestCase((short)99),
			new STShortTestCase((short)909)
		};
	}
	
	public void testEquals(){
		Query q = newQuery();
		q.constrain(new STShortTestCase((short)0));  
		
		// Primitive default values are ignored, so we need an 
		// additional constraint:
		q.descend(DESCENDANT).constrain(new Short((short)0));
		com.db4o.db4ounit.common.soda.util.SodaTestUtil.expectOne(q, _array[0]);
	}
	
	public void testNotEquals(){
		Query q = newQuery();
		
		q.constrain(_array[0]);
		q.descend(DESCENDANT).constrain(new Short((short)0)).not();
		expect(q, new int[] {1, 2, 3});
	}
	
	public void testGreater(){
		Query q = newQuery();
		q.constrain(new STShortTestCase((short)9));
		q.descend(DESCENDANT).constraints().greater();
		
		expect(q, new int[] {2, 3});
	}
	
	public void testSmaller(){
		Query q = newQuery();
		q.constrain(new STShortTestCase((short)1));
		q.descend(DESCENDANT).constraints().smaller();
		com.db4o.db4ounit.common.soda.util.SodaTestUtil.expectOne(q, _array[0]);
	}
	
	public void testContains(){
		Query q = newQuery();
		q.constrain(new STShortTestCase((short)9));
		q.descend(DESCENDANT).constraints().contains();
		
		expect(q, new int[] {2, 3});
	}
	
	public void testNotContains(){
		Query q = newQuery();
		q.constrain(new STShortTestCase((short)0));
		q.descend(DESCENDANT).constrain(new Short((short)0)).contains().not();
		
		expect(q, new int[] {1, 2});
	}
	
	public void testLike(){
		Query q = newQuery();
		q.constrain(new STShortTestCase((short)90));
		q.descend(DESCENDANT).constraints().like();
		com.db4o.db4ounit.common.soda.util.SodaTestUtil.expectOne(q, _array[3]);
		q = newQuery();
		q.constrain(new STShortTestCase((short)10));
		q.descend(DESCENDANT).constraints().like();
		expect(q, new int[] {});
	}
	
	public void testNotLike(){
		Query q = newQuery();
		q.constrain(new STShortTestCase((short)1));
		q.descend(DESCENDANT).constraints().like().not();
		
		expect(q, new int[] {0, 2, 3});
	}
	
	public void testIdentity(){
		Query q = newQuery();
		q.constrain(new STShortTestCase((short)1));
		ObjectSet set = q.execute();
		STShortTestCase identityConstraint = (STShortTestCase)set.next();
		identityConstraint.i_short = 9999;
		q = newQuery();
		q.constrain(identityConstraint).identity();
		identityConstraint.i_short = 1;
		com.db4o.db4ounit.common.soda.util.SodaTestUtil.expectOne(q,_array[1]);
	}
	
	public void testNotIdentity(){
		Query q = newQuery();
		q.constrain(new STShortTestCase((short)1));
		ObjectSet set = q.execute();
		STShortTestCase identityConstraint = (STShortTestCase)set.next();
		identityConstraint.i_short = 9080;
		q = newQuery();
		q.constrain(identityConstraint).identity().not();
		identityConstraint.i_short = 1;
		
		expect(q, new int[] {0, 2, 3});
	}
	
	public void testConstraints(){
		Query q = newQuery();
		q.constrain(new STShortTestCase((short)1));
		q.constrain(new STShortTestCase((short)0));
		Constraints cs = q.constraints();
		Constraint[] csa = cs.toArray();
		if(csa.length != 2){
			db4ounit.Assert.fail("Constraints not returned");
		}
	}
	
	
}

