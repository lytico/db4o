package com.db4odoc.concurrency.transactions;


import com.db4o.ObjectContainer;

public interface TransactionAction {
    void inTransaction(ObjectContainer container);
}


