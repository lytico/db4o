package com.db4odoc.indexing.costbenefit;

import com.db4o.Db4oEmbedded;
import com.db4o.ObjectContainer;
import com.db4o.config.EmbeddedConfiguration;
import com.db4o.query.Query;

import java.io.File;
import java.util.Random;


public class CostBenefit {
    private static final String DATABASE_FILE = "database.db4o";

    public static void main(String[] args) {
        doRun(noIndex(),"noIndex");
        doRun(withIndex(),"withIndex");
        doRun(withIndexExt(),"withIndexLarge");

        doRun(noIndex(),"noIndex");
        doRun(withIndex(),"withIndex");
        doRun(withIndexExt(),"withIndexLarge");
    }

    private static EmbeddedConfiguration noIndex() {
        return Db4oEmbedded.newConfiguration();
    }
    private static EmbeddedConfiguration withIndex() {
        EmbeddedConfiguration config = Db4oEmbedded.newConfiguration();
        config.common().objectClass(IndexedClass.class).objectField("intIndex").indexed(true);
        return config;
    }
    private static EmbeddedConfiguration withIndexExt() {
        EmbeddedConfiguration config = Db4oEmbedded.newConfiguration();
        config.common().bTreeNodeSize(4096);
        config.common().objectClass(IndexedClass.class).objectField("intIndex").indexed(true);
        return config;
    }

    private static void doRun(EmbeddedConfiguration config,String name) {
        cleanUp();
        System.out.println("Run: "+name);
        ObjectContainer container = Db4oEmbedded.openFile(config,DATABASE_FILE);
        try {
            measureTimingStore(container);
            measureTimingQuery(container);
        } finally {
            container.close();
        }
    }

    private static void cleanUp() {
        new File(DATABASE_FILE).delete();
    }

    private static void measureTimingStore(ObjectContainer container) {
        long time = System.currentTimeMillis();
        storeLoadsOfObjects(container);
        long timeUsed = System.currentTimeMillis()-time;
        System.out.println("Store took:"+timeUsed);
    }

    private static void measureTimingQuery(ObjectContainer container) {
        long time = System.currentTimeMillis();
        for(int i=0;i<100;i++){
            query(container);

        }
        long timeUsed = System.currentTimeMillis()-time;
        System.out.println("Query took:"+timeUsed);

    }

    private static void query(ObjectContainer container) {
        final int number = new Random().nextInt();
        final Query query = container.query();
        query.constrain(IndexedClass.class);
        query.descend("intIndex").constrain(number);
        query.execute().size();
    }

    private static void storeLoadsOfObjects(ObjectContainer container) {
        for(int i=0;i<100;i++){
            storeBatchOfObjects(container);
        }
    }

    private static void storeBatchOfObjects(ObjectContainer container) {
        Random rnd = new Random();
        for(int i=0;i<1000;i++){
            container.store(IndexedClass.create(rnd));
        }
        container.commit();
    }


    static class IndexedClass {
        private int intIndex;
        private String otherData;

        public IndexedClass(int intIndex) {
            this.intIndex = intIndex;
            this.otherData = "This is more data =)";
        }

        public static IndexedClass create(Random rnd) {
            int intIndex = newInt(rnd);
            return new IndexedClass(intIndex);
        }

        private static int newInt(Random rnd) {
            return rnd.nextInt();
        }


    }
}
