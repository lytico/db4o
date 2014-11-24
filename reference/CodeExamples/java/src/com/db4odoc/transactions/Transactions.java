package com.db4odoc.transactions;

import com.db4o.Db4oEmbedded;
import com.db4o.ObjectContainer;


public class Transactions {
    public static void main(String[] args) {

        ObjectContainer container = Db4oEmbedded.openFile("database.db4o");
        try {
            commitChanges(container);
            rollbackChanges(container);
            refreshAfterRollback(container);

        } finally {
            container.close();
        }
    }

    private static void rollbackChanges(ObjectContainer container) {
        // #example: Commit changes
        container.store(new Pilot("John"));
        container.store(new Pilot("Joanna"));

        container.commit();
        // #end example
    }

    private static void commitChanges(ObjectContainer container) {
        // #example: Rollback changes
        container.store(new Pilot("John"));
        container.store(new Pilot("Joanna"));

        container.rollback();
        // #end example
    }

    private static void refreshAfterRollback(ObjectContainer container) {
        // #example: Refresh objects after rollback
        final Pilot pilot = container.query(Pilot.class).get(0);
        pilot.setName("New Name");
        container.store(pilot);
        container.rollback();

        // use refresh to return the in memory objects back
        // to the state in the database.
        container.ext().refresh(pilot,Integer.MAX_VALUE);
        // #end example
    }


    static class Pilot{
        private String name;

        Pilot(String name) {
            this.name = name;
        }

        public String getName() {
            return name;
        }

        public void setName(String name) {
            this.name = name;
        }
    }
}
