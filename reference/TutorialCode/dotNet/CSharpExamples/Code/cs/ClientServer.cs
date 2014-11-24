using System;
using Db4objects.Db4o;
using Db4objects.Db4o.CS;

namespace Db4oTutorialCode.Code.ClientServer
{
public class ClientServer {
    private const string DatabaseFile = "database.db4o";

    public static void Main(string[] args) {

        SessionContainers();
        RunClientServer();
        StartServer();
    }

    private static void SessionContainers() {
        using(IObjectContainer rootContainer = Db4oEmbedded.OpenFile(DatabaseFile))
        {
            // #example: Creating a session container
            using(IObjectContainer container = rootContainer.Ext().OpenSession())
            {
                // We now can use this session container like any other container
            } 
            // #end example
        } 
    }

    private static void StartServer() {
        // #example: Open server
        using(IObjectServer server = Db4oClientServer.OpenServer("database.db4o",8080))
        {
            // allow access to this server
            server.GrantAccess("user","password");

            // Keep server running as long as you need it
            Console.Out.WriteLine("Press any key to exit.");
            Console.Read();
            Console.Out.WriteLine("Exiting...");
        }
        // #end example
    }

    private static void RunClientServer()
    {
        using (IObjectServer server = Db4oClientServer.OpenServer("database.db4o", 8080))
        {
            server.GrantAccess("user","password");

            OpenClient();
        }
    }

    private static void OpenClient() {
        // #example: Using the client
        using (IObjectContainer container 
            = Db4oClientServer.OpenClient("localhost",8080,"user","password"))
        {
            // Use the client object container as usual
        }
        // #end example
    }
}
}