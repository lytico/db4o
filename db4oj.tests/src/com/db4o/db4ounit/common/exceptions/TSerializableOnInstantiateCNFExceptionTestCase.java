/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.exceptions;

import java.io.*;

import com.db4o.config.*;
import com.db4o.internal.*;

import db4ounit.*;
import db4ounit.extensions.*;
import db4ounit.extensions.fixtures.*;


public class TSerializableOnInstantiateCNFExceptionTestCase extends
		AbstractDb4oTestCase implements OptOutDefragSolo {

	public static void main(String[] args) {
		new TSerializableOnInstantiateCNFExceptionTestCase().runAll();
	}

	/**
	 * @sharpen.ignore
	 */
	
	public static class SerializableItem implements Serializable {
		private void readObject(ObjectInputStream in)
				throws IOException, ClassNotFoundException {
			throw new ClassNotFoundException();
		}
	}

	/**
	 * @sharpen.ignore
	 */
	protected void configure(Configuration config) {
		config.objectClass(SerializableItem.class).translate(
				new TSerializable());
	}

	/**
	 * @sharpen.ignore
	 */
	protected void store() throws Exception {
		store(new SerializableItem());
	}

	/**
	 * @sharpen.ignore
	 */
	public void testOnInstantiateException() {
		Assert.expect(ReflectException.class, ClassNotFoundException.class,
				new CodeBlock() {
					public void run() throws Throwable {
						TSerializableOnInstantiateCNFExceptionTestCase.this
								.retrieveOnlyInstance(SerializableItem.class);
					}
				});
	}
}
