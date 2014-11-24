package com.db4odoc.concurrency.locking;


import com.db4o.Db4oEmbedded;
import com.db4o.ObjectContainer;
import com.db4o.ObjectSet;
import com.db4o.query.Predicate;

import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;
import java.util.concurrent.Future;

public class LockingOperations {
    private final Object dataLock = new Object();
    private final ExecutorService executor = Executors.newCachedThreadPool();

    public static void main(String[] args) throws Exception {
        new LockingOperations().main();
    }

    public void main() throws Exception {
        final ObjectContainer container = Db4oEmbedded.openFile("database.db4o");
        try {
            storeInitialObjects(container);

            // #example: Schedule back-ground tasks
            // Schedule back-ground tasks
            final Future<?> task = executor.submit(new Runnable() {
                @Override
                public void run() {
                    updateSomePeople(container);
                }
            });
            // While doing other work
            listAllPeople(container);
            // #end example

            // Wait for the tasks to finish
            task.get();
        } finally {
            container.close();
        }
    }

    // #example: Grab the lock to show the data
    private void listAllPeople(ObjectContainer container) {
        synchronized (dataLock) {
            for (Person person : container.query(Person.class)) {
                System.out.println(person.getName());
            }
        }
    }
    // #end example

    // #example: Grab the lock protecting the data
    private void updateSomePeople(ObjectContainer container) {
        synchronized (dataLock) {
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
        }
    }
    // #end example:

    private void storeInitialObjects(ObjectContainer container) {
        synchronized (dataLock) {
            container.store(new Person("Joe"));
            container.store(new Person("Jan"));
            container.store(new Person("Joanna"));
            container.store(new Person("Phil"));
        }
    }

}
