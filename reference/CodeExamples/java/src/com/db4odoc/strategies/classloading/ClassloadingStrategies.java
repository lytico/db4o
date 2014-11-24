package com.db4odoc.strategies.classloading;

import com.db4o.Db4oEmbedded;
import com.db4o.ObjectContainer;
import com.db4o.config.EmbeddedConfiguration;
import com.db4o.reflect.jdk.JdkReflector;


public class ClassloadingStrategies {

    public static void useTheContextClassloader(){

        //#example: Use the context classloader
        EmbeddedConfiguration configuration = Db4oEmbedded.newConfiguration();
        configuration.common().reflectWith(new JdkReflector(Thread.currentThread().getContextClassLoader()));
        // #end example
        ObjectContainer container = Db4oEmbedded.openFile(configuration, "database.db4o");
        try {

        } finally {
            container.close();
        }
    }
}
