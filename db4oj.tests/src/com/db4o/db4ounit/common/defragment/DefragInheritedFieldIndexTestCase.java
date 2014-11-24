package com.db4o.db4ounit.common.defragment;

import com.db4o.*;
import com.db4o.config.*;
import com.db4o.query.*;

import db4ounit.*;
import db4ounit.extensions.*;
import db4ounit.extensions.fixtures.*;

public class DefragInheritedFieldIndexTestCase extends AbstractDb4oTestCase implements OptOutMultiSession {

	private static final String FIELD_NAME = "_name";
	private static final String[] NAMES = {"Foo", "Bar", "Baz"};
	
	@Override
	protected void configure(Configuration config) throws Exception {
		config.objectClass(ParentItem.class).objectField(FIELD_NAME).indexed(true);
	}

	@Override
	protected void store() throws Exception {
		for (String name : NAMES) {
			store(new ChildItem(name));
		}
	}

	public void testDefragInheritedFieldIndex() throws Exception {
		assertQueryByIndex();
		defragment();
		assertQueryByIndex();
	}
	
	private void assertQueryByIndex() {
		Query query = newQuery(ChildItem.class);
		query.descend(FIELD_NAME).constrain(NAMES[0]);
		ObjectSet<ChildItem> result = query.execute();
		Assert.areEqual(1, result.size());
		Assert.areEqual(NAMES[0], result.next()._name);
	}

	public static class ParentItem {
		public String _name;
		
		public ParentItem(String name) {
			_name = name;
		}
	}
	
	public static class ChildItem extends ParentItem {
		public ChildItem(String name) {
			super(name);
		}
	}
}
