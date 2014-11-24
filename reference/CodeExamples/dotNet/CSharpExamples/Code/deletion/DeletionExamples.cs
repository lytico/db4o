using System;
using System.Collections.Generic;
using System.IO;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;

namespace Db4oDoc.Code.Deletion
{
    public class DeletionExamples
    {
        private const string DatabaseFile = "database.db4o";

        public static void Main(string[] args)
        {
            SimpleDeletion();
            ReferenceIsSetToNull();
            CascadeDeletion();

            RemoveFromCollection();
            RemoveAndDelete();
        }

        private static void SimpleDeletion()
        {
            PrepareDBWithCarAndPilot();
            using (IObjectContainer container = Db4oEmbedded.OpenFile(DatabaseFile))
            {
                // #example: Deleting object is as simple as storing
                Car car = FindCar(container);
                container.Delete(car);
                // We've deleted the only care there is
                AssertEquals(0, AllCars(container).Count);
                // The pilots are still there
                AssertEquals(1, AllPilots(container).Count);
                // #end example
            }
        }

        private static void ReferenceIsSetToNull()
        {
            PrepareDBWithCarAndPilot();
            using (IObjectContainer container = Db4oEmbedded.OpenFile(DatabaseFile))
            {
                // #example: Delete the pilot
                Pilot pilot = FindPilot(container);
                container.Delete(pilot);
                // #end example
            }
            using (IObjectContainer container = Db4oEmbedded.OpenFile(DatabaseFile))
            {
                // #example: Reference is null after deleting
                // Now the car's reference to the car is set to null
                Car car = FindCar(container);
                AssertEquals(null, car.Pilot);
                // #end example
            }
        }

        private static void CascadeDeletion()
        {
            PrepareDBWithCarAndPilot();
            // #example: Mark field for cascading deletion
            IEmbeddedConfiguration config = Db4oEmbedded.NewConfiguration();
            config.Common.ObjectClass(typeof (Car)).ObjectField("pilot").CascadeOnDelete(true);
            using (IObjectContainer container = Db4oEmbedded.OpenFile(config, DatabaseFile))
            {
                // #end example
                // #example: Cascade deletion
                Car car = FindCar(container);
                container.Delete(car);
                // Now the pilot is also gone
                AssertEquals(0, AllPilots(container).Count);
                // #end example
            }
        }

        private static void RemoveFromCollection()
        {
            PrepareDBWithPilotGroup();
            using (IObjectContainer container = Db4oEmbedded.OpenFile(DatabaseFile))
            {
                // #example: Removing from a collection doesn't delete the collection-members
                PilotGroup group = FindGroup(container);
                Pilot pilot = group.Pilots[0];
                group.Pilots.Remove(pilot);
                container.Store(group.Pilots);

                AssertEquals(3, AllPilots(container).Count);
                AssertEquals(2, group.Pilots.Count);
                // #end example
            }
        }

        private static void RemoveAndDelete()
        {
            PrepareDBWithPilotGroup();
            using (IObjectContainer container = Db4oEmbedded.OpenFile(DatabaseFile))
            {
                // #example: Remove and delete
                PilotGroup group = FindGroup(container);
                Pilot pilot = group.Pilots[0];
                group.Pilots.Remove(pilot);
                container.Store(group.Pilots);
                container.Delete(pilot);

                AssertEquals(2, AllPilots(container).Count);
                AssertEquals(2, group.Pilots.Count);
                // #end example
            }
        }

        private static void AssertEquals(object expectedValue, object actualValue)
        {
            if (!Equals(expectedValue, actualValue))
            {
                throw new Exception("Expected " + expectedValue + " but got " + actualValue);
            }
        }

        private static Pilot FindPilot(IObjectContainer container)
        {
            return AllPilots(container)[0];
        }

        private static IList<Pilot> AllPilots(IObjectContainer container)
        {
            return container.Query<Pilot>();
        }

        private static Car FindCar(IObjectContainer container)
        {
            return AllCars(container)[0];
        }

        private static IList<Car> AllCars(IObjectContainer container)
        {
            return container.Query<Car>();
        }

        private static PilotGroup FindGroup(IObjectContainer container)
        {
            return container.Query<PilotGroup>()[0];
        }

        private static void PrepareDBWithCarAndPilot()
        {
            CleanUp();
            using (IObjectContainer container = Db4oEmbedded.OpenFile(DatabaseFile))
            {
                container.Store(new Car(new Pilot("John"), "VM Golf"));
            }
        }

        private static void PrepareDBWithPilotGroup()
        {
            CleanUp();
            using (IObjectContainer container = Db4oEmbedded.OpenFile(DatabaseFile))
            {
                container.Store(new PilotGroup(
                                    new Pilot("John"),
                                    new Pilot("Jenny"),
                                    new Pilot("Joanna")
                                    ));
            }
        }

        private static void CleanUp()
        {
            File.Delete(DatabaseFile);
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

    internal class Car
    {
        private string name;
        private Pilot pilot;

        public Car(Pilot pilot, string name)
        {
            this.name = name;
            this.pilot = pilot;
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public Pilot Pilot
        {
            get { return pilot; }
            set { pilot = value; }
        }
    }

    internal class PilotGroup
    {
        private readonly IList<Pilot> pilots = new List<Pilot>();

        internal PilotGroup(params Pilot[] pilots)
        {
            foreach (Pilot pilot in pilots)
            {
                this.pilots.Add(pilot);
            }
        }

        public IList<Pilot> Pilots
        {
            get { return pilots; }
        }
    }
}