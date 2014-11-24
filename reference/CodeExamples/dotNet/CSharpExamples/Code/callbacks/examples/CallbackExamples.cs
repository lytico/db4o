using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Db4objects.Db4o;
using Db4objects.Db4o.Events;
using Db4objects.Db4o.Linq;

namespace Db4oDoc.Code.Callbacks.Examples
{
    public class CallbackExamples
    {
        private const string DatabaseFile = "database.db4o";

        public static void Main(string[] args)
        {
            CleanUp();


            StoreTestObjects();
            ReferentialIntegrity();
        }

        private static void ReferentialIntegrity()
        {
            using (IObjectContainer container = Db4oEmbedded.OpenFile(DatabaseFile))
            {
                // #example: Register handler
                IEventRegistry events = EventRegistryFactory.ForObjectContainer(container);
                events.Deleting += ReferentialIntegrityCheck;
                // #end example

                Pilot pilot = container.Query<Pilot>()[0];
                container.Delete(pilot);
            }
        }

        // #example: Referential integrity
        private static void ReferentialIntegrityCheck(object sender,
                                                      CancellableObjectEventArgs eventArguments)
        {
            object toDelete = eventArguments.Object;
            if (toDelete is Pilot)
            {
                IObjectContainer container = eventArguments.ObjectContainer();
                IEnumerable<Car> cars = from Car c in container
                                        where c.Pilot == toDelete
                                        select c;
                if (cars.Count() > 0)
                {
                    eventArguments.Cancel();
                }
            }
        }
        // #end example

        private static void StoreTestObjects()
        {
            using (IObjectContainer container = Db4oEmbedded.OpenFile(DatabaseFile))
            {
                Pilot pilot = new Pilot("John");
                container.Store(pilot);
                container.Store(new Car(pilot));
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
        }
    }

    internal class Car
    {
        private Pilot pilot;

        public Car(Pilot pilot)
        {
            this.pilot = pilot;
        }

        public Pilot Pilot
        {
            get { return pilot; }
        }
    }
}