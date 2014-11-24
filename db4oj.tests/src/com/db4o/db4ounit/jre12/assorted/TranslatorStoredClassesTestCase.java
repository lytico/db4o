/* Copyright (C) 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.jre12.assorted;

import java.io.*;
import java.math.*;

import com.db4o.*;
import com.db4o.config.*;
import com.db4o.db4ounit.common.api.*;

/**
 */
@decaf.Ignore(decaf.Platform.JDK11)
public class TranslatorStoredClassesTestCase extends Db4oTestWithTempFile {
	
	public static class DataRawChild implements Serializable {
		public int _id;

		public DataRawChild(int id) {
			_id=id;
		}
	}

	public static class DataRawParent {
		public DataRawChild _child;

		public DataRawParent(int id) {
			_child=new DataRawChild(id);
		}
	}

	public static class DataBigDecimal {
		public BigDecimal _bd;

		public DataBigDecimal(int id) {
			_bd=new BigDecimal(String.valueOf(id));
		}
	}
	
	public void testBigDecimal() {
		assertStoredClassesAfterTranslator(BigDecimal.class,new DataBigDecimal(42));
	}

	public void testRaw() {
		assertStoredClassesAfterTranslator(DataRawChild.class,new DataRawParent(42));
	}

	public void assertStoredClassesAfterTranslator(Class translated,Object data) {
		createFile(translated,data);
		check(translated);
	}

	private void createFile(Class translated,Object data) {
        ObjectContainer server = db(translated, new TSerializable());
        server.store(data);
        server.close();
	}

	private void check(Class translated) {
		ObjectContainer db=db(translated, null);
		db.ext().storedClasses();
		db.close();
	}

	private ObjectContainer db(Class translated, ObjectTranslator translator) {
		EmbeddedConfiguration config = newConfiguration();
		config.common().objectClass(translated).translate(translator);
		config.file().recoveryMode(translator == null); // Test expects translator validation to not thrown.
		
		return Db4oEmbedded.openFile(config, tempFile());
	}

}
