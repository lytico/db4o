package com.db4o.instrumentation.test.core;

import java.io.*;
import java.util.*;

import com.db4o.foundation.io.*;
import com.db4o.instrumentation.file.*;

import db4ounit.*;
import db4ounit.extensions.util.*;

public class DefaultFilePathRootTestCase implements TestLifeCycle {

	private static final File BASEDIR = new File(Path4.getTempPath(), "filePathRoot");
	
	public void test() throws IOException {
		File dirA = mkdir(BASEDIR, "a");
		File dirB = mkdir(BASEDIR, "b");
		File dirC = mkdir(dirB, "c");
		File fileA = createFile(dirA, "a.txt");
		File fileB = createFile(dirB, "b.txt");
		File fileC = createFile(dirC, "c.txt");
		File fileD = createFile(dirC, "d.txt");
		createFile(dirA, "a.txt.bkp");
		String[] rootDirs = new String[]{ dirA.getAbsolutePath(), dirB.getAbsolutePath() };
		FilePathRoot root = new DefaultFilePathRoot(rootDirs, ".txt");
		ArrayAssert.areEqual(rootDirs, root.rootDirs());
		List files = new ArrayList();
		for(Iterator fileIter = root.iterator(); fileIter.hasNext(); ) {
			files.add(fileIter.next());
		}
		Collections.sort(files);
		InstrumentationClassSource[] expected = { 
				new FileInstrumentationClassSource(dirA, fileA), 
				new FileInstrumentationClassSource(dirB, fileB), 
				new FileInstrumentationClassSource(dirB, fileC), 
				new FileInstrumentationClassSource(dirB, fileD), 
		};
		InstrumentationClassSource[] actual = (InstrumentationClassSource[]) files.toArray(new InstrumentationClassSource[files.size()]);
		ArrayAssert.areEqual(expected, actual);
	}

	public void setUp() throws Exception {
		IOUtil.deleteDir(BASEDIR.getAbsolutePath());
		BASEDIR.mkdirs();
	}

	public void tearDown() throws Exception {
		IOUtil.deleteDir(BASEDIR.getAbsolutePath());
	}

	private File mkdir(File base, String name) {
		File dir = new File(base, name);
		dir.mkdir();
		return dir;
	}

	private File createFile(File base, String name) throws IOException {
		File file = new File(base, name);
		file.createNewFile();
		return file;
	}
}
