package com.db4odoc.concurrency.transactions;

import com.db4o.ObjectContainer;

public interface TransactionFunction<T> {
    T inTransaction(ObjectContainer container);
}
