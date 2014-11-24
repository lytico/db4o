/* Copyright (C) 2009 Versant Inc. http://www.db4o.com */
package com.db4o.db4ounit.jre12.ta;

import com.db4o.*;
import com.db4o.config.*;
import com.db4o.db4ounit.common.api.*;
import com.db4o.ext.*;
import com.db4o.reflect.generic.*;
import com.db4o.reflect.jdk.*;
import com.db4o.ta.*;

import db4ounit.*;
import db4ounit.extensions.util.*;

/**
 * @sharpen.ignore
 */
@decaf.Ignore(decaf.Platform.JDK11)
public class TAVirtualFieldTestCase extends Db4oTestWithTempFile {
	
	public static void main(String[] args) {
		new ConsoleTestRunner(TAVirtualFieldTestCase.class).run();
	}
	
	private Db4oUUID _uuid;
	
	public static class Item {
		public Item _next;
	}
	
	public void test() {
		ObjectContainer db = Db4oEmbedded.openFile(config(true), tempFile());
		ObjectSet result = db.query(Item.class);
		Assert.areEqual(1, result.size());
		Object obj = result.next();
		Assert.isInstanceOf(GenericObject.class, obj);
		Assert.areEqual(_uuid, db.ext().getObjectInfo(obj).getUUID());
		db.close();
	}

	public void setUp() throws Exception {
		ObjectContainer db = Db4oEmbedded.openFile(config(false), tempFile());
		Item obj = new Item();
		db.store(obj);
		_uuid = db.ext().getObjectInfo(obj).getUUID();
		db.close();
	}

	private EmbeddedConfiguration config(boolean withCL) {
		EmbeddedConfiguration config = newConfiguration();
		config.file().generateUUIDs(ConfigScope.GLOBALLY);
		config.common().add(new TransparentActivationSupport());
		if(withCL) {
			ClassLoader cl = new ExcludingClassLoader(Item.class.getClassLoader(), new Class[] { Item.class });
			config.common().reflectWith(new JdkReflector(cl));
		}
		return config;
	}
}