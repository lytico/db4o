package com.db4odoc.concurrency.transactions;


import com.db4o.Db4oEmbedded;
import com.db4o.ObjectContainer;
import com.db4o.ObjectSet;
import com.db4o.query.Predicate;

import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;
import java.util.concurrent.Future;

public class ReadWriteTransactionOperations {
    private final ExecutorService executor = Executors.newCachedThreadPool();

    public static void main(String[] args) throws Exception {
        new ReadWriteTransactionOperations().main();
    }

    public void main() throws Exception {
        final DatabaseSupportWithReadWriteLock database = new DatabaseSupportWithReadWriteLock(Db4oEmbedded.openFile("database.db4o"));
        try {
            storeInitialObjects(database);

            // Schedule back-ground tasks
            final Future<?> task = executor.submit(new Runnable() {
                @Override
                public void run() {
                    updateAllJoes(database);
                }
            });

            // While doing other work
            listAllPeople(database);

            // Wait for the task to finish
            task.get();
        } finally {
            database.close();
        }
    }

    // #example: Use a read transaction for reading objects
    private void listAllPeople(DatabaseSupportWithReadWriteLock dbSupport) {
        dbSupport.inReadTransaction(new TransactionAction() {
            @Override
            public void inTransaction(ObjectContainer container) {
                final ObjectSet<Person> result = container.query(Person.class);
                for (Person person : result) {
                    System.out.println(person.getName());
                }
            }
        });
    }
    // #end example

    // #example: Use a write transaction to update objects
    private void updateAllJoes(DatabaseSupportWithReadWriteLock dbSupport) {
        dbSupport.inWriteTransaction(new TransactionAction() {
            @Override
            public void inTransaction(ObjectContainer container) {
                final ObjectSet<Person> allJoes = container.query(new Predicate<Person>() {
                    @Override
                    public boolean match(Person person) {
                        return person.getName().equals("Joe");
                    }
                });
                for (Person joe : allJoes) {
                    joe.setName("New Joe");
                    container.store(joe);
                }
            }
        });
    }
    // #end example

    private void storeInitialObjects(DatabaseSupportWithReadWriteLock dbSupport) {
        dbSupport.inWriteTransaction(new TransactionAction() {
            @Override
            public void inTransaction(ObjectContainer container) {
                container.store(new Person("Joe"));
                container.store(new Person("Jan"));
                container.store(new Person("Joanna"));
                container.store(new Person("Phil"));
            }
        });
    }

}
