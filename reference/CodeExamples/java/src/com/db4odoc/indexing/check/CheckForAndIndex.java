package com.db4odoc.indexing.check;

import com.db4o.Db4oEmbedded;
import com.db4o.ObjectContainer;
import com.db4o.config.EmbeddedConfiguration;
import com.db4o.ext.StoredClass;
import com.db4o.ext.StoredField;


public class CheckForAndIndex {
    public static void main(String[] args) {

        EmbeddedConfiguration configuration = Db4oEmbedded.newConfiguration();
        configuration.common().objectClass(IndexedClass.class).objectField("id").indexed(true);
        ObjectContainer container = Db4oEmbedded.openFile(configuration, "database.db4o");
        try {
            container.store(new IndexedClass(1));

            // #example: Check for a index
            StoredClass metaInfo = container.ext().storedClass(IndexedClass.class);
            // list a fields and check if they have a index
            for (StoredField field : metaInfo.getStoredFields()) {
                if(field.hasIndex()){
                    System.out.println("The field '"+field.getName()+"' is indexed");
                } else{
                    System.out.println("The field '"+field.getName()+"' isn't indexed");
                }
            }
            // #end example
        } finally {
            container.close();
        }
    }

    static class IndexedClass {
        private int id;
        private String data;

        public IndexedClass(int id) {
            this.id = id;
        }

        public int getId() {
            return id;
        }
    }

}
