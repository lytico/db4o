/* Copyright (C) 2009 Versant Inc.   http://www.db4o.com */
package com.db4o.db4ounit.common.defragment;

import java.io.*;

import com.db4o.*;
import com.db4o.defragment.*;
import com.db4o.ext.*;
import com.db4o.foundation.io.*;
import com.db4o.reflect.*;

import db4ounit.*;

public class StoredClassFilterTestCase extends DefragmentTestCaseBase {

	public static class SimpleClass {
		public String _simpleField;
		public SimpleClass(String simple){
			_simpleField = simple;
		}
	}
	
	public static void main(String[] args) {
		new ConsoleTestRunner(StoredClassFilterTestCase.class).run();
	}
	
	public void test() throws Exception {
		deleteAllFiles();
		String fname = createDatabase();
		defrag(fname);
		assertStoredClasses(fname);
	}

	private void deleteAllFiles() {
		File4.delete(sourceFile());
		File4.delete(backupFile());		
	}

	private void assertStoredClasses(String fname) {
		ObjectContainer db = Db4oEmbedded.openFile(newConfiguration(), fname);
		try {
			ReflectClass[] knownClasses = db.ext().knownClasses();
			assertKnownClasses(knownClasses);
		} finally {
			db.close();
		}
	}

	private void assertKnownClasses(ReflectClass[] knownClasses) {
		for (int i = 0; i < knownClasses.length; i++) {
			Assert.areNotEqual(fullyQualifiedName(SimpleClass.class), knownClasses[i].getName());
		}
	}

	private String fullyQualifiedName(Class klass) {
		return db4ounit.extensions.util.CrossPlatformServices.fullyQualifiedName(klass);
	}

	private void defrag(String fname) throws IOException {
		DefragmentConfig config = new DefragmentConfig(fname);
		config.db4oConfig(newConfiguration());
		config.storedClassFilter(ignoreClassFilter(SimpleClass.class));
		Defragment.defrag(config);
	}
	
	private StoredClassFilter ignoreClassFilter(final Class klass) {
		return new StoredClassFilter(){
			public boolean accept(StoredClass storedClass) {
				return !storedClass.getName().equals(fullyQualifiedName(klass));
			}
		};
	}

	private String createDatabase() {
		String fname = sourceFile();
		ObjectContainer db = Db4oEmbedded.openFile(newConfiguration(), fname);
		try {
			db.store(new SimpleClass("verySimple"));
			db.commit();
		} finally {
			db.close();
		}
		return fname;
	}	

}
