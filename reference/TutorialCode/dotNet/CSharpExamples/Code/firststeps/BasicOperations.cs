using System;
using System.Linq;
using Db4objects.Db4o;
using Db4objects.Db4o.Linq;

namespace Db4oTutorialCode.Code.FirstSteps
{
    public class BasicOperations
    {
        public static void Main(string[] args)
        {
            OpenAndCloseTheContainer();
            StoreObject();
            Query();
            UpdateObject();
            DeleteObject();
        }

        private static void OpenAndCloseTheContainer()
        {
            // #example: Open and close db4o
            using (IObjectContainer container = Db4oEmbedded.OpenFile("databaseFile.db4o"))
            {
                // use the object container in here
            }
            // #end example
        }

        private static void StoreObject()
        {
            // #example: Store an object
            using (IObjectContainer container = Db4oEmbedded.OpenFile("databaseFile.db4o"))
            {
                var driver = new Driver("Joe");
                container.Store(driver);
            }
            // #end example
        }

        private static void Query()
        {
            // #example: Query for objects
            using (IObjectContainer container = Db4oEmbedded.OpenFile("databaseFile.db4o"))
            {
                var drivers = from Driver d in container
                              where d.Name == "Joe"
                              select d;
                Console.WriteLine("Stored Pilots:");
                foreach (Driver driver in drivers)
                {
                    Console.WriteLine(driver.Name);
                }
            }

            // #end example
        }

        private static void UpdateObject()
        {
            // #example: Update an object
            using (IObjectContainer container = Db4oEmbedded.OpenFile("databaseFile.db4o"))
            {
                var drivers = from Driver d in container
                              where d.Name == "Joe"
                              select d;
                Driver driver = drivers.First();
                Console.WriteLine("Old name {0}", driver.Name);
                driver.Name = "John";
                Console.WriteLine("New name {0}", driver.Name);
                // update the pilot
                container.Store(driver);
            }
            // #end example
        }

        private static void DeleteObject()
        {
            // #example: Delete an object
            using (IObjectContainer container = Db4oEmbedded.OpenFile("databaseFile.db4o"))
            {
                var drivers = from Driver d in container
                              where d.Name == "Joe"
                              select d;
                Driver driver = drivers.First();
                Console.WriteLine("Deleting {0}", driver.Name);
                container.Delete(driver);
            }
            // #end example
        }
    }
}