package com.db4odoc.disconnectedobj.idexamples;

import com.db4o.ObjectContainer;
import com.db4o.config.EmbeddedConfiguration;
import com.db4o.events.*;
import com.db4o.foundation.Iterator4;
import com.db4o.internal.LazyObjectReference;
import com.db4o.query.Predicate;


public class AutoIncrementExample implements IdExample<Integer> {

    public static IdExample<Integer> create(){
        return new AutoIncrementExample();
    }

    public Integer idForObject(Object obj, ObjectContainer container) {
        // #example: get the id
        IDHolder idHolder = (IDHolder)obj;
        int id = idHolder.getId();
        // #end example
        return id;
    }

    public Object objectForID(Integer idForObject, ObjectContainer container) {
        final int id = idForObject; 
        // #example: get an object by its id
        Object object = container.query(new Predicate<IDHolder>() {
            @Override
            public boolean match(IDHolder o) {
                return o.getId() == id;
            }
        }).get(0);
        // #end example
        return object;
    }

    public void configure(EmbeddedConfiguration configuration) {
        // #example: index the id-field
        configuration.common().objectClass(IDHolder.class).objectField("id").indexed(true);
        // #end example
    }

    public void registerEventOnContainer(final ObjectContainer container) {
        // #example: use events to assign the ids
        final AutoIncrement increment = new AutoIncrement(container);
        EventRegistry eventRegistry = EventRegistryFactory.forObjectContainer(container);
        eventRegistry.creating().addListener(new EventListener4<CancellableObjectEventArgs>() {
            public void onEvent(Event4<CancellableObjectEventArgs> event4,
                                CancellableObjectEventArgs objectArgs) {
                if(objectArgs.object() instanceof IDHolder){
                    IDHolder idHolder = (IDHolder) objectArgs.object();
                    idHolder.setId(increment.getNextID(idHolder.getClass()));
                }
            }
        });
        eventRegistry.committing().addListener(new EventListener4<CommitEventArgs>() {
            public void onEvent(Event4<CommitEventArgs> commitEventArgsEvent4,
                                CommitEventArgs commitEventArgs) {
                increment.storeState();
            }
        });
        // #end example
    }
}
