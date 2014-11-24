/* Copyright (C) 2010  Versant Inc.   http://www.db4o.com */

package com.db4o.omplus.ui.dialog.login.model.test;

import static org.easymock.EasyMock.*;

import org.easymock.*;
import org.junit.*;

import com.db4o.omplus.connection.*;


public class CustomConfigModelTestCase extends CustomConfigModelTestCaseBase {

	@Test
	public void testAddJars() throws Exception {
		final String[] jarPaths = { "foo.jar", "bar.jar" };
		final String[] configNames = { "FooConfig", "BarConfig" };
		assertAddJars(jarPaths, configNames, new String[0], new String[0], new String[0]);	
		assertCommit(jarPaths, configNames);
	}

	@Test
	public void testAddDuplicates() throws Exception {
		final String[] jarPaths = { "foo.jar", "foo.jar" };
		final String[] configNames = { "FooConfig", "FooConfig" };
		assertAddJars(jarPaths, configNames, new String[0], new String[0], new String[0]);	
		assertCommit(canonicalPaths(jarPaths[0]), arr(configNames[0]));
	}

	@Test
	public void testAddJarsAndDeselectOne() throws Exception {
		final String[] jarPaths = { "foo.jar", "bar.jar" };
		final String[] configNames = { "FooConfig", "BarConfig" };
		assertAddJars(jarPaths, configNames, new String[0], new String[0], new String[0]);	
		assertSelectConfigNames(jarPaths, configNames, arr(configNames[0]));
		assertCommit(jarPaths, arr(configNames[0]));
	}

	@Test
	public void testMultipleAddJarsAndDeselectOne() throws Exception {
		final String firstJar = "foo.jar";
		final String firstConfigName = "FooConfig";
		assertAddJars(arr(firstJar), arr(firstConfigName), new String[0], new String[0], new String[0]);
		
		final String secondJar = "bar.jar";
		final String secondConfigName = "BarConfig";
		assertAddJars(arr(secondJar), arr(secondConfigName), arr(firstJar), arr(firstConfigName), arr(firstConfigName, secondConfigName));
		assertSelectConfigNames(arr(firstJar, secondJar), arr(firstConfigName, secondConfigName), arr(firstConfigName));
		assertCommit(arr(firstJar, secondJar), arr(firstConfigName));
	}

	@Test
	public void testDuplicateAddJars() throws Exception {
		final String firstJar = "foo.jar";
		final String firstConfigName = "FooConfig";
		assertAddJars(arr(firstJar), arr(firstConfigName), new String[0], new String[0], new String[0]);
		
		final String secondJar = "foo.jar";
		final String secondConfigName = "FooConfig";
		assertAddJars(arr(secondJar), arr(secondConfigName), arr(firstJar), arr(firstConfigName), arr(firstConfigName));
		assertCommit(arr(firstJar), arr(firstConfigName));
	}

	@Test
	public void testAddJarsFailure() throws Exception {
		assertAddJarsFailure(new String[] { "foo.jar", "bar.jar" }, 1);
		assertCommit(new String[0], new String[0]);
	}

	@Test
	public void testAddJarsFailureRetainsLastState() throws Exception {
		final String firstJar = "foo.jar";
		final String firstConfigName = "FooConfig";
		assertAddJars(arr(firstJar), arr(firstConfigName), new String[0], new String[0], new String[0]);
		assertAddJarsFailure(arr("bar.jar"), 0);
		assertCommit(arr(firstJar), arr(firstConfigName));
	}

	@Test
	public void testRemoveJars() throws Exception {
		final String retainedJar = "foo.jar";
		final String removedJar = "bar.jar";
		final String retainedConfigName = "FooConfig";
		final String removedConfigName = "BarConfig";
		assertAddJars(arr(retainedJar, removedJar), arr(retainedConfigName, removedConfigName), new String[0], new String[0], new String[0]);
		assertSelectConfigNames(arr(retainedJar, removedJar), arr(retainedConfigName, removedConfigName), arr(retainedConfigName));
		assertRemoveJars(arr(removedJar), arr(retainedJar), arr(retainedConfigName), arr(retainedConfigName));
		assertCommit(arr(retainedJar), arr(retainedConfigName));
	}

	@Test
	public void testMultipleRemoveJars() throws Exception {
		final String retainedJar = "foo.jar";
		final String removedJar = "bar.jar";
		final String retainedConfigName = "FooConfig";
		final String removedConfigName = "BarConfig";
		assertAddJars(arr(retainedJar, removedJar), arr(retainedConfigName, removedConfigName), new String[0], new String[0], new String[0]);
		assertRemoveJars(arr(removedJar), arr(retainedJar), arr(retainedConfigName), arr(retainedConfigName));
		assertRemoveJars(arr(retainedJar), new String[0], new String[0], new String[0]);
		assertCommit(new String[0], new String[0]);
	}

	@Test
	public void testConfigClassNameFailureOnAdd() throws Exception {
		final String jarPath = "foo.jar";
		expect(extractor.acceptJarFile(file(jarPath))).andReturn(true);
		expect(extractor.configuratorClassNames(file(jarPath))).andThrow(new DBConnectException(""));
		checkOrder(errSink, false);
		errSink.showError(EasyMock.<String>anyObject());
		errSink.showExc(EasyMock.<String>anyObject(), eqExc(DBConnectException.class));
		listener.customConfig(aryEq(canonicalPaths(jarPath)), aryEq(new String[0]), aryEq(new String[0]));
		replayMocks();
		model.addJarPaths("foo.jar");
		verifyMocks();
		resetMocks();
		
		assertCommit(canonicalPaths(jarPath), new String[0]);
	}

	@Test
	public void testConfigClassNameFailureOnRemove() throws Exception {
		final String retainedJar = "foo.jar";
		final String retainedConfigName = "FooConfig";
		final String removedJar = "bar.jar";
		final String removedConfigName = "BarConfig";
		assertAddJars(canonicalPaths(retainedJar, removedJar), arr(retainedConfigName, removedConfigName), new String[0], new String[0], new String[0]);
		expect(extractor.configuratorClassNames(file(retainedJar))).andThrow(new DBConnectException(""));
		checkOrder(errSink, false);
		errSink.showError(EasyMock.<String>anyObject());
		errSink.showExc(EasyMock.<String>anyObject(), eqExc(DBConnectException.class));
		listener.customConfig(aryEq(canonicalPaths(retainedJar)), aryEq(new String[0]), aryEq(new String[0]));
		replayMocks();
		model.removeJarPaths(removedJar);
		verifyMocks();
		resetMocks();
		assertCommit(canonicalPaths(retainedJar), new String[0]);
	}

	@Test
	public void testPreselectConfigNamesOnFirstAdd() {
		
	}
	
	@Override
	protected String[] initialConfigNames() {
		return new String[0];
	}

	@Override
	protected String[] initialJarPaths() {
		return new String[0];
	}
	
}
