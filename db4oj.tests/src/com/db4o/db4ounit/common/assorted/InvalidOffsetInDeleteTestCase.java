/* Copyright (C) 2009 Versant Inc. http://www.db4o.com */
package com.db4o.db4ounit.common.assorted;

import com.db4o.*;
import com.db4o.config.*;
import com.db4o.db4ounit.common.api.*;
import com.db4o.diagnostic.*;
import com.db4o.query.*;

import db4ounit.*;

public class InvalidOffsetInDeleteTestCase extends Db4oTestWithTempFile implements DiagnosticListener{
	
	public static class Parent {
		public String _parentName;
	}
	
	public static class Item extends Parent{
		public String _itemName;
	}
	
	public void test(){
		EmbeddedConfiguration config = newConfiguration();
		configure(config);
		ObjectContainer objectContainer = Db4oEmbedded.openFile(config, tempFile());
		Item item = new Item();
		item._itemName = "item";
		item._parentName = "parent";
		objectContainer.store(item);
		objectContainer.close();
		config = newConfiguration();
		configure(config);
		objectContainer = Db4oEmbedded.openFile(config, tempFile());
		Query query = objectContainer.query();
		query.constrain(Item.class);
		ObjectSet objectSet = query.execute();
		item = (Item) objectSet.next();
		objectContainer.store(item);
		objectContainer.close();
	}

	private void configure(EmbeddedConfiguration config) {
		config.common().diagnostic().addListener(this);
		config.file().generateCommitTimestamps(true);
		config.file().generateUUIDs(ConfigScope.GLOBALLY);
		config.common().objectClass(Item.class).objectField("_itemName").indexed(true);
		config.common().objectClass(Parent.class).objectField("_parentName").indexed(true);
	}

	public void onDiagnostic(Diagnostic d) {
		if(d instanceof DeletionFailed){
			Assert.fail("No deletion failed diagnostic message expected.");
		}
	}

}
