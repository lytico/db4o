using System;
using System.Collections.Generic;
using System.Linq;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Linq;
using Db4objects.Db4o.Monitoring;

namespace Db4oDoc.Code.Tuning.Monitoring
{
    public class QueryMonitoring
    {
        public static void Main(string[] args)
        {
            IEmbeddedConfiguration configuration = Db4oEmbedded.NewConfiguration();
            // #example: Add query monitoring
            configuration.Common.Add(new QueryMonitoringSupport());
            configuration.Common.Add(new NativeQueryMonitoringSupport());
            // #end example
            configuration.Common.ObjectClass(typeof (DataObject)).ObjectField("indexedNumber").Indexed(true);
            using (IObjectContainer container = Db4oEmbedded.OpenFile(configuration, "database.db4o"))
            {
                StoreALotOfObjects(container);
                Console.WriteLine("Press any key to end application...");
                QueryLoop(container);
                Console.WriteLine("done.");
            }
        }


        private static void QueryLoop(IObjectContainer container)
        {
            while (!Console.KeyAvailable)
            {
                RunLINQOptimizedQuery(container);
                RunLINQNotOptimizedQuery(container);
                RunUnoptimizedQuery(container);
                RunQueryOnNotIndexedField(container);
                RunQueryOnIndexedField(container);
            }
        }

        private static void RunLINQOptimizedQuery(IObjectContainer container)
        {
            var result = (from DataObject o in container
                          where o.IndexedNumber == 42
                          select o).ToArray();
        }

        private static void RunLINQNotOptimizedQuery(IObjectContainer container)
        {
            var result = (from DataObject o in container
                          where o.IndexedNumber == new Random().Next()
                          select o).ToArray();
        }

        private static void RunQueryOnIndexedField(IObjectContainer container)
        {
            IList<DataObject> result = container.Query(delegate(DataObject o) { return o.IndexedNumber == 42; });
        }

        private static void RunQueryOnNotIndexedField(IObjectContainer container)
        {
            IList<DataObject> result = container.Query(delegate(DataObject o) { return o.Number == 42; });
        }

        private static void RunUnoptimizedQuery(IObjectContainer container)
        {
            IList<DataObject> result =
                container.Query(delegate(DataObject o) { return o.Number == new Random().Next(); });
        }

        private static void StoreALotOfObjects(IObjectContainer container)
        {
            Random rnd = new Random();
            for (int i = 0; i < 10000; i++)
            {
                container.Store(new DataObject(rnd.Next()));
            }
        }

        private class DataObject
        {
            private int number;
            private int indexedNumber;

            public DataObject(int number)
            {
                this.number = number;
                indexedNumber = number;
            }

            public int Number
            {
                get { return number; }
                set { number = value; }
            }

            public int IndexedNumber
            {
                get { return indexedNumber; }
                set { indexedNumber = value; }
            }
        }
    }
}