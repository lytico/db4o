package com.db4o;

/**
 * @exclude
 */
public interface TransactionAware {
	void setTrans(Transaction a_trans);
}
