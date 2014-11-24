package com.db4odoc.drs.vod;

import javax.jdo.PersistenceManager;
import javax.jdo.PersistenceManagerFactory;


public class StoreObjectsWithJDO {

    public static void main(String[] args) {
        // #example: Use the persistence manager to store objects
        PersistenceManagerFactory factory = JDOUtilities.createPersistenceFactory();

        PersistenceManager persistence = factory.getPersistenceManager();
        persistence.currentTransaction().begin();

        Pilot john = new Pilot("John",42);
        Car car = new Car(john,"Fiat Punto");

        persistence.makePersistent(car);

        persistence.currentTransaction().commit();
        persistence.close();
        // #end example
    }


}
