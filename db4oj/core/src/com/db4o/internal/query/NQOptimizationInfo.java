/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.internal.query;

import com.db4o.query.*;

public class NQOptimizationInfo {
	private Predicate _predicate;
	private String _message;
	private Object _optimized;

	public NQOptimizationInfo(Predicate predicate, String message, Object optimized) {
		this._predicate = predicate;
		this._message = message;
		this._optimized = optimized;
	}

	public String message() {
		return _message;
	}

	public Object optimized() {
		return _optimized;
	}

	public Predicate predicate() {
		return _predicate;
	}

	public String toString() {
		return message()+"/"+optimized();
	}
}
