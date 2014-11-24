using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Db4objects.Db4o;
using Db4objects.Db4o.Linq;

namespace Db4oDoc.Code.DisconnectedObj.ObjectIdentity
{
    public class ObjectIdentityExamples
    {
        private const string DatabaseFileName = "database.db4o";


        public static void Main(string[] args)
        {
            UpdateWorksOnSameContainer();
            NewObjectIsStoredIfDifferentContainer();
        }

        private static void NewObjectIsStoredIfDifferentContainer()
        {
            CleanUp();
            StoreJoe();

            // #example: Update doesn't works when using the different object containers
            Pilot joe;
            using (IObjectContainer container = OpenDatabase())
            {
                joe = QueryByName(container, "Joe");
            }
            // The update on another object 
            joe.Name = "Joe New";
            using (IObjectContainer otherContainer = OpenDatabase())
            {
                otherContainer.Store(joe);
            }
            using (IObjectContainer container = OpenDatabase())
            {
                // instead of updating the existing pilot,
                // a new instance was stored.
                IList<Pilot> pilots = container.Query<Pilot>();
                Console.WriteLine("Amount of pilots: " + pilots.Count);
                foreach (Pilot pilot in pilots)
                {
                    Console.WriteLine(pilot);
                }
            }
            // #end example

            CleanUp();
        }

        private static void UpdateWorksOnSameContainer()
        {
            CleanUp();
            StoreJoe();

            // #example: Update works when using the same object container
            using (IObjectContainer container = OpenDatabase())
            {
                Pilot joe = QueryByName(container, "Joe");
                joe.Name = "Joe New";
                container.Store(joe);
            }
            using (IObjectContainer container = OpenDatabase())
            {
                IList<Pilot> pilots = container.Query<Pilot>();
                Console.WriteLine("Amount of pilots: " + pilots.Count);
                foreach (Pilot pilot in pilots)
                {
                    Console.WriteLine(pilot);
                }
            }
            // #end example

            CleanUp();
        }

        private static Pilot QueryByName(IObjectContainer container, string name)
        {
            return (from Pilot p in container
                    where p.Name.Equals(name)
                    select p).First();
        }

        private static void StoreJoe()
        {
            using (IObjectContainer container = OpenDatabase())
            {
                container.Store(new Pilot("Joe"));
            }
        }


        private static void CleanUp()
        {
            File.Delete(DatabaseFileName);
        }


        private static IObjectContainer OpenDatabase()
        {
            return Db4oEmbedded.OpenFile(DatabaseFileName);
        }
    }
}