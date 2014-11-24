/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package com.db4o.omplus.connection.test;

import static org.junit.Assert.*;
import static com.db4o.omplus.test.util.Db4oTestUtil.*;

import java.io.*;
import java.util.*;

import org.hamcrest.*;
import org.junit.*;

import com.db4o.*;
import com.db4o.ext.*;
import com.db4o.internal.*;
import com.db4o.omplus.connection.*;

public class FileConnectionParamsTestCase {

	@Test
	public void testNotFound() throws IOException {
		File file = nonExistentFile();
		try {
			new FileConnectionParams(file.getAbsolutePath()).connect();
			fail();
		}
		catch(DBConnectException exc) {
			FileNotFoundException cause = (FileNotFoundException) exc.getCause();
			assertEquals(file.getAbsolutePath(), cause.getMessage());
		}
	}

	@Test
	public void testLocked() throws IOException {
		File file = nonExistentFile();
		LocalObjectContainer db = (LocalObjectContainer) Db4oEmbedded.openFile(Db4oEmbedded.newConfiguration(), file.getAbsolutePath());
		try {
			new FileConnectionParams(file.getAbsolutePath()).connect();
			fail();
		}
		catch(DBConnectException exc) {
			assertThat(exc.getCause(), CoreMatchers.instanceOf(DatabaseFileLockedException.class));
		}
		finally {
			db.close();
			file.delete();
		}
	}

	@Test
	public void testOpen() throws Exception {
		File file = createEmptyDatabase();
		try {
			LocalObjectContainer opened = (LocalObjectContainer) new FileConnectionParams(file.getAbsolutePath()).connect();
			try {
				assertEquals(file.getPath(), opened.fileName());
				assertFalse(opened.config().isReadOnly());
			}
			finally {
				opened.close();
			}
		}
		finally {
			file.delete();
		}
	}

	@Test
	public void testOpenReadOnly() throws Exception {
		File file = createEmptyDatabase();
		try {
			LocalObjectContainer opened = (LocalObjectContainer) new FileConnectionParams(file.getAbsolutePath(), true).connect();
			try {
				assertEquals(file.getPath(), opened.fileName());
				assertTrue(opened.config().isReadOnly());
			}
			finally {
				opened.close();
			}
		}
		finally {
			file.delete();
		}
	}

	@Test
	public void testEquals() {
		String[] paths = { "foo.db4o", "bar.db4o" };
		boolean[] readOnly = { true, false };
		String[][] jarPaths = { {}, { "baz.jar" } };
		String[] configNames = { "BazConfig" };
		List<List<FileConnectionParams>> params = new ArrayList<List<FileConnectionParams>>();
		List<List<FileConnectionParams>> clones = new ArrayList<List<FileConnectionParams>>();
		for (String path : paths) {
			List<FileConnectionParams> curParams = new ArrayList<FileConnectionParams>();
			List<FileConnectionParams> curClones = new ArrayList<FileConnectionParams>();
			for (boolean ro : readOnly) {
				for (String[] jarPath : jarPaths) {
					curParams.add(new FileConnectionParams(path, ro, Arrays.copyOf(jarPath, jarPath.length), new String[0]));
					curClones.add(new FileConnectionParams(path, ro, Arrays.copyOf(jarPath, jarPath.length), new String[0]));
					if(jarPath.length > 0) {
						curParams.add(new FileConnectionParams(path, ro, Arrays.copyOf(jarPath, jarPath.length), Arrays.copyOf(configNames, configNames.length)));
						curClones.add(new FileConnectionParams(path, ro, Arrays.copyOf(jarPath, jarPath.length), Arrays.copyOf(configNames, configNames.length)));
					}
				}
			}
			params.add(curParams);
			clones.add(curClones);
		}
		for (int outer = 0; outer < params.size(); outer++) {
			assertEquals(clones.get(outer), params.get(outer));
			assertEquals(clones.get(outer).hashCode(), params.get(outer).hashCode());
			for (int inner = 0; inner < params.size(); inner++) {
				if(outer == inner) {
					continue;
				}
				assertFalse(params.get(outer).equals(params.get(inner)));
			}				
		}
	}
}
