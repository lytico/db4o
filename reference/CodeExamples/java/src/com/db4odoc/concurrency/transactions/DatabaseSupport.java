package com.db4odoc.concurrency.transactions;


import com.db4o.ObjectContainer;
import com.db4o.ext.Db4oIOException;

public class DatabaseSupport {
    private final Object lock = new Object();
    private final ObjectContainer database;

    public DatabaseSupport(ObjectContainer database) {
        this.database = database;
    }

    // #example: A transaction method
    public <T> T inTransaction(TransactionFunction<T> transactionClosure) {
        synchronized (lock) {
            try {
                return transactionClosure.inTransaction(database);
            } catch (Exception e) {
                database.rollback();
                throw new TransactionFailedException(e.getMessage(), e);
            } finally {
                database.commit();
            }
        }
    }
    // #end example

    public void inTransaction(final TransactionAction transactionClosure) {
        inTransaction(new TransactionFunction<Void>() {
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
