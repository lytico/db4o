package com.db4odoc.concurrency.transactions;


import com.db4o.ObjectContainer;
import com.db4o.ext.Db4oIOException;

import java.util.concurrent.locks.Lock;
import java.util.concurrent.locks.ReadWriteLock;
import java.util.concurrent.locks.ReentrantReadWriteLock;

public class DatabaseSupportWithReadWriteLock {
    //#example: Read write lock
    private final ReadWriteLock dataLock = new ReentrantReadWriteLock();
    // #end example
    private final ObjectContainer database;

    public DatabaseSupportWithReadWriteLock(ObjectContainer database) {
        this.database = database;
    }


    // #example: The transaction implementation
    private <T> T inTransaction(Lock lockToGrab,TransactionFunction<T> transactionClosure) {
        lockToGrab.lock();
        try {
            return transactionClosure.inTransaction(database);
        } catch (Exception e) {
            database.rollback();
            throw new TransactionFailedException(e.getMessage(), e);
        } finally {
            database.commit();
            lockToGrab.unlock();
        }
    }
    // #end example


    // #example: The read transaction method
    public <T> T inReadTransaction(TransactionFunction<T> transactionClosure) {
        return inTransaction(dataLock.readLock(),transactionClosure);
    }
    // #end example

    // #example: The write transaction method
    public <T> T inWriteTransaction(TransactionFunction<T> transactionClosure) {
        return inTransaction(dataLock.writeLock(),transactionClosure);
    }
    // #end example


    public void inReadTransaction(final TransactionAction transactionClosure) {
        inReadTransaction(new TransactionFunction<Void>() {
            @Override
            public Void inTransaction(ObjectContainer container) {
                transactionClosure.inTransaction(container);
                return null;  //To change body of implemented methods use File | Settings | File Templates.
            }
        });
    }

    public void inWriteTransaction(final TransactionAction transactionClosure) {
        inWriteTransaction(new TransactionFunction<Void>() {
            @Override
            public Void inTransaction(ObjectContainer container) {
                transactionClosure.inTransaction(container);
                return null;  //To change body of implemented methods use File | Settings | File Templates.
            }
        });
    }

    public boolean close() throws Db4oIOException {
        return database.close();
    }
}
