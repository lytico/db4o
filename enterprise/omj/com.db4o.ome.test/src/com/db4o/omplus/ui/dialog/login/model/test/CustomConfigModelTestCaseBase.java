/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */
package com.db4o.omplus.ui.dialog.login.model.test;

import static org.easymock.EasyMock.*;

import java.io.*;
import java.util.*;

import org.easymock.*;
import org.junit.*;

import com.db4o.omplus.*;
import com.db4o.omplus.ui.dialog.login.model.*;
import com.db4o.omplus.ui.dialog.login.model.CustomConfigModel.CustomConfigListener;

public abstract class CustomConfigModelTestCaseBase {

	protected CustomConfigModel model;

	protected CustomConfigListener listener;
	private CustomConfigSink sink;
	protected ConfiguratorExtractor extractor;
	protected ErrorMessageSink errSink;

	protected static class ExceptionMatcher<T extends Throwable> implements IArgumentMatcher {
		private Class<T> expected;
		
		public ExceptionMatcher(Class<T> expected) {
			this.expected = expected;
		}
		
		@Override
		public void appendTo(StringBuffer str) {
			str.append("eqExc(" + expected.getName() + ")");
		}

		@Override
		public boolean matches(Object other) {
			return other != null && expected.isAssignableFrom(other.getClass());
		}
	}

	@Before
	public void setUp() {
		sink = createMock(CustomConfigSink.class);
		listener = createMock(CustomConfigListener.class);
		extractor = createMock(ConfiguratorExtractor.class);
		errSink = createMock(ErrorMessageSink.class);
		model = createModel(new ErrorMessageHandler(errSink));
	}

	public static <T extends Throwable> T eqExc(Class<T> expected) {
	    reportMatcher(new ExceptionMatcher<T>(expected));
	    return null;
	}

	public CustomConfigModelTestCaseBase() {
		super();
	}

	protected void assertCommit(String[] jarPaths, String[] configNames) throws IOException {
		sink.customConfig(aryEq(canonicalPaths(jarPaths)), aryEq(sort(configNames)));
		replayMocks();
		model.commit();
		verifyMocks();
	}

	protected void assertAddJars(String[] jarPaths, String[] configNames, String[] existingJarPaths, String[] existingConfigNames, String[] selectedConfigNames) throws Exception {
		final List<String> allJarsList = concatUnique(canonicalPaths(jarPaths), canonicalPaths(existingJarPaths));
		final List<String> allConfigNamesList = concatUnique(configNames, existingConfigNames);
		final List<String> allSelectedConfigNamesList = concatUnique(configNames, selectedConfigNames);
		final String[] allJars = allJarsList.toArray(new String[allJarsList.size()]);
		final String[] allConfigNames = allConfigNamesList.toArray(new String[allConfigNamesList.size()]);
		final String[] allSelectedConfigNames = allSelectedConfigNamesList.toArray(new String[allSelectedConfigNamesList.size()]);
				
		listener.customConfig(aryEq(allJars), aryEq(allConfigNames), aryEq(allSelectedConfigNames));
		for (String jarName : jarPaths) {
			expectAcceptJarFileInvocation(jarName, true);
		}
		for (String curJar : allJars) {
			expect(extractor.configuratorClassNames(eq(file(curJar)))).andReturn(allConfigNamesList);
		}
		replayMocks();
		model.addJarPaths(jarPaths);
		verifyMocks();
		resetMocks();
	}

	protected void assertAddJarsFailure(String[] jarPaths, int failureIndex) throws IOException {
		for (int jarIdx = 0; jarIdx < failureIndex; jarIdx++) {
			expectAcceptJarFileInvocation(jarPaths[jarIdx], true);
		}
		expectAcceptJarFileInvocation(jarPaths[failureIndex], false);
		errSink.showError(EasyMock.<String>anyObject());
		replayMocks();
		model.addJarPaths(jarPaths);
		verifyMocks();
		resetMocks();		
	}

	protected void assertRemoveJars(String[] removedJars, String[] retainedJars, String[] retainedConfigNames, String[] selectedRetainedConfigNames) throws Exception {
		for (String curJar : retainedJars) {
			expect(extractor.configuratorClassNames(file(curJar))).andReturn(Arrays.asList(retainedConfigNames));
		}
		listener.customConfig(aryEq(canonicalPaths(retainedJars)), aryEq(retainedConfigNames), aryEq(selectedRetainedConfigNames));
		replayMocks();
		
		model.removeJarPaths(removedJars);
		verifyMocks();
		resetMocks();
	}

	protected void assertSelectConfigNames(String[] jarPaths, String[] configNames, String[] selectedConfigNames) throws IOException {
		listener.customConfig(aryEq(canonicalPaths(jarPaths)), aryEq(sort(configNames)), aryEq(sort(selectedConfigNames)));
		replayMocks();		
		model.selectConfigClassNames(selectedConfigNames);
		verifyMocks();
		resetMocks();
	}

	private void expectAcceptJarFileInvocation(final String jarPath, final boolean retVal) throws IOException {
		expect(extractor.acceptJarFile(file(jarPath))).andReturn(retVal);
	}

	protected void replayMocks() {
		replay(listener, sink, extractor, errSink);
	}

	protected void verifyMocks() {
		verify(listener, sink, extractor, errSink);
	}

	protected void resetMocks() {
		reset(listener, sink, extractor, errSink);
	}

	private List<File> files(String... paths) throws IOException {
		List<File> files = new ArrayList<File>(paths.length);
		for (String path : paths) {
			files.add(file(path));
		}
		Collections.sort(files);
		return files;
	}

	protected File file(final String path) throws IOException {
		return new File(path).getCanonicalFile();
	}

	protected String[] canonicalPaths(String... paths) throws IOException {
		final List<File> files = files(paths);
		String[] canonicalPaths = new String[files.size()];
		for (int pathIdx = 0; pathIdx < files.size(); pathIdx++) {
			canonicalPaths[pathIdx] = files.get(pathIdx).getAbsolutePath();
		}
		return canonicalPaths;
	}

	protected <T extends Comparable<T>> T[] sort(T... arr) {
		final T[] copy = Arrays.copyOf(arr, arr.length);
		Arrays.sort(copy);
		return copy;
	}

	protected <T> T[] arr(T... arr) {
		return arr;
	}

	private <T extends Comparable<T>> List<T> concatUnique(T[] first, T[] second) {
		Set<T> set = new HashSet<T>();
		set.addAll(Arrays.asList(first));
		set.addAll(Arrays.asList(second));
		List<T> list = new ArrayList<T>(set);
		Collections.sort(list);
		return list;
	}

	protected CustomConfigModel createModel(ErrorMessageHandler errHandler) {
		try {
			String[] initialJarPaths = initialJarPaths();
			String[] initialConfigNames = initialConfigNames();
			for (String jarPath : initialJarPaths) {
				expect(extractor.acceptJarFile(file(jarPath))).andReturn(true);
			}
			for (String jarPath : initialJarPaths) {
				expect(extractor.configuratorClassNames(file(jarPath))).andReturn(Arrays.asList(initialConfigNames));
			}
			replayMocks();
			CustomConfigModel model = new CustomConfigModelImpl(initialJarPaths, initialConfigNames, sink, extractor, errHandler);
			verifyMocks();
			resetMocks();

			listener.customConfig(aryEq(canonicalPaths(initialJarPaths)), aryEq(sort(initialConfigNames)), aryEq(sort(initialConfigNames)));
			replayMocks();
			model.addListener(listener);
			verifyMocks();
			resetMocks();

			return model;
		} 
		catch (Exception exc) {
			throw new RuntimeException(exc);
		} 
	}

	protected abstract String[] initialConfigNames();

	protected abstract String[] initialJarPaths();

}