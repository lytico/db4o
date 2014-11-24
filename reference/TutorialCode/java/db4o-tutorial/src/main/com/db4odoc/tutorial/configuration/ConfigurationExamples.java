package com.db4odoc.tutorial.configuration;


import com.db4o.Db4oEmbedded;
import com.db4o.ObjectContainer;
import com.db4o.config.BigMathSupport;
import com.db4o.config.EmbeddedConfiguration;
import com.db4o.config.UuidSupport;

public class ConfigurationExamples {

    public void configureDb4o(){
        // #example: Important configuration switches
        EmbeddedConfiguration configuration = Db4oEmbedded.newConfiguration();
        // #end example


        // #example: A few examples
        // If you are using BigDecimal or BigInteger, add bigmath-support
        configuration.common().add(new BigMathSupport());
        // If you are using UUIDs, add support for those
        configuration.common().add(new UuidSupport());
        // Add index
        configuration.common().objectClass(Driver.class).indexed(true);
        // #end example

        // #example: Finally pass the configuration container factory
        ObjectContainer container = Db4oEmbedded.openFile(configuration, "database.db4o");
        // #end example
    }

    private static class Driver{

    }
}
