using System;
using System.IO;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Diagnostic;
using Db4objects.Db4o.Query;
using NUnit.Framework;

namespace Db4oDoc.Code.Performance.Query.Soda
{
    public class BadPerformance
    {
        private const string DatabaseFile = "good-performance.db4o";
        private const int NumberOfItems = 50000;
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
            container = Db4oEmbedded.OpenFile(NewCfg(), DatabaseFile);
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
        public void EqualsAcrossObjectFields()
        {
            StopWatchUtil.Time(
                () =>
                    {
                        var criteria = Item.DataString(rnd.Next(NumberOfItems));

                        // #example: Navigation across non concrete typed fields
                        // The type of the 'indexedReference' is an object
                        // Therefore the query engine cannot know the type and use that index
                        var query = container.Query();
                        query.Constrain(typeof (ObjectHolder));
                        query.Descend("indexedReference").Descend("indexedString")
                            .Constrain(criteria);
                        // #end example


                        var result = query.Execute();
                        Console.WriteLine("Number of result items {0}", result.Count);
                    }
                );
        }


        [Test]
        public void StartsWith()
        {
            StopWatchUtil.Time(
                () =>
                    {
                        var criteria = Item.DataString(rnd.Next(NumberOfItems));
                        var query = container.Query();
                        query.Constrain(typeof (Item));
                        query.Descend("indexedString")
                            .Constrain(criteria).StartsWith(true);


                        var result = query.Execute();
                        Console.WriteLine("Number of result items {0}", result.Count);
                    }
                );
        }


        [Test]
        public void EndsWith()
        {
            StopWatchUtil.Time(
                () =>
                    {
                        var criteria = Item.DataString(rnd.Next(NumberOfItems));
                        var query = container.Query();
                        query.Constrain(typeof (Item));
                        query.Descend("indexedString")
                            .Constrain(criteria).EndsWith(true);

                        var result = query.Execute();
                        Console.WriteLine("Number of result items {0}", result.Count);
                    }
                );
        }

        [Test]
        public void Contains()
        {
            StopWatchUtil.Time(
                () =>
                    {
                        var criteria = Item.DataString(rnd.Next(NumberOfItems));

                        // #example: Contains is slow
                        var query = container.Query();
                        query.Constrain(typeof (Item));
                        query.Descend("indexedString")
                            .Constrain(criteria).Contains();
                        // #end example

                        var result = query.Execute();
                        Console.WriteLine("Number of result items {0}", result.Count);
                    }
                );
        }

        [Test]
        public void Like()
        {
            StopWatchUtil.Time(
                () =>
                    {
                        var criteria = Item.DataString(rnd.Next(NumberOfItems));
                        // #example: Like is slow
                        var query = container.Query();
                        query.Constrain(typeof (Item));
                        query.Descend("indexedString")
                            .Constrain(criteria).Like();
                        // #end example

                        var result = query.Execute();
                        Console.WriteLine("Number of result items {0}", result.Count);
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
                        var query = container.Query();
                        query.Constrain(typeof (CollectionHolder));
                        query.Descend("items")
                            .Constrain(itemToQueryFor);
                        // #end example

                        var result = query.Execute();
                        Console.WriteLine("Number of result items {0}", result.Count);
                    }
                );
        }

        [Test]
        public void DescentIntoCollection()
        {
            StopWatchUtil.Time(
                () =>
                    {
                        var criteria = Item.DataString(rnd.Next(NumberOfItems));

                        // #example: Navigate into collection
                        var query = container.Query();
                        query.Constrain(typeof (CollectionHolder));
                        query.Descend("items")
                            .Descend("indexedString").Constrain(criteria);
                        // #end example


                        var result = query.Execute();
                        Console.WriteLine("Number of result items {0}", result.Count);
                    }
                );
        }

        [Test]
        public void SortingByIndexedField()
        {
            StopWatchUtil.Time(
                () =>
                    {
                        // #example: Sorting a huge result set
                        var query = container.Query();
                        query.Constrain(typeof (Item));
                        query.Descend("indexedString").OrderAscending();
                        // #end example

                        var result = query.Execute();
                        Console.WriteLine("Number of result items {0}", result.Count);
                    }
                );
        }


        [Test]
        public void Evalutions()
        {
            StopWatchUtil.Time(
                () =>
                    {
                        // #example: Evaluations
                        var query = container.Query();
                        query.Constrain(typeof (Item));
                        query.Descend("indexedString").Constrain(new OnlyAbcItemsEvaluation());
                        // #end example

                        var result = query.Execute();
                        Console.WriteLine("Number of result items {0}", result.Count);
                    }
                );
        }

        // #example: Evaluation class
        internal class OnlyAbcItemsEvaluation : IEvaluation
        {
            public void Evaluate(ICandidate candidate)
            {
                if (candidate.GetObject() is string)
                {
                    var value = (string) candidate.GetObject();
                    if (value.Equals("abc"))
                    {
                        candidate.Include(true);
                    }
                }
            }
        }

        // #end example

        private Item LoadItemFromDatabase()
        {
            var criteria = Item.DataString(rnd.Next(NumberOfItems));
            var itemQuery = container.Query();
            itemQuery.Constrain(typeof (Item));
            itemQuery.Descend("indexedString")
                .Constrain(criteria);
            return (Item) itemQuery.Execute()[0];
        }

        private static IEmbeddedConfiguration NewCfg()
        {
            var cfg = Db4oEmbedded.NewConfiguration();
            cfg.Common.Diagnostic.AddListener(new DiagnosticToConsole());
            return cfg;
        }

        private static void StoreTestData(IObjectContainer container)
        {
            for (int i = 0; i < NumberOfItems; i++)
            {
                Item item = new Item(i);
                container.Store(ObjectHolder.Create(item));
                container.Store(
                    CollectionHolder.Create(
                        item,
                        new Item(NumberOfItems + i),
                        new Item(2*NumberOfItems + i)));
            }
        }
    }
}