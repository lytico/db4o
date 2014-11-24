/* Copyright (C) 2008  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.uuid;

import com.db4o.config.*;
import com.db4o.ext.*;

import db4ounit.*;
import db4ounit.extensions.*;

/**
 * @exclude
 */
public class UUIDDefragmentTestCase extends AbstractDb4oTestCase{
	
	public static class Item {
		
		public String name;
		
	}
	
	protected void configure(Configuration config) throws Exception {
		config.generateUUIDs(ConfigScope.GLOBALLY);
	}
	
	protected void store() throws Exception {
		Item item = new Item();
		item.name = "one";
		store(item);
	}
	
	public void test() throws Exception{
		Db4oUUID uuidBeforeDefragment = singleItemUUID();
		byte[] signatureBeforeDefragment = uuidBeforeDefragment.getSignaturePart();
		long longPartBeforeDefragment = uuidBeforeDefragment.getLongPart();
		defragment();
		
		Db4oUUID uuidAfterDefragment = singleItemUUID();
		byte[] signatureAfterDefragment = uuidAfterDefragment.getSignaturePart();
		long longPartAfterDefragment = uuidAfterDefragment.getLongPart();
		
		ArrayAssert.areEqual(signatureBeforeDefragment, signatureAfterDefragment);
		Assert.areEqual(longPartBeforeDefragment, longPartAfterDefragment);
		
	}

	private Db4oUUID singleItemUUID() {
		Item item = (Item) retrieveOnlyInstance(Item.class);
		ObjectInfo objectInfo = db().getObjectInfo(item);
		Db4oUUID uuid = objectInfo.getUUID();
		return uuid;
	}

}
