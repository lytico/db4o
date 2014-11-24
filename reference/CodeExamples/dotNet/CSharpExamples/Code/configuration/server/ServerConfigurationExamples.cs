using Db4objects.Db4o;
using Db4objects.Db4o.CS;
using Db4objects.Db4o.CS.Config;

namespace Db4oDoc.Code.Configuration.Server
{
    public class ServerConfigurationExamples
    {

        private static void SocketTimeout()
        {
            // #example: configure the socket-timeout
            IServerConfiguration configuration = Db4oClientServer.NewServerConfiguration();
            configuration.TimeoutServerSocket = (10 * 60 * 1000);
            // #end example

            IObjectServer container = Db4oClientServer.OpenServer(configuration, "database.db4o", 1337);

            container.Close();
        }
    }

}