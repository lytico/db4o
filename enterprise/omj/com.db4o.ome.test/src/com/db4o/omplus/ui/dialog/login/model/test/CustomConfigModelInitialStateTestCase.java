/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */
package com.db4o.omplus.ui.dialog.login.model.test;

import org.junit.*;

public class CustomConfigModelInitialStateTestCase extends CustomConfigModelTestCaseBase {

	private final static String INITIAL_JAR_PATH = "initial.jar";
	private final static String INITIAL_CONFIG_NAME = "InitialConfig";
	
	@Test
	public void testInitialState() throws Exception {
		assertCommit(canonicalPaths(INITIAL_JAR_PATH), arr(INITIAL_CONFIG_NAME));
	}

	@Test
	public void testAddJarsToInitialState() throws Exception {
		final String jarPath = "foo.jar";
		final String configName = "FooConfig";
		assertAddJars(canonicalPaths(jarPath), arr(configName), canonicalPaths(INITIAL_JAR_PATH), arr(INITIAL_CONFIG_NAME), arr(INITIAL_CONFIG_NAME));
		assertSelectConfigNames(canonicalPaths(INITIAL_JAR_PATH, jarPath), sort(INITIAL_CONFIG_NAME, configName), sort(INITIAL_CONFIG_NAME, configName));
		assertCommit(canonicalPaths(INITIAL_JAR_PATH, jarPath), sort(INITIAL_CONFIG_NAME, configName));
	}

	@Test
	public void testRemoveJarsFromInitialState() throws Exception {
		assertRemoveJars(canonicalPaths(INITIAL_JAR_PATH), new String[0], new String[0], new String[0]);
		assertCommit(new String[0], new String[0]);
	}

	@Override
	protected String[] initialConfigNames() {
		return new String[] { INITIAL_CONFIG_NAME };
	}

	@Override
	protected String[] initialJarPaths() {
		return new String[] { INITIAL_JAR_PATH };

	}
	
}
