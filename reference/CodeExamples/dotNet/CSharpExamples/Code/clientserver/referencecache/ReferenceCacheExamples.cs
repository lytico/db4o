using System;
using System.IO;
using Db4objects.Db4o;
using Db4objects.Db4o.CS;

namespace Db4oDoc.Code.ClientServer.ReferenceCache
{
    public class ReferenceCacheExamples
    {
        private const string DatabaseFile = "database.db4o";

        public static void Main(string[] args)
        {
            CleanUp();
            using (IObjectServer server = Db4oClientServer.OpenServer(DatabaseFile, 1337))
            {
                server.GrantAccess("sa", "sa");
                StoreData(server);

                ReferenceCacheExample();
                UnitOfWork();
            }
        }

        private static void ReferenceCacheExample()
        {
            using (IObjectContainer client1 = Db4oClientServer.OpenClient("localhost", 1337, "sa", "sa"),
                                    client2 = Db4oClientServer.OpenClient("localhost", 1337, "sa", "sa"))
            {
                // #example: Reference cache in client server
                Person personOnClient1 = QueryForPerson(client1);
                Person personOnClient2 = QueryForPerson(client2);
                Console.Write(QueryForPerson(client2).Name);

                personOnClient1.Name = ("New Name");
                client1.Store(personOnClient1);
                client1.Commit();

                // The other client still has the old data in the cache
                Console.Write(QueryForPerson(client2).Name);

                client2.Ext().Refresh(personOnClient2, int.MaxValue);

                // After refreshing the date is visible
                Console.Write(QueryForPerson(client2).Name);
                // #end example
            }
        }

        private static void UnitOfWork()
        {
            using (IObjectContainer client
                = Db4oClientServer.OpenClient("localhost", 1337, "sa", "sa"))
            {
                // #example: Clean cache for each unit of work
                using (IObjectContainer container = client.Ext().OpenSession())
                {
                    // do work
                }
                // Start with a fresh cache:
                using (IObjectContainer container = client.Ext().OpenSession())
                {
                    // do work
                }
                // #end example
            }
        }

        private static Person QueryForPerson(IObjectContainer container)
        {
            return container.Query<Person>()[0];
        }

        private static void StoreData(IObjectServer server)
        {
            using (IObjectContainer container = server.OpenClient())
            {
                container.Store(new Person("Joe"));
            }
        }

        private static void CleanUp()
        {
            File.Delete(DatabaseFile);
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