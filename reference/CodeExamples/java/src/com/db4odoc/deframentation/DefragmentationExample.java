package com.db4odoc.deframentation;

import com.db4o.Db4oEmbedded;
import com.db4o.ObjectContainer;
import com.db4o.defragment.Defragment;
import com.db4o.defragment.DefragmentConfig;
import com.db4o.defragment.DefragmentInfo;
import com.db4o.defragment.DefragmentListener;
import com.db4o.query.Predicate;

import java.io.File;
import java.io.IOException;


public class DefragmentationExample {
    public static final String DATABASE_FILE = "database.db4o";

    public static void main(String[] args) throws Exception {
        simplestPossibleDefragment();
        simpleDefragmentationWithBackupLocation();
        defragmentWithConfiguration();
        defragmentationWithIdMissing();
    }

    private static void defragmentationWithIdMissing() throws IOException {
        createAndFillDatabase();


        // #example: Use a defragmentation listener
        DefragmentConfig config = new DefragmentConfig("database.db4o");
        Defragment.defrag(config,new DefragmentListener() {
            @Override
            public void notifyDefragmentInfo(DefragmentInfo defragmentInfo) {
                System.out.println(defragmentInfo);
            }
        });
        // #end example
    }

    private static void defragmentWithConfiguration() throws IOException {
        createAndFillDatabase();
        // #example: Defragment with configuration
        DefragmentConfig config = new DefragmentConfig("database.db4o");
        Defragment.defrag(config);
        // #end example
    }

    private static void simpleDefragmentationWithBackupLocation() throws IOException {
        createAndFillDatabase();
        // #example: Specify backup file explicitly
        Defragment.defrag("database.db4o", "database.db4o.bak");
        // #end example
    }


    private static void simplestPossibleDefragment() throws IOException {
        createAndFillDatabase();
        // #example: Simplest possible defragment use case
        Defragment.defrag("database.db4o");
        // #end example
    }

    private static void createAndFillDatabase() {
        cleanUp();
        ObjectContainer container = Db4oEmbedded.openFile(DATABASE_FILE);
        try {
            container.store(new Person("Joe"));
            container.store(new Person("Joanna"));
            container.store(new Person("Jenny"));
            container.store(new Person("Julia"));
            container.store(new Person("John"));
            container.store(new Person("JJ"));
            Person jimmy = new Person("Jimmy");
            jimmy.setBestFriend(new Person("Bunk"));
            container.store(jimmy);
        } finally {
            container.close();
        }
        leaveInvalidId();
    }


    private static void leaveInvalidId() {
        ObjectContainer container = Db4oEmbedded.openFile(DATABASE_FILE);
        try {
            final Person bunk = container.query(new Predicate<Person>() {
                @Override
                public boolean match(Person o) {
                    return o.getName().equals("Bunk");
                }
            }).get(0);
            container.delete(bunk);
        }finally {
            container.close();
        }
    }

    private static void cleanUp() {
        new File(DATABASE_FILE).delete();
        new File(DATABASE_FILE + ".backup").delete();
        new File(DATABASE_FILE + ".bak").delete();
    }


    static class Person {
        private String name;
        private Person bestFriend;

        Person(String name) {
            this.name = name;
        }

        public String getName() {
            return name;
        }

        public void setName(String name) {
            this.name = name;
        }

        public Person getBestFriend() {
            return bestFriend;
        }

        public void setBestFriend(Person bestFriend) {
            this.bestFriend = bestFriend;
        }
    }
}
