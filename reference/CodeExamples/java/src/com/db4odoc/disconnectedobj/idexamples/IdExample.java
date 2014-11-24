package com.db4odoc.disconnectedobj.idexamples;

import com.db4o.ObjectContainer;
import com.db4o.config.EmbeddedConfiguration;
import com.db4o.events.EventRegistry;


public interface IdExample<TId> {
    TId idForObject(Object obj, ObjectContainer database);
    Object objectForID(TId idForObject, ObjectContainer database);
    void configure(EmbeddedConfiguration configuration);
    void registerEventOnContainer(ObjectContainer container);

}
