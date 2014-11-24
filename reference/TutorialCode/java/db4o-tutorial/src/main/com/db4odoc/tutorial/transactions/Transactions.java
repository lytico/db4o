package com.db4odoc.tutorial.transactions;


import com.db4o.Db4oEmbedded;
import com.db4o.ObjectContainer;
import com.db4o.config.EmbeddedConfiguration;
import com.db4o.ta.DeactivatingRollbackStrategy;
import com.db4o.ta.TransparentPersistenceSupport;

public class Transactions {
    private static final String DATABASE_FILE = "database.db4o";

    public static void main(String[] args) {
        storeExampleObjects();
        commitTransactions();
        rollbackTransactions();
        objectStateAfterRollbackWithoutTP();
        objectStateAfterRollbackWithTP();
        multipleTransactions();
    }

    private static void commitTransactions() {
        ObjectContainer container = openDatabase();
        try {
            Car toyota = new Car("Toyota Corolla");
            Driver jimmy = new Driver("Jimmy", toyota);
            container.store(jimmy);
            // #example: Committing changes
            container.commit();
            // #end example
        } finally {
            container.close();
        }
    }

    private static void rollbackTransactions() {
        ObjectContainer container = openDatabase();
        try {
            Car toyota = new Car("Toyota Corolla");
            Driver jimmy = new Driver("Jimmy", toyota);
            container.store(jimmy);
            // #example: Rollback changes
            container.rollback();
            // #end example
        } finally {
            container.close();
        }
    }

    private static void objectStateAfterRollbackWithoutTP() {
        ObjectContainer container = Db4oEmbedded.openFile(DATABASE_FILE);
        try {
            // #example: Without transparent persistence objects in memory aren't rolled back
            Driver driver = queryForDriver(container);
            driver.setName("New Name");
            System.out.println("Name before rollback " + driver.getName());
            container.rollback();
            // Without transparent persistence objects keep the state in memory
            System.out.println("Name after rollback " + driver.getName());
            // After refreshing the object is has the state like in the database
            container.ext().refresh(driver, Integer.MAX_VALUE);
            System.out.println("Name after rollback " + driver.getName());
            // #end example
        } finally {
            container.close();
        }
    }

    private static void objectStateAfterRollbackWithTP() {
        ObjectContainer container = openDatabase();
        try {
            // #example: Transparent persistence rolls back objects in memory
            Driver driver = queryForDriver(container);
            driver.setName("New Name");
            System.out.println("Name before rollback " + driver.getName());
            container.rollback();
            // Thanks to transparent persistence with the rollback strategy
            // the object state is rolled back
            System.out.println("Name after rollback " + driver.getName());
            // #end example
        } finally {
            container.close();
        }
    }
    private static void multipleTransactions() {
        ObjectContainer rootContainer = openDatabase();
        try {
            // #example: Opening a new transaction
            ObjectContainer container = rootContainer.ext().openSession();
            try{
                // We do our operations in this transaction
            } finally {
                container.close();
            }
            // #end example
        } finally {
            rootContainer.close();
        }
    }


    private static void storeExampleObjects() {
        ObjectContainer container = openDatabase();
        try {
            Car vwBeetle = new Car("VW Beetle");
            Car audi = new Car("Audi A6");
            Car ferrari = new Car("Ferrari");

            Driver joe = new Driver("Joe", audi);
            Driver joanna = new Driver("Joanna", vwBeetle);
            Driver jenny = new Driver("Jenny");
            Driver john = new Driver("John", ferrari);
            Driver jim = new Driver("Jim", audi);

            container.store(joe);
            container.store(joanna);
            container.store(jenny);
            container.store(john);
            container.store(jim);
        } finally {
            container.close();
        }
    }

    private static ObjectContainer openDatabase() {
        // #example: Rollback strategy for the transaction
        EmbeddedConfiguration configuration = Db4oEmbedded.newConfiguration();
        configuration.common().add(new TransparentPersistenceSupport(new DeactivatingRollbackStrategy()));
        // #end example
        return Db4oEmbedded.openFile(configuration, DATABASE_FILE);
    }

    private static Driver queryForDriver(ObjectContainer container) {
        return container.query(Driver.class).get(0);
    }
}
