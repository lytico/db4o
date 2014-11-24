/* Copyright (C) 2009  Versant Corp.  http://www.db4o.com */
/**
 * @sharpen.if !SILVERLIGHT
 */
package com.db4o.db4ounit.common.staging;

import com.db4o.*;
import com.db4o.config.*;
import com.db4o.cs.config.*;
import com.db4o.cs.internal.config.*;
import com.db4o.ext.*;

import db4ounit.*;
import db4ounit.extensions.*;
import db4ounit.extensions.fixtures.*;

/**
 * COR-1762
 */
public class DeepPrefetchingCacheConcurrencyTestCase extends AbstractDb4oTestCase implements OptOutAllButNetworkingCS {
	
	public static class Item {

		public String _name;

		public Item(String name) {
			_name = name;
		}

	}
	
	@Override
	protected void configure(Configuration config) throws Exception {
		ClientConfiguration clientConfiguration = Db4oClientServerLegacyConfigurationBridge.asClientConfiguration(config);
		clientConfiguration.prefetchDepth(3);
		clientConfiguration.prefetchObjectCount(3);
	}

	@Override
	protected void store() throws Exception {
		for (int i = 0; i < 2; i++) {
			Item item = new Item("original");
			store(item);
		}
	}
	
	public void test(){
		int[] ids = new int[2];
		
		ObjectSet<Item> originalResult = newQuery(Item.class).execute();
		Item firstOriginalItem = originalResult.next();
		db().purge(firstOriginalItem);
		
		ExtObjectContainer otherClient = openNewSession();
		ObjectSet<Item> updateResult = otherClient.query(Item.class);
		int idx = 0;
		for(Item updateItem : updateResult){
			ids[idx] = (int) otherClient.getID(updateItem);
			updateItem._name = "updated";
			otherClient.store(updateItem);
			idx++;
		}
		otherClient.commit();
		otherClient.close();
		
		for (int i = 0; i < ids.length; i++) {
			Item checkItem = db().getByID(ids[i]);
			db().activate(checkItem);
			Assert.areEqual("updated", checkItem._name);
		}
		
//		ObjectSet<Item> checkResult = newQuery(Item.class).execute();
//		for (Item checkItem : checkResult) {
//			Assert.areEqual("updated", checkItem._name);
//		}
	}
	
	

}
