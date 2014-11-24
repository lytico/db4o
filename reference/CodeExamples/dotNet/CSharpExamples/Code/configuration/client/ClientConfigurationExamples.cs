using Db4objects.Db4o;
using Db4objects.Db4o.CS;
using Db4objects.Db4o.CS.Config;

namespace Db4oDoc.Code.Configuration.Client
{

    public class ClientConfigurationExamples
    {

        public static void PrefetchDepth()
        {
            // #example: Configure the prefetch depth
            IClientConfiguration configuration = Db4oClientServer.NewClientConfiguration();
            configuration.PrefetchDepth = 5;
            // #end example
            IObjectContainer container = Db4oClientServer.OpenClient(configuration, "localhost", 1337, "user", "password");
            container.Close();

        }
        public static void PrefetchObjectCount()
        {
            // #example: Configure the prefetch object count
            IClientConfiguration configuration = Db4oClientServer.NewClientConfiguration();
            configuration.PrefetchObjectCount = 500;
            // #end example
            IObjectContainer container = Db4oClientServer.OpenClient(configuration, "localhost", 1337, "user", "password");
            container.Close();

        }
        public static void PrefetchSlotCacheSize()
        {
            // #example: Configure the slot cache
            IClientConfiguration configuration = Db4oClientServer.NewClientConfiguration();
            configuration.PrefetchSlotCacheSize = 1024;
            // #end example
            IObjectContainer container = Db4oClientServer.OpenClient(configuration, "localhost", 1337, "user", "password");
            container.Close();

        }
        public static void PrefetchIDCount()
        {
            // #example: Configure the prefetch id count
            IClientConfiguration configuration = Db4oClientServer.NewClientConfiguration();
            configuration.PrefetchSlotCacheSize = 128;
            // #end example
            IObjectContainer container = Db4oClientServer.OpenClient(configuration, "localhost", 1337, "user", "password");
            container.Close();

        }
        public static void ConnectionTimeOut()
        {
            // #example: Configure the timeout
            IClientConfiguration configuration = Db4oClientServer.NewClientConfiguration();
            configuration.TimeoutClientSocket = (1 * 60 * 1000);
            // #end example
            IObjectContainer container = Db4oClientServer.OpenClient(configuration, "localhost", 1337, "user", "password");
            container.Close();

        }
    }
}