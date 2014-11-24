using System;
using System.Collections.Generic;
using Db4objects.Db4o;
using Db4objects.Db4o.Events;
using Db4objects.Db4o.IO;
using NUnit.Framework;

namespace Db4oDoc.Code.Strategies.Paging
{
    public class TestPagingUtility
    {
        [Test]
        public void LimitsEntries()
        {
            var input = new List<int> {1, 2, 3, 4, 5};
            var result = PagingUtility.Paging(input, 2);
            AssertContains(result, 1, 2);
        }

        [Test]
        public void PageTo()
        {
            var input = new List<int> {1, 2, 3, 4, 5};
            var result = PagingUtility.Paging(input, 2, 2);
            AssertContains(result, 3, 4);
        }

        [Test]
        public void ToLargeLimitResultInSizeOfList()
        {
            var input = new List<int> {1};
            var result = PagingUtility.Paging(input, 2);
            AssertContains(result, 1);
        }


        [Test]
        [ExpectedException(typeof (ArgumentException))]
        public void StartOutOfRange()
        {
            var input = new List<int> {1};
            PagingUtility.Paging(input, 2, 1);
        }


        [Test]
        public void StartAtTheEndReturnsEmpty()
        {
            var input = new List<int> {1, 2, 3};
            var result = PagingUtility.Paging(input, 3, 10);
            Assert.AreEqual(0, result.Count);
        }

        [Test]
        public void DoesOnlyActivateNeededItems()
        {
            MemoryStorage memoryFileSystem = new MemoryStorage();
            StoreItems(memoryFileSystem);
            IObjectContainer container = NewDB(memoryFileSystem);
            var counter = ActivationCounter(container);

            // #example: Use the paging utility
            IList<StoredItems> queryResult = container.Query<StoredItems>();
            IList<StoredItems> pagedResult = PagingUtility.Paging(queryResult, 2, 2);
            // #end example
            foreach (StoredItems storedItems in pagedResult)
            {
            }
            Assert.AreEqual(2, counter.Value);
        }

        private static MutableInteger ActivationCounter(IObjectContainer container)
        {
            var activationCounter = new MutableInteger();
            EventRegistryFactory.ForObjectContainer(container)
                .Activating += (sender, args) => activationCounter.Increment();
            return activationCounter;
        }

        private static void StoreItems(MemoryStorage memoryFileSystem)
        {
            using (IObjectContainer container = NewDB(memoryFileSystem))
            {
                for (int i = 0; i < 10; i++)
                {
                    container.Store(new StoredItems());
                }
            }
        }

        private static IObjectContainer NewDB(MemoryStorage memoryFileSystem)
        {
            var configuration = Db4oEmbedded.NewConfiguration();
            configuration.File.Storage = memoryFileSystem;
            return Db4oEmbedded.OpenFile(configuration, "MemoryDB:");
        }


        private static void AssertContains<T>(IList<T> result, params T[] entries)
        {
            foreach (T entry in entries)
            {
                Assert.IsTrue(result.Contains(entry));
            }
        }

        private class MutableInteger
        {
            private int value;

            public int Increment()
            {
                value++;
                return value;
            }

            public int Value
            {
                get { return value; }
            }
        }

        private class StoredItems
        {
        }
    }
}