package com.db4odoc.tutorial.queries;


import com.db4o.Db4oEmbedded;
import com.db4o.ObjectContainer;
import com.db4o.query.Predicate;
import com.db4o.query.Query;

import java.util.List;

public class Queries {
    private static final String DATABASE_FILE = "databaseFile.db4o";

    public static void main(String[] args) {
        storeExampleObjects();
        queryForJoe();
        queryPeopleWithPowerfulCar();
        unoptimizableQuery();
        sodaQueryForJoe();
        sodaQueryForPowerfulCars();
    }

    private static void queryForJoe() {
        ObjectContainer container = Db4oEmbedded.openFile(DATABASE_FILE);
        try {
            // #example: Query for drivers named Joe
            List<Driver> drivers = container.query(new Predicate<Driver>() {
                @Override
                public boolean match(Driver driver) {
                    return driver.getName().equals("Joe");
                }
            });
            // #end example
            System.out.println("Driver named Joe");
            for (Driver driver : drivers) {
                System.out.println(driver);
            }
        } finally {
            container.close();
        }
    }

    private static void queryPeopleWithPowerfulCar() {
        ObjectContainer container = Db4oEmbedded.openFile(DATABASE_FILE);
        try {
            // #example: Query for people with powerful cars
            List<Driver> drivers = container.query(new Predicate<Driver>() {
                @Override
                public boolean match(Driver driver) {
                    return driver.getMostLovedCar().getHorsePower()>=150 && driver.getAge()>=18;
                }
            });
            // #end example
            System.out.println("People with powerful cars:");
            for (Driver driver : drivers) {
                System.out.println(driver);
            }
        } finally {
            container.close();
        }
    }

    private static void unoptimizableQuery() {
        ObjectContainer container = Db4oEmbedded.openFile(DATABASE_FILE);
        try {
            // #example: Unoptimized query
            List<Driver> drivers = container.query(new Predicate<Driver>() {
                @Override
                public boolean match(Driver driver) {
                    // Add a break point here. If the debugger stops, the query couldn't be optimized
                    // That means it runs very slowly and we should find an alternative.
                    // This example query cannot be optimized because the hash code isn't a stored in database
                    return driver.hashCode() == 42;
                }
            });
            // #end example
            for (Driver driver : drivers) {
                System.out.println(driver);
            }
        } finally {
            container.close();
        }
    }

    private static void sodaQueryForJoe() {
        ObjectContainer container = Db4oEmbedded.openFile(DATABASE_FILE);
        try {
            // #example: Query for drivers named Joe with SODA
            Query query = container.query();
            query.constrain(Driver.class);
            query.descend("name").constrain("Joe");
            List<Driver> drivers = query.execute();
            // #end example
            System.out.println("Driver named Joe");
            for (Driver driver : drivers) {
                System.out.println(driver);
            }
        } finally {
            container.close();
        }
    }

    private static void sodaQueryForPowerfulCars() {
        ObjectContainer container = Db4oEmbedded.openFile(DATABASE_FILE);
        try {
            // #example: Query for people with powerful cars with SODA
            Query query = container.query();
            query.constrain(Driver.class);
            query.descend("mostLovedCar").descend("horsePower").constrain(150).greater();
            query.descend("age").constrain(18).greater().equal();
            List<Driver> drivers = query.execute();
            // #end example
            System.out.println("People with powerful cars:");
            for (Driver driver : drivers) {
                System.out.println(driver);
            }
        } finally {
            container.close();
        }
    }


    private static void storeExampleObjects() {
        ObjectContainer container = Db4oEmbedded.openFile(DATABASE_FILE);
        try {
            Car vwBeetle = new Car("VW Beetle",90);
            Car audi = new Car("Audi A6",175);
            Car ferrari = new Car("Ferrari",215);

            Driver joe = new Driver("Joe",42,audi);
            Driver joanna = new Driver("Joanna",24,vwBeetle);
            Driver jenny = new Driver("Jenny",54);
            Driver john = new Driver("John",17,ferrari);
            Driver jim = new Driver("Jim",18,audi);

            container.store(joe);
            container.store(joanna);
            container.store(jenny);
            container.store(john);
            container.store(jim);
        } finally {
            container.close();
        }
    }
}
