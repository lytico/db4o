package com.db4odoc.concurrency.units;


import com.db4o.Db4oEmbedded;
import com.db4o.ObjectContainer;
import com.db4o.ObjectSet;
import com.db4o.query.Predicate;

import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;
import java.util.concurrent.Future;

public class UnitsOfWork {
    private final ExecutorService executor = Executors.newCachedThreadPool();

    public static void main(String[] args) throws Exception {
        new UnitsOfWork().main();

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

    // #example: An object container for this unit of work
    private void listAllPeople(ObjectContainer rootContainer) {
        ObjectContainer container = rootContainer.ext().openSession();
        try {
            for (Person person : container.query(Person.class)) {
                System.out.println(person.getName());
            }
        } finally {
            container.close();
        }
    }
    // #end example

    // #example: An object container for the background task
    private void updateSomePeople(ObjectContainer rootContainer) {
        ObjectContainer container = rootContainer.ext().openSession();
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
            container.close();
        }
    }
    // #end example:


    private void storeInitialObjects(ObjectContainer rootContainer) {
        ObjectContainer container = rootContainer.ext().openSession();
        try {
            container.store(new Person("Joe"));
            container.store(new Person("Jan"));
            container.store(new Person("Joanna"));
            container.store(new Person("Phil"));
        } finally {
            container.close();
        }
    }
}
