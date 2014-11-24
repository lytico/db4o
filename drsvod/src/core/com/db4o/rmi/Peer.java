package com.db4o.rmi;

public interface Peer<T> {

	T sync();

	T async();

	<R> T async(Callback<R> callback);
	
}
