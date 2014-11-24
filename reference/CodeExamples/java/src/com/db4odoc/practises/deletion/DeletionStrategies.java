package com.db4odoc.practises.deletion;

import com.db4o.Db4oEmbedded;
import com.db4o.ObjectContainer;
import com.db4o.events.*;


public class DeletionStrategies {
    public static void main(String[] args) {

        ObjectContainer container = Db4oEmbedded.openFile("database.db4o");
        try {
            installDeletionFlagSupport(container);

        } finally {
            container.close();
        }
    }

    private static void installDeletionFlagSupport(ObjectContainer container) {
        // #example: Deletion-Flag
        EventRegistry events = EventRegistryFactory.forObjectContainer(container);
        events.deleting().addListener(new EventListener4<CancellableObjectEventArgs>() {
            public void onEvent(Event4<CancellableObjectEventArgs> events,
                                CancellableObjectEventArgs eventArgument) {
                Object obj = eventArgument.object();
                // if the object has a deletion-flag:
                // set the flag instead of deleting the object
                if (obj instanceof Deletable) {
                    ((Deletable) obj).delete();
                    eventArgument.objectContainer().store(obj);
                    eventArgument.cancel();
                }
            }
        });
        // #end example
    }
}
