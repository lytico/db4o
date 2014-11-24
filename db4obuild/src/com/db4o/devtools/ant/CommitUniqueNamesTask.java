package com.db4o.devtools.ant;

import org.apache.tools.ant.BuildException;

public class CommitUniqueNamesTask extends UniqueNameTaskBase {
	@Override
	public void execute() throws BuildException {
		for (String prefix : _prefixes.split(",")) {
			getProject().setProperty(prefix, propertyValue(prefix));
		}
	}

	private String propertyValue(String prefix) {
		return getProject().getProperty(getProject().getProperty(propertyNameFor(prefix)));
	}
}
