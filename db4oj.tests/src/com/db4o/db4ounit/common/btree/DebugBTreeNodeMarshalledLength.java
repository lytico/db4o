/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.btree;

import com.db4o.config.*;
import com.db4o.internal.btree.*;
import com.db4o.internal.classindex.*;

import db4ounit.extensions.*;


public class DebugBTreeNodeMarshalledLength extends AbstractDb4oTestCase{
	
	public static class Item{
		public int _int;
		public String _string;
	}

	public static void main(String[] args) {
		new DebugBTreeNodeMarshalledLength().runSolo();
	}
	
	protected void configure(Configuration config) throws Exception {
		super.configure(config);
		config.objectClass(Item.class).objectField("_int").indexed(true);
		config.objectClass(Item.class).objectField("_string").indexed(true);
	}
	
	protected void store() throws Exception {
		for (int i = 0; i < 50000; i++) {
			store(new Item());
		}
	}
	
	public void test(){
		BTree btree = btree().debugLoadFully(systemTrans());
		store(new Item());
		btree.write(systemTrans());
	}
	
	private BTree btree(){
		ClassIndexStrategy index = classMetadataFor(Item.class).index();
		return ((BTreeClassIndexStrategy)index).btree();
	}

}
