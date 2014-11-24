package com.db4odoc.indexing.types;

import com.db4o.Db4oEmbedded;
import com.db4o.ObjectContainer;
import com.db4o.ObjectSet;
import com.db4o.config.BigMathSupport;
import com.db4o.config.EmbeddedConfiguration;
import com.db4o.query.Query;

import java.io.File;
import java.math.BigInteger;
import java.util.Random;


public class TypeIndexing {
    public static final String DATABASE_FILE = "database.db4o";
    private static final int START_VALUE_FOR_STORE = 50000;
    private static final int RUNS = 10;

    public static void main(String[] args) {
        cleanUp();
        fillDatabaseWithIndex();
    }

    private static void cleanUp() {
        new File(DATABASE_FILE).delete();
    }

    private static void fillDatabaseWithIndex() {
        final ObjectContainer container = Db4oEmbedded.openFile(indexConfiguration(), "database.db4o");
        try {
            for(int i=0;i<RUNS;i++){
                System.out.println("RUN(Objects="+((i+1)*START_VALUE_FOR_STORE)+")--------------");
                oneRun(container);
            }

        } finally {
            container.close();
        }
    }

    private static void oneRun(final ObjectContainer container) {
        storeLotsOfObjects(container);
        timeRun(new Runnable() {
            @Override
            public void run() {
                final ObjectSet<Object> objects = queryFor(container,"intIndex");
            }
        },"intIndex" );
        timeRun(new Runnable() {
            @Override
            public void run() {
                final ObjectSet<Object> objects = queryFor(container,"longIndex");
            }
        },"longIndex" );
        timeRun(new Runnable() {
            @Override
            public void run() {
                final ObjectSet<Object> objects = queryForAsString(container,"stringIndex");
            }
        },"stringIndex" );
        timeRun(new Runnable() {
            @Override
            public void run() {
                final ObjectSet<Object> objects = queryForAsBigInt(container,"bigIntIndex");
            }
        },"bigIntIndex" );
    }

    private static ObjectSet<Object> queryFor(ObjectContainer container, String field) {
        final Random rnd = new Random();
        final Query query = container.query();
        query.constrain(IndexedClass.class);
        query.descend(field).constrain(rnd.nextInt(START_VALUE_FOR_STORE *10));
        return query.execute();
    }

    private static ObjectSet<Object> queryForAsString(ObjectContainer container,
                                              String field) {
        final Random rnd = new Random();
        final Query query = container.query();
        query.constrain(IndexedClass.class);
        query.descend(field).constrain(String.valueOf(rnd.nextInt(START_VALUE_FOR_STORE *10)));
        return query.execute();
    }

    private static ObjectSet<Object> queryForAsBigInt(ObjectContainer container,
                                              String field) {
        final Random rnd = new Random();
        final Query query = container.query();
        query.constrain(IndexedClass.class);
        query.descend(field).constrain(new BigInteger(String.valueOf(rnd.nextInt(START_VALUE_FOR_STORE *10))));
        return query.execute();
    }

    private static void timeRun(Runnable toRun, String label) {
        long time = System.currentTimeMillis();
        for(int i=0;i<10;i++){
            toRun.run();             
        }
        System.out.println("Run "+label+" took "+(System.currentTimeMillis()-time));
    }


    private static void storeLotsOfObjects(ObjectContainer container) {
        Random rnd = new Random();
        for (int i = 0; i < START_VALUE_FOR_STORE; i++) {
            container.store(IndexedClass.create(rnd));
        }
        container.commit();
    }

    private static EmbeddedConfiguration indexConfiguration() {
        EmbeddedConfiguration config = Db4oEmbedded.newConfiguration();
        config.common().objectClass(IndexedClass.class).objectField("intIndex").indexed(true);
        config.common().objectClass(IndexedClass.class).objectField("longIndex").indexed(true);
        config.common().objectClass(IndexedClass.class).objectField("stringIndex").indexed(true);
        config.common().objectClass(IndexedClass.class).objectField("bigIntIndex").indexed(true);

        config.common().add(new BigMathSupport());
        return config;
    }


    static class IndexedClass {
        private int intIndex;
        private long longIndex;
        private String stringIndex;
        private BigInteger bigIntIndex;

        public IndexedClass(int intIndex) {
            this.intIndex = intIndex;
            this.longIndex = intIndex;
            this.stringIndex = String.valueOf(intIndex);
            this.bigIntIndex = new BigInteger(stringIndex);
        }

        public static IndexedClass create(Random rnd) {
            int intIndex = newInt(rnd);
            return new IndexedClass(intIndex);
        }

        private static int newInt(Random rnd) {
            return rnd.nextInt(START_VALUE_FOR_STORE *10);
        }


    }
}
