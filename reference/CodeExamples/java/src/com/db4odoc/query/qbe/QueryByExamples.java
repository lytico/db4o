package com.db4odoc.query.qbe;

import com.db4o.Db4oEmbedded;
import com.db4o.ObjectContainer;
import com.db4o.ObjectSet;

import java.io.File;


public class QueryByExamples {
    private static final String DATABASE_FILE = "database.db4o";

    public static void main(String[] args) {
        cleanUp();
        ObjectContainer container = Db4oEmbedded.openFile(DATABASE_FILE);
        try {
            storeData(container);

            queryForName(container);
            queryForAge(container);
            queryForNameAndAge(container);

            nestedObjects(container);
            allObjects(container);
            allObjectsOfAType(container);
            allObjectsOfATypeWithEmptyObject(container);

            containsQuery(container);
            structuredContains(container);
        } finally {
            container.close();
        }
    }

    private static void queryForName(ObjectContainer container) {
        // #example: Query for John by example
        Pilot theExample = new Pilot();
        theExample.setName("John");
        final ObjectSet<Pilot> result = container.queryByExample(theExample);
        // #end example

        listResult(result);
    }

    private static void queryForAge(ObjectContainer container) {
        // #example: Query for 33 year old pilots
        Pilot theExample = new Pilot();
        theExample.setAge(33);
        final ObjectSet<Pilot> result = container.queryByExample(theExample);
        // #end example

        listResult(result);
    }

    private static void queryForNameAndAge(ObjectContainer container) {
        // #example: Query a 29 years old Jo
        Pilot theExample = new Pilot();
        theExample.setName("Jo");
        theExample.setAge(29);
        final ObjectSet<Pilot> result = container.queryByExample(theExample);
        // #end example

        listResult(result);
    }

    private static void allObjects(ObjectContainer container) {
        // #example: All objects
        final ObjectSet<Pilot> result = container.queryByExample(null);
        // #end example

        listResult(result);
    }

    private static void allObjectsOfAType(ObjectContainer container) {
        // #example: All objects of a type by passing the type
        final ObjectSet<Pilot> result = container.queryByExample(Pilot.class);
        // #end example

        listResult(result);
    }

    private static void allObjectsOfATypeWithEmptyObject(ObjectContainer container) {
        // #example: All objects of a type by passing a empty example
        Pilot example = new Pilot();
        final ObjectSet<Pilot> result = container.queryByExample(example);
        // #end example

        listResult(result);
    }

    private static void nestedObjects(ObjectContainer container) {
        // #example: Nested objects example
        Pilot pilotExample = new Pilot();
        pilotExample.setName("Jenny");

        Car carExample = new Car();
        carExample.setPilot(pilotExample);
        final ObjectSet<Car> result = container.queryByExample(carExample);
        // #end example

        listResult(result);
    }

    private static void containsQuery(ObjectContainer container) {
        // #example: Contains in collections
        BlogPost pilotExample = new BlogPost();
        pilotExample.addTags("db4o");
        final ObjectSet<Car> result = container.queryByExample(pilotExample);
        // #end example

        listResult(result);
    }

    private static void structuredContains(ObjectContainer container) {
        // #example: Structured contains
        BlogPost pilotExample = new BlogPost();
        pilotExample.addAuthors(new Author("John"));
        final ObjectSet<Car> result = container.queryByExample(pilotExample);
        // #end example

        listResult(result);
    }


    private static void listResult(ObjectSet result) {
        for (Object object : result) {
            System.out.println(object);
        }
    }

    private static void cleanUp() {
        new File(DATABASE_FILE).delete();
    }

    private static void storeData(ObjectContainer container) {
        Pilot john = new Pilot("John", 42);
        Pilot joanna = new Pilot("Joanna", 45);
        Pilot jenny = new Pilot("Jenny", 21);
        Pilot rick = new Pilot("Rick", 33);
        Pilot juliette = new Pilot("Juliette", 33);
        container.store(new Pilot("Jo", 34));
        container.store(new Pilot("Jo", 29));
        container.store(new Pilot("Jimmy", 33));


        container.store(new Car(john, "Ferrari"));
        container.store(new Car(joanna, "Mercedes"));
        container.store(new Car(jenny, "Volvo"));
        container.store(new Car(rick, "Fiat"));
        container.store(new Car(juliette, "Suzuki"));


        BlogPost firstPost = new BlogPost("db4o", "Content about db4o");
        firstPost.addTags("db4o", ".net", "java", "database");
        firstPost.addMetaData("comment-feed-link", "localhost/rss");
        firstPost.addAuthors(new Author("John"), new Author("Jenny"), new Author("Joanna"));

        container.store(firstPost);

        BlogPost secondPost = new BlogPost("cars", "Speedy cars");
        secondPost.addTags("cars", "fast");
        secondPost.addMetaData("comment-feed-link", "localhost/rss");
        secondPost.addMetaData("source", "www.wikipedia.org");
        secondPost.addAuthors(new Author("Joanna"), new Author("Jenny"));

        container.store(secondPost);
    }
}
