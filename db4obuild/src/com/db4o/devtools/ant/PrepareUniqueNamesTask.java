package com.db4o.devtools.ant;

import org.apache.tools.ant.BuildException;

public class PrepareUniqueNamesTask extends UniqueNameTaskBase {
	
	@Override
	public void execute() throws BuildException {
		for (String prefix : _prefixes.split(",")) {
			getProject().setProperty(propertyNameFor(prefix), uniqueName(prefix));			
		}
	}

	private String uniqueName(String prefix) {
		return "_" + System.currentTimeMillis() + propertyNameFor(prefix);
	}
}
