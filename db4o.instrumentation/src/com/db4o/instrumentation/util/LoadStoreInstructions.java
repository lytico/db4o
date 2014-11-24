/**
 * 
 */
package com.db4o.instrumentation.util;

public class LoadStoreInstructions {
	public final int load;
	public final int store;
	
	public LoadStoreInstructions(int load_, int store_) {
		load = load_;
		store = store_;
	}
}