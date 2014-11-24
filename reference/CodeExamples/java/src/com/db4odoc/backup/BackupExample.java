package com.db4odoc.backup;

import com.db4o.Db4oEmbedded;
import com.db4o.ObjectContainer;
import com.db4o.io.FileStorage;

import java.io.File;


public class BackupExample {
    private static final String DATABASE_FILE = "database.db4o";

    public static void main(String[] args) {
        cleanDB();
        simpleBackup();
        backupWithStorage();
    }

    private static void cleanDB() {
        new File(DATABASE_FILE).delete();
    }

    private static void backupWithStorage() {
        ObjectContainer container = Db4oEmbedded.openFile(DATABASE_FILE);
        try {
            storeObjects(container);
            // #example: Store a backup with storage
            container.ext().backup(new FileStorage(),"advanced-backup.db4o");
            // #end example
        } finally {
            container.close();
        }
    }

    private static void simpleBackup() {
        ObjectContainer container = Db4oEmbedded.openFile(DATABASE_FILE);
        try {
            storeObjects(container);
            // #example: Store a backup while using the database
            container.ext().backup("backup.db4o");
            // #end example
        } finally {
            container.close();
        }
    }

    private static void storeObjects(ObjectContainer container) {
        container.store(new Person("John","Walker"));
        container.store(new Person("Joanna","Waterman"));
        container.commit();
    }

    static class Person{
        private String sirname;
        private final String firstname;

        Person(String name,String firstname) {
            this.firstname = firstname;
            this.sirname = name;
        }

        public String getName() {
            return sirname;
        }

        public String getFirstname() {
            return firstname;
        }
    }
}
