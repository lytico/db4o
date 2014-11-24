package com.db4odoc.ta.collections;

import com.db4o.Db4oEmbedded;
import com.db4o.ObjectContainer;
import com.db4o.config.EmbeddedConfiguration;
import com.db4o.ta.TransparentActivationSupport;

import java.io.File;

public class CollectionsExample {
    private static final String DATABASE_FILE_NAME = "database.db4o";
    public static void main(String[] args) {
        cleanUp();
        {
            ObjectContainer container = openDatabaseWithTA();
            Team team = new Team();
            team.add(new Pilot("John"));
            team.add(new Pilot("Max"));
            container.store(team);
            container.close();
        }
        {
            ObjectContainer container = openDatabaseWithTA();
            Team team = container.query(Team.class).get(0);
            for (Pilot pilot : team.getPilots()) {
                System.out.println(pilot);
            }
            container.close();
        }
        cleanUp();
    }

    private static ObjectContainer openDatabaseWithTA() {
        EmbeddedConfiguration configuration = Db4oEmbedded.newConfiguration();
        configuration.common().add(new TransparentActivationSupport());
        return Db4oEmbedded.openFile(configuration, DATABASE_FILE_NAME);
    }

    private static void cleanUp() {
        new File(DATABASE_FILE_NAME).delete();
    }
}
