/* Copyright (C) 2009  Versant Inc.  http://www.db4o.com */

package db4ounit.extensions;

import com.db4o.foundation.*;
import com.db4o.internal.*;
import com.db4o.internal.btree.*;

import db4ounit.*;

public class FieldIndexAssert {
	
	private final Class _clazz;
	
	private final String _name;
	
	public FieldIndexAssert(Class clazz, String name){
		_clazz = clazz;
		_name = name;
	}
	
	public void assertSingleEntry(LocalObjectContainer container, final long id) {
		final BooleanByRef called = new BooleanByRef();
		index(container).traverseKeys(container.systemTransaction(), new Visitor4<FieldIndexKey>() {
			public void visit(FieldIndexKey key) {
				Assert.areEqual(id, key.parentID());
				Assert.isFalse(called.value);
				called.value = true;
			}
		});
		Assert.isTrue(called.value);
	}

	private BTree index(LocalObjectContainer container) {
		return fieldMetadata(container).getIndex(null);
	}

	private FieldMetadata fieldMetadata(LocalObjectContainer container) {
		return classMetadata(container).fieldMetadataForName(_name);
	}

	private ClassMetadata classMetadata(LocalObjectContainer container) {
		return container.classMetadataForReflectClass(container.reflector().forClass(_clazz));
	}


}
