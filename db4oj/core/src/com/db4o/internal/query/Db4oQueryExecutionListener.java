/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.internal.query;


public interface Db4oQueryExecutionListener {
	void notifyQueryExecuted(NQOptimizationInfo info);
}
