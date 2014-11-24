package com.db4o.devtools.ant;

import org.apache.tools.ant.Task;

public abstract class UniqueNameTaskBase extends Task {

	protected String _suffix;
	protected String _prefixes;

	public UniqueNameTaskBase() {
		super();
	}

	public void setSuffix(String suffix) {
		_suffix = suffix;
	}

	public void setPrefixes(String prefixes) {
		_prefixes = prefixes;
	}
	
	protected String propertyNameFor(String prefix) {
		return prefix + "." + _suffix;
	}
}