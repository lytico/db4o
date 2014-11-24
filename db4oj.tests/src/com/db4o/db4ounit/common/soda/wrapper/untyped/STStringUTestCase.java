/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.db4ounit.common.soda.wrapper.untyped;
import com.db4o.*;
import com.db4o.query.*;


public class STStringUTestCase extends com.db4o.db4ounit.common.soda.util.SodaBaseTestCase {

	public Object str;

	public STStringUTestCase() {
	}

	public STStringUTestCase(String str) {
		this.str = str;
	}

	public Object[] createData() {
		return new Object[] {
			new STStringUTestCase(null),
			new STStringUTestCase("aaa"),
			new STStringUTestCase("bbb"),
			new STStringUTestCase("dod")};
	}

	public void testEquals() {
		Query q = newQuery();
		q.constrain(_array[2]);
		com.db4o.db4ounit.common.soda.util.SodaTestUtil.expectOne(q, _array[2]);
	}

	public void testNotEquals() {
		Query q = newQuery();
		q.constrain(_array[2]);
		q.descend("str").constraints().not();
		
		expect(q, new int[] { 0, 1, 3 });
	}

	public void testDescendantEquals() {
		Query q = newQuery();
		q.constrain(new STStringUTestCase());
		q.descend("str").constrain("bbb");
		com.db4o.db4ounit.common.soda.util.SodaTestUtil.expectOne(q, new STStringUTestCase("bbb"));
	}

	public void testContains() {
		Query q = newQuery();
		q.constrain(new STStringUTestCase("od"));
		q.descend("str").constraints().contains();
		com.db4o.db4ounit.common.soda.util.SodaTestUtil.expectOne(q, new STStringUTestCase("dod"));
	}

	public void testNotContains() {
		Query q = newQuery();
		q.constrain(new STStringUTestCase("od"));
		q.descend("str").constraints().contains().not();
		com.db4o.db4ounit.common.soda.util.SodaTestUtil.expect(
			q,
			new Object[] { new STStringUTestCase(null), new STStringUTestCase("aaa"), new STStringUTestCase("bbb")});
	}

	public void testLike() {
		Query q = newQuery();
		q.constrain(new STStringUTestCase("do"));
		q.descend("str").constraints().like();
		com.db4o.db4ounit.common.soda.util.SodaTestUtil.expectOne(q, new STStringUTestCase("dod"));
		q = newQuery();
		q.constrain(new STStringUTestCase("od"));
		q.descend("str").constraints().like();
		com.db4o.db4ounit.common.soda.util.SodaTestUtil.expectOne(q,_array[3]);
	}

	public void testNotLike() {
		Query q = newQuery();
		q.constrain(new STStringUTestCase("aaa"));
		q.descend("str").constraints().like().not();
		com.db4o.db4ounit.common.soda.util.SodaTestUtil.expect(
			q,
			new Object[] { new STStringUTestCase(null), new STStringUTestCase("bbb"), new STStringUTestCase("dod")});
		q = newQuery();
		q.constrain(new STStringUTestCase("xxx"));
		q.descend("str").constraints().like();
		expect(q, new int[] {});
	}

	public void testIdentity() {
		Query q = newQuery();
		q.constrain(new STStringUTestCase("aaa"));
		ObjectSet set = q.execute();
		STStringUTestCase identityConstraint = (STStringUTestCase) set.next();
		identityConstraint.str = "hihs";
		q = newQuery();
		q.constrain(identityConstraint).identity();
		identityConstraint.str = "aaa";
		com.db4o.db4ounit.common.soda.util.SodaTestUtil.expectOne(q, new STStringUTestCase("aaa"));
	}

	public void testNotIdentity() {
		Query q = newQuery();
		q.constrain(new STStringUTestCase("aaa"));
		ObjectSet set = q.execute();
		STStringUTestCase identityConstraint = (STStringUTestCase) set.next();
		identityConstraint.str = null;
		q = newQuery();
		q.constrain(identityConstraint).identity().not();
		identityConstraint.str = "aaa";
		com.db4o.db4ounit.common.soda.util.SodaTestUtil.expect(
			q,
			new Object[] { new STStringUTestCase(null), new STStringUTestCase("bbb"), new STStringUTestCase("dod")});
	}

	public void testNull() {
		Query q = newQuery();
		q.constrain(new STStringUTestCase(null));
		q.descend("str").constrain(null);
		com.db4o.db4ounit.common.soda.util.SodaTestUtil.expectOne(q, new STStringUTestCase(null));
	}

	public void testNotNull() {
		Query q = newQuery();
		q.constrain(new STStringUTestCase(null));
		q.descend("str").constrain(null).not();
		com.db4o.db4ounit.common.soda.util.SodaTestUtil.expect(
			q,
			new Object[] { new STStringUTestCase("aaa"), new STStringUTestCase("bbb"), new STStringUTestCase("dod")});
	}

	public void testConstraints() {
		Query q = newQuery();
		q.constrain(new STStringUTestCase("aaa"));
		q.constrain(new STStringUTestCase("bbb"));
		Constraints cs = q.constraints();
		Constraint[] csa = cs.toArray();
		if (csa.length != 2) {
			db4ounit.Assert.fail("Constraints not returned");
		}
	}

}
