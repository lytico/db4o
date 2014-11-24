package com.db4odoc.tutorial.firststeps;


import com.db4o.Db4oEmbedded;
import com.db4o.ObjectContainer;
import com.db4o.query.Predicate;

import java.util.List;

public class BasicOperations {

    public static void main(String[] args) {
        openAndCloseDatabase();
        storeObject();
        query();
        updateObject();
        deleteObject();
    }

    public static void openAndCloseDatabase(){
        // #example: Open and close db4o
        ObjectContainer container = Db4oEmbedded.openFile("database.db4o");
        try{
            // use the object container in here
        } finally {
            container.close();
        }
        // #end example
    }

    private  static void storeObject() {
        // #example: Store an object
        ObjectContainer container = Db4oEmbedded.openFile("databaseFile.db4o");
        try {
            Driver driver = new Driver("Joe");
            container.store(driver);
        } finally {
            container.close();
        }
        // #end example
    }

    private static void query() {
        // #example: Query for objects
        ObjectContainer container = Db4oEmbedded.openFile("databaseFile.db4o");
        try {
            List<Driver> drivers = container.query(new Predicate<Driver>() {
                public boolean match(Driver d) {
                    return d.getName().equals("Joe");
                }
            });
            System.out.println("Stored Pilots:");
            for (Driver driver : drivers) {
                System.out.println(driver.getName());
            }
        } finally {
            container.close();
        }
        // #end example
    }

    private static void updateObject() {
        // #example: Update an object
        ObjectContainer container = Db4oEmbedded.openFile("databaseFile.db4o");
        try {
            List<Driver> drivers = container.query(new Predicate<Driver>() {
                public boolean match(Driver d) {
                    return d.getName().equals("Joe");
                }
            });
            Driver driver = drivers.get(0);
            System.out.println("Old name" +driver.getName());
            driver.setName("John");
            System.out.println("New name" +driver.getName());
            // update the pilot
            container.store(driver);
        } finally {
            container.close();
        }
        // #end example
    }
    private static void deleteObject() {
        // #example: Delete an object
        ObjectContainer container = Db4oEmbedded.openFile("databaseFile.db4o");
        try {
            List<Driver> drivers = container.query(new Predicate<Driver>() {
                public boolean match(Driver d) {
                    return d.getName().equals("Joe");
                }
            });
            Driver driver = drivers.get(0);
            System.out.println("Deleting " +driver.getName());
            container.delete(driver);
        } finally {
            container.close();
        }
        // #end example
    }
}
