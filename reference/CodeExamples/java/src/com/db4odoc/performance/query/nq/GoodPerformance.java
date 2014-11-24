package com.db4odoc.performance.query.nq;


import com.db4o.Db4oEmbedded;
import com.db4o.ObjectContainer;
import com.db4o.query.Predicate;
import com.db4o.query.Query;
import com.db4odoc.javalang.NoArgAction;
import com.db4odoc.performance.GenericItemHolder;
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
import java.util.List;
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
        warmupNQ();
    }

    private void warmupNQ() {
        final List<Item> result = container.query(new Predicate<Item>() {
            @Override
            public boolean match(Item o) {
                return o.getIndexedString().equals("doMatchNothing");
            }
        });
    }

    @After
    public void cleanUp() {
        container.close();
    }
    @Test
    public void equalsOnIndexedField() {
        StopWatch.time(new NoArgAction() {
            @Override
            public void invoke() {
                // #example: Equals operation
                final String criteria = Item.dataString(rnd.nextInt(NUMBER_OF_ITEMS));
                final List<Item> result = container.query(new Predicate<Item>() {
                    @Override
                    public boolean match(Item o) {
                        return o.getIndexedString().equals(criteria);
                    }
                });
                // #end example
                System.out.println("Number of result items " + result.size());
            }
        });
    }
    @Test
    public void notEquals() {
        StopWatch.time(new NoArgAction() {
            @Override
            public void invoke() {
                // #example: Not equals operation
                final String criteria = Item.dataString(rnd.nextInt(NUMBER_OF_ITEMS));
                final List<Item> result = container.query(new Predicate<Item>() {
                    @Override
                    public boolean match(Item o) {
                        return !o.getIndexedString().equals(criteria);
                    }
                });
                // #end example
                System.out.println("Number of result items " + result.size());
            }
        });
    }
    @Test
    public void equalsAcrossIndexedFields() {
        StopWatch.time(new NoArgAction() {
            @Override
            public void invoke() {
                // #example: Navigate across object references
                final String criteria = Item.dataString(rnd.nextInt(NUMBER_OF_ITEMS));
                final List<ItemHolder> result = container.query(new Predicate<ItemHolder>() {
                    @Override
                    public boolean match(ItemHolder o) {
                        return o.getIndexedReference().getIndexedString().equals(criteria);
                    }
                });
                // #end example
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
                final Item item = loadItemFromDatabase();

                final List<ItemHolder> result = container.query(new Predicate<ItemHolder>() {
                    @Override
                    public boolean match(ItemHolder o) {
                        return o.getIndexedReference()==item;
                    }
                });
                // #end example
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
                final List<Item> result = container.query(new Predicate<Item>() {
                    @Override
                    public boolean match(Item o) {
                        return o.getIndexNumber()>criteria;
                    }
                });
                // #end example

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
                final List<Item> result = container.query(new Predicate<Item>() {
                    @Override
                    public boolean match(Item o) {
                        return o.getIndexNumber()>biggerThanThis && o.getIndexNumber() <smallerThanThis;
                    }
                });
                // #end example

                System.out.println("Number of result items " + result.size());
            }
        });
    }
    @Test
    public void date() {
        StopWatch.time(new NoArgAction() {
            @Override
            public void invoke() {
                final Date date = new DateTime(rnd.nextInt(NUMBER_OF_ITEMS)).toDate();
                // #example: Search for a date
                final List<Item> result = container.query(new Predicate<Item>() {
                    @Override
                    public boolean match(Item o) {
                        return o.getIndexDate().equals(date);
                    }
                });
                // #end example
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

    private void storeTestData(ObjectContainer container) {
        for (int i = 0; i < NUMBER_OF_ITEMS; i++) {
            final Item item = new Item(i);
            container.store(GenericItemHolder.create(item));
        }
    }
}
