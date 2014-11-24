using System;
using System.IO;
using System.Linq;
using Db4oDoc.Code.Performance.Query.Soda;
using Db4objects.Db4o;
using Db4objects.Db4o.Linq;
using NUnit.Framework;

namespace Db4oDoc.Code.Performance.Query.Linq
{
    public class BadPerformance
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
        public void Contains()
        {
            StopWatchUtil.Time(
                () =>
                    {
                        var criteria = Item.DataString(rnd.Next(NumberOfItems));

                        // #example: Contains is slow
                        var result = from Item i in container
                                     where i.IndexedString.Contains(criteria)
                                     select i;
                        // #end example

                        Console.WriteLine("Number of result items {0}", result.Count());
                    }
                );
        }

        [Test]
        public void QueryForItemInCollection()
        {
            StopWatchUtil.Time(
                () =>
                    {
                        Item itemToQueryFor = LoadItemFromDatabase();

                        // #example: Contains on collection
                        var result = from CollectionHolder h in container
                                     where h.Items.Contains(itemToQueryFor)
                                     select h;
                        // #end example

                        Console.WriteLine("Number of result items {0}", result.Count());
                    }
                );
        }

        [Test]
        public void PropertiesWithSideEffects()
        {
            StopWatchUtil.Time(
                () =>
                    {
                        var criteria = Item.DataString(rnd.Next(NumberOfItems));

                        // #example: Complex property
                        var result = from Item i in container
                                     where i.PropertyWhichFiresEvent == criteria
                                     select i;
                        // #end example

                        Console.WriteLine("Number of result items {0}", result.Count());
                    }
                );
        }

        [Test]
        public void CallingAMethod()
        {
            StopWatchUtil.Time(
                () =>
                    {
                        // #example: Calling a method
                        var result = from Item i in container
                                     where i.ComplexMethod()
                                     select i;
                        // #end example

                        Console.WriteLine("Number of result items {0}", result.Count());
                    }
                );
        }

        [Test]
        public void AdvancedLinqQueries()
        {
            StopWatchUtil.Time(
                () =>
                    {
                        var criteria = Item.DataString(rnd.Next(NumberOfItems));

                        // #example: Nagivating into collection
                        var result = from CollectionHolder h in container
                                     where h.Items.Any(i => i.IndexedString == criteria)
                                     select h;
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
                container.Store(
                    CollectionHolder.Create(
                        item,
                        new Item(NumberOfItems + i),
                        new Item(2*NumberOfItems + i)));
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