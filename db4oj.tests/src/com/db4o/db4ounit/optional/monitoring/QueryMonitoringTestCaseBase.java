/* Copyright (C) 2009 Versant Inc. http://www.db4o.com */

package com.db4o.db4ounit.optional.monitoring;

import com.db4o.query.Predicate;

@decaf.Remove(unlessCompatible=decaf.Platform.JDK15)
public abstract class QueryMonitoringTestCaseBase extends MBeanTestCaseBase {

	public static final class OptimizableQuery extends Predicate<Item> {
		@Override public boolean match(Item candidate) {
			return candidate._id.equals("foo");
		}
	}

	public static final class UnoptimizableQuery extends Predicate<Item> {
		@Override
		public boolean match(Item candidate) {
			return candidate._id.toLowerCase().equals("FOO");
		}
	}
	
	protected void triggerOptimizedQuery() {
		db().query(new OptimizableQuery()).toArray();
	}

	protected void triggerUnoptimizedQuery() {
		db().query(unoptimizableQuery()).toArray();
	}

	protected Predicate<Item> unoptimizableQuery() {
		return new UnoptimizableQuery();
	}
}
