using System;
using System.Collections.Generic;
using Db4objects.Db4o;
using Db4objects.Db4o.CS;
using Db4objects.Db4o.CS.Config;
using Db4objects.Db4o.CS.Monitoring;

namespace Db4oDoc.Code.Tuning.Monitoring
{
    internal class CSMonitoring
    {
        private const int PortNumber = 1337;
        private const string User = "sa";
        private const string Password = "pwd";

        public static void Main(string[] args)
        {
            using (IObjectServer server = StartServer())
            {
                RunClient();
            }
        }

        private static void RunClient()
        {
            IClientConfiguration configuration = Db4oClientServer.NewClientConfiguration();
            configuration.Common.Add(new NetworkingMonitoringSupport());
            using (IObjectContainer client = Db4oClientServer.OpenClient(configuration,
                                                                         "localhost", PortNumber, User, Password))
            {
                DoOperationsOnClient(client);
            }
        }

        private static void DoOperationsOnClient(IObjectContainer container)
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

        private static IObjectServer StartServer()
        {
            IServerConfiguration configuration = Db4oClientServer.NewServerConfiguration();
            // #example: Add the network monitoring support
            configuration.Common.Add(new NetworkingMonitoringSupport());
            // #end example
            // #example: Add the client connections monitoring support
            configuration.AddConfigurationItem(new ClientConnectionsMonitoringSupport());
            // #end example
            IObjectServer server = Db4oClientServer.OpenServer(configuration, "database.db4o", PortNumber);
            server.GrantAccess(User, Password);
            return server;
        }
    }
}