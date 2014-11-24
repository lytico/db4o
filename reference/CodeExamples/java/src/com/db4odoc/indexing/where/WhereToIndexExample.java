package com.db4odoc.indexing.where;

import com.db4o.Db4oEmbedded;
import com.db4o.ObjectContainer;
import com.db4o.ObjectSet;
import com.db4o.config.EmbeddedConfiguration;
import com.db4o.diagnostic.Diagnostic;
import com.db4o.diagnostic.DiagnosticListener;
import com.db4o.diagnostic.LoadedFromClassIndex;
import com.db4o.query.Predicate;

import java.io.File;
import java.util.Random;


public class WhereToIndexExample {
    private static final String DATABASE_FILE = "database.db4o";

    public static void main(String[] args) {
        storeObjects();
        runQuery();
        addIndex();
        runQuery();

    }

    private static void addIndex() {
        final EmbeddedConfiguration configuration = Db4oEmbedded.newConfiguration();
        configuration.common().objectClass(IndexedClass.class).objectField("id").indexed(true);
        ObjectContainer container = Db4oEmbedded.openFile(configuration,DATABASE_FILE);
        container.query(IndexedClass.class);
        container.close();
    }

    private static void runQuery() {
        final EmbeddedConfiguration configuration = Db4oEmbedded.newConfiguration();
        // #example: Find queries which cannot use index
        configuration.common().diagnostic().addListener(new DiagnosticListener() {
            @Override
            public void onDiagnostic(Diagnostic diagnostic) {
                if(diagnostic instanceof LoadedFromClassIndex)
                {
                    System.out.println("This query couldn't use field indexes "+
                        ((LoadedFromClassIndex)diagnostic).reason());
                    System.out.println(diagnostic);
                }
            }
        });
        // #end example
        ObjectContainer container = Db4oEmbedded.openFile(configuration,DATABASE_FILE);
        try {
            final ObjectSet<IndexedClass> result = container.query(new Predicate<IndexedClass>() {
                @Override
                public boolean match(IndexedClass o) {
                    return o.getId() == 42;
                }
            });
            result.size();
        } finally {
            container.close();
        }
    }

    private static void storeObjects() {
        cleanUp();
        ObjectContainer container = Db4oEmbedded.openFile(DATABASE_FILE);
        try {
            storeObjects(container);
        } finally {
            container.close();
        }
    }
    private static void cleanUp() {
        new File(DATABASE_FILE).delete();
    }

    private static void storeObjects(ObjectContainer container) {
        Random rnd = new Random();
        for(int i=0;i<10000;i++){
            container.store(IndexedClass.create(rnd));
        }
    }

    static class IndexedClass {
        private int id;
        private String otherData;

        public IndexedClass(int id) {
            this.id = id;
            this.otherData = "This is more data =)";
        }

        public static IndexedClass create(Random rnd) {
            int intIndex = newInt(rnd);
            return new IndexedClass(intIndex);
        }

        private static int newInt(Random rnd) {
            return rnd.nextInt();
        }

        public int getId() {
            return id;
        }
    }
}
