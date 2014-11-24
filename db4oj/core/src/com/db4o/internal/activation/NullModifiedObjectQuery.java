package com.db4o.internal.activation;

public class NullModifiedObjectQuery implements ModifiedObjectQuery {
	
	public final static ModifiedObjectQuery INSTANCE = new NullModifiedObjectQuery();

	private NullModifiedObjectQuery() {
	}
	
	public boolean isModified(Object ref) {
		return false;
	}

}
