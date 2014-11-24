package com.db4odoc.pitfalls.activation;

import com.db4o.Db4oEmbedded;
import com.db4o.ObjectContainer;
import com.db4o.config.EmbeddedConfiguration;
import com.db4o.query.Predicate;

import java.io.File;


public class ActivationDepthPitfall {
    public static final String DATABASE_FILE = "database.db4o";

    public static void main(String[] args) {
        cleanUp();
        prepareDeepObjGraph();

        try {
            runIntoActivationIssue();
        } catch (Exception e) {
            e.printStackTrace();
        }

        fixItWithExplicitlyActivating();
        fixItWithHigherActivationDepth();
        deactivate();
    }

    private static void fixItWithExplicitlyActivating() {
        ObjectContainer container = Db4oEmbedded.openFile(DATABASE_FILE);
        try {
            final Person jodie = queryForJodie(container);

            // #example: Fix with explicit activation
            Person julia = jodie.mother().mother().mother().mother().mother();
            container.activate(julia,5);

            System.out.println(julia.getName());
            String joannaName = julia.mother().getName();
            System.out.println(joannaName);
            // #end example
        } finally {
            container.close();
        }
    }
    private static void deactivate() {
        ObjectContainer container = Db4oEmbedded.openFile(DATABASE_FILE);
        try {
            final Person jodie = queryForJodie(container);

            container.activate(jodie,5);

            // #example: Deactivate an object
            System.out.println(jodie.getName());
            container.deactivate(jodie,5);
            // Now all fields will be null or 0
            // The same applies for all references objects up to a depth of 5
            System.out.println(jodie.getName());
            // #end example
        } finally {
            container.close();
        }
    }
    private static void fixItWithHigherActivationDepth() {

        EmbeddedConfiguration configuration = Db4oEmbedded.newConfiguration();
        configuration.common().activationDepth(16);
        ObjectContainer container = Db4oEmbedded.openFile(configuration, DATABASE_FILE);
        try {
            final Person jodie = queryForJodie(container);

            Person julia = jodie.mother().mother().mother().mother().mother();

            System.out.println(julia.getName());
            String joannaName = julia.mother().getName();
            System.out.println(joannaName);
        } finally {
            container.close();
        }
    }

    private static void runIntoActivationIssue() {
        ObjectContainer container = Db4oEmbedded.openFile(DATABASE_FILE);
        try {
            // #example: Run into not activated objects
            final Person jodie = queryForJodie(container);
            Person julia = jodie.mother().mother().mother().mother().mother();
            // This will print null
            // Because julia is not activated
            // and therefore all fields are not set
            System.out.println(julia.getName());
            // This will throw a NullPointerException.
            // Because julia is not activated
            // and therefore all fields are not set
            String joannaName = julia.mother().getName();
            // #end example
        } finally {
            container.close();
        }
    }

    private static void cleanUp() {
        new File(DATABASE_FILE).delete();
    }

    private static Person queryForJodie(ObjectContainer container) {
        return container.query(new Predicate<Person>() {
        @Override
        public boolean match(Person o) {
            return o.getName().equals("Jodie");
        }
    }).get(0);
    }

    private static void prepareDeepObjGraph() {
        ObjectContainer container = Db4oEmbedded.openFile(DATABASE_FILE);
        try {
            Person joanna = new Person("Joanna");
            Person jenny = new Person(joanna, "Jenny");
            Person julia = new Person(jenny, "Julia");
            Person jill = new Person(julia, "Jill");
            Person joel = new Person(jill, "Joel");
            Person jamie = new Person(joel, "Jamie");
            Person jodie = new Person(jamie, "Jodie");
            container.store(jodie);
        } finally {
            container.close();
        }
    }
}
