package com.db4odoc.clientserver.hilo;

import com.db4o.ObjectContainer;
import com.db4o.events.*;


public class HiLoIdSupport {

    public static void install(ObjectContainer container) {
        final EventRegistry events = EventRegistryFactory.forObjectContainer(container);

        final HiLoIdGenerator generator = new HiLoIdGenerator();
        events.creating().addListener(new EventListener4<CancellableObjectEventArgs>() {
            public void onEvent(Event4<CancellableObjectEventArgs> event,
                                CancellableObjectEventArgs objectInfo) {
                Object added = objectInfo.object();
                ObjectContainer container = objectInfo.objectContainer();
                if (needsId(added)) {
                    assignId(added, container, generator);
                }

            }
        });
    }

    private static void assignId(Object object, ObjectContainer container, HiLoIdGenerator generator) {
        IdHolder idHolder = (IdHolder) object;
        idHolder.setId(generator.nextIdFor(object.getClass(), container));
    }

    private static boolean needsId(Object object) {
        return object instanceof IdHolder;
    }
}
