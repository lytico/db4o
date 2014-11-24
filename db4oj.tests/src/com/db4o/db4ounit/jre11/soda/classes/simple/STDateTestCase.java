/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.db4ounit.jre11.soda.classes.simple;
import java.util.*;

import com.db4o.query.*;


public class STDateTestCase extends com.db4o.db4ounit.common.soda.util.SodaBaseTestCase{
	
	public Date i_date;
	
	public STDateTestCase(){
	}
	
	private STDateTestCase(Date a_date){
		i_date = a_date;
	}
	
	public Object[] createData() {
		return new Object[]{
			new STDateTestCase(null),
			new STDateTestCase(new Date(4000)),
			new STDateTestCase(new Date(5000)),
			new STDateTestCase(new Date(6000)),
			new STDateTestCase(new Date(7000)),
		};
	}
	
	public void testEquals(){
		Query q = newQuery();
		q.constrain(_array[1]); 
		com.db4o.db4ounit.common.soda.util.SodaTestUtil.expectOne(q, _array[1]);
	}
	
	public void testGreater(){
		Query q = newQuery();
		q.constrain(_array[2]);
		q.descend("i_date").constraints().greater();
		
		expect(q, new int[] { 3, 4});
	}
	
	public void testSmaller(){
		Query q = newQuery();
		q.constrain(_array[4]);
		q.descend("i_date").constraints().smaller();
		
		expect(q, new int[] {1, 2, 3});
	}
	
	public void testGreaterOrEqual(){
	    Query q = newQuery();
	    q.constrain(_array[2]);
	    q.descend("i_date").constraints().greater().equal();
	    
	    expect(q, new int[] {2, 3, 4});
	    
	}
	
	public void testNotGreaterOrEqual(){
		Query q = newQuery();
		q.constrain(_array[3]);
		q.descend("i_date").constraints().not().greater().equal();
		
		expect(q, new int[] {0, 1, 2});
	}
	
	public void testNull(){
		Query q = newQuery();
		q.constrain(new STDateTestCase());
		q.descend("i_date").constrain(null);
		com.db4o.db4ounit.common.soda.util.SodaTestUtil.expectOne(q, new STDateTestCase(null));
	}
	
	public void testNotNull(){
		Query q = newQuery();
		q.constrain(new STDateTestCase());
		q.descend("i_date").constrain(null).not();
		
		expect(q, new int[] {1, 2, 3, 4});
	}
}

