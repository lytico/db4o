package com.db4o.db4ounit.common.reflect;

import com.db4o.ext.*;
import com.db4o.internal.*;
import com.db4o.reflect.*;

import db4ounit.*;

public class ReflectFieldExceptionTestCase implements TestCase {

	public static class Item {
		public String _name;
	}
	
	public void testExceptionIsPropagated() {
		Reflector reflector = Platform4.reflectorForType(Item.class);
		final ReflectField field = reflector.forClass(Item.class).getDeclaredField("_name");
		Assert.expect(Db4oException.class, IllegalArgumentException.class, new CodeBlock() {
			public void run() {
				field.set(new Item(), 42);
			}
		});
	}
	
}
