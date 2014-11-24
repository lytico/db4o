package com.db4o.db4ounit.jre12.assorted;

import com.db4o.*;
import com.db4o.config.*;
import com.db4o.foundation.io.*;
import com.db4o.query.*;
import com.db4o.reflect.*;
import com.db4o.reflect.generic.*;
import com.db4o.reflect.jdk.*;

import db4ounit.*;
import db4ounit.extensions.util.*;

/**
 * @sharpen.ignore
 */
@decaf.Ignore(decaf.Platform.JDK11)
public class GenericPrimitiveArrayTestCase implements TestCase {

	private static final byte[] BYTES = new byte[]{1,2};

	public static class Data {
		public byte[] _bytes;

		public Data(byte[] bytes) {
			_bytes = bytes;
		}
	}
	
	public void testGenericPrimitiveArray() {
		final String filePath = Path4.combine(Path4.getTempPath(), "generic.db4o");
		store(filePath);
		ClassLoader loader = new ExcludingClassLoader(Data.class.getClassLoader(), new Class[]{Data.class});
		Configuration config = Db4o.newConfiguration();
		config.reflectWith(new JdkReflector(loader));
		ObjectContainer db = Db4o.openFile(config, filePath);
		GenericReflector reflector = db.ext().reflector();
		ReflectClass clazz = reflector.forName(Data.class.getName());
		ReflectField field = clazz.getDeclaredField("_bytes");
		Assert.isTrue(field.getFieldType().isArray());
		Query query = db.query();
		query.constrain(clazz);
		ObjectSet result = query.execute();
		Assert.areEqual(1, result.size());
		Object retrieved = result.next();
		Assert.areEqual(clazz, reflector.forObject(retrieved));
		byte[] bytes = (byte[]) field.get(retrieved);
		ArrayAssert.areEqual(BYTES, bytes);
		db.close();
	}

	private void store(final String filePath) {
		File4.delete(filePath);
		ObjectContainer db = Db4o.openFile(Db4o.newConfiguration(), filePath);
		db.store(new Data(BYTES));
		db.close();
	}

	public static void main(String[] args) {
		new ConsoleTestRunner(GenericPrimitiveArrayTestCase.class).run();
	}
}
