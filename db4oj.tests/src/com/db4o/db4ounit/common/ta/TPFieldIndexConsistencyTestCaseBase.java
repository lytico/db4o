/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.ta;

import com.db4o.*;
import com.db4o.activation.*;
import com.db4o.config.*;
import com.db4o.internal.*;
import com.db4o.internal.btree.*;
import com.db4o.query.*;
import com.db4o.reflect.*;
import com.db4o.ta.*;

import db4ounit.*;
import db4ounit.extensions.*;

public abstract class TPFieldIndexConsistencyTestCaseBase extends AbstractDb4oTestCase {

	protected static final String ID_FIELD_NAME = "_id";

	public static class Item implements Activatable {
		public int _id;
		private transient Activator _activator;

		public Item(int id) {
			_id = id;
		}
		
		public int id() {
			activate(ActivationPurpose.READ);
			return _id;
		}
		
		public void id(int id) {
			activate(ActivationPurpose.WRITE);
			_id = id;
		}

		public void activate(ActivationPurpose purpose) {
			if(_activator != null) {
				_activator.activate(purpose);
			}
		}

		public void bind(Activator activator) {
			if(_activator != null && activator != null && _activator != activator) {
				throw new IllegalStateException();
			}
			_activator = activator;
		}
	}

	@Override
	protected void configure(Configuration config) throws Exception {
		config.add(new TransparentPersistenceSupport());
		config.objectClass(Item.class).objectField(ID_FIELD_NAME).indexed(true);
	}
	
	protected void assertFieldIndex(int id) {
		ReflectClass claxx = reflector().forClass(Item.class);
		ClassMetadata classMetadata = fileSession().classMetadataForReflectClass(claxx);
		FieldMetadata field = classMetadata.fieldMetadataForName(ID_FIELD_NAME);
		BTreeRange indexRange = field.search(trans(), id);
		Assert.areEqual(1, indexRange.size());
	}

	protected void assertItemQuery(int id) {
		Query query = newQuery(Item.class);
		query.descend(ID_FIELD_NAME).constrain(id);
		ObjectSet<Item> result = query.execute();
		Assert.areEqual(1, result.size());
		Assert.areEqual(id, result.next().id());
	}

}
