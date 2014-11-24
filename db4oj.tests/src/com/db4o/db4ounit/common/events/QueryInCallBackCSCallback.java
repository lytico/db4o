/* Copyright (C) 2011 Versant Inc. http://www.db4o.com */
/**
 * @sharpen.if !SILVERLIGHT
 */

package com.db4o.db4ounit.common.events;

import static com.db4o.events.EventRegistryFactory.forObjectContainer;

import com.db4o.ObjectContainer;
import com.db4o.ObjectServer;
import com.db4o.cs.Db4oClientServer;
import com.db4o.cs.config.ServerConfiguration;
import com.db4o.events.CancellableObjectEventArgs;
import com.db4o.events.Event4;
import com.db4o.events.EventListener4;
import com.db4o.io.MemoryStorage;

import db4ounit.ConsoleTestRunner;
import db4ounit.TestCase;
import db4ounit.TestLifeCycle;

/**
 * @author roman.stoffel@gamlor.info
 */
public class QueryInCallBackCSCallback implements TestCase, TestLifeCycle {
    private ObjectServer server;
    public static final int PORT = 1337;
    public static final String USERNAME_AND_PASSWORD = "sa";

    public static void main(String[] args) {
        new ConsoleTestRunner(QueryInCallBackCSCallback.class).run();
    }


    public void testCreatingCallbackUnknownMetaData() {
        ObjectContainer client = openClient();
        addListenerTo(client);
        client.store(new ToStore());
    }


	private void addListenerTo(ObjectContainer client) {
		forObjectContainer(client).creating().addListener(new EventListener4<CancellableObjectEventArgs>() {
		    public void onEvent(Event4<CancellableObjectEventArgs> event,
		                        CancellableObjectEventArgs objectInfo) {
		        ObjectContainer container = objectInfo.objectContainer();
		        // this crashes if the MetaInfoWithEnum-class is unknown!
		        container.query(MetaInfo.class);
		    }
		});
	}


    public void testCreatingCallbackWithKnownMetaData() {
        ObjectContainer client = openClient();
        ensureMetaDataAreKnown(client);
        addListenerTo(client);
        client.store(new ToStore());
    }


    public void testUpdatingCallback() {
        ObjectContainer client = openClient();
        addListenerTo(client);
        client.store(new ToStore());
    }


    public void testActivating() {
        ObjectContainer client = openClient();
        addListenerTo(client);
        client.store(new ToStore());
    }

    private void ensureMetaDataAreKnown(ObjectContainer client) {
        client.store(new MetaInfo());

        client.store(new ToStore());
        client.commit();
    }

    private ObjectContainer openClient() {
        return Db4oClientServer.openClient("localhost", PORT, USERNAME_AND_PASSWORD, USERNAME_AND_PASSWORD);
    }

    public void setUp() throws Exception {
        ServerConfiguration config = Db4oClientServer.newServerConfiguration();
        config.file().storage(new MemoryStorage());
        this.server = Db4oClientServer.openServer(config, "InMemory:File", PORT);
        this.server.grantAccess(USERNAME_AND_PASSWORD, USERNAME_AND_PASSWORD);
    }

    public void tearDown() throws Exception {
        server.close();
    }

    static class ToStore {
    }

    static class MetaInfo {
        int data;
    }
}


