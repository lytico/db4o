/**
 * 
 */
package com.db4o.junit.launcher;

import java.io.PrintWriter;
import java.util.Collection;

import db4ounit.TestSuiteBuilder;
import db4ounit.extensions.fixtures.Db4oAndroid;

public final class SelectiveDb4oAndroidFixture extends Db4oAndroid {
	private final Collection<String> ignoreList;
	private final PrintWriter out;
	private final PrintWriter acceptedClasses;

	public SelectiveDb4oAndroidFixture(PrintWriter acceptedClasses, PrintWriter out, Collection<String> ignoreList) {
		this.acceptedClasses = acceptedClasses;
		this.out = out;
		this.ignoreList = ignoreList;
	}

	@SuppressWarnings("unchecked")
	@Override
	public boolean accept(Class clazz) {
		
		if (true) return super.accept(clazz);
		
		if (!super.accept(clazz)) return false;
		
		String name = clazz.getName();
		
		if (!TestSuiteBuilder.class.isAssignableFrom(clazz) && ignoreList.contains(name)) {
			out.println("IGNORING: " + name);
			return false;
		}
		
		if (acceptedClasses != null && !name.contains("AllTest")) {
			acceptedClasses.println(name);
			acceptedClasses.flush();
		}
		
		return true;
	}
}