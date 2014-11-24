/* Copyright (C) 2009   Versant Inc.   http://www.db4o.com */
package com.db4o.db4ounit.common.assorted;

import java.util.*;

import com.db4o.config.*;
import com.db4o.db4ounit.common.handlers.*;
import com.db4o.ext.*;
import com.db4o.foundation.*;
import com.db4o.internal.*;
import com.db4o.reflect.*;

import db4ounit.*;

/**
 * @sharpen.ignore
 */
@decaf.Remove(decaf.Platform.JDK11)
public class KnownClassesMigrationTestCase extends FormatMigrationTestCaseBase {

	@Override
	protected void assertObjectsAreReadable(ExtObjectContainer objectContainer) {
		if (isVersionWithoutTCollection())
			return;
		
		final ReflectClass[] knownClasses = objectContainer.knownClasses();
		
		Assert.isNotNull(knownClasses);
		Assert.isGreater(2, knownClasses.length);
		
		ReflectClass type = objectContainer.reflector().forClass(TCollection.class);
		Assert.isGreaterOrEqual(0, Arrays4.indexOfIdentity(knownClasses, type));
	}

	private boolean isVersionWithoutTCollection() {
		return db4oMajorVersion() < 5;
	}

	@Override
	protected String fileNamePrefix() {
		return "KnownClasses";
	}

	@Override
	protected void store(ObjectContainerAdapter objectContainer) {
		objectContainer.store(new Item(new LinkedList()));
	}
	
	private static class Item {
		public Item(LinkedList list) {
			_list = list;
		}
		
		public LinkedList _list;
	}

}
