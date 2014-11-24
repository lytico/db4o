/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

package com.db4o.drs.test.cobra;


import static com.db4o.qlin.QLinSupport.*;

import com.db4o.drs.test.versant.*;

import db4ounit.*;

public class CobraObjectLifecycleTestCase extends VodCobraTestCaseBase {
	
	public void test(){
		_cobra.produceSchema(Item.class);
		Item item = new Item("one");
		item.setLongs(new long[] {1, 2});
		_cobra.store(item);
		_cobra.commit();
		Item i = prototype(Item.class);
		Item retrievedItem = _cobra.from(Item.class).where(i.getName()).equal("one").single();
		Assert.areEqual("one", retrievedItem.getName());
		ArrayAssert.areEqual(new long[]{1,2}, retrievedItem.getLongs());
		_cobra.delete(retrievedItem);
		_cobra.commit();
	}

}
