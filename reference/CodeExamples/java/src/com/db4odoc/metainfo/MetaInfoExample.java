package com.db4odoc.metainfo;

import com.db4o.Db4oEmbedded;
import com.db4o.ObjectContainer;
import com.db4o.ObjectSet;
import com.db4o.ext.StoredClass;
import com.db4o.ext.StoredField;


public class MetaInfoExample {
    public static void main(String[] args) {

        ObjectContainer container = Db4oEmbedded.openFile("database.db4o");
        try {
            container.store(new Person("Johnson","Roman",42));
            container.store(new Person("Miller","John",21));

            // #example: All stored classes
            // Get the information about all stored classes.
            StoredClass[] classesInDB = container.ext().storedClasses();
            for (StoredClass storedClass : classesInDB) {
                System.out.println(storedClass.getName());
            }

            // Information for a certain class
            StoredClass metaInfo = container.ext().storedClass(Person.class);
            // #end example

            // #example: Accessing stored fields
            StoredClass metaInfoForPerson = container.ext().storedClass(Person.class);
            // Access all existing fields
            for (StoredField field : metaInfoForPerson.getStoredFields()) {
                System.out.println("Field: "+field.getName());
            }
            // Accessing the field 'name' of any type.
            StoredField nameField = metaInfoForPerson.storedField("name", null);
            // Accessing the string field 'name'. Important if this field had another time in previous
            // versions of the class model
            StoredField ageField = metaInfoForPerson.storedField("age",int.class);

            // Check if the field is indexed
            boolean isAgeFieldIndexed = ageField.hasIndex();

            // Get the type of the field
            String fieldType = ageField.getStoredType().getName();
            // #end example

            // #example: Access via meta data
            StoredClass metaForPerson = container.ext().storedClass(Person.class);
            StoredField metaNameField = metaForPerson.storedField("name", null);

            ObjectSet<Person> persons = container.query(Person.class);
            for (Person person : persons) {
                String name = (String)metaNameField.get(person);
                System.out.println("Name is "+name);
            }
            // #end example
        } finally {
            container.close();
        }
    }
}


class Person{
    private String sirname;
    private String firstname;
    private int age;

    public Person(String sirname, String firstname, int age) {
        this.sirname = sirname;
        this.firstname = firstname;
        this.age = age;
    }

    public String getSirname() {
        return sirname;
    }

    public String getFirstname() {
        return firstname;
    }

    public int getAge() {
        return age;
    }
}