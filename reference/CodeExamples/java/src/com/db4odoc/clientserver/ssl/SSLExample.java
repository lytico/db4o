package com.db4odoc.clientserver.ssl;

import com.db4o.ObjectContainer;
import com.db4o.ObjectServer;
import com.db4o.cs.Db4oClientServer;
import com.db4o.cs.config.ClientConfiguration;
import com.db4o.cs.config.ServerConfiguration;
import com.db4o.cs.ssl.SSLSupport;

import javax.net.ssl.SSLContext;
import java.io.File;
import java.io.IOException;
import java.security.KeyStoreException;
import java.security.NoSuchAlgorithmException;

public class SSLExample {
    private static final String DATABASE_FILE_NAME = "database.db4o";
    private static final String USER_AND_PASSWORD = "sa";
    private static final int PORT_NUMBER = 1337;

    public static void main(String[] args) throws Exception {
        sslExample();
        specialSSLExample();
    }


    /**
     * Note that this example requires a valid configured key & trust-store.
     * Read more about Java's SSL-encryption on the Java-documentation.
     * <br/>
     * For trying this example out,
     * you can create a self-signed test-certificate with JDK's keytool console-application:
     * <code>keytool -genkeypair -keystore KeyStoreFileName </code><br/>
     * 
     * Then specify for the server to use that certificate. Pass it as argument:<br/>
     * <code>-Djavax.net.ssl.keyStore=KeyStoreFileName<br/>
     * -Djavax.net.ssl.keyStorePassword=passwordYouUsed</code><br/>
     *
     * Then add use the same certificate store for trusted certificates for the client:<br/>
     * <code>-Djavax.net.ssl.trustStore=KeyStoreFileName <br/>
     * -Djavax.net.ssl.trustStorePassword=passwordYouUsed</code>
     */
    private static void sslExample() throws Exception {
        cleanUp();
        // #example: Add SSL-support to the server
        ServerConfiguration configuration = Db4oClientServer.newServerConfiguration();
        configuration.common().add(new SSLSupport());
        // #end example

        ObjectServer server = openServer(configuration);

        sslClient();

        server.close();
        cleanUp();
    }

    /**
     * See also {@link #sslExample}
     */
    private static void specialSSLExample() throws Exception {
        cleanUp();
        // #example: You can build up a regular SSL-Context and us it

        // You can build your own SSLContext and use all features of Java's SSL-support.
        // To keep this example small, we just use the default-context instead of building one.
        SSLContext context = SSLContext.getDefault();


        ServerConfiguration configuration = Db4oClientServer.newServerConfiguration();
        configuration.common().add(new SSLSupport(context));
        // #end example

        ObjectServer server = openServer(configuration);

        sslClient();

        server.close();
        cleanUp();
    }


    private static ObjectServer openServer(ServerConfiguration configuration) {
        ObjectServer server = Db4oClientServer.openServer(configuration, DATABASE_FILE_NAME, PORT_NUMBER);
        server.grantAccess(USER_AND_PASSWORD,USER_AND_PASSWORD);
        return server;
    }

    private static void sslClient() throws NoSuchAlgorithmException, KeyStoreException, IOException {
       // #example: Add SSL-support to the client
        ClientConfiguration configuration = Db4oClientServer.newClientConfiguration();
        configuration.common().add(new SSLSupport());
        // #end example
        ObjectContainer container = openClient(configuration);
        container.store(new Person());
        System.out.println("Stored person");
        container.close();
    }

    private static ObjectContainer openClient(ClientConfiguration configuration) throws NoSuchAlgorithmException, KeyStoreException, IOException {


        return Db4oClientServer.openClient(configuration,"localhost",
                PORT_NUMBER, USER_AND_PASSWORD, USER_AND_PASSWORD);
    }

    private static void cleanUp() {
        new File(DATABASE_FILE_NAME).delete();
    }


    private static class Person{
        private String name;

        private Person() {
            name = "Joe";
        }
    }
}
