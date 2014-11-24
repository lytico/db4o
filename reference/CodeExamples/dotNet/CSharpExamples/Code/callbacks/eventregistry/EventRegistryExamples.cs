using System;
using System.IO;
using System.Linq;
using System.Threading;
using Db4objects.Db4o;
using Db4objects.Db4o.CS;
using Db4objects.Db4o.Events;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Linq;

namespace Db4oDoc.Code.Callbacks.EventRegistry
{
    public class EventRegistryExamples
    {
        private const string DatabaseFileName = "database.db4o";
        private const int PortNumber = 1337;
        private const string EmbeddedUser = "user";
        private const string EmbeddedPassword = "user";

        public static void Main(string[] args)
        {
            Console.WriteLine("--Events in embedded mode--");
            EventsInLocalContainer();
            Console.WriteLine("--Events in client/server mode--");
            EventsClientServer();
            Console.WriteLine("--Cancel in event --");
            CancelInEvent();
            Console.WriteLine("--Commit-events --");
            CommitEvents();
        }


        private static void EventsInLocalContainer()
        {
            CleanUp();
            StoreJoe();

            using (IObjectContainer container = OpenEmbedded())
            {
                // #example: Obtain the event-registry
                IEventRegistry events = EventRegistryFactory.ForObjectContainer(container);
                // #end example

                RegisterAFewEvents(events, "local embedded container");
                RunOperations(container);
            }
            CleanUp();
        }

        private static void RegisterForEventsOnTheServer()
        {
            // #example: register for events on the server
            IObjectServer server = 
                    Db4oClientServer.OpenServer(DatabaseFileName, PortNumber);
            IEventRegistry eventsOnServer = 
                    EventRegistryFactory.ForObjectContainer(server.Ext().ObjectContainer());
            // #end example
        }

        private static void EventsClientServer()
        {
            CleanUp();
            StoreJoe();

            using (IObjectServer server = OpenServer())
            {
                IEventRegistry eventsOnServer = EventRegistryFactory.ForObjectContainer(server.Ext().ObjectContainer());
                RegisterAFewEvents(eventsOnServer, "db4o server");

                IObjectContainer client1 = OpenClient();
                IEventRegistry eventsOnClient1 = EventRegistryFactory.ForObjectContainer(client1);
                RegisterAFewEvents(eventsOnClient1, "db4o client 1");
                RunOperations(client1);


                IObjectContainer client2 = OpenClient();
                IEventRegistry eventsOnClient2 = EventRegistryFactory.ForObjectContainer(client2);
                RegisterAFewEvents(eventsOnClient2, "db4o client 2");

                SleepForAWhile();
                client1.Dispose();
                client2.Dispose();
            }

            CleanUp();
        }

        private static void CancelInEvent()
        {
            CleanUp();
            StoreJoe();

            using (IObjectContainer container = OpenEmbedded())
            {
                // #example: Cancel store operation
                IEventRegistry events = EventRegistryFactory.ForObjectContainer(container);
                events.Creating +=
                    delegate(object sender, CancellableObjectEventArgs args)
                        {
                            if (args.Object is Person)
                            {
                                Person p = (Person) args.Object;
                                if (p.Name.Equals("Joe Junior"))
                                {
                                    args.Cancel();
                                }
                            }
                        };

                // #end example
                container.Store(new Person("Joe Junior"));

                int personCount = container.Query<Person>().Count;
                Console.WriteLine("Only " + personCount + " because store was cancelled");
            }
            CleanUp();
        }

        private static void CommitEvents()
        {
            CleanUp();
            StoreJoe();

            using (IObjectContainer container = OpenEmbedded())
            {
                // #example: Commit-info
                IEventRegistry events = EventRegistryFactory.ForObjectContainer(container);
                events.Committed +=
                    delegate(object sender, CommitEventArgs args)
                        {
                            foreach (LazyObjectReference reference in args.Added)
                            {
                                Console.WriteLine("Added " + reference.GetObject());
                            }
                            foreach (LazyObjectReference reference in args.Updated)
                            {
                                Console.WriteLine("Updated " + reference.GetObject());
                            }
                            foreach (FrozenObjectInfo reference in args.Deleted)
                            {
                                //the deleted info might doesn't contain the object anymore and
                                //return the null.
                                Console.WriteLine("Deleted " + reference.GetObject());
                            }
                        };
                // #end example
                RunOperations(container);
            }
            CleanUp();
        }

        private static void RunOperations(IObjectContainer container)
        {
            var joe = (from Person p in container
                             where p.Name.Equals("Joe")
                             select p).First();
            joe.Name = "Joe Senior";
            container.Store(joe);
            container.Store(new Person("Joe Junior"));
            container.Commit();
        }

        private static void StoreJoe()
        {
            using (IObjectContainer container = OpenEmbedded())
            {
                container.Store(new Person("Joe"));
            }
        }

        private static void RegisterAFewEvents(IEventRegistry events, string containerName)
        {
            events.Activating +=
                delegate { Console.Out.WriteLine("Activating on {0}", containerName); };
            events.Activated +=
                delegate { Console.Out.WriteLine("Activated on {0}", containerName); };
            events.Creating +=
                delegate { Console.Out.WriteLine("Creating on {0}", containerName); };
            events.Created +=
                delegate { Console.Out.WriteLine("Created on {0}", containerName); };
            events.Updating +=
                delegate { Console.Out.WriteLine("Updating on {0}", containerName); };
            events.Updated +=
                delegate { Console.Out.WriteLine("Updated on {0}", containerName); };
            events.QueryStarted +=
                delegate { Console.Out.WriteLine("QueryStarted on {0}", containerName); };
            events.QueryFinished +=
                delegate { Console.Out.WriteLine("QueryFinished on {0}", containerName); };
            events.Committing +=
                delegate { Console.Out.WriteLine("Committing on {0}", containerName); };
            events.Committed +=
                delegate { Console.Out.WriteLine("Committing on {0}", containerName); };
            // #example: register for a event
            events.Committing += HandleCommitting;
            // #end example
        }

        // #example: implement your event handling
        private static void HandleCommitting(object sender,
            CommitEventArgs commitEventArgs)
        {
            // handle the event           
        }

        // #end example


        private static IObjectContainer OpenEmbedded()
        {
            return Db4oEmbedded.OpenFile(DatabaseFileName);
        }

        private static IObjectContainer OpenClient()
        {
            return Db4oClientServer.OpenClient("localhost", PortNumber, EmbeddedUser, EmbeddedPassword);
        }

        private static IObjectServer OpenServer()
        {
            IObjectServer server = Db4oClientServer.OpenServer(DatabaseFileName, PortNumber);
            server.GrantAccess(EmbeddedUser, EmbeddedPassword);
            return server;
        }

        private static void CleanUp()
        {
            File.Delete(DatabaseFileName);
        }

        private static void SleepForAWhile()
        {
            Thread.Sleep(2000);
        }
    }

    public class Person
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

        public override string ToString()
        {
            return string.Format("Name: {0}", name);
        }
    }
}