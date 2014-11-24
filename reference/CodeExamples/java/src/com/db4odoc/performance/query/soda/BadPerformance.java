package com.db4odoc.performance.query.soda;


import com.db4o.Db4oEmbedded;
import com.db4o.ObjectContainer;
import com.db4o.ObjectSet;
import com.db4o.config.EmbeddedConfiguration;
import com.db4o.diagnostic.DiagnosticToConsole;
import com.db4o.query.Candidate;
import com.db4o.query.Evaluation;
import com.db4o.query.Query;
import com.db4odoc.javalang.NoArgAction;
import com.db4odoc.performance.*;
import org.junit.After;
import org.junit.Before;
import org.junit.BeforeClass;
import org.junit.Test;

import java.io.File;
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
        container = Db4oEmbedded.openFile(newCfg(), DATABASE_FILE);
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

                // #example: Navigation across non concrete typed fields
                // The type of the 'indexedReference' is the generic parameter 'T'.
                // Due to type type erasure that type is unknown to db4o
                final Query query = container.query();
                query.constrain(GenericItemHolder.class);
                query.descend("indexedReference").descend("indexedString")
                        .constrain(criteria);
                // #end example

                final ObjectSet<GenericItemHolder<Item>> result = query.execute();
                System.out.println("Number of result items " + result.size());
            }
        });
    }


    @Test
    public void startsWith() {
        StopWatch.time(new NoArgAction() {
            @Override
            public void invoke() {
                final String criteria = Item.dataString(rnd.nextInt(NUMBER_OF_ITEMS));

                final Query query = container.query();
                query.constrain(Item.class);
                query.descend("indexedString")
                        .constrain(criteria).startsWith(true);

                final ObjectSet<GenericItemHolder<Item>> result = query.execute();
                System.out.println("Number of result items " + result.size());
            }
        });
    }

    @Test
    public void endsWith() {
        StopWatch.time(new NoArgAction() {
            @Override
            public void invoke() {
                final String criteria = Item.dataString(rnd.nextInt(NUMBER_OF_ITEMS));

                final Query query = container.query();
                query.constrain(Item.class);
                query.descend("indexedString")
                        .constrain(criteria).endsWith(true);

                final ObjectSet<GenericItemHolder<Item>> result = query.execute();
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

                // #example: Contains is slow
                final Query query = container.query();
                query.constrain(Item.class);
                query.descend("indexedString")
                        .constrain(criteria).contains();
                // #end example

                final ObjectSet<GenericItemHolder<Item>> result = query.execute();
                System.out.println("Number of result items " + result.size());
            }
        });
    }

    @Test
    public void like() {
        StopWatch.time(new NoArgAction() {
            @Override
            public void invoke() {
                final String criteria = Item.dataString(rnd.nextInt(NUMBER_OF_ITEMS));

                // #example: Like is slow
                final Query query = container.query();
                query.constrain(Item.class);
                query.descend("indexedString")
                        .constrain(criteria).like();
                // #end example

                final ObjectSet<GenericItemHolder<Item>> result = query.execute();
                System.out.println("Number of result items " + result.size());
            }
        });
    }

    @Test
    public void queryForItemInCollection() {
        StopWatch.time(new NoArgAction() {
            @Override
            public void invoke() {
                Item itemToQueryFor = itemToQueryFor();

                // #example: Contains on collection
                final Query query = container.query();
                query.constrain(CollectionHolder.class);
                query.descend("items")
                        .constrain(itemToQueryFor);
                // #end example

                final ObjectSet<GenericItemHolder<Item>> result = query.execute();
                System.out.println("Number of result items " + result.size());
            }
        });
    }

    @Test
    public void descentIntoCollection() {
        StopWatch.time(new NoArgAction() {
            @Override
            public void invoke() {
                final String criteria = Item.dataString(rnd.nextInt(NUMBER_OF_ITEMS));

                // #example: Navigate into collection
                final Query query = container.query();
                query.constrain(CollectionHolder.class);
                query.descend("items")
                        .descend("indexedString").constrain(criteria);
                // #end example

                final ObjectSet<GenericItemHolder<Item>> result = query.execute();
                System.out.println("Number of result items " + result.size());
            }
        });
    }

    @Test
    public void sortingByIndexedField() {
        StopWatch.time(new NoArgAction() {
            @Override
            public void invoke() {

                // #example: Sorting a huge result set
                final Query query = container.query();
                query.constrain(Item.class);
                query.descend("indexedString").orderAscending();
                // #end example

                final ObjectSet<GenericItemHolder<Item>> result = query.execute();
                System.out.println("Number of result items " + result.size());
            }
        });
    }


    @Test
    public void evalutions() {
        StopWatch.time(new NoArgAction() {
            @Override
            public void invoke() {

                // #example: Evaluations
                final Query query = container.query();
                query.constrain(Item.class);
                query.descend("indexedString").constrain(new Evaluation() {
                    @Override
                    public void evaluate(Candidate candidate) {
                        if (candidate.getObject() instanceof String) {
                            String value = (String) candidate.getObject();
                            if (value.matches("abc")) {
                                candidate.include(true);
                            }
                        }
                    }
                });
                // #end example

                final ObjectSet<GenericItemHolder<Item>> result = query.execute();
                System.out.println("Number of result items " + result.size());
            }
        });
    }

    private EmbeddedConfiguration newCfg() {
        EmbeddedConfiguration cfg = Db4oEmbedded.newConfiguration();
        cfg.common().diagnostic().addListener(new DiagnosticToConsole());
        return cfg;
    }

    private Item itemToQueryFor() {
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
            container.store(
                    CollectionHolder.create(
                            item,
                            new Item(NUMBER_OF_ITEMS + i),
                            new Item(2 * NUMBER_OF_ITEMS + i)));
        }
    }
}
