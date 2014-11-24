using System;
using System.Collections.Generic;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Monitoring;

namespace Db4oDoc.Code.Tuning.Monitoring
{
    public class ReferenceSystemMonitoring
    {
        public static void Main(string[] args)
        {
            IEmbeddedConfiguration configuration = Db4oEmbedded.NewConfiguration();
            // #example: Add reference system monitoring
            configuration.Common.Add(new ReferenceSystemMonitoringSupport());
            // #end example
            using (IObjectContainer container = Db4oEmbedded.OpenFile(configuration, "database.db4o"))
            {
                StoreObjects(container);
                Console.WriteLine("Press any key to end application...");
                BlowReferenceSystem(container);
                Console.WriteLine("done.");
            }
        }


        private static void StoreObjects(IObjectContainer container)
        {
            Random rnd = new Random();
            for (int i = 0; i < 500000; i++)
            {
                container.Store(new DataObject(rnd));
            }
            container.Commit();
        }

        private static void BlowReferenceSystem(IObjectContainer container)
        {
            IList<DataObject> dataObjects = container.Query<DataObject>();
            List<DataObject> hardReferences = new List<DataObject>();
            while (!Console.KeyAvailable)
            {
                foreach (DataObject reference in dataObjects)
                {
                    hardReferences.Add(reference);
                }
                hardReferences.Clear();
            }
        }
    }
}