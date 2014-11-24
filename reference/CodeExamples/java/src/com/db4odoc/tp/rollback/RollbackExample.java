package com.db4odoc.tp.rollback;


import com.db4o.Db4oEmbedded;
import com.db4o.ObjectContainer;
import com.db4o.config.EmbeddedConfiguration;
import com.db4o.query.Predicate;
import com.db4o.ta.DeactivatingRollbackStrategy;
import com.db4o.ta.TransparentPersistenceSupport;

import java.util.List;

public class RollbackExample {
    public static void main(String[] args) {
        rollbackDemo();
        changeAndQuery();

    }

    private static void changeAndQuery() {
        EmbeddedConfiguration configuration = Db4oEmbedded.newConfiguration();
        configuration.common()
                .add(new TransparentPersistenceSupport(new DeactivatingRollbackStrategy()));
        ObjectContainer container = Db4oEmbedded.openFile(configuration,"database.db4o");
        try {

            // #example: Rollback with rollback strategy
            Pilot pilot = container.query(Pilot.class).get(0);
            pilot.setName("MagicValue");


            List<Pilot> pilotU = container.query(new Predicate<Pilot>() {
                @Override
                public boolean match(Pilot o) {
                    return o.getName().equals("MagicValue");
                }
            });
            for (Pilot pp : pilotU) {
                System.out.println(pp);
            }

        } finally {
            container.close();
        }
    }

    private static void rollbackDemo() {
        // #example: Configure rollback strategy
        EmbeddedConfiguration configuration = Db4oEmbedded.newConfiguration();
        configuration.common()
                .add(new TransparentPersistenceSupport(new DeactivatingRollbackStrategy()));
        // #end example
        ObjectContainer container = Db4oEmbedded.openFile(configuration,"database.db4o");
        try {
            storePilot(container);

            // #example: Rollback with rollback strategy
            Pilot pilot = container.query(Pilot.class).get(0);
            pilot.setName("NewName");
            // Rollback
            container.rollback();
            // Now the pilot has the old name again
            System.out.println(pilot.getName());
            // #end example
        } finally {
            container.close();
        }
    }

    private static void storePilot(ObjectContainer container) {
        container.store(new Pilot("John"));
        container.commit();
    }

}
