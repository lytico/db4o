using System;
using System.Threading;
using Db4objects.Db4o;
using Db4objects.Db4o.CS;

namespace Db4oDoc.Code.ClientServer.Basics
{
    public class ServerExample
    {
        public static void Main(string[] args)
        {
            // #example: Start a db4o server
            using (IObjectServer server = Db4oClientServer.OpenServer("database.db4o", 8080))
            {
                server.GrantAccess("user", "password");

                // Let the server run.
                LetServerRun();
            }
            // #end example
        }

        private static void LetServerRun()
        {
            Console.WriteLine("Press a key to close the server");
            while (Console.KeyAvailable)
            {
                Thread.Sleep(1000);
            }
        }
    }
}