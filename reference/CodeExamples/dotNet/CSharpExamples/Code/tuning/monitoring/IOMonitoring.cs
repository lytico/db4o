using System;
using System.Collections.Generic;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Monitoring;

namespace Db4oDoc.Code.Tuning.Monitoring
{
    public class IOMonitoring
    {
        public static void Main(string[] args)
        {
            IEmbeddedConfiguration configuration = Db4oEmbedded.NewConfiguration();
            // #example: Add IO-Monitoring
            configuration.Common.Add(new IOMonitoringSupport());
            // #end example
            using (IObjectContainer container = Db4oEmbedded.OpenFile(configuration, "database.db4o"))
            {
                Console.WriteLine("Press any key to end application...");
                DoIoOperations(container);
                Console.WriteLine("done.");
            }
        }

        private static void DoIoOperations(IObjectContainer container)
        {
            while (!Console.KeyAvailable)
            {
                StoreALot(container);
                ReadALot(container);
            }
        }

        private static void ReadALot(IObjectContainer container)
        {
            IList<DataObject> allObjects = container.Query<DataObject>();
            foreach (DataObject obj in allObjects)
            {
                obj.ToString();
            }
        }

        private static void StoreALot(IObjectContainer container)
        {
            Random rnd = new Random();
            for (int i = 0; i < 1024; i++)
            {
                container.Store(new DataObject(rnd));
            }
            container.Commit();
        }
    }
}