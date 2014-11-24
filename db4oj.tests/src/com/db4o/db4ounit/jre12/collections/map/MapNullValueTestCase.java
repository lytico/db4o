package com.db4o.db4ounit.jre12.collections.map;

import java.util.*;

import db4ounit.*;
import db4ounit.extensions.*;

// COR-404
/**
 */
@decaf.Ignore(decaf.Platform.JDK11)
public class MapNullValueTestCase extends AbstractDb4oTestCase {

	public static class Data {
		public Map _map;

		public Data(Map _map) {
			this._map = _map;
		}
	}

	private static final String KEY = "KEY";
	
	protected void store() throws Exception {
		Map map=new HashMap();
		map.put(KEY,null);
		Data data=new Data(map);
		Assert.isTrue(data._map.containsKey(KEY));
		store(data);
	}
	
	public void testNullValueIsPersisted() {
		Data data=(Data)retrieveOnlyInstance(Data.class);
		Assert.isTrue(data._map.containsKey(KEY));
	}
	
}
