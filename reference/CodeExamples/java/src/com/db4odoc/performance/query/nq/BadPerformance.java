package com.db4odoc.performance.query.nq;


import com.db4o.Db4oEmbedded;
import com.db4o.ObjectContainer;
import com.db4o.query.Predicate;
import com.db4odoc.javalang.NoArgAction;
import com.db4odoc.performance.CollectionHolder;
import com.db4odoc.performance.GenericItemHolder;
import com.db4odoc.performance.Item;
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

public class BadPerformance {
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

    @Test
    public void equalsAcrossGenericFields() {
        StopWatch.time(new NoArgAction() {
            @Override
            public void invoke() {
                final String criteria = Item.dataString(rnd.nextInt(NUMBER_OF_ITEMS));

                // #example: Navigation non concrete types
                // The type of the 'indexedReference' is the generic parameter 'T'.
                // Due to type type erasure that type is unknown to db4o
                final List<GenericItemHolder<Item>> result = container.query(new Predicate<GenericItemHolder<Item>>() {
                    @Override
                    public boolean match(GenericItemHolder<Item> o) {
                        return o.getIndexedReference().getIndexedString().equals(criteria);
                    }
                });
                // #end example

                System.out.println("Number of result items " + result.size());
            }
        });
    }

    @Test
    public void contains() {
        StopWatch.time(new NoArgAction() {
            @Override
            public void invoke() {
                final String criteria = Item.dataString(rnd.nextInt(NUMBER_OF_ITEMS));

                // #example: Contains and other string operations are slow
                final List<Item> result = container.query(new Predicate<Item>() {
                    @Override
                    public boolean match(Item o) {
                        return o.getIndexedString().contains(criteria);
                    }
                });
                // #end example

                System.out.println("Number of result items " + result.size());
            }
        });
    }

    @Test
    public void dateComparison() {
        StopWatch.time(new NoArgAction() {
            @Override
            public void invoke() {
                final Date date = new DateTime(rnd.nextInt(NUMBER_OF_ITEMS)).toDate();
                // #example: Slow date query
                final List<Item> result = container.query(new Predicate<Item>() {
                    @Override
                    public boolean match(Item o) {
                        return o.getIndexDate().after(date);
                    }
                });
                // #end example
                System.out.println("Number of result items " + result.size());
            }
        });
    }


    @Test
    public void queryForItemInCollection() {
        StopWatch.time(new NoArgAction() {
            @Override
            public void invoke() {
                final Item item = loadItemFromDatabase();

                // #example: Contains on collection
                final List<CollectionHolder> result = container.query(new Predicate<CollectionHolder>() {
                    @Override
                    public boolean match(CollectionHolder o) {
                        return o.getItems().contains(item);
                    }
                });
                // #end example

                System.out.println("Number of result items " + result.size());
            }
        });
    }

    @Test
    public void expressionInCode() {
        StopWatch.time(new NoArgAction() {
            @Override
            public void invoke() {
                final int number = rnd.nextInt(NUMBER_OF_ITEMS);
                // #example: Computing expression in query
                final List<Item> result = container.query(new Predicate<Item>() {
                    @Override
                    public boolean match(Item o) {
                        return o.getIndexedString().equals("data for " + number);
                    }
                });
                // #end example
                System.out.println("Number of result items " + result.size());
            }
        });
    }

    @Test
    public void fixExpressionInCode() {
        StopWatch.time(new NoArgAction() {
            @Override
            public void invoke() {
                final int number = rnd.nextInt(NUMBER_OF_ITEMS);
                // #example: Fix computing expression in query
                final String criteria = "data for " + number;
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
    public void callingMethod() {
        StopWatch.time(new NoArgAction() {
            @Override
            public void invoke() {
                // #example: Call complex method
                final List<Item> result = container.query(new Predicate<Item>() {
                    @Override
                    public boolean match(Item o) {
                        return o.complexMethod();
                    }
                });
                // #end example
                System.out.println("Number of result items " + result.size());
            }
        });
    }

    private Item loadItemFromDatabase() {
        final String criteria = Item.dataString(rnd.nextInt(NUMBER_OF_ITEMS));
        return container.query(new Predicate<Item>() {
            @Override
            public boolean match(Item o) {
                return o.getIndexedString().equals(criteria);
            }
        }).get(0);
    }

    private void storeTestData(ObjectContainer container) {
        for (int i = 0; i < NUMBER_OF_ITEMS; i++) {
            final Item item = new Item(i);
            container.store(GenericItemHolder.create(item));
            container.store(
                    CollectionHolder.create(
                            item,
                            new Item(NUMBER_OF_ITEMS + i),
                            new Item(2 * NUMBER_OF_ITEMS + i)));
        }
    }
}
