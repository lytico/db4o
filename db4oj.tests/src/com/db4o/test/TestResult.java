/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.test;

public class TestResult {
	private int _numFailures;
	private long _timeTaken;
	private int _numAssertions;

	public TestResult(int numAssertions,int numFailures, long timeTaken) {
		_numAssertions = numAssertions;
		_numFailures = numFailures;
		_timeTaken = timeTaken;
	}

	public int numFailures() {
		return _numFailures;
	}

	public long timeTaken() {
		return _timeTaken;
	}

	public int numAssertions() {
		return _numAssertions;
	}
	
	public String toString() {
		return _numFailures+" failures, "+_numAssertions+" assertions, "+_timeTaken+" ms";
	}
}
