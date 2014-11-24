/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.ta.instrumentation.test.integration;

public class PrioritizedProject extends Project {

	private int _priority;
	
	public PrioritizedProject(String name, int priority) {
		super(name);
		_priority = priority;
	}

	public int getPriority() {
		return _priority;
	}
}
