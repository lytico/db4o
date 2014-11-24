/* Copyright (C) 2007 Versant Inc. http://www.db4o.com */

package com.db4o.db4ounit.common.exceptions;

import com.db4o.*;
import com.db4o.config.*;
import com.db4o.internal.*;

import db4ounit.*;
import db4ounit.extensions.*;
import db4ounit.extensions.fixtures.*;


public class StoredClassExceptionBubblesUpTestCase extends AbstractDb4oTestCase implements OptOutMultiSession, OptOutDefragSolo {
	
	public static void main(String[] args) {
		new StoredClassExceptionBubblesUpTestCase().runSolo();
	}
	
	public static final class ItemTranslator implements ObjectTranslator {

		public void onActivate(ObjectContainer container,
				Object applicationObject, Object storedObject) {		
		}

		public Object onStore(ObjectContainer container,
				Object applicationObject) {
			return null;
		}

		public Class storedClass() {
			throw new ItemException();
		}
		
	}
	
	protected void configure(Configuration config) {
		config.objectClass(Item.class).translate(new ItemTranslator());
	}
	
	public void test() {
		Assert.expect(ReflectException.class, ItemException.class,
				new CodeBlock() {
					public void run() throws Throwable {
						store(new Item());
					}
				});
	}

}
