package com.db4odoc.strategies.refactoring.arraychange;

import com.db4o.Db4oEmbedded;
import com.db4o.ObjectContainer;

import java.io.File;
import java.util.List;


public class ChangeArrayType {
    public static final String DATABASE_FILE = "database.db4o";

    public static void main(String[] args) {
        cleanUp();
        storeOldData();
        listItems(PersonOld.class);
        refactorToArrayType();
        listItems(PersonNew.class);
    }

    private static void refactorToArrayType() {
        ObjectContainer container = Db4oEmbedded.openFile(DATABASE_FILE);
        try {
            // #example: Copy the string-field to the new string-array field
            List<PersonOld> oldPersons = container.query(PersonOld.class);
            for (PersonOld old : oldPersons) {
                PersonNew newPerson = new PersonNew();
                newPerson.setName(new String[]{old.getName()});
                container.store(newPerson);
                container.delete(old);
            }
            // #end example
        } finally {
            container.close();
        }
    }

    private static void listItems(Class personOldClass) {
        ObjectContainer container = Db4oEmbedded.openFile(DATABASE_FILE);
        try {
            for (Object obj : container.query(personOldClass)) {
                System.out.println(obj);
            }

        } finally {
            container.close();
        }
    }

    private static void storeOldData() {
        ObjectContainer container = Db4oEmbedded.openFile(DATABASE_FILE);
        try {
            container.store(new PersonOld("Joe"));
            container.store(new PersonOld("Joanna"));
            container.store(new PersonOld("Joel"));
        } finally {
            container.close();
        }
    }


    private static void cleanUp() {
        new File(DATABASE_FILE).delete();
    }
}
