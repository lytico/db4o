package com.db4odoc.tutorial.activation;


import com.db4o.Db4oEmbedded;
import com.db4o.ObjectContainer;
import com.db4o.config.EmbeddedConfiguration;
import com.db4o.query.Predicate;

public class ActivationConcept {
    private static final String DATABASE_FILE = "database.db4o";

    public static void main(String[] args) {
        storeExampleObjects();
        runningIntoActivationLimit();
        dealWithActivation();
        increaseActivationDepth();

    }

    private static void storeExampleObjects() {
        ObjectContainer container = Db4oEmbedded.openFile(DATABASE_FILE);
        try {
            // #example: Store a deep object hierarchy
            Person eva = new Person("Eva",null);
            Person julia = new Person("Julia",eva);
            Person jennifer = new Person("Jennifer",julia);
            Person jamie = new Person("Jamie",jennifer);
            Person jill = new Person("Jill",jamie);
            Person joanna = new Person("Joanna",jill);

            Person joelle = new Person("Joelle",joanna);
            container.store(joelle);
            // #end example
        } finally {
            container.close();
        }
    }

    private static void runningIntoActivationLimit() {
        ObjectContainer container = Db4oEmbedded.openFile(DATABASE_FILE);
        try {
            // #example: Activation depth in action
            Person joelle = queryForJoelle(container);
            Person jennifer = joelle.getMother().getMother().getMother().getMother();
            System.out.println("Is activated: " + jennifer);
            // Now we step across the activation boundary
            // therefore the next person isn't activate anymore.
            // That means all fields are set to null or default-value
            Person julia = jennifer.getMother();
            System.out.println("Isn't activated anymore"+julia);
            // #end example

            try{
                // #example: NullPointer exception due to not activated objects
                String nameOfMother = julia.getMother().getName();
                // #end example
            } catch (NullPointerException ex){
                System.out.println("Exception due to not activated object "+ex);
            }
        } finally {
            container.close();
        }
    }

    private static void dealWithActivation() {
        ObjectContainer container = Db4oEmbedded.openFile(DATABASE_FILE);
        try {
            Person joelle = queryForJoelle(container);
            Person julia = joelle.getMother().getMother().getMother().getMother().getMother();

            // #example: Check if an instance is activated
            boolean isActivated = container.ext().isActive(julia);
            // #end example
            System.out.println("Is activated? "+ isActivated);
            // #example: Activate instance to a depth of five
            container.activate(julia,5);
            // #end example
            System.out.println("Is activated? "+container.ext().isActive(julia));

        } finally {
            container.close();
        }
    }

    private static void increaseActivationDepth() {
        // #example: Increase the activation depth to 10
        EmbeddedConfiguration configuration = Db4oEmbedded.newConfiguration();
        configuration.common().activationDepth(10);
        ObjectContainer container = Db4oEmbedded.openFile(configuration,DATABASE_FILE);
        // #end example
        try {
            Person joelle = queryForJoelle(container);
            Person julia = joelle.getMother().getMother().getMother().getMother().getMother();

            boolean isActivated = container.ext().isActive(julia);
            System.out.println("Is activated? "+ isActivated);

        } finally {
            container.close();
        }
    }

    private static EmbeddedConfiguration moreActivationOptions() {
        // #example: More activation options
        EmbeddedConfiguration configuration = Db4oEmbedded.newConfiguration();
        // At least activate persons to a depth of 10
        configuration.common().objectClass(Person.class).minimumActivationDepth(10);
        // Or maybe we just want to activate all referenced objects
        configuration.common().objectClass(Person.class).cascadeOnActivate(true);
        // #end example
        return configuration;
    }

    private static Person queryForJoelle(ObjectContainer container) {
        return container.query(new Predicate<Person>() {
                    @Override
                    public boolean match(Person p) {
                        return p.getName().equals("Joelle");
                    }
                }).get(0);
    }
}
