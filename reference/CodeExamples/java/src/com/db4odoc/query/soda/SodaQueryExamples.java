package com.db4odoc.query.soda;

import com.db4o.Db4oEmbedded;
import com.db4o.ObjectContainer;
import com.db4o.ObjectSet;
import com.db4o.config.EmbeddedConfiguration;
import com.db4o.query.Query;

import java.io.File;


public class SodaQueryExamples {
    private static final String DATABASE_FILE = "database.db4o";

    public static void main(String[] args) {
        cleanUp();
        final EmbeddedConfiguration cfg = Db4oEmbedded.newConfiguration();
        ObjectContainer container = Db4oEmbedded.openFile(cfg,DATABASE_FILE);
        try {
            storeData(container);

            simplestPossibleQuery(container);
            equalsConstrain(container);
            greaterThanConstrain(container);
            greaterOrEqualConstrain(container);
            notConstrain(container);
            combiningConstrains(container);
            stringConstrains(container);
            compareWithStoredObject(container);
            descentDeeper(container);
            
            containsOnCollection(container);
            descendIntoCollectionMembers(container);
            containsOnMaps(container);
            fieldObject(container);
                         
            genericConstrains(container);
            descendIntoNotExistingField(container);
            mixWithQueryByExample(container);

        } finally {
            container.close();
        }
    }

    private static void simplestPossibleQuery(ObjectContainer container) {
        System.out.println("Type constrain for the objects");
        // #example: Type constrain for the objects
        final Query query = container.query();
        query.constrain(Pilot.class);

        ObjectSet result = query.execute();
        // #end example
        listResult(result);
    }
    private static void equalsConstrain(ObjectContainer container) {
        System.out.println("A simple constrain on a field");
        // #example: A simple constrain on a field
        final Query query = container.query();
        query.constrain(Pilot.class);
        query.descend("name").constrain("John");

        final ObjectSet<Object> result = query.execute();
        // #end example
        listResult(result);
    }

    private static void greaterThanConstrain(ObjectContainer container) {
        System.out.println("A greater than constrain");
        // #example: A greater than constrain
        Query query = container.query();
        query.constrain(Pilot.class);
        query.descend("age").constrain(42).greater();

        ObjectSet<Object> result = query.execute();
        // #end example
        listResult(result);
    }
    private static void greaterOrEqualConstrain(ObjectContainer container) {
        System.out.println("A greater than or equals constrain");
        // #example: A greater than or equals constrain
        Query query = container.query();
        query.constrain(Pilot.class);
        query.descend("age").constrain(42).greater().equal();

        ObjectSet<Object> result = query.execute();
        // #end example
        listResult(result);
    }
    private static void notConstrain(ObjectContainer container) {
        System.out.println("Not constrain");
        // #example: Not constrain
        Query query = container.query();
        query.constrain(Pilot.class);
        query.descend("age").constrain(42).not();

        ObjectSet<Object> result = query.execute();
        // #end example
        listResult(result);
    }
    private static void combiningConstrains(ObjectContainer container) {
        System.out.println("Logical combination of constrains");
        // #example: Logical combination of constrains
        Query query = container.query();
        query.constrain(Pilot.class);
        query.descend("age").constrain(42).greater()
                .or(query.descend("age").constrain(30).smaller());

        ObjectSet<Object> result = query.execute();
        // #end example

        listResult(result);
    }

    private static void stringConstrains(ObjectContainer container) {
        System.out.println("String comparison");
        // #example: String comparison
        Query query = container.query();
        query.constrain(Pilot.class);
        // First strings, you can use the contains operator
        query.descend("name").constrain("oh").contains()
                // Or like, which is like .contains(), but case insensitive
                .or(query.descend("name").constrain("AnN").like())
                        // The .endsWith and .startWith constrains are also there,
                        // the true for case-sensitive, false for case-insensitive
                .or(query.descend("name").constrain("NY").endsWith(false));

        ObjectSet<Object> result = query.execute();
        // #end example

        listResult(result);
    }

    private static void compareWithStoredObject(ObjectContainer container) {
        System.out.println("Compare with existing object");
        // #example: Compare with existing object
        Pilot pilot = container.query(Pilot.class).get(0);

        Query query = container.query();
        query.constrain(Car.class);
        // if the given object is stored, its compared by identity
        query.descend("pilot").constrain(pilot);

        ObjectSet<Object> carsOfPilot = query.execute();
        // #end example

        listResult(carsOfPilot);
    }

    private static void descentDeeper(ObjectContainer container) {
        System.out.println("Descend over multiple fields");

        // #example: Descend over multiple fields
        Query query = container.query();
        query.constrain(Car.class);
        query.descend("pilot").descend("name").constrain("John");

        ObjectSet<Object> result = query.execute();
        // #end example
        listResult(result);
    }

    private static void containsOnCollection(ObjectContainer container) {
        System.out.println("Collection contains constrain");
        // #example: Collection contains constrain
        Query query = container.query();
        query.constrain(BlogPost.class);
        query.descend("tags").constrain("db4o");

        ObjectSet<Object> result = query.execute();
        // #end example
        listResult(result);
    }

    private static void descendIntoCollectionMembers(ObjectContainer container) {
        System.out.println("Descend into collection members");
        // #example: Descend into collection members
        Query query = container.query();
        query.constrain(BlogPost.class);
        query.descend("authors").descend("name").constrain("Jenny");

        ObjectSet<Object> result = query.execute();
        // #end example
        listResult(result);
    }

    private static void containsOnMaps(ObjectContainer container) {
        System.out.println("Map contains a key constrain");
        // #example: Map contains a key constrain
        Query query = container.query();
        query.constrain(BlogPost.class);
        query.descend("metaData").constrain("source");

        ObjectSet<Object> result = query.execute();
        // #end example
        listResult(result);
    }

    private static void fieldObject(ObjectContainer container) {
        System.out.println("Return the object of a field");
        // #example: Return the object of a field
        Query query = container.query();
        query.constrain(Car.class);
        query.descend("name").constrain("Mercedes");

        // returns the pilot of these cars
        ObjectSet<Object> result = query.descend("pilot").execute();
        // #end example
        listResult(result);
    }

    private static void genericConstrains(ObjectContainer container) {
        System.out.println("Pure field constrains");
        // #example: Pure field constrains
        Query query = container.query();
        // You can simple filter objects which have a certain field
        query.descend("name").constrain(null).not();

        ObjectSet<Object> result = query.execute();
        // #end example
        listResult(result);
    }

    private static void descendIntoNotExistingField(ObjectContainer container) {
        System.out.println("Using not existing fields excludes objects");
        // #example: Using not existing fields excludes objects
        Query query = container.query();
        query.constrain(Pilot.class);
        // using not existing fields doesn't throw an exception
        // but rather exclude all object which don't use this field
        query.descend("notExisting").constrain(null).not();

        ObjectSet<Object> result = query.execute();
        // #end example

        listResult(result);
    }

    private static void mixWithQueryByExample(ObjectContainer container) {
        System.out.println("Mix with query by example");
        // #example: Mix with query by example
        Query query = container.query();
        query.constrain(Car.class);
        // if the given object is not stored,
        // it will behave like query by example for the given object
        final Pilot examplePilot = new Pilot(null, 42);
        query.descend("pilot").constrain(examplePilot);

        ObjectSet<Object> carsOfPilot = query.execute();
        // #end example

        listResult(carsOfPilot);
    }



    private static void cleanUp() {
        new File(DATABASE_FILE).delete();
    }


    private static void listResult(ObjectSet result) {
        for (Object object : result) {
            System.out.println(object);
        }
    }

    private static void storeData(ObjectContainer container) {
        Pilot john = new Pilot("John",42);
        Pilot joanna = new Pilot("Joanna",45);
        Pilot jenny = new Pilot("Jenny",21);
        Pilot rick = new Pilot("Rick",33);

        container.store(new Car(john,"Ferrari"));
        container.store(new Car(joanna,"Mercedes"));
        container.store(new Car(jenny,"Volvo"));
        container.store(new Car(rick,"Fiat"));

        BlogPost firstPost = new BlogPost("db4o","Content about db4o");
        firstPost.addTags("db4o",".net","java","database");
        firstPost.addMetaData("comment-feed-link", "localhost/rss");
        firstPost.addAuthors(new Author("John"),new Author("Jenny"), new Author("Joanna"));

        container.store(firstPost);

        BlogPost secondPost = new BlogPost("cars","Speedy cars");
        secondPost.addTags("cars","fast");
        secondPost.addMetaData("comment-feed-link", "localhost/rss");
        secondPost.addMetaData("source","www.wikipedia.org");
        secondPost.addAuthors(new Author("Joanna"),new Author("Jenny"));

        container.store(secondPost);

    }
}
