/* Copyright (C) 2007   Versant Inc.   http://www.db4o.com */
package com.db4o.db4ounit.common.soda;

import java.util.*;

import com.db4o.*;
import com.db4o.config.*;
import com.db4o.db4ounit.util.*;
import com.db4o.query.*;

import db4ounit.*;
import db4ounit.extensions.*;

public class CollectionIndexedJoinTestCase extends AbstractDb4oTestCase {

	private static final String COLLECTIONFIELDNAME = "_data";
	private static final String IDFIELDNAME = "_id";
	private static final int NUMENTRIES = 3;

	public static class DataHolder {
		public Vector _data;

		public DataHolder(int id) {
			_data=new Vector();
			_data.addElement(new Data(id));
		}
	}

	public static class Data {
		public int _id;

		public Data(int id) {
			this._id = id;
		}
	}
	
	protected void configure(Configuration config) {
		config.objectClass(Data.class).objectField(IDFIELDNAME).indexed(true);
	}

	protected void store() throws Exception {
		for(int i=0;i<NUMENTRIES;i++) {
			store(new DataHolder(i));
		}
	}
	
	public void testIndexedOrTwo() {
		assertIndexedOr(new int[]{0,1,-1},2);
	}

	private void assertIndexedOr(int[] values, int expectedResultCount) {
		TestConfig config=new TestConfig(values.length);
		while(config.moveNext()) {
			assertIndexedOr(values, expectedResultCount, config.rootIndex(), config.connectLeft());
		}
	}

	public void testIndexedOrAll() {
		assertIndexedOr(new int[]{0,1,2},3);
	}

	public void testTwoJoinLegs() {
		Query query=newQuery(DataHolder.class).descend(COLLECTIONFIELDNAME);
		Constraint left=query.descend(IDFIELDNAME).constrain(new Integer(0));
		left.or(query.descend(IDFIELDNAME).constrain(new Integer(1)));
		Constraint right=query.descend(IDFIELDNAME).constrain(new Integer(2));
		right.or(query.descend(IDFIELDNAME).constrain(new Integer(-1)));
		left.or(right);
		ObjectSet result=query.execute();
		Assert.areEqual(3,result.size());
	}
	
	public void assertIndexedOr(int[] values,int expectedResultCount,int rootIdx,boolean connectLeft) {
		Query query=newQuery(DataHolder.class).descend(COLLECTIONFIELDNAME);
		Constraint constraint=query.descend(IDFIELDNAME).constrain(new Integer(values[rootIdx]));
		for(int idx=0;idx<values.length;idx++) {
			if(idx!=rootIdx) {
				Constraint curConstraint = query.descend(IDFIELDNAME).constrain(new Integer(values[idx]));
				if(connectLeft) {
					constraint.or(curConstraint);
				}
				else {
					curConstraint.or(constraint);
				}
			}
		}
		ObjectSet result=query.execute();
		Assert.areEqual(expectedResultCount,result.size());
	}
	
	private static class TestConfig extends PermutingTestConfig {
		public TestConfig(int numValues) {
			super(new Object[][]{
					new Object[] { new Integer(0),new Integer(numValues-1) },
					new Object[] { Boolean.FALSE,Boolean.TRUE } } );
		}

		public int rootIndex() {
			return ((Integer)current(0)).intValue();
		}
		
		public boolean connectLeft() {
			return ((Boolean)current(1)).booleanValue();
		}
	}
}
