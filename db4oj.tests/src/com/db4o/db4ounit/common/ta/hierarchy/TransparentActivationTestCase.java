/* Copyright (C) 2007   Versant Inc.   http://www.db4o.com */
package com.db4o.db4ounit.common.ta.hierarchy;

import java.util.*;

import com.db4o.config.*;
import com.db4o.db4ounit.common.ta.*;
import com.db4o.db4ounit.common.ta.collections.*;

import db4ounit.*;

/**
 */
@decaf.Ignore(decaf.Platform.JDK11)
public class TransparentActivationTestCase
	extends TransparentActivationTestCaseBase {

	public static void main(String[] args) {
		new TransparentActivationTestCase().runAll();
	}
	
	private static final int PRIORITY = 42;

	protected void configure(Configuration config) {
		super.configure(config);
		config.add(new PagedListSupport());
	}
	
	protected void store() throws Exception {
		Project project = new PrioritizedProject("db4o",PRIORITY);
		project.logWorkDone(new UnitOfWork("ta kick-off", new Date(1000), new Date(2000)));
		store(project);
	}
	
	public void test() {
		final PrioritizedProject project = (PrioritizedProject) retrieveOnlyInstance(Project.class);
		
		Assert.areEqual(PRIORITY, project.getPriority());
		// Project.totalTimeSpent needs the UnitOfWork objects to be activated
		Assert.areEqual(1000, project.totalTimeSpent());
	}
}
