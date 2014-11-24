/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package com.db4o.polepos.test.continuous;

import java.io.*;
import java.util.*;

import com.db4o.polepos.continuous.*;
import com.db4o.polepos.continuous.filealgebra.*;

import db4ounit.*;

public class RevisionBasedMostRecentJarFileSelectionStrategyTestCase implements TestCase {
	
	private static final int INITIAL_REVISION = 42;

	public void testIllegalConstructorArgs() {
		Assert.expect(IllegalArgumentException.class, new CodeBlock() {
			public void run() throws Throwable {
				strategy(null, new int[]{});
			}
		});
		Assert.expect(IllegalArgumentException.class, new CodeBlock() {
			public void run() throws Throwable {
				strategy(null, 1, -1);
			}
		});
	}

	public void testSelectsAll() {
		for(int numInputFiles = 2; numInputFiles < 10; numInputFiles++) {
			FileSource db4oJars = db4oJarSequence(INITIAL_REVISION, INITIAL_REVISION + numInputFiles);
			for(int numSelected = 2; numSelected <= numInputFiles; numSelected++) {
				List<File> actualOthers = strategy(db4oJars, createAllIndices(numSelected - 1)).files();
				List<File> expectedOthers = db4oJarSequence(INITIAL_REVISION + numInputFiles - numSelected, INITIAL_REVISION + numInputFiles - 1).files();
				IteratorAssert.sameContent(expectedOthers, actualOthers);
			}
		}
	}

	public void testSelectsFirstAndLast() {
		for(int numInputFiles = 2; numInputFiles < 10; numInputFiles++) {
			FileSource db4oJars = db4oJarSequence(INITIAL_REVISION, INITIAL_REVISION + numInputFiles);
			for(int numSelected = 2; numSelected <= numInputFiles; numSelected++) {
				List<File> actualOthers = strategy(db4oJars, numSelected - 1).files();
				List<File> expectedOthers = db4oJar(INITIAL_REVISION + numInputFiles - numSelected).files();
				IteratorAssert.sameContent(expectedOthers, actualOthers);
			}
		}
	}

	public void testSelectsSingleIntermediateJar() {
		FileSource inputFiles = files("foo", db4oJarName(42), "bar", db4oJarName(43), "baz", db4oJarName(44));
		List<File> otherJars = strategy(inputFiles, 1).files();
		IteratorAssert.sameContent(db4oJar(43).files(), otherJars);
	}
	
	public void testSelectsAvailableSubsetAndOldestForOneMissing() {
		FileSource inputFiles = files("foo", db4oJarName(42), "bar", db4oJarName(43), "baz", db4oJarName(44), db4oJarName(45));
		List<File> otherJars = strategy(inputFiles, 2, 5).files();
		IteratorAssert.sameContent(db4oJars(43, 42).files(), otherJars);
	}

	public void testSelectsAvailableSubsetAndOldestForTwoMissing() {
		FileSource inputFiles = files("foo", db4oJarName(42), "bar", db4oJarName(43), "baz", db4oJarName(44), db4oJarName(45));
		List<File> otherJars = strategy(inputFiles, 2, 5, 10).files();
		IteratorAssert.sameContent(db4oJars(43, 42).files(), otherJars);
	}

	private int[] createAllIndices(int length) {
		int[] indices = new int[length];
		for (int idx = 0; idx < length; idx++) {
			indices[idx] = idx + 1;
		}
		return indices;
	}
	
	private FileSource strategy(FileSource source, int... selectedIndices) {
		return new LenientIndexSelectingFileSource(new Db4oJarSortedFileSource(source), selectedIndices);
	}
	
	private String db4oJarName(int revision) {
		return "db4o-7.10.103." + revision + "-all-java5.jar";
	}

	private FileSource db4oJar(int rev) {
		return files(db4oJarNameSequence(rev, rev + 1));
	}

	private String[] db4oJarNames(int... revisions) {
		String[] names = new String[revisions.length];
		for (int fileIdx = 0; fileIdx < names.length; fileIdx++) {
			names[fileIdx] = db4oJarName(revisions[fileIdx]);
		}
		return names;
	}

	private FileSource db4oJars(int... revisions) {
		return files(db4oJarNames(revisions));
	}

	private FileSource db4oJarSequence(int from, int to) {
		return files(db4oJarNameSequence(from, to));
	}

	private String[] db4oJarNameSequence(int from, int to) {
		String[] names = new String[to - from];
		for (int fileIdx = 0; fileIdx < names.length; fileIdx++) {
			names[fileIdx] = db4oJarName(fileIdx + from);
		}
		return names;
	}
	
	private FileSource files(String... paths) {
		final List<File> files = new ArrayList<File>(paths.length);
		for (int fileIdx = 0; fileIdx < paths.length; fileIdx++) {
			files.add(new File(paths[fileIdx]));
		}
		return new FileSource() {
			public List<File> files() {
				return files;
			}
		};
	}

}
