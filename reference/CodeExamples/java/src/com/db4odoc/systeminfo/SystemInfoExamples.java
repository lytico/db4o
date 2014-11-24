package com.db4odoc.systeminfo;

import com.db4o.Db4oEmbedded;
import com.db4o.ObjectContainer;


public class SystemInfoExamples {
    public static void main(String[] args) {

        ObjectContainer container = Db4oEmbedded.openFile("database.db4o");
        try {
            // #example: Freespace size info
            long freeSpaceSize = container.ext().systemInfo().freespaceSize();
            System.out.println("Freespace in bytes: "+freeSpaceSize);
            // #end example

            // #example: Freespace entry count info
            int freeSpaceEntries = container.ext().systemInfo().freespaceEntryCount();
            System.out.println("Freespace-entries count: "+freeSpaceEntries);
            // #end example

            // #example: Database size info
            long databaseSize = container.ext().systemInfo().totalSize();
            System.out.println("Database size: "+databaseSize);
            // #end example

        } finally {
            container.close();
        }
        
    }
}
