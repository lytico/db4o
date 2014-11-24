package com.db4o.internal;

/**
 * A transaction local variable.
 * 
 * @see Transaction#get(TransactionLocal)
 */
public class TransactionLocal<T> {
	public T initialValueFor(Transaction transaction) {
		return null;
	}
}
