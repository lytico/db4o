package com.db4odoc.collections.bigset;

import com.db4o.Db4oEmbedded;
import com.db4o.ObjectContainer;
import com.db4o.ObjectSet;
import com.db4o.collections.CollectionFactory;
import com.db4o.config.EmbeddedConfiguration;

import java.io.File;
import java.util.Random;
import java.util.Set;


public class BigSetExample {
    private static final String DATABASE_FILE_NAME = "database.db4o";
    private static final int POPULATION_SIZE = 10000;

    public static void main(String[] args) {
        cleanUp();

        storeBigSet();
        checkInBigSet();
        bigSetIsByIdentity();
        
        cleanUp();
    }


    private static void storeBigSet() {
        ObjectContainer container = openDatabase();

        City city = createCity(container);
        container.store(city);
        storeOtherPeople(container);
        container.commit();
        
        container.close();
    }

    private static void checkInBigSet() {
        ObjectContainer container = openDatabase();


        City city = container.query(City.class).get(0);
        System.out.println("City's population "+city.population());

        checkAFewPersons(container, city);

        container.close();
    }
    private static void bigSetIsByIdentity() {
        ObjectContainer container = openDatabase();

        City city = container.query(City.class).get(0);

        // #example: Note that the big-set compares by identity, not by equality
        Person aCitizen = city.citizen().iterator().next();
        System.out.println("The big-set uses the identity, not equality of an object");
        System.out.println("Therefore it .contains() on the same person-object is "
                +city.citizen().contains(aCitizen));
        Person equalPerson = new Person(aCitizen.getName());
        System.out.println("Therefore it .contains() on a equal person-object is "
                +city.citizen().contains(equalPerson));
        // #end example

        container.close();
    }

    private static City createCity(ObjectContainer container) {
        // #example: Crate a big-set instance
        Set<Person> citizen= CollectionFactory.forObjectContainer(container).newBigSet();
        // now you can use the big-set like a normal set:
        citizen.add(new Person("Citizen Kane"));
        // #end example
        for(int i=0;i< POPULATION_SIZE;i++){
            citizen.add(new Person("Citizen No "+i));
        }
        return new City(citizen);
    }

    private static void storeOtherPeople(ObjectContainer container) {
        for(int i=0;i< POPULATION_SIZE;i++){
            container.store(new Person("Citizen No "+i));
        }
    }

    private static void checkAFewPersons(ObjectContainer container, City city) {
        Random random = new Random();
        ObjectSet<Person> persons = container.query(Person.class);
        int personCount = persons.size();
        for(int i=0;i<10;i++){
            Person aPerson = persons.get(random.nextInt(personCount));
            printCitizenStatus(city, aPerson);
        }
    }

    private static void printCitizenStatus(City city, Person aPerson) {
        if(city.isCitizen(aPerson)){
            System.out.println(aPerson+" is a citizen");
        } else{
            System.out.println(aPerson+" isn't a citizen");
        }
    }


    private static ObjectContainer openDatabase() {
        return Db4oEmbedded.openFile(DATABASE_FILE_NAME);
    }

    private static void cleanUp() {
        new File(DATABASE_FILE_NAME).delete();
    }
}
