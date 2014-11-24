using Db4objects.Db4o;
using Db4objects.Db4o.CS;

namespace Db4oDoc.Code.ClientServer.Pooling
{
    internal class ConnectionPoolExamples
    {
        private const int Port = 1337;
        public const string UserAndPassword = "sa";


        public static void Main(string[] args)
        {
            IObjectServer server = StartServer();

            ConnectionPool connectionPool = new ConnectionPool(CreateClientConnection);

            UseThePool(connectionPool);

            server.Close();
        }

        private static IObjectContainer CreateClientConnection()
        {
            // #example: Open clients for the pool
            IObjectContainer client = Db4oClientServer.OpenClient("localhost", 
                Port, UserAndPassword, UserAndPassword);
            // #end example
            return client;
        }

        private static void UseThePool(ConnectionPool connectionPool)
        {
            IObjectContainer session = connectionPool.Acquire();
            try
            {
                session.Store(new Person("Joe"));
            }
            finally
            {
                connectionPool.CloseAndRelease(session);
            }
        }

        private static IObjectServer StartServer()
        {
            IObjectServer server = Db4oClientServer.OpenServer("In:Memory", Port);
            server.GrantAccess(UserAndPassword, UserAndPassword);
            return server;
        }

        private class Person
        {
            private string name;

            public Person(string name)
            {
                this.name = name;
            }

            public string Name
            {
                get { return name; }
                set { name = value; }
            }
        }
    }
}