package com.db4o.drs.test.data;

@OptOutRdbms
public final class ItemWithCloneable {
	public Cloneable value;
	
	public ItemWithCloneable() {
	}
	
	public ItemWithCloneable(Cloneable c) {
		value = c;
	}
}