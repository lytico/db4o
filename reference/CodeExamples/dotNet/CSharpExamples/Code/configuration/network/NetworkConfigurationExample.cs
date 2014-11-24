using Db4objects.Db4o;
using Db4objects.Db4o.CS;
using Db4objects.Db4o.CS.Config;
using Db4objects.Db4o.CS.Foundation;
using Db4objects.Db4o.CS.Internal.Config;

namespace Db4oDoc.Code.Configuration.Network
{
    public class NetworkConfigurationExample
    {

        private static void EnableBatchMode()
        {
            // #example: enable or disable batch mode
            IClientConfiguration configuration = Db4oClientServer.NewClientConfiguration();
            configuration.Networking.BatchMessages = true;
            // #end example
            IObjectContainer container = Db4oClientServer.OpenClient(configuration, "localhost", 1337, "sa", "sa");
        }

        private static void ReplaceClientServerFactory()
        {
            // #example: exchange the way a client or server is created
            IClientConfiguration configuration = Db4oClientServer.NewClientConfiguration();
            configuration.Networking.ClientServerFactory = new StandardClientServerFactory();
            // #end example
            IObjectContainer container = Db4oClientServer.OpenClient(configuration, "localhost", 1337, "sa", "sa");
        }

        private static void MaxBatchQueueSize()
        {
            // #example: change the maximum batch queue size
            IClientConfiguration configuration = Db4oClientServer.NewClientConfiguration();
            configuration.Networking.MaxBatchQueueSize = 1024;
            // #end example
            IObjectContainer container = Db4oClientServer.OpenClient(configuration, "localhost", 1337, "sa", "sa");
        }

        private static void SingleThreadedClient()
        {
            // #example: single threaded client
            IClientConfiguration configuration = Db4oClientServer.NewClientConfiguration();
            configuration.Networking.SingleThreadedClient = true;
            // #end example
            IObjectContainer container = Db4oClientServer.OpenClient(configuration, "localhost", 1337, "sa", "sa");
        }

        private static void PluggableSocket()
        {
            // #example: Exchange the socket-factory
            IClientConfiguration configuration = Db4oClientServer.NewClientConfiguration();
            configuration.Networking.SocketFactory = new StandardSocket4Factory();
            // #end example
            IObjectContainer container = Db4oClientServer.OpenClient(configuration, "localhost", 1337, "sa", "sa");
        }


    }
}