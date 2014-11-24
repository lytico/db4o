/* Copyright (C) 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.internal.metadata;

import com.db4o.foundation.*;
import com.db4o.internal.*;
import com.db4o.internal.metadata.*;

import db4ounit.*;
import db4ounit.extensions.*;

public class ClassMetadataIntegrationTestCase extends AbstractDb4oTestCase {
	
	public static class SuperClazz {
		public int _id;
		public String _name;
	}

	public static class SubClazz extends SuperClazz {
		public int _age;
	}

	protected void store() throws Exception {
		store(new SubClazz());
	}
	
	public void testFieldTraversal() {		
		final Collection4 expectedNames=new Collection4(new ArrayIterator4(new String[]{"_id","_name","_age"}));
		ClassMetadata classMetadata = classMetadataFor(SubClazz.class);
		
        classMetadata.traverseAllAspects(new TraverseFieldCommand() {
    		
			@Override
			protected void process(FieldMetadata field) {
				Assert.isNotNull(expectedNames.remove(field.getName()));
			}
		});

		Assert.isTrue(expectedNames.isEmpty());
	}
	
	
	public void testPrimitiveArrayMetadataIsPrimitiveTypeMetadata() {
		ClassMetadata byteArrayMetadata = container().produceClassMetadata(reflectClass(byte[].class));
		Assert.isInstanceOf(PrimitiveTypeMetadata.class, byteArrayMetadata);
	}
}
