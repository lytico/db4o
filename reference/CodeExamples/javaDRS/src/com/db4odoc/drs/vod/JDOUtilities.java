package com.db4odoc.drs.vod;


import javax.jdo.JDOHelper;
import javax.jdo.PersistenceManager;
import javax.jdo.PersistenceManagerFactory;
import java.io.IOException;
import java.io.InputStream;

class JDOUtilities {
    static final String VERSANT_PROPERTY_FILE = "versant.properties";

    private JDOUtilities(){}

    static PersistenceManagerFactory createPersistenceFactory() {
        // #example: Opening the persistence factory
        InputStream propertySource = Thread.currentThread()
                .getContextClassLoader().getResourceAsStream(VERSANT_PROPERTY_FILE);
        if (null == propertySource) {
            throw new RuntimeException("Couldn't find resource '" + VERSANT_PROPERTY_FILE + "' in the classpath");
        }
        try {
            return JDOHelper.getPersistenceManagerFactory(propertySource);
        } finally {
            try {
                propertySource.close();
            } catch (IOException ignored) {
            }
        }
        // #end example
    }

    static void inTransaction(PersistenceManagerFactory factory, JDOTransaction txOperation){
        PersistenceManager manager = factory.getPersistenceManager();
        try{
            manager.currentTransaction().begin();
            txOperation.invoke(manager);
            manager.currentTransaction().commit();
        } finally {
            manager.close();
        }
    }

    private static String extractName(String connectionURL) {
        return connectionURL.substring("versant:".length()).split("@")[0];
    }

    private static boolean notVersantDBConnection(String connectionURL) {
        return !connectionURL.startsWith("versant");
    }

    private static boolean isEmpty(String connectionURL) {
        return null==connectionURL || 0==connectionURL.trim().length();
    }
}
