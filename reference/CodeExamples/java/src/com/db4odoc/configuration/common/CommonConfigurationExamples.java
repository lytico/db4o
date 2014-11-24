package com.db4odoc.configuration.common;

import com.db4o.Db4oEmbedded;
import com.db4o.ObjectContainer;
import com.db4o.config.EmbeddedConfiguration;
import com.db4o.config.QueryEvaluationMode;
import com.db4o.config.SimpleNameProvider;
import com.db4o.config.encoding.StringEncodings;
import com.db4o.diagnostic.DiagnosticToConsole;
import com.db4o.reflect.jdk.JdkLoader;
import com.db4o.reflect.jdk.JdkReflector;
import com.db4o.ta.TransparentPersistenceSupport;

import java.io.File;
import java.net.MalformedURLException;
import java.net.URL;
import java.net.URLClassLoader;


public class CommonConfigurationExamples {
    private static final String DATABASE_FILE = "database.db4o";

    private static void changeGlobalActivationDepth() {
        // #example: Change activation depth
        EmbeddedConfiguration configuration = Db4oEmbedded.newConfiguration();
        configuration.common().activationDepth(2);
        // #end example
        
        ObjectContainer container = Db4oEmbedded.openFile(configuration, DATABASE_FILE);
        container.close();
    }
    private static void updateFileFormat() {
        // #example: Update the database-format
        EmbeddedConfiguration configuration = Db4oEmbedded.newConfiguration();
        configuration.common().allowVersionUpdates(true);

        // reopen and close the database to do the update
        ObjectContainer container = Db4oEmbedded.openFile(configuration, DATABASE_FILE);
        container.close();
        // #end example
    }

    private static void disableAutomaticShutdown() {
        // #example: Disable automatic shutdown
        EmbeddedConfiguration configuration = Db4oEmbedded.newConfiguration();
        configuration.common().automaticShutDown(false);
        // #end example
        
        ObjectContainer container = Db4oEmbedded.openFile(configuration, DATABASE_FILE);
        container.close();
    }

    private static void changeBTreeNodeSize() {
        // #example: Change B-tree node size
        EmbeddedConfiguration configuration = Db4oEmbedded.newConfiguration();
        configuration.common().bTreeNodeSize(256);
        // #end example
        
        ObjectContainer container = Db4oEmbedded.openFile(configuration, DATABASE_FILE);
        container.close();
    }

    private static void disableCallbacks() {
        // #example: Disable callbacks
        EmbeddedConfiguration configuration = Db4oEmbedded.newConfiguration();
        configuration.common().callbacks(false);
        // #end example

        ObjectContainer container = Db4oEmbedded.openFile(configuration, DATABASE_FILE);
        container.close();
    }

    private static void callConstructors() {
        // #example: Call constructors
        EmbeddedConfiguration configuration = Db4oEmbedded.newConfiguration();
        configuration.common().callConstructors(true);
        // #end example

        ObjectContainer container = Db4oEmbedded.openFile(configuration, DATABASE_FILE);
        container.close();
    }

    private static void disableSchemaEvolution() {
        // #example: Disable schema evolution
        EmbeddedConfiguration configuration = Db4oEmbedded.newConfiguration();
        configuration.common().detectSchemaChanges(false);
        // #end example

        ObjectContainer container = Db4oEmbedded.openFile(configuration, DATABASE_FILE);
        container.close();
    }


    private static void addDiagnosticListener() {
        // #example: Add a diagnostic listener
        EmbeddedConfiguration configuration = Db4oEmbedded.newConfiguration();
        configuration.common().diagnostic().addListener(new DiagnosticToConsole());
        // #end example

        ObjectContainer container = Db4oEmbedded.openFile(configuration, DATABASE_FILE);
        container.close();
    }

    private static void addTransparentPersistence() {
        // #example: add an additional configuration item
        EmbeddedConfiguration configuration = Db4oEmbedded.newConfiguration();
        configuration.common().add(new TransparentPersistenceSupport());
        // other configurations...
        ObjectContainer container = Db4oEmbedded.openFile(configuration, DATABASE_FILE);
        // #end example
        container.close();
    }

    private static void disableExceptionsOnNotStorableObjects() {
        // #example: Disable exceptions on not storable objects
        EmbeddedConfiguration configuration = Db4oEmbedded.newConfiguration();
        configuration.common().exceptionsOnNotStorable(false);
        // #end example

        ObjectContainer container = Db4oEmbedded.openFile(configuration, DATABASE_FILE);

        container.close();
    }


    private static void changeMessageLevel() {
        // #example: Change the message-level
        EmbeddedConfiguration configuration = Db4oEmbedded.newConfiguration();
        configuration.common().messageLevel(4);
        // #end example

        ObjectContainer container = Db4oEmbedded.openFile(configuration, DATABASE_FILE);
        container.close();
    }




    private static void changeOutputStream() {
        // #example: Change the output stream
        EmbeddedConfiguration configuration = Db4oEmbedded.newConfiguration();
        // you can use any output stream for the message-output
        configuration.common().outStream(System.err);
        // #end example

        ObjectContainer container = Db4oEmbedded.openFile(configuration, DATABASE_FILE);
        container.close();
    }


    private static void changeQueryMode() {
        // #example: Change the query mode
        EmbeddedConfiguration configuration = Db4oEmbedded.newConfiguration();
        configuration.common().queries().evaluationMode(QueryEvaluationMode.IMMEDIATE);
        // #end example

        ObjectContainer container = Db4oEmbedded.openFile(configuration, DATABASE_FILE);
        container.close();
    }


    private static void internStrings() {
        // #example: intern strings
        EmbeddedConfiguration configuration = Db4oEmbedded.newConfiguration();
        configuration.common().internStrings(true);
        // #end example

        ObjectContainer container = Db4oEmbedded.openFile(configuration, DATABASE_FILE);

        container.close();
    }


    private static void changeReflector() {
        // #example: Change the reflector
        EmbeddedConfiguration configuration = Db4oEmbedded.newConfiguration();
        configuration.common().reflectWith(
                new JdkReflector(Thread.currentThread().getContextClassLoader()));
        // #end example

        ObjectContainer container = Db4oEmbedded.openFile(configuration, DATABASE_FILE);
        container.close();
    }

    private static void multipleClassLookUps() throws MalformedURLException {
        // #example: Complex class loader scenario
        EmbeddedConfiguration configuration = Db4oEmbedded.newConfiguration();
        
        JdkLoader classLookUp = new ClassLoaderLookup(
                Thread.currentThread().getContextClassLoader(),
                new URLClassLoader(new URL[]{new URL("file://./some/other/location")}));
        configuration.common().reflectWith(new JdkReflector(classLookUp));

        ObjectContainer container = Db4oEmbedded.openFile("database.db4o");
        // #end example
        container.close();
    }

    private static void maxStackSize() {
        // #example: Set the stack size
        EmbeddedConfiguration configuration = Db4oEmbedded.newConfiguration();
        configuration.common().maxStackDepth(16);
        // #end example

        ObjectContainer container = Db4oEmbedded.openFile(configuration, DATABASE_FILE);
        
        container.close();
    }

    private static void nameProvider() {
        // #example: set a name-provider
        EmbeddedConfiguration configuration = Db4oEmbedded.newConfiguration();
        configuration.common().nameProvider(new SimpleNameProvider("Database"));
        // #end example

        ObjectContainer container = Db4oEmbedded.openFile(configuration, DATABASE_FILE);

        container.close();
    }


    private static void changeWeakReferenceCollectionIntervall() {
        // #example: change weak reference collection interval
        EmbeddedConfiguration configuration = Db4oEmbedded.newConfiguration();
        configuration.common().weakReferenceCollectionInterval(10*1000);
        // #end example

        ObjectContainer container = Db4oEmbedded.openFile(configuration, DATABASE_FILE);

        container.close();
    }
    
    private static void disableWeakReferences() {
        // #example: Disable weak references
        EmbeddedConfiguration configuration = Db4oEmbedded.newConfiguration();
        configuration.common().weakReferences(false);
        // #end example

        ObjectContainer container = Db4oEmbedded.openFile(configuration, DATABASE_FILE);
        container.close();
    }

    private static void markTransient() {
        cleanUp();

        // #example: add an transient marker annotatin
        EmbeddedConfiguration configuration = Db4oEmbedded.newConfiguration();
        configuration.common().markTransient(TransientMarker.class.getName());
        // #end example

        ObjectContainer container = Db4oEmbedded.openFile(configuration, DATABASE_FILE);
        container.store(new WithTransient());
        container.close();

        readWithTransientMarker();

        cleanUp();
    }

    private static void changeStringEncoding() {
        // #example: Use the utf8 encoding
        EmbeddedConfiguration configuration = Db4oEmbedded.newConfiguration();
        configuration.common().stringEncoding(StringEncodings.utf8());
        // #end example

        ObjectContainer container = Db4oEmbedded.openFile(configuration, DATABASE_FILE);
        container.close();
    } 

    private static void disableRuntimeNQOptimizer() {
        // #example: Disable the runtime native query optimizer
        EmbeddedConfiguration configuration = Db4oEmbedded.newConfiguration();
        configuration.common().optimizeNativeQueries(false);
        // #end example

        ObjectContainer container = Db4oEmbedded.openFile(configuration, DATABASE_FILE);
        container.close();
    }

    private static void disableTestingConstructors() {
        // #example: Disable testing for callable constructors
        EmbeddedConfiguration configuration = Db4oEmbedded.newConfiguration();
        configuration.common().testConstructors(false);
        // #end example

        ObjectContainer container = Db4oEmbedded.openFile(configuration, DATABASE_FILE);
        container.close();
    }

    private static void increasingUpdateDepth() {
        // #example: Increasing the update-depth
        EmbeddedConfiguration configuration = Db4oEmbedded.newConfiguration();
        configuration.common().updateDepth(2);
        // #end example

        ObjectContainer container = Db4oEmbedded.openFile(configuration, DATABASE_FILE);
        container.close();
    }

    private static void readWithTransientMarker() {
        EmbeddedConfiguration configuration = Db4oEmbedded.newConfiguration();
        configuration.common().markTransient(TransientMarker.class.getName());
        ObjectContainer container = Db4oEmbedded.openFile(configuration, DATABASE_FILE);
        WithTransient instance = container.query(WithTransient.class).get(0);

        assertTransientNotStored(instance);

        container.close();
    }

    private static void assertTransientNotStored(WithTransient instance) {
        if(null!=instance.getTransientString()){
            throw new RuntimeException("Transient was stored!");
        }
    }

    private static void cleanUp() {
        new File(DATABASE_FILE).delete();
    }



    private static class WithTransient{
        @TransientMarker
        private String transientString = "New";

        public String getTransientString() {
            return transientString;
        }

        public void setTransientString(String transientString) {
            this.transientString = transientString;
        }
    }

}
