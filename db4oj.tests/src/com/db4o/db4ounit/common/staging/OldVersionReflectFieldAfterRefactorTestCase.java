/* Copyright (C) 2010  Versant Inc.   http://www.db4o.com */
package com.db4o.db4ounit.common.staging;

import com.db4o.foundation.*;
import com.db4o.internal.*;

import db4ounit.*;
import db4ounit.extensions.*;

/*
 * COR-1937
 */
public class OldVersionReflectFieldAfterRefactorTestCase extends AbstractDb4oTestCase {

	private static final int ID_VALUE = 42;

	public static class ItemBefore {
		public int _id;
		
		public ItemBefore(int id) {
			_id = id;
		}
	}

	public static class ItemAfter {
		public String _id;
	}

	public void testReflectField() throws Exception {
		store(new ItemBefore(ID_VALUE));
		reopen();
		fileSession().storedClass(ItemBefore.class).rename(ItemAfter.class.getName());
		reopen();
		
		ClassMetadata classMetadata = container().classMetadataForName(ItemAfter.class.getName());
		final ByRef<FieldMetadata> originalField = new ByRef<FieldMetadata>();
		classMetadata.traverseDeclaredFields(new Procedure4<FieldMetadata>() {
			public void apply(FieldMetadata field) {
				if (originalField.value == null 
						&& field.getName().equals("_id") 
						&& field.fieldType().getName().equals(Integer.class.getName())) {
					
					originalField.value = field;
				}
			}
		});
		
		Assert.areEqual(int.class.getName(), originalField.value.getStoredType().getName());		
	}

}
