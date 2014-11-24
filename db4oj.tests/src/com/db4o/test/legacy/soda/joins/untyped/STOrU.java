/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.test.legacy.soda.joins.untyped;

import com.db4o.query.*;
import com.db4o.test.legacy.soda.*;

public class STOrU implements STClass {

	public static transient SodaTest st;
	
	Object orInt;
	Object orString;

	public STOrU() {
	}

	private STOrU(int a_int, String a_string) {
		if(a_int != 0){
			orInt = new Integer(a_int);
		}
		orString = a_string;
	}
	
	private STOrU(Integer a_int, String a_string) {
		orInt = a_int;
		orString = a_string;
	}
	

	public String toString() {
		return "STOr: int:" + orInt + " str:" + orString;
	}

	public Object[] store() {
		return new Object[] {
			new STOrU(new Integer(0), "hi"),
			new STOrU(5, null),
			new STOrU(1000, "joho"),
			new STOrU(30000, "osoo"),
			new STOrU(Integer.MAX_VALUE - 1, null),
			};
	}
	
	
	public void testSmallerGreater() {
		Query q = st.query();
		q.constrain(new STOrU());
		Query sub = q.descend("orInt");
		sub.constrain(new Integer(30000)).greater().or(
			sub.constrain(new Integer(5)).smaller());
		Object[] r = store();
		st.expect(q, new Object[] { r[0], r[4] });
	}

	public void testGreaterGreater() {
		Query q = st.query();
		q.constrain(new STOrU());
		Query sub = q.descend("orInt");
		sub.constrain(new Integer(30000)).greater().or(
			sub.constrain(new Integer(5)).greater());
		Object[] r = store();
		st.expect(q, new Object[] { r[2], r[3], r[4] });
	}

	public void testGreaterEquals() {
		Query q = st.query();
		q.constrain(new STOrU());
		Query sub = q.descend("orInt");
		sub.constrain(new Integer(1000)).greater().or(
			sub.constrain(new Integer(0)));
		Object[] r = store();
		st.expect(q, new Object[] { r[0], r[3], r[4] });
	}

	public void testEqualsNull() {
		Query q = st.query();
		q.constrain(new STOrU(1000, null));
		q.descend("orInt").constraints().or(
			q.descend("orString").constrain(null));
		Object[] r = store();
		st.expect(q, new Object[] { r[1], r[2], r[4] });
	}


	public void testAndOrAnd() {
		Query q = st.query();
		q.constrain(new STOrU(0, null));
		(
			q.descend("orInt").constrain(new Integer(5)).and(
			q.descend("orString").constrain(null))
		).or(
			q.descend("orInt").constrain(new Integer(1000)).and(
			q.descend("orString").constrain("joho"))
		);
		Object[] r = store();
		st.expect(q, new Object[] { r[1], r[2]});
	}
	
	public void testOrAndOr() {
		Query q = st.query();
		q.constrain(new STOrU(0, null));
		(
			q.descend("orInt").constrain(new Integer(5)).or(
			q.descend("orString").constrain(null))
		).and(
			q.descend("orInt").constrain(new Integer(Integer.MAX_VALUE - 1)).or(
			q.descend("orString").constrain("joho"))
		);
		st.expectOne(q, store()[4]);
	}
	
	public void testOrOrAnd() {
		Query q = st.query();
		q.constrain(new STOrU(0, null));
		(
			q.descend("orInt").constrain(new Integer(Integer.MAX_VALUE - 1)).or(
			q.descend("orString").constrain("joho"))
		).or(
			q.descend("orInt").constrain(new Integer(5)).and(
			q.descend("orString").constrain(null))
		);
		Object[] r = store();
		st.expect(q, new Object[] { r[1], r[2], r[4]});
	}
	
	public void testMultiOrAnd(){
		Query q = st.query();
		q.constrain(new STOrU(0, null));
		(
			(
				q.descend("orInt").constrain(new Integer(Integer.MAX_VALUE - 1)).or(
				q.descend("orString").constrain("joho"))
			).or(
				q.descend("orInt").constrain(new Integer(5)).and(
				q.descend("orString").constrain("joho"))
			)
		).or(
			(
				q.descend("orInt").constrain(new Integer(Integer.MAX_VALUE - 1)).or(
				q.descend("orString").constrain(null))
			).and(
				q.descend("orInt").constrain(new Integer(5)).or(
				q.descend("orString").constrain(null))
			)
		);
		Object[] r = store();
		st.expect(q, new Object[] {r[1],  r[2], r[4]});
	}
	
	public void testNotSmallerGreater() {
		Query q = st.query();
		q.constrain(new STOrU());
		Query sub = q.descend("orInt");
		(sub.constrain(new Integer(30000)).greater().or(
			sub.constrain(new Integer(5)).smaller())).not();
		Object[] r = store();
		st.expect(q, new Object[] { r[1], r[2], r[3] });
	}

	public void testNotGreaterGreater() {
		Query q = st.query();
		q.constrain(new STOrU());
		Query sub = q.descend("orInt");
		(sub.constrain(new Integer(30000)).greater().or(
			sub.constrain(new Integer(5)).greater())).not();
		Object[] r = store();
		st.expect(q, new Object[] { r[0], r[1]});
	}

	public void testNotGreaterEquals() {
		Query q = st.query();
		q.constrain(new STOrU());
		Query sub = q.descend("orInt");
		(sub.constrain(new Integer(1000)).greater().or(
			sub.constrain(new Integer(0)))).not();
		Object[] r = store();
		st.expect(q, new Object[] { r[1], r[2]});
	}

	public void testNotEqualsNull() {
		Query q = st.query();
		q.constrain(new STOrU(1000, null));
		(q.descend("orInt").constraints().or(
			q.descend("orString").constrain(null))).not();
		Object[] r = store();
		st.expect(q, new Object[] { r[0], r[3]});
	}

	public void testNotAndOrAnd() {
		Query q = st.query();
		q.constrain(new STOrU(0, null));
		(
			q.descend("orInt").constrain(new Integer(5)).and(
			q.descend("orString").constrain(null))
		).or(
			q.descend("orInt").constrain(new Integer(1000)).and(
			q.descend("orString").constrain("joho"))
		).not();
		Object[] r = store();
		st.expect(q, new Object[] { r[0], r[3], r[4]});
	}
	
	public void testNotOrAndOr() {
		Query q = st.query();
		q.constrain(new STOrU(0, null));
		(
			q.descend("orInt").constrain(new Integer(5)).or(
			q.descend("orString").constrain(null))
		).and(
			q.descend("orInt").constrain(new Integer(Integer.MAX_VALUE - 1)).or(
			q.descend("orString").constrain("joho"))
		).not();
		Object[] r = store();
		st.expect(q, new Object[] { r[0], r[1], r[2], r[3]});
	}
	
	public void testNotOrOrAnd() {
		Query q = st.query();
		q.constrain(new STOrU(0, null));
		(
			q.descend("orInt").constrain(new Integer(Integer.MAX_VALUE - 1)).or(
			q.descend("orString").constrain("joho"))
		).or(
			q.descend("orInt").constrain(new Integer(5)).and(
			q.descend("orString").constrain(null))
		).not();
		Object[] r = store();
		st.expect(q, new Object[] { r[0], r[3]});
	}
	
	public void testNotMultiOrAnd(){
		Query q = st.query();
		q.constrain(new STOrU(0, null));
		(
			(
				q.descend("orInt").constrain(new Integer(Integer.MAX_VALUE - 1)).or(
				q.descend("orString").constrain("joho"))
			).or(
				q.descend("orInt").constrain(new Integer(5)).and(
				q.descend("orString").constrain("joho"))
			)
		).or(
			(
				q.descend("orInt").constrain(new Integer(Integer.MAX_VALUE - 1)).or(
				q.descend("orString").constrain(null))
			).and(
				q.descend("orInt").constrain(new Integer(5)).or(
				q.descend("orString").constrain(null))
			)
		).not();
		Object[] r = store();
		st.expect(q, new Object[] {r[0],  r[3]});
	}
	
	public void testOrNotAndOr() {
		Query q = st.query();
		q.constrain(new STOrU(0, null));
		(
			q.descend("orInt").constrain(new Integer(Integer.MAX_VALUE - 1)).or(
			q.descend("orString").constrain("joho"))
		).not().and(
			q.descend("orInt").constrain(new Integer(5)).or(
			q.descend("orString").constrain(null))
		);
		Object[] r = store();
		st.expect(q, new Object[] { r[1]});
	}
	
	public void testAndNotAndAnd() {
		Query q = st.query();
		q.constrain(new STOrU(0, null));
		(
			q.descend("orInt").constrain(new Integer(Integer.MAX_VALUE - 1)).and(
			q.descend("orString").constrain(null))
		).not().and(
			q.descend("orInt").constrain(new Integer(5)).or(
			q.descend("orString").constrain("osoo"))
		);
		Object[] r = store();
		st.expect(q, new Object[] { r[1], r[3]});
	}
	
}
