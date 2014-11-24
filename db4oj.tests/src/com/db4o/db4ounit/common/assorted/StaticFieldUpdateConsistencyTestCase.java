/* Copyright (C) 2010  Versant Inc.   http://www.db4o.com */
package com.db4o.db4ounit.common.assorted;

import com.db4o.*;
import com.db4o.config.*;
import com.db4o.consistency.*;

import db4ounit.*;
import db4ounit.extensions.*;

public class StaticFieldUpdateConsistencyTestCase extends AbstractDb4oTestCase {

	public static class SimpleEnum {
		public String _name;
		
		public SimpleEnum(String name) {
			_name = name;
		}
		
		public static SimpleEnum A = new SimpleEnum("A");
		public static SimpleEnum B = new SimpleEnum("B");
	}
	
	public static class Item {
		public SimpleEnum _value;
		
		public Item(SimpleEnum value) {
			_value = value;
		}
	}

	
	@Override
	protected void configure(Configuration config) throws Exception {
		config.updateDepth(5);
		config.objectClass(SimpleEnum.class).persistStaticFieldValues();
	}

	@Override
	protected void store() throws Exception {
		store(new Item(SimpleEnum.A));
	}
	
	public void test() throws Exception {
		updateAll();
		ConsistencyReport consistencyReport = new ConsistencyChecker(fileSession()).checkSlotConsistency();
		Assert.isTrue(consistencyReport.consistent(), consistencyReport.toString());
	}

	private void updateAll() {
		ObjectSet<Item> result = newQuery(Item.class).execute();
		while(result.hasNext()) {
			Item item = result.next();
			item._value = (item._value == SimpleEnum.A) ? SimpleEnum.B : SimpleEnum.A;
			store(item);
		}
		commit();
	}
	
}
