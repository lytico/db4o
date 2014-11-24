package com.db4o.db4ounit.common.refactor;

import com.db4o.*;
import com.db4o.config.*;
import com.db4o.query.*;

import db4ounit.*;
import db4ounit.extensions.*;
import db4ounit.extensions.fixtures.*;
import db4ounit.extensions.util.*;

public class RemoveArrayFieldTestCase extends AbstractDb4oTestCase implements OptOutDefragSolo {
	public static class DataBefore {
		
		public Object[] array;	
		public String name;
		public boolean status;

		public DataBefore(String name, boolean status, Object[] array) {
			this.name = name;
			this.status = status;
			this.array = array;	
		}
	}

	public static class DataAfter {
		
		public String name;
		public boolean status;

		public DataAfter(String name, boolean status) {
			this.name = name;
			this.status = status;
		}
	}
		
	public void testRemoveArrayField() throws Exception {
		DataBefore dataA = new DataBefore("a", true, new Object[] { "X" });
		DataBefore dataB = new DataBefore("b", false, new Object[0]);
		store(dataA);
		store(dataB);

        ObjectClass oc = fixture().config().objectClass(DataBefore.class);
        // we must use ReflectPlatform here as the string must include
        // the assembly name in .net
        oc.rename(CrossPlatformServices.fullyQualifiedName(DataAfter.class));        
        reopen();
        
        Query query = newQuery(DataAfter.class);
		query.descend("name").constrain("a");
		ObjectSet result = query.execute();
		Assert.areEqual(1, result.size());
		DataAfter data = (DataAfter) result.next();
		Assert.areEqual(dataA.name, data.name);
		Assert.areEqual(dataA.status, data.status);
	}
}
