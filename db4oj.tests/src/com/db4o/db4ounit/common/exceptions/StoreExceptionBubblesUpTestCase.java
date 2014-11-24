/* Copyright (C) 2007 Versant Inc. http://www.db4o.com */

package com.db4o.db4ounit.common.exceptions;

import com.db4o.*;
import com.db4o.config.*;
import com.db4o.internal.*;

import db4ounit.*;
import db4ounit.extensions.*;


public class StoreExceptionBubblesUpTestCase extends AbstractDb4oTestCase {
	
	public static void main(String[] args) {
		new StoreExceptionBubblesUpTestCase().runNetworking();
	}
	
	public static final class ItemTranslator implements ObjectTranslator {

		public void onActivate(ObjectContainer container,
				Object applicationObject, Object storedObject) {		
		}

		public Object onStore(ObjectContainer container,
				Object applicationObject) {
			throw new ItemException();
		}

		public Class storedClass() {
			return Item.class;
		}
		
	}
	
	protected void configure(Configuration config) {
		config.objectClass(Item.class).translate(new ItemTranslator());
	}
	
	public void test() {
		CodeBlock exception = new CodeBlock() {
					public void run() throws Throwable {
						store(new Item());
					}
				};
		Assert.expect(ReflectException.class, exception);
	}

}
