package com.db4o.db4ounit.common.exceptions.propagation;

import db4ounit.fixtures.*;

public abstract class TopLevelOperation implements Labeled {
	private final String _label;
	
	public TopLevelOperation(String label) {
		_label = label;
	}

	public abstract void apply(DatabaseContext context);
	
	public String label() {
		return _label;
	}
	
}
