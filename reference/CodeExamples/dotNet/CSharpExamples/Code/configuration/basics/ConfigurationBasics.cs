using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.CS;
using Db4objects.Db4o.CS.Config;

namespace Db4oDoc.Code.Configuration.Basics
{
    public class ConfigurationBasics
    {
        public static void Main(string[] args)
        {
            EmbeddedConfiguration();
            ServerConfiguration();
            ClientConfiguration();
        }

        private static void EmbeddedConfiguration()
        {
            // #example: Configure embedded object container
            IEmbeddedConfiguration configuration = Db4oEmbedded.NewConfiguration();
            // change the configuration...
            IObjectContainer container = Db4oEmbedded.OpenFile(configuration, "database.db4o");
            // #end example
            container.Close();
        }

        private static void ServerConfiguration()
        {
            // #example: Configure the db4o-server
            IServerConfiguration configuration = Db4oClientServer.NewServerConfiguration();
            // change the configuration...
            IObjectServer server = Db4oClientServer.OpenServer(configuration, "database.db4o", 1337);
            // #end example
            server.Close();
        }
        private static void ClientConfiguration()
        {
            // #example: Configure a client object container
            IClientConfiguration configuration = Db4oClientServer.NewClientConfiguration();
            // change the configuration...
            IObjectContainer container = Db4oClientServer.OpenClient(configuration, "localhost", 1337, "user", "pwd");
            // #end example
            container.Close();
        }
    }

}