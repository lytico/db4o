package com.db4odoc.performance.query.soda;


import com.db4o.Db4oEmbedded;
import com.db4o.ObjectContainer;
import com.db4o.ObjectSet;
import com.db4o.query.Query;
import com.db4odoc.javalang.NoArgAction;
import com.db4odoc.performance.Item;
import com.db4odoc.performance.ItemHolder;
import com.db4odoc.performance.StopWatch;
import org.joda.time.DateTime;
import org.junit.After;
import org.junit.Before;
import org.junit.BeforeClass;
import org.junit.Test;

import java.io.File;
import java.util.Date;
import java.util.Random;

public class GoodPerformance {
    private static final String DATABASE_FILE = "good-performance.db4o";
    private static final int NUMBER_OF_ITEMS = 200000;
    private ObjectContainer container;
    private Random rnd = new Random();

    @BeforeClass
    public static void removeDatabase() {
        new File(DATABASE_FILE).delete();
    }

    @Before
    public void prepareData() {
        container = Db4oEmbedded.openFile(DATABASE_FILE);
        if (container.query().execute().size() == 0) {
            storeTestData(container);
        }
    }

    @After
    public void cleanUp() {
        container.close();
    }

    private void storeTestData(ObjectContainer container) {
        for (int i = 0; i < NUMBER_OF_ITEMS; i++) {
            container.store(ItemHolder.create(new Item(i)));
        }
    }

    @Test
    public void equalsOnIndexedField() {
        StopWatch.time(new NoArgAction() {
            @Override
            public void invoke() {
                final String criteria = Item.dataString(rnd.nextInt(NUMBER_OF_ITEMS));

                // #example: Equals on indexed field
                final Query query = container.query();
                query.constrain(Item.class);
                query.descend("indexedString")
                        .constrain(criteria);
                // #end example

                final ObjectSet<ItemHolder> result = query.execute();
                System.out.println("Number of result items " + result.size());
            }
        });

    }
    @Test
    public void notEquals() {
        StopWatch.time(new NoArgAction() {
            @Override
            public void invoke() {
                final String criteria = Item.dataString(rnd.nextInt(NUMBER_OF_ITEMS));

                // #example: Not equals on indexed field
                final Query query = container.query();
                query.constrain(Item.class);
                query.descend("indexedString")
                        .constrain(criteria).not();
                // #end example

                final ObjectSet<ItemHolder> result = query.execute();
                System.out.println("Number of result items " + result.size());
            }
        });
    }

    @Test
    public void equalsAcrossIndexedFields() {
        StopWatch.time(new NoArgAction() {
            @Override
            public void invoke() {
                final String criteria = Item.dataString(rnd.nextInt(NUMBER_OF_ITEMS));

                // #example: Equals across indexed fields
                // Note that the type of the 'indexedReference' has to the specific type
                // which holds the 'indexedString'
                final Query query = container.query();
                query.constrain(ItemHolder.class);
                query.descend("indexedReference").descend("indexedString")
                        .constrain(criteria);
                // #end example

                final ObjectSet<ItemHolder> result = query.execute();
                System.out.println("Number of result items " + result.size());
            }
        });
    }

    @Test
    public void searchByReference() {
        StopWatch.time(new NoArgAction() {
            @Override
            public void invoke() {
                // #example: Query by reference
                Item item = loadItemFromDatabase();

                final Query query = container.query();
                query.constrain(ItemHolder.class);
                query.descend("indexedReference")
                        .constrain(item);
                // #end example

                final ObjectSet<ItemHolder> result = query.execute();
                System.out.println("Number of result items " + result.size());
            }
        });
    }

    @Test
    public void biggerThan() {
        StopWatch.time(new NoArgAction() {
            @Override
            public void invoke() {
                final int criteria = rnd.nextInt(NUMBER_OF_ITEMS);

                // #example: Bigger than
                final Query query = container.query();
                query.constrain(Item.class);
                query.descend("indexNumber")
                        .constrain(criteria).greater();
                // #end example

                final ObjectSet<ItemHolder> result = query.execute();
                System.out.println("Number of result items " + result.size());
            }
        });

    }
    @Test
    public void notBiggerThan() {
        StopWatch.time(new NoArgAction() {
            @Override
            public void invoke() {
                final int criteria = rnd.nextInt(NUMBER_OF_ITEMS);
                final Query query = container.query();
                query.constrain(Item.class);
                query.descend("indexNumber")
                        .constrain(criteria).not().greater();

                final ObjectSet<ItemHolder> result = query.execute();
                System.out.println("Number of result items " + result.size());
            }
        });

    }

    @Test
    public void inBetween() {
        StopWatch.time(new NoArgAction() {
            @Override
            public void invoke() {
                final int criteria = rnd.nextInt(NUMBER_OF_ITEMS);
                final int biggerThanThis = criteria - 10;
                final int smallerThanThis = criteria + 10;

                // #example: In between
                final Query query = container.query();
                query.constrain(Item.class);
                query.descend("indexNumber")
                        .constrain(biggerThanThis).greater().and(
                        query.descend("indexNumber").constrain(smallerThanThis).smaller());
                // #end example

                final ObjectSet<ItemHolder> result = query.execute();
                System.out.println("Number of result items " + result.size());
            }
        });
    }
    @Test
    public void findADate() {
        StopWatch.time(new NoArgAction() {
            @Override
            public void invoke() {
                Date date = new DateTime(rnd.nextInt(NUMBER_OF_ITEMS)).toDate();

                // #example: Date comparisons are also fast
                final Query query = container.query();
                query.constrain(Item.class);
                query.descend("indexDate")
                        .constrain(date);
                // #end example

                final ObjectSet<ItemHolder> result = query.execute();
                System.out.println("Number of result items " + result.size());
            }
        });
    }
    @Test
    public void newerData() {
        StopWatch.time(new NoArgAction() {
            @Override
            public void invoke() {
                Date date = new DateTime(rnd.nextInt(NUMBER_OF_ITEMS)).toDate();

                // #example: Find a newer date
                final Query query = container.query();
                query.constrain(Item.class);
                query.descend("indexDate")
                        .constrain(date).greater();
                // #end example

                final ObjectSet<ItemHolder> result = query.execute();
                System.out.println("Number of result items " + result.size());
            }
        });
    }

    private Item loadItemFromDatabase() {
        final String criteria = Item.dataString(rnd.nextInt(NUMBER_OF_ITEMS));
        final Query itemQuery = container.query();
        itemQuery.constrain(Item.class);
        itemQuery.descend("indexedString")
                .constrain(criteria);
        return (Item) itemQuery.execute().get(0);
    }
}
