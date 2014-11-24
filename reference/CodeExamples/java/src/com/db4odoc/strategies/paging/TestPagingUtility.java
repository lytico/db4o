package com.db4odoc.strategies.paging;

import com.db4o.Db4oEmbedded;
import com.db4o.ObjectContainer;
import com.db4o.ObjectSet;
import com.db4o.config.EmbeddedConfiguration;
import com.db4o.events.CancellableObjectEventArgs;
import com.db4o.events.Event4;
import com.db4o.events.EventListener4;
import com.db4o.events.EventRegistryFactory;
import com.db4o.io.MemoryStorage;
import org.junit.Test;

import java.util.Arrays;
import java.util.List;
import java.util.concurrent.atomic.AtomicInteger;

import static junit.framework.Assert.assertEquals;
import static org.junit.Assert.assertTrue;


public class TestPagingUtility {


    @Test
    public void limitsEntries() {
        List<Integer> input = Arrays.asList(1, 2, 3, 4, 5);
        final List<Integer> result = PagingUtility.paging(input, 2);
        assertContains(result, 1, 2);
    }

    @Test
    public void pageTo() {
        List<Integer> input = Arrays.asList(1, 2, 3, 4, 5);
        final List<Integer> result = PagingUtility.paging(input, 2, 2);
        assertContains(result, 3, 4);
    }

    @Test
    public void toLargeLimitResultInSizeOfList() {
        List<Integer> input = Arrays.asList(1);
        final List<Integer> result = PagingUtility.paging(input, 2);
        assertContains(result, 1);
    }


    @Test(expected = IllegalArgumentException.class)
    public void startOutOfRange() {
        List<Integer> input = Arrays.asList(1);
        PagingUtility.paging(input, 2, 1);
    }


    @Test()
    public void startAtTheEndReturnsEmpty() {
        List<Integer> input = Arrays.asList(1, 2, 3);
        final List<Integer> result = PagingUtility.paging(input, 3, 10);
        assertEquals(0, result.size());
    }

    @Test
    public void doesOnlyActivateNeededItems() {
        final MemoryStorage memoryFileSystem = new MemoryStorage();
        storeItems(memoryFileSystem);
        ObjectContainer container = newDB(memoryFileSystem);
        final AtomicInteger counter = activationCounter(container);
        // #example: Use the paging utility
        final ObjectSet<StoredItems> queryResult = container.query(StoredItems.class);
        List<StoredItems> pagedResult = PagingUtility.paging(queryResult, 2, 2);
        // #end example
        for (StoredItems storedItems : pagedResult) {

        }
        assertEquals(2, counter.get());

    }

    private AtomicInteger activationCounter(ObjectContainer container) {
        final AtomicInteger activationCounter = new AtomicInteger();
        EventRegistryFactory.forObjectContainer(container)
                .activating().addListener(new EventListener4<CancellableObjectEventArgs>() {
            @Override
            public void onEvent(Event4<CancellableObjectEventArgs> cancellableObjectEventArgsEvent4, CancellableObjectEventArgs cancellableObjectEventArgs) {
                activationCounter.incrementAndGet();
            }
        });
        return activationCounter;
    }

    private void storeItems(MemoryStorage memoryFileSystem) {
        ObjectContainer container = newDB(memoryFileSystem);
        try {
            for (int i = 0; i < 10; i++) {
                container.store(new StoredItems());
            }
        } finally {
            container.close();
        }
    }

    private ObjectContainer newDB(MemoryStorage memoryFileSystem) {
        EmbeddedConfiguration configuration = Db4oEmbedded.newConfiguration();
        configuration.file().storage(memoryFileSystem);
        ObjectContainer container = Db4oEmbedded.openFile(configuration, "MemoryDB:");
        return container;
    }


    private <T> void assertContains(List<T> result, T... entries) {
        for (T entry : entries) {
            assertTrue(result.contains(entry));
        }
    }

    private static class StoredItems {

    }
}
