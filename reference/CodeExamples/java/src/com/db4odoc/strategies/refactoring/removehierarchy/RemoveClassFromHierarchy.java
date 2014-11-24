package com.db4odoc.strategies.refactoring.removehierarchy;

import com.db4o.Db4oEmbedded;
import com.db4o.ObjectContainer;
import com.db4o.ObjectSet;


public class RemoveClassFromHierarchy {
    private static final String DATABASE_FILE_NAME = "database.db4o";

    public static void main(String[] args) {
        storeOldObjectLayout();
        listItems();
        System.out.println("--After refactoring--");
        copyToNewType();
        listItems();
    }


    private static void copyToNewType() {
        ObjectContainer container = Db4oEmbedded.openFile(DATABASE_FILE_NAME);
        try {
            // #example: copy the data from the old type to the new one
            ObjectSet<Human> allMammals = container.query(Human.class);
            for (Human oldHuman : allMammals) {
                HumanNew newHuman = new HumanNew("");
                newHuman.setBodyTemperature(oldHuman.getBodyTemperature());
                newHuman.setIq(oldHuman.getIq());
                newHuman.setName(oldHuman.getName());

                container.store(newHuman);
                container.delete(oldHuman);
            }
            // #end example

        } finally {
            container.close();
        }
    }


    private static void listItems() {
        ObjectContainer container = Db4oEmbedded.openFile(DATABASE_FILE_NAME);
        try {
            ObjectSet<Mammal> allMammals = container.query(Mammal.class);
            for (Mammal mammal : allMammals) {
                System.out.println(mammal);
            }

        } finally {
            container.close();
        }
    }

    private static void storeOldObjectLayout() {
        ObjectContainer container = Db4oEmbedded.openFile(DATABASE_FILE_NAME);
        try {
            container.store(new Human("Joe"));
            container.store(new Human("Joey"));
        } finally {
            container.close();
        }
    }
}
