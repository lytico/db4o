using System;
using System.Linq;
using Db4objects.Db4o;
using Db4objects.Db4o.Linq;

namespace Db4oDoc.Code.Basics
{
    public class Db4oBasics
    {
        public static void Main(string[] args)
        {
            OpenAndCloseTheContainer();
            StoreObject();
            Query();
            UpdateDatabase();
            StoreObject();
            DeleteObject();

            AllOperationsInOnGo();
        }

        private static void StoreObject()
        {
            // #example: Store an object
            using (IObjectContainer container = Db4oEmbedded.OpenFile("databaseFile.db4o"))
            {
                Pilot pilot = new Pilot("Joe");
                container.Store(pilot);
            }
            // #end example
        }

        private static void Query()
        {
            // #example: Query for objects
            using (IObjectContainer container = Db4oEmbedded.OpenFile("databaseFile.db4o"))
            {
                var pilots = from Pilot p in container
                             where p.Name == "Joe"
                             select p;
                foreach (var pilot in pilots)
                {
                    Console.Out.WriteLine(pilot.Name);
                }
            }
            // #end example
        }

        private static void UpdateDatabase()
        {
            // #example: Update a pilot
            using (IObjectContainer container = Db4oEmbedded.OpenFile("databaseFile.db4o"))
            {
                var pilot = (from Pilot p in container
                             where p.Name == "Joe"
                             select p).First();
                pilot.Name = "New Name";
                // update the pilot
                container.Store(pilot);
            }
            // #end example
        }

        private static void DeleteObject()
        {
            // #example: Delete a object
            using (IObjectContainer container = Db4oEmbedded.OpenFile("databaseFile.db4o"))
            {
                var pilot = (from Pilot p in container
                             where p.Name == "Joe"
                             select p).First();
                container.Delete(pilot);
            }
            // #end example
        }

        private static void OpenAndCloseTheContainer()
        {
            // #example: Open the object container to use the database
            using (IObjectContainer container = Db4oEmbedded.OpenFile("databaseFile.db4o"))
            {
                // use the object container
            }
            // #end example
        }

        private static void AllOperationsInOnGo()
        {
            // #example: The basic operations
            using(IObjectContainer container = Db4oEmbedded.OpenFile("databaseFile.db4o"))
            {
                // store a new pilot
                Pilot pilot = new Pilot("Joe");
                container.Store(pilot);

                // query for pilots
                var pilots = from Pilot p in container
                             where p.Name.StartsWith("Jo")
                             select p;

                // update pilot
                Pilot toUpdate = pilots.First();
                toUpdate.Name = "New Name";
                container.Store(toUpdate);

                // delete pilot
                container.Delete(toUpdate);
            } 
            // #end example
        }
    }

    internal class Pilot
    {
        private string name;

        public Pilot(string name)
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