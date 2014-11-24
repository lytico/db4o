package com.db4odoc.omj;

import com.db4o.EmbeddedObjectContainer;
import com.db4o.config.EmbeddedConfiguration;
import com.db4o.config.EmbeddedConfigurationItem;
import com.db4o.config.UuidSupport;

// #example: A configuration item for Java
public class ConfigureDBForOmj implements EmbeddedConfigurationItem{
    public void prepare(EmbeddedConfiguration embeddedConfiguration) {
        // Your configuration goes here.
        // For example:
        embeddedConfiguration.common().add(new UuidSupport());
    }

    public void apply(EmbeddedObjectContainer embeddedObjectContainer) {
    }
}
// #end example
