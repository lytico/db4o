/* Copyright (C) 2007   Versant Inc.   http://www.db4o.com */
package com.db4o.db4ounit.common.fieldindex;

import com.db4o.*;
import com.db4o.ext.*;
import com.db4o.query.*;

import db4ounit.*;
import db4ounit.extensions.*;
import db4ounit.extensions.fixtures.*;

public class RuntimeFieldIndexTestCase extends AbstractDb4oTestCase implements OptOutMultiSession {
	
	private static final String FIELDNAME = "_id";

	public static class Data {
		public int _id;

		public Data(int id) {
			_id = id;
		}		
	}
	
	protected void store() throws Exception {
		for(int i=1; i <= 3; i++) {
			store(new Data(i));
		}
	}
	
	public void testCreateIndexAtRuntime() {
		StoredField field = storedField();
		Assert.isFalse(field.hasIndex());
		field.createIndex();
		Assert.isTrue(field.hasIndex());
		assertQuery();
		field.createIndex(); // ensure that second call is ignored
	}

	private void assertQuery() {
		Query query = newQuery(Data.class);
		query.descend(FIELDNAME).constrain(new Integer(2));
		ObjectSet result = query.execute();
		Assert.areEqual(1, result.size());
	}
	
	public void testDropIndex(){
		StoredField field = storedField();
		field.createIndex();
		assertQuery();
		field.dropIndex();
		Assert.isFalse(field.hasIndex());
		assertQuery();
	}

	private StoredField storedField() {
		return db().storedClass(Data.class).storedField(FIELDNAME,null);
	}

}
