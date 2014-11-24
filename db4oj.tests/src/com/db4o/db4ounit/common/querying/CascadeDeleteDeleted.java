/* Copyright (C) 2004 - 2005  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.querying;

import com.db4o.*;
import com.db4o.config.*;
import com.db4o.query.*;

import db4ounit.*;
import db4ounit.extensions.*;

public class CascadeDeleteDeleted extends AbstractDb4oTestCase {

	public static class CddMember {
		public String name;
	}

	public String name;

	public Object untypedMember;

	public CddMember typedMember;

	public CascadeDeleteDeleted() {
	}

	public CascadeDeleteDeleted(String name) {
		this.name = name;
	}

	protected void configure(Configuration config) {
		config.objectClass(this).cascadeOnDelete(true);
	}

	protected void store() {
		membersFirst("membersFirst commit");
		membersFirst("membersFirst");

		twoRef("twoRef");
		twoRef("twoRef commit");
		twoRef("twoRef delete");
		twoRef("twoRef delete commit");
	}

	private void membersFirst(String name) {
		CascadeDeleteDeleted cdd = new CascadeDeleteDeleted(name);
		cdd.untypedMember = new CddMember();
		cdd.typedMember = new CddMember();
		db().store(cdd);
	}

	private void twoRef(String name) {
		CascadeDeleteDeleted cdd = new CascadeDeleteDeleted(name);
		cdd.untypedMember = new CddMember();
		cdd.typedMember = new CddMember();
		CascadeDeleteDeleted cdd2 = new CascadeDeleteDeleted(name);
		cdd2.untypedMember = cdd.untypedMember;
		cdd2.typedMember = cdd.typedMember;
		db().store(cdd);
		db().store(cdd2);

	}

	public void test() {
		tMembersFirst("membersFirst commit");
		tMembersFirst("membersFirst");
		tTwoRef("twoRef");
		tTwoRef("twoRef commit");
		tTwoRef("twoRef delete");
		tTwoRef("twoRef delete commit");
		Assert.areEqual(0, countOccurences(CddMember.class));
	}

	private void tMembersFirst(String name) {
		boolean commit = name.indexOf("commit") > 1;

		Query q = newQuery(this.getClass());
		q.descend("name").constrain(name);
		ObjectSet objectSet = q.execute();
		CascadeDeleteDeleted cdd = (CascadeDeleteDeleted) objectSet.next();
		db().delete(cdd.untypedMember);
		db().delete(cdd.typedMember);
		if (commit) {
			db().commit();
		}
		db().delete(cdd);
		if (!commit) {
			db().commit();
		}
	}

	private void tTwoRef(String name) {
		boolean commit = name.indexOf("commit") > 1;
		boolean delete = name.indexOf("delete") > 1;
		Query q = newQuery(this.getClass());
		q.descend("name").constrain(name);
		ObjectSet objectSet = q.execute();
		CascadeDeleteDeleted cdd = (CascadeDeleteDeleted) objectSet.next();
		CascadeDeleteDeleted cdd2 = (CascadeDeleteDeleted) objectSet.next();
		if (delete) {
			db().delete(cdd.untypedMember);
			db().delete(cdd.typedMember);
		}
		db().delete(cdd);
		if (commit) {
			db().commit();
		}
		db().delete(cdd2);
		if (!commit) {
			db().commit();
		}
	}
}
