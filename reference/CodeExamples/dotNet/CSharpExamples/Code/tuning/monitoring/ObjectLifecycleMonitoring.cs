using System;
using System.Collections.Generic;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Monitoring;

namespace Db4oDoc.Code.Tuning.Monitoring
{
    public class ObjectLifecycleMonitoring
    {
        public static void Main(string[] args)
        {
            IEmbeddedConfiguration configuration = Db4oEmbedded.NewConfiguration();
            // #example: Monitor the object lifecycle statistics
            configuration.Common.Add(new ObjectLifecycleMonitoringSupport());
            // #end example
            using (IObjectContainer container = Db4oEmbedded.OpenFile(configuration, "database.db4o"))
            {
                Console.WriteLine("Press any key to end application...");
                WorkWithObjects(container);
                Console.WriteLine("done.");
            }
        }

        private static void WorkWithObjects(IObjectContainer container)
        {
            while (!Console.KeyAvailable)
            {
                Random rnd = new Random();
                StoreData(container, rnd);
                DeleteData(container, rnd);
                container.Commit();
            }
        }

        private static void DeleteData(IObjectContainer container, Random rnd)
        {
            IList<DataObject> data = container.Query<DataObject>();
            for (int i = 0; i < rnd.Next(4096); i++)
            {
                DataObject obj = data[rnd.Next(data.Count)];
                if (null != obj)
                {
                    container.Delete(obj);
                }
            }
        }

        private static void StoreData(IObjectContainer container, Random rnd)
        {
            for (int i = 0; i < rnd.Next(4096); i++)
            {
                container.Store(new DataObject(rnd));
            }
        }
    }
}