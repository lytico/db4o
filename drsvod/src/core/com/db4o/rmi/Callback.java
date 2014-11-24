package com.db4o.rmi;

public interface Callback<T> {

	void returned(T value);
	
}
