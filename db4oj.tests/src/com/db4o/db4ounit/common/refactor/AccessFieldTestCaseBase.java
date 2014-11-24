/* Copyright (C) 2009  Versant Inc.  http://www.db4o.com */
package com.db4o.db4ounit.common.refactor;

import com.db4o.*;
import com.db4o.config.*;
import com.db4o.db4ounit.common.api.*;
import com.db4o.ext.*;

import db4ounit.*;

public abstract class AccessFieldTestCaseBase extends Db4oTestWithTempFile {

	public void setUp() throws Exception {
		withDatabase(new DatabaseAction() {
			public void runWith(ObjectContainer db) {
				db.store(newOriginalData());
			}
		});
	}

	protected void renameClass(Class origClazz, String targetName) {
		EmbeddedConfiguration config = newConfiguration();
		config.common().objectClass(origClazz).rename(targetName);
		withDatabase(config, new DatabaseAction() {
			public void runWith(ObjectContainer db) {
				// do nothing
			}
		});
	}

	protected abstract Object newOriginalData();

	protected <T, F> void assertField(final Class<T> targetClazz, final String fieldName, final Class<F> fieldType,
			final F fieldValue) {
				withDatabase(new DatabaseAction() {
					public void runWith(ObjectContainer db) {
						StoredClass storedClass = db.ext().storedClass(targetClazz);
						StoredField storedField = storedClass.storedField(fieldName, fieldType);
						ObjectSet<T> result = db.query(targetClazz);
						Assert.areEqual(1, result.size());
						T obj = result.next();
						F value = (F)storedField.get(obj);
						Assert.areEqual(fieldValue, value);
					}
				});
			}

	private static interface DatabaseAction {
		void runWith(ObjectContainer db);
	}

	private void withDatabase(DatabaseAction action) {
		withDatabase(newConfiguration(), action);
	}

	private void withDatabase(EmbeddedConfiguration config, DatabaseAction action) {
		ObjectContainer db = Db4oEmbedded.openFile(config, tempFile());
		try {
			action.runWith(db);
		}
		finally {
			db.close();
		}
	}

}