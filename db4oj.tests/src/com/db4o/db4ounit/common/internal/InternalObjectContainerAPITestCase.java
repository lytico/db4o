package com.db4o.db4ounit.common.internal;

import com.db4o.internal.*;

import db4ounit.*;
import db4ounit.extensions.*;
import db4ounit.extensions.util.*;

public class InternalObjectContainerAPITestCase extends AbstractDb4oTestCase {

	public static class Item {
	}

	protected void store() throws Exception {
		store(new Item());
	}
	
	public void testClassMetadataForName() {
		String className = CrossPlatformServices.fullyQualifiedName(Item.class);
		ClassMetadata clazz = ((InternalObjectContainer)db()).classMetadataForName(className);
		Assert.areEqual(className, clazz.getName());
		Assert.areEqual(reflector().forClass(Item.class), clazz.classReflector());
	}
	
	public static void main(String[] args) {
		new InternalObjectContainerAPITestCase().runAll();
	}
}
