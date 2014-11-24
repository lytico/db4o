package com.db4o.db4ounit.common.staging;

import com.db4o.db4ounit.common.assorted.*;
import com.db4o.internal.*;

import db4ounit.*;

@decaf.Remove(unlessCompatible=decaf.Platform.JDK15)
public class UnavailableEnumTestCase extends UnavailableClassTestCaseBase {
	
	public static enum Status {
		Open, Closed
	}
	
	public static class Item {
		
		Status _status;
		
		public Item(Status status) {
			_status = status;
        }
	}
	
	@Override
	protected void store() throws Exception {
	    store(new Item(Status.Open));
	}
	
	public void test() throws Exception {
		reopenHidingClasses(Item.class, Status.class);
		for (Object o : newQuery().execute()) {
			Assert.areSame(
				reflector().forName(ReflectPlatform.fullyQualifiedName(Item.class)),
				reflector().forObject(o));
		}
	}

}
