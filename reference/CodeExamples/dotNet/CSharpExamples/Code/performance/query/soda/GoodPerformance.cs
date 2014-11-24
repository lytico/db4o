using System;
using System.IO;
using Db4objects.Db4o;
using NUnit.Framework;

namespace Db4oDoc.Code.Performance.Query.Soda
{
    [TestFixture]
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
                        var query = container.Query();
                        query.Constrain(typeof (Item));
                        query.Descend("indexedString")
                            .Constrain(criteria);
                        // #end example

                        var result = query.Execute();
                        Console.WriteLine("Number of result items {0}", result.Count);
                    });
        }

        [Test]
        public void NotEquals()
        {
            StopWatchUtil.Time(
                () =>
                    {
                        var criteria = Item.DataString(rnd.Next(NumberOfItems));

                        // #example: Not equals on indexed field
                        var query = container.Query();
                        query.Constrain(typeof (Item));
                        query.Descend("indexedString")
                            .Constrain(criteria).Not();
                        // #end example

                        var result = query.Execute();
                        Console.WriteLine("Number of result items {0}", result.Count);
                    }
                );
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
                        var query = container.Query();
                        query.Constrain(typeof (ItemHolder));
                        query.Descend("indexedReference").Descend("indexedString")
                            .Constrain(criteria);
                        // #end example

                        var result = query.Execute();
                        Console.WriteLine("Number of result items {0}", result.Count);
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

                        var query = container.Query();
                        query.Constrain(typeof (ItemHolder));
                        query.Descend("indexedReference")
                            .Constrain(item);
                        // #end example

                        var result = query.Execute();
                        Console.WriteLine("Number of result items {0}", result.Count);
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

                        var query = container.Query();
                        query.Constrain(typeof(GenericHolder<Item>));
                        query.Descend("indexedReference")
                            .Constrain(item);

                        var result = query.Execute();
                        Console.WriteLine("Number of result items {0}", result.Count);
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
                        var query = container.Query();
                        query.Constrain(typeof (Item));
                        query.Descend("indexNumber")
                            .Constrain(criteria).Greater();
                        // #end example

                        var result = query.Execute();
                        Console.WriteLine("Number of result items {0}", result.Count);
                    }
                );
        }

        [Test]
        public void NotBiggerThan()
        {
            StopWatchUtil.Time(
                () =>
                    {
                        int criteria = rnd.Next(NumberOfItems);

                        var query = container.Query();
                        query.Constrain(typeof (Item));
                        query.Descend("indexNumber")
                            .Constrain(criteria).Not().Greater();

                        var result = query.Execute();
                        Console.WriteLine("Number of result items {0}", result.Count);
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
                        var query = container.Query();
                        query.Constrain(typeof (Item));
                        query.Descend("indexNumber")
                            .Constrain(biggerThanThis).Greater().And(
                                query.Descend("indexNumber").Constrain(smallerThanThis).Smaller());
                        // #end example

                        var result = query.Execute();
                        Console.WriteLine("Number of result items {0}", result.Count);
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
                        var query = container.Query();
                        query.Constrain(typeof (Item));
                        query.Descend("indexDate")
                            .Constrain(date);
                        // #end example

                        var result = query.Execute();
                        Console.WriteLine("Number of result items {0}", result.Count);
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
                        var query = container.Query();
                        query.Constrain(typeof (Item));
                        query.Descend("indexDate")
                            .Constrain(date).Greater();
                        // #end example

                        var result = query.Execute();
                        Console.WriteLine("Number of result items {0}", result.Count);
                    }
                );
        }


        private void StoreTestData(IObjectContainer container)
        {
            for (int i = 0; i < NumberOfItems; i++)
            {
                var item = new Item(i);
                container.Store(ItemHolder.Create(item));
                container.Store(GenericHolder.Create(item));
            }
        }


        private Item LoadItemFromDatabase()
        {
            var criteria = Item.DataString(rnd.Next(NumberOfItems));
            var itemQuery = container.Query();
            itemQuery.Constrain(typeof (Item));
            itemQuery.Descend("indexedString")
                .Constrain(criteria);
            return (Item) itemQuery.Execute()[0];
        }
    }
}