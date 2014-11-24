package com.db4o.db4ounit.jre12.assorted;

import db4ounit.*;
import db4ounit.extensions.*;

/**
 */
@decaf.Ignore(decaf.Platform.JDK11)
public class FinalFieldTestCase extends AbstractDb4oTestCase {
	
	public static class Item {
		
		public final int fi;
		public final String fs;
		public int i;
		public String s;
		
		public Item() {
			fi = 0;
			fs = "";
		}
		
		public Item(int i, String s) {
			this.i = this.fi = i;
			this.s = this.fs = s;
		}
	}
	
	protected void store() {
		db().store(new Item(42, "jb"));
	}
	
	public void _testFinalField() {
		Item i = (Item)retrieveOnlyInstance(Item.class);
		Assert.areEqual(42, i.i);
		Assert.areEqual(42, i.fi);
		Assert.areEqual("jb", i.s);
		Assert.areEqual("jb", i.fs);
	}
	
	public static void main(String[] args) {
		new FinalFieldTestCase().runSolo();
	}
}
