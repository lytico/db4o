using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Db4objects.Db4o;
using Db4objects.Db4o.CS;
using Db4objects.Db4o.CS.Config;
using Db4objects.Db4o.Events;
using Db4objects.Db4o.Internal;

namespace Db4oDoc.Code.ClientServer.Refresh
{
    public class RefreshingObjects
    {
        private const int PortNumber = 1337;
        private const string UsertNameAndPassword = "sa";
        private const string DatabaseFileName = "database.db4o";

        public static void Main(string[] args)
        {
            UseEventsToRefreshObjects();
            RefreshOnDemand();
        }

        private static void RefreshOnDemand()
        {
            CleanUp();

            using (IObjectServer server = OpenServer())
            {
                server.GrantAccess(UsertNameAndPassword, UsertNameAndPassword);
                StoreJoeOnOtherClient();

                IObjectContainer client = OpenClient();
                IList<Person> allPersons = ListAllPersons(client);
                PrintPersons(allPersons);

                UpdateJoeOnOtherClient();

                // the persons are not in the most current state
                PrintPersons(allPersons);

                // but you can explicitly refresh the objects
                Refresh(client, allPersons);
                PrintPersons(allPersons);


                WaitForALittleWhile();
            }
            CleanUp();
        }

        private static void Refresh(IObjectContainer db, IList<Person> allPersons)
        {
            foreach (Person objToRefresh in allPersons)
            {
                // #example: refresh a object
                db.Ext().Refresh(objToRefresh, int.MaxValue);
                // #end example
            }
        }

        private static void UseEventsToRefreshObjects()
        {
            CleanUp();

            using (IObjectServer server = OpenServer())
            {
                server.GrantAccess(UsertNameAndPassword, UsertNameAndPassword);
                StoreJoeOnOtherClient();


                IObjectContainer client = OpenClient();
                RegisterEvent(client);
                List<Person> allPersons = ListAllPersons(client);
                PrintPersons(allPersons);

                UpdateJoeOnOtherClient();

                // the events are asynchronously transported over the network
                // which takes a while
                WaitForALittleWhile();
                PrintPersons(allPersons);


                WaitForALittleWhile();
            }
            CleanUp();
        }

        private static void RegisterEvent(IObjectContainer container)
        {
            // #example: On the updated-event we refresh the objects
            IEventRegistry events = EventRegistryFactory.ForObjectContainer(container);
            events.Committed += 
                delegate(object sender, CommitEventArgs args)
                    {
                        foreach (LazyObjectReference updated in args.Updated)
                        {
                            object obj = updated.GetObject();
                            args.ObjectContainer().Ext().Refresh(obj, 1);
                        }
                    };
            // #end example
        }

        private static void PrintPersons(IList<Person> allPersons)
        {
            foreach (Person person in allPersons)
            {
                Console.WriteLine(person);
            }
        }

        private static void StoreJoeOnOtherClient()
        {
            using (IObjectContainer client = OpenClient())
            {
                client.Store(new Person("Joe"));
            }
        }

        private static void UpdateJoeOnOtherClient()
        {
            using (IObjectContainer container = OpenClient())
            {
                IList<Person> persons = container.Query<Person>();
                foreach (Person person in persons)
                {
                    person.Name = "New " + person.Name;
                    container.Store(person);
                }
            }
        }


        private static List<Person> ListAllPersons(IObjectContainer container)
        {
            IList<Person> persons = container.Query<Person>();
            return new List<Person>(persons);
        }

        private static void WaitForALittleWhile()
        {
            Thread.Sleep(200);
        }

        private static IObjectContainer OpenClient()
        {
            return Db4oClientServer.OpenClient("localhost", PortNumber, UsertNameAndPassword, UsertNameAndPassword);
        }


        private static IObjectServer OpenServer()
        {
            IServerConfiguration configuration = Db4oClientServer.NewServerConfiguration();
            return Db4oClientServer.OpenServer(configuration, DatabaseFileName, PortNumber);
        }

        private static void CleanUp()
        {
            File.Delete(DatabaseFileName);
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