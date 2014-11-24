package com.db4odoc.concurrency.locking;


import com.db4o.Db4oEmbedded;
import com.db4o.ObjectContainer;
import com.db4o.ObjectSet;
import com.db4o.query.Predicate;

import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;
import java.util.concurrent.Future;
import java.util.concurrent.locks.ReadWriteLock;
import java.util.concurrent.locks.ReentrantReadWriteLock;

public class ReadWriteLockingOperations {
    //#example: Read write lock
    private final ReadWriteLock dataLock = new ReentrantReadWriteLock();
    // #end example
    private final ExecutorService executor = Executors.newCachedThreadPool();

    public static void main(String[] args) throws Exception {
        new ReadWriteLockingOperations().main();
    }

    public void main() throws Exception {
        final ObjectContainer container = Db4oEmbedded.openFile("database.db4o");
        try {
            storeInitialObjects(container);

            // Schedule back-ground tasks
            final Future<?> task = executor.submit(new Runnable() {
                @Override
                public void run() {
                    updateSomePeople(container);
                }
            });
            // While doing other work
            listAllPeople(container);

            // Wait for the tasks to finish
            task.get();
        } finally {
            container.close();
        }
    }

    // #example: Grab the read-lock to show the data
    private void listAllPeople(ObjectContainer container) {
        dataLock.readLock().lock();
        try{
            for (Person person : container.query(Person.class)) {
                System.out.println(person.getName());
            }

        } finally{
            dataLock.readLock().unlock();
        }
    }
    // #end example

    // #example: Grab the write-lock to change the data
    private void updateSomePeople(ObjectContainer container) {
        dataLock.writeLock().lock();
        try {
            final ObjectSet<Person> people = container.query(new Predicate<Person>() {
                @Override
                public boolean match(Person person) {
                    return person.getName().equals("Joe");
                }
            });
            for (Person joe : people) {
                joe.setName("New Joe");
                container.store(joe);
            }
        } finally {
            dataLock.writeLock().unlock();
        }
    }
    // #end example:

    private void storeInitialObjects(ObjectContainer container) {
        dataLock.writeLock().lock();
        try {
            container.store(new Person("Joe"));
            container.store(new Person("Jan"));
            container.store(new Person("Joanna"));
            container.store(new Person("Phil"));
        }  finally {
            dataLock.writeLock().unlock();
        }
    }

}
