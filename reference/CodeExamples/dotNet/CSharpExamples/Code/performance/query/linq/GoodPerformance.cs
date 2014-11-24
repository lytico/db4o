using System;
using System.IO;
using System.Linq;
using Db4objects.Db4o;
using Db4objects.Db4o.Linq;
using NUnit.Framework;

namespace Db4oDoc.Code.Performance.Query.Linq
{
    public class GoodPerformance
    {
        private const string DatabaseFile = "good-performance.db4o";
        private const int NumberOfItems = 200000;
        private IObjectContainer container;
        private readonly Random rnd = new Random();

        [TestFixtureSetUp]
        public static void RemoveDatabase()
        {
            File.Delete(DatabaseFile);
        }

        [SetUp]
        public void PrepareData()
        {
            container = Db4oEmbedded.OpenFile(DatabaseFile);
            if (container.Query().Execute().Count == 0)
            {
                StoreTestData(container);
            }
            WarmUpLinq(container);
        }

        [TearDown]
        public void CleanUp()
        {
            container.Dispose();
        }

        [Test]
        public void EqualsOnIndexedField()
        {
            StopWatchUtil.Time(
                () =>
                    {
                        var criteria = Item.DataString(rnd.Next(NumberOfItems));

                        // #example: Equals on indexed field
                        var result = from Item i in container
                                     where i.IndexedString == criteria
                                     select i;
                        // #end example

                        Console.WriteLine("Number of result items {0}", result.Count());
                    });
        }

        [Test]
        public void NotEquals()
        {
            StopWatchUtil.Time(
                () =>
                    {
                        var criteria = Item.DataString(rnd.Next(NumberOfItems));

                        // #example: Not equals
                        var result = from Item i in container
                                     where i.IndexedString != criteria
                                     select i;
                        // #end example

                        Console.WriteLine("Number of result items {0}", result.Count());
                    });
        }

        [Test]
        public void EqualsAcrossIndexedFields()
        {
            StopWatchUtil.Time(
                () =>
                    {
                        var criteria = Item.DataString(rnd.Next(NumberOfItems));

                        // #example: Equals across indexed fields
                        // Note that the type of the 'indexedReference' has to the specific type
                        // which holds the 'indexedString'
                        var result = from ItemHolder h in container
                                     where h.IndexedReference.IndexedString == criteria
                                     select h;
                        // #end example

                        Console.WriteLine("Number of result items {0}", result.Count());
                    }
                );
        }

        [Test]
        public void SearchByReference()
        {
            StopWatchUtil.Time(
                () =>
                    {
                        // #example: Query by reference
                        Item item = LoadItemFromDatabase();

                        // #example: Query by reference
                        // Note that the type of the 'indexedReference' has to the specific type
                        // which holds the 'indexedString'
                        var result = from ItemHolder h in container
                                     where h.IndexedReference == item
                                     select h;
                        // #end example

                        Console.WriteLine("Number of result items {0}", result.Count());
                    }
                );
        }

        [Test]
        public void SearchByGenericField()
        {
            StopWatchUtil.Time(
                () =>
                    {
                        Item item = LoadItemFromDatabase();

                        var result = from GenericHolder<Item> h in container
                                     where h.IndexedReference == item
                                     select h;

                        Console.WriteLine("Number of result items {0}", result.Count());
                    }
                );
        }


        [Test]
        public void BiggerThan()
        {
            StopWatchUtil.Time(
                () =>
                    {
                        int criteria = rnd.Next(NumberOfItems);

                        // #example: Bigger than
                        var result = from Item i in container
                                     where i.IndexNumber > criteria
                                     select i;
                        // #end example

                        Console.WriteLine("Number of result items {0}", result.Count());
                    }
                );
        }

        [Test]
        public void InBetween()
        {
            StopWatchUtil.Time(
                () =>
                    {
                        int criteria = rnd.Next(NumberOfItems);
                        int biggerThanThis = criteria - 10;
                        int smallerThanThis = criteria + 10;

                        // #example: In between
                        var result = from Item i in container
                                     where i.IndexNumber > biggerThanThis
                                           && i.IndexNumber < smallerThanThis
                                     select i;
                        // #end example

                        Console.WriteLine("Number of result items {0}", result.Count());
                    });
        }

        [Test]
        public void FindADate()
        {
            StopWatchUtil.Time(
                () =>
                    {
                        var date = new DateTime(rnd.Next(NumberOfItems));

                        // #example: Date comparisons are also fast
                        var result = from Item i in container
                                     where i.IndexDate == date
                                     select i;
                        // #end example

                        Console.WriteLine("Number of result items {0}", result.Count());
                    }
                );
        }

        [Test]
        public void NewerData()
        {
            StopWatchUtil.Time(
                () =>
                    {
                        var date = new DateTime(rnd.Next(NumberOfItems));

                        // #example: Find a newer date
                        var result = from Item i in container
                                     where i.IndexDate > date
                                     select i;
                        // #end example

                        Console.WriteLine("Number of result items {0}", result.Count());
                    }
                );
        }

        private Item LoadItemFromDatabase()
        {
            var criteria = Item.DataString(rnd.Next(NumberOfItems));
            return (from Item i in container
                    where i.IndexedString == criteria
                    select i).First();
        }

        private void StoreTestData(IObjectContainer container)
        {
            for (int i = 0; i < NumberOfItems; i++)
            {
                var item = new Item(i);
                container.Store(ItemHolder.Create(item));
                container.Store(GenericHolder.Create(item));
            }
            container.Commit();
        }

        private int WarmUpLinq(IObjectContainer container)
        {
            var result = (from Item i in container
                          where i.IndexedString == "No-Match"
                          select i).ToList();
            return result.Count;
        }
    }
}