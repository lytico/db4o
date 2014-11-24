package com.db4odoc.performance.index;


import com.db4o.Db4oEmbedded;
import com.db4o.ObjectContainer;
import com.db4o.ObjectSet;
import com.db4o.query.Query;
import com.db4odoc.javalang.NoArgAction;
import com.db4odoc.performance.Item;
import com.db4odoc.performance.StopWatch;
import org.junit.After;
import org.junit.Before;
import org.junit.Test;

import java.io.File;
import java.util.Random;

public class IndexSelectivity {
    private static final String DATABASE_FILE = "good-performance.db4o";
    private static final int NUMBER_OF_ITEMS =500000;
    private ObjectContainer container;
    private Random rnd = new Random();

    @Before
    public void prepareData() {
        new File(DATABASE_FILE).delete();
        container = Db4oEmbedded.openFile(DATABASE_FILE);
    }

    @After
    public void cleanUp() {
        container.close();
    }

    @Test
    public void goodSelectivity() {
        threeItemsMatch(3);
    }

    @Test
    public void badSelectivity() {
        threeItemsMatch(NUMBER_OF_ITEMS/3);
    }

    private void threeItemsMatch(int amoutOfExpectedIndexMatches) {
        final int maxValue = (NUMBER_OF_ITEMS / amoutOfExpectedIndexMatches)-1;
        for (int i = 0; i < NUMBER_OF_ITEMS; i++) {
            container.store(new Item(i % maxValue));
        }
        container.commit();

        StopWatch.time(new NoArgAction() {
            @Override
            public void invoke() {
                final String criteria = Item.dataString(rnd.nextInt(maxValue));
                final Query query = container.query();
                query.constrain(Item.class);
                query.descend("indexedString")
                        .constrain(criteria);

                final ObjectSet<Item> result = query.execute();
                System.out.println("Matches " + result.size());
            }
        });
    }
}
