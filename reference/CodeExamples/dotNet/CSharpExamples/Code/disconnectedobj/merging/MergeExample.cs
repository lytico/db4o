using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Linq;

namespace Db4oDoc.Code.DisconnectedObj.Merging
{
    public class MergeExample
    {
        private const string DatabaseFileName = "database.db4o";


        public static void Main(string[] args)
        {
            RunMergeExample();
        }

        private static void RunMergeExample()
        {
            CleanUp();

            StoreCar();
            PrintCars();

            Car car = GetCarByName("Slow Car");
            UpdateCar(car);

            UpdateWithMerging(car);


            PrintCars();

            CleanUp();
        }

        private static void UpdateWithMerging(Car disconnectedCar)
        {
            // #example: merging
            using (IObjectContainer container = OpenDatabase())
            {
                // first get the object from the database
                Car carInDb = GetCarById(container, disconnectedCar.ObjectId);

                // copy the value-objects (int, long, double, string etc)
                carInDb.Name = disconnectedCar.Name;

                // traverse into the references
                Pilot pilotInDB = carInDb.Pilot;
                Pilot disconnectedPilot = disconnectedCar.Pilot;

                // check if the object is still the same
                if (pilotInDB.ObjectId.Equals(disconnectedPilot.ObjectId))
                {
                    // if it is, copy the value-objects
                    pilotInDB.Name = disconnectedPilot.Name;
                    pilotInDB.Points = disconnectedPilot.Points;
                }
                else
                {
                    // otherwise replace the object
                    carInDb.Pilot = disconnectedPilot;
                }

                // finally store the changes
                container.Store(pilotInDB);
                container.Store(carInDb);
                // #end example
            }
        }

        private static void UpdateCar(Car car)
        {
            car.Name = "Fast Car";
            car.Pilot.Points = 300;
        }

        private static void PrintCars()
        {
            using(IObjectContainer container = OpenDatabase()){
                IList<Car> cars = container.Query<Car>();
                foreach (Car car in cars)
                {
                    Console.WriteLine(car);
                }
            }
        }

        private static Car GetCarById(IObjectContainer container, Guid id)
        {
            return container.Query(delegate(Car car) { return car.ObjectId.Equals(id); })[0];
        }

        private static Car GetCarByName(string carName)
        {
            using (IObjectContainer container = OpenDatabase())
            {
                var result = (from Car car in container
                              where car.Name.Equals(carName)
                              select car).First();

                return result;
            }
        }


        private static void StoreCar()
        {
             using(IObjectContainer container = OpenDatabase())
             {
                 container.Store(new Car(new Pilot("Joe", 200), "Slow Car"));
             }
        }


        private static void CleanUp()
        {
            File.Delete(DatabaseFileName);
        }


        private static IObjectContainer OpenDatabase()
        {
            IEmbeddedConfiguration configuration = Db4oEmbedded.NewConfiguration();
            configuration.Common.ObjectClass(typeof (IDHolder)).ObjectField("guid").Indexed(true);
            return Db4oEmbedded.OpenFile(configuration, DatabaseFileName);
        }
    }
}