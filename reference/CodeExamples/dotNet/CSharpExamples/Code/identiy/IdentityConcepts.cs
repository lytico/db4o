using System;
using System.Collections;
using System.IO;
using Db4objects.Db4o;

namespace Db4oDoc.Code.Identiy
{
    public class IdentityConcepts
    {
        private const string DatabaseFile = "database.db4o";

        public static void Main(string[] args)
        {
            CleanUp();
            storeAObject();
            ReferenceEquals();

            StoreAndLoadWithTheSame();
            StoreOnDifferentContainers();

            RemoveFromReferenceCache();
        }

        private static void ReferenceEquals()
        {
            using (IObjectContainer container = Db4oEmbedded.OpenFile(DatabaseFile))
            {
                // #example: db4o ensures reference equality
                Car theCar = container.Query<Car>()[0];
                Pilot thePilot = container.Query<Pilot>()[0];
                Pilot pilotViaCar = theCar.Pilot;
                AssertTrue(thePilot == pilotViaCar);
                // #end example
            }
        }

        private static void StoreAndLoadWithTheSame()
        {
            using (IObjectContainer rootContainer = Db4oEmbedded.OpenFile(DatabaseFile),
                                    container1 = rootContainer.Ext().OpenSession(),
                                    container2 = rootContainer.Ext().OpenSession())
            {
                // #example: Loading with different object container results in different objects
                Car loadedWithContainer1 = container1.Query<Car>()[0];
                Car loadedWithContainer2 = container2.Query<Car>()[0];
                AssertFalse(loadedWithContainer1 == loadedWithContainer2);
                // #end example
            }
        }

        private static void StoreOnDifferentContainers()
        {
            using (IObjectContainer rootContainer = Db4oEmbedded.OpenFile(DatabaseFile),
                                    container1 = rootContainer.Ext().OpenSession(),
                                    container2 = rootContainer.Ext().OpenSession())
            {
                // #example: Don't use different object-container for the same object.
                Car loadedWithContainer1 = container1.Query<Car>()[0];
                container2.Store(loadedWithContainer1);
                // Now the car is store twice.
                // Because the container2 cannot recognize objects from other containers
                // Therefore always use the same container to store and load objects
                PrintAll(container2.Query<Car>());
                // #end example
            }
        }

        private static void RemoveFromReferenceCache()
        {
            using (IObjectContainer container = Db4oEmbedded.OpenFile(DatabaseFile))
            {
                // #example: With purge you can remove objects from the reference cache
                Car theCar = container.Query<Car>()[0];
                container.Ext().Purge(theCar);
                // #end example
            }
        }

        private static void PrintAll(IEnumerable objects)
        {
            foreach (object obj in objects)
            {
                Console.WriteLine(obj);
            }
        }

        private static void AssertTrue(bool mustBeTrue)
        {
            if (!mustBeTrue)
            {
                throw new Exception("expected true");
            }
        }

        private static void AssertFalse(bool mustBeTrue)
        {
            if (mustBeTrue)
            {
                throw new Exception("expected false");
            }
        }

        private static void CleanUp()
        {
            File.Delete(DatabaseFile);
        }

        private static void storeAObject()
        {
            using (IObjectContainer container = Db4oEmbedded.OpenFile(DatabaseFile))
            {
                container.Store(new Car(new Pilot("John"), "VW Golf"));
            }
        }
    }
}