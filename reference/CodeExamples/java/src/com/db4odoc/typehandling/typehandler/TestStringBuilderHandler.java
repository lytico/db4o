package com.db4odoc.typehandling.typehandler;

import com.db4o.Db4oEmbedded;
import com.db4o.EmbeddedObjectContainer;
import com.db4o.config.EmbeddedConfiguration;
import com.db4o.defragment.Defragment;
import com.db4o.defragment.DefragmentConfig;
import com.db4o.io.MemoryStorage;
import com.db4o.io.Storage;
import com.db4o.typehandlers.SingleClassTypeHandlerPredicate;
import org.junit.Assert;
import org.junit.Before;
import org.junit.Test;

import java.io.IOException;


public class TestStringBuilderHandler {
    static final String DATA_BASE_FILE = "database.db4o";
    private EmbeddedObjectContainer container;
    private Storage storage;

    @Before
    public void setup(){
        storage = new MemoryStorage();
        EmbeddedConfiguration configuration = newConfig();

        this.container = Db4oEmbedded.openFile(configuration, DATA_BASE_FILE);
    }

    @Test
    public void canStore(){
        storeInstance();
    }
    @Test
    public void canRead(){
        storeInstance();
        assertCanReadData();
    }


    @Test
    public void canReadAfterDefragment() throws IOException {
        storeInstance();
        container.close();

        DefragmentConfig cfg = new DefragmentConfig(DATA_BASE_FILE);
        cfg.db4oConfig(newConfig());

        Defragment.defrag(cfg);

        this.container = Db4oEmbedded.openFile(newConfig(), DATA_BASE_FILE);
        assertCanReadData();
        
    }

    private EmbeddedConfiguration newConfig() {
        EmbeddedConfiguration configuration = Db4oEmbedded.newConfiguration();
        configuration.file().storage(storage);
        configuration.common().registerTypeHandler(new SingleClassTypeHandlerPredicate(StringBuilder.class),
                new StringBuilderHandler());
        return configuration;
    }

    private void assertCanReadData() {
        final Holder holder = container.query(Holder.class).get(0);
        Assert.assertEquals(holder.getBuilder().toString(),"TestData");
    }


    private void storeInstance() {
        container.store(new Holder());
    }

    private static class Holder{
        StringBuilder builder ;

        private Holder(String data) {
            this.builder = new StringBuilder(data);
        }

        public Holder() {
            this("TestData");
        }

        public StringBuilder getBuilder() {
            return builder;
        }

        public void setBuilder(StringBuilder builder) {
            this.builder = builder;
        }
    }

}
