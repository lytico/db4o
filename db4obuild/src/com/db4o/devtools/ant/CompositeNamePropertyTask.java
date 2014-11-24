/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package com.db4o.devtools.ant;

import java.util.regex.*;

import org.apache.tools.ant.*;

/**
 * Handles nested property references like "x.${y.${z}}"
 */
public class CompositeNamePropertyTask extends Task {

	private final Pattern pattern = Pattern.compile("\\$\\{([^\\{\\}]+)\\}");
	private String _name;
	private String _value;
	
	public void setName(String name) {
		_name = name;
	}
	
	public void setValue(String value) {
		_value = value;
	}
	
	@Override
	public void execute() throws BuildException {
		String name = expand(_name);
		String value = expand(_value);
		getProject().setProperty(name, value);
	}
	
	private String expand(String orig) {
		String str = orig;
		boolean matchFound = false;
		do {
			matchFound = false;
			Matcher matcher = pattern.matcher(str);
			int idx = 0;
			while(matcher.find(idx)) {
				String inner = matcher.group(1);
				str = str.substring(0, matcher.start()) + getProject().getProperty(inner) + str.substring(matcher.end());
				idx = matcher.end() + 1;
				matchFound = true;
			}
		} while(matchFound);
		return str;
	}
	
}
