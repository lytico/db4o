package com.db4odoc.configuration.objectconfig;

import com.db4o.Db4oEmbedded;
import com.db4o.ObjectContainer;
import com.db4o.config.EmbeddedConfiguration;

/**
 * @author roman.stoffel@gamlor.info
 * @since 14.06.11
 */
public class Tests {
    public static void main(String[] args) {
        {
            ObjectContainer container = Db4oEmbedded.openFile("database.db4o");
            try {
                container.store(new CallMyConstructor());
            } finally {
                container.close();
            }
        }
        {
            final EmbeddedConfiguration cfg = Db4oEmbedded.newConfiguration();
            //cfg.common().objectClass(CallMyConstructor.class).callConstructor(true);
            ObjectContainer container = Db4oEmbedded.openFile(cfg,"database.db4o");
            try {
                CallMyConstructor c = container.query(CallMyConstructor.class).get(0);
                System.out.println(c);
            } finally {
                container.close();
            }
        }
    }
}
