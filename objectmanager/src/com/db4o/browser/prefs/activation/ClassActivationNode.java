/*
 * Copyright (C) 2005 db4objects Inc.  http://www.db4o.com
 */
package com.db4o.browser.prefs.activation;

/**
 * ClassActivationNode.  Represents class activation levels
 *
 * @author djo
 */
public class ClassActivationNode {
	public final String className;
	public int level;
	
	public ClassActivationNode(String className, int level) {
		this.className = className;
		this.level = level;
	}
}
