/* Copyright (C) 2008  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.jre12.handlers;

import java.util.*;

import com.db4o.foundation.*;
import com.db4o.internal.*;
import com.db4o.typehandlers.*;

import db4ounit.*;
import db4ounit.extensions.*;

/**
 */
@decaf.Ignore(decaf.Platform.JDK11)
public class TypeHandlerConfigurationTestCase extends AbstractDb4oTestCase {
	
	public static class Holder {
		
		public Object _storedObject;
		
		public Holder(Object storedObject){
			_storedObject = storedObject;
		}
		
	}
	
	public void store(){
		addMetadata(new ArrayList());
	}

	private void addMetadata(Object storedObject) {
		store(new Holder(storedObject));
	}
	
	public void test(){
		assertSingleNullTypeHandlerAspect(ArrayList.class);
		assertSingleNullTypeHandlerAspect(AbstractList.class);
		assertSingleTypeHandlerAspect(AbstractCollection.class, CollectionTypeHandler.class);
	}

	private void assertSingleNullTypeHandlerAspect(Class storedClass) {
		assertSingleTypeHandlerAspect(storedClass, IgnoreFieldsTypeHandler.class);
	}

	private void assertSingleTypeHandlerAspect(Class storedClass,
			final Class typeHandlerClass) {
		final IntByRef aspectCount = new IntByRef(0);
		ClassMetadata classMetadata = classMetadata(storedClass);
		classMetadata.traverseDeclaredAspects(new Procedure4() {
			public void apply(Object arg) {
				aspectCount.value ++;
				Assert.isSmaller(2, aspectCount.value);
				ClassAspect aspect = (ClassAspect) arg;
				Assert.isInstanceOf(TypeHandlerAspect.class, aspect);
				TypeHandlerAspect typeHandlerAspect = (TypeHandlerAspect) aspect;
				Assert.isInstanceOf(typeHandlerClass, typeHandlerAspect._typeHandler);
			}
		});
	}

	private ClassMetadata classMetadata(Class clazz) {
		return container().classMetadataForName(clazz.getName());
	}

}
