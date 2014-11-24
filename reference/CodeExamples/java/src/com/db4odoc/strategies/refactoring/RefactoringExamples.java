package com.db4odoc.strategies.refactoring;

import com.db4o.Db4oEmbedded;
import com.db4o.ObjectContainer;
import com.db4o.ObjectSet;
import com.db4o.config.EmbeddedConfiguration;
import com.db4o.ext.StoredClass;
import com.db4o.ext.StoredField;


public class RefactoringExamples {
    private static final String DATABASE_FILE = "database.db4o";

    public static void main(String[] args) {
        renameFieldAndClass();
        changeType();
    }


    private static void renameFieldAndClass() {
        createOldDatabase();


        EmbeddedConfiguration configuration = refactorClassAndFieldName();
        ObjectContainer container = Db4oEmbedded.openFile(configuration, DATABASE_FILE);
        ObjectSet<PersonNew> persons = container.query(PersonNew.class);
        for (PersonNew person : persons) {
            System.out.println(person.getSirname());
        }
        container.close();
    }

    private static void changeType() {
        storeInDB(new Person(),new Person("John"));

        
        // #example: copying the data from the old field type to the new one
        ObjectContainer container = Db4oEmbedded.openFile( "database.db4o");
        try{
            // first get all objects which should be updated
            ObjectSet<Person> persons = container.query(Person.class);
            for (Person person : persons) {
                // get the database-metadata about this object-type
                StoredClass dbClass = container.ext().storedClass(person);
                // get the old field which was an int-type
                StoredField oldField = dbClass.storedField("id", int.class);
                if(null!=oldField){
                    // Access the old data and copy it to the new field!
                    Object oldValue = oldField.get(person);
                    if(null!=oldValue){
                        person.id = new Identity((Integer)oldValue);
                        container.store(person);
                    }                   
                }
            }
        } finally {
            container.close();
        }
        // #end example
    }

    private static EmbeddedConfiguration refactorClassAndFieldName() {
        EmbeddedConfiguration configuration = Db4oEmbedded.newConfiguration();
        // #example: Rename a class
        configuration.common().objectClass("com.db4odoc.strategies.refactoring.PersonOld")
                .rename("com.db4odoc.strategies.refactoring.PersonNew");
        // #end example:
        // #example: Rename field
        configuration.common().objectClass("com.db4odoc.strategies.refactoring.PersonOld")
                .objectField("name").rename("sirname");
        // #end example
        return configuration;
    }

    private static void createOldDatabase() {
        storeInDB(new PersonOld(),new PersonOld("Papa Joe"));
    }

    private static void storeInDB(Object...objects) {
        ObjectContainer container = Db4oEmbedded.openFile(DATABASE_FILE);
        try{
            for (Object object : objects) {
                container.store(object);
            }
        } finally {
            container.close();
        }
    }
}
