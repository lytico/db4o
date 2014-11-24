using System;
using System.Collections.Generic;
using System.Linq;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Linq;

namespace Db4oTutorialCode.Code.Updating
{
    public class UpdateConcept
    {
        private const string DatabaseFile = "database.db4o";

        public static void Main(string[] args)
        {
            StoreExampleObjects();
            UpdatingDriverDoesNotUpdateCar();
            DealWithUpdateDepth();
            IncreaseUpdateDepth();
        }

        private static void StoreExampleObjects()
        {
            using (IObjectContainer container = Db4oEmbedded.OpenFile(DatabaseFile))
            {
                // #example: Store a driver and his cars
                var beetle = new Car("VW Beetle");
                var ferrari = new Car("Ferrari");

                var driver = new Driver("John", ferrari)
                                 {
                                     OwnedCars = new List<Car> {beetle, ferrari}
                                 };

                container.Store(driver);
                // #end example
            }
        }

        private static void UpdatingDriverDoesNotUpdateCar()
        {
            using (IObjectContainer container = Db4oEmbedded.OpenFile(DatabaseFile))
            {
                // #example: Update the driver and his cars
                Driver driver = QueryForDriver(container);
                driver.Name = "Johannes";
                driver.MostLovedCar.CarName = "Red Ferrari";
                driver.AddOwnedCar(new Car("Fiat Punto"));
                container.Store(driver);
                // #end example
            }
            PrintOutContent();
        }

        private static void IncreaseUpdateDepth()
        {
            // #example: Increase the update depth to 2
            IEmbeddedConfiguration configuration = Db4oEmbedded.NewConfiguration();
            configuration.Common.UpdateDepth = 2;
            // #end example

            using (IObjectContainer container = Db4oEmbedded.OpenFile(configuration, DatabaseFile))
            {
                Driver driver = QueryForDriver(container);
                driver.Name = "Joe";
                driver.MostLovedCar.CarName = "Red Ferrari";
                driver.AddOwnedCar(new Car("Fiat Punto"));
                container.Store(driver);
            }
            PrintOutContent();
        }

        private static void DealWithUpdateDepth()
        {
            using (IObjectContainer container = Db4oEmbedded.OpenFile(DatabaseFile))
            {
                Driver driver = QueryForDriver(container);
                driver.Name = "Joe";
                driver.MostLovedCar.CarName = "Red Ferrari";
                driver.AddOwnedCar(new Car("Fiat Punto"));
                // #example: Update everything explicitly
                container.Store(driver);
                container.Store(driver.MostLovedCar);
                container.Store(driver.OwnedCars);
                // #end example
            }
            PrintOutContent();
        }

        private static void PrintOutContent()
        {
            using (IObjectContainer container = Db4oEmbedded.OpenFile(DatabaseFile))
            {
                // #example: Check the updated objects
                Driver driver = QueryForDriver(container);
                // Is updated
                Console.WriteLine(driver.Name);
                // Isn't updated at all
                Console.WriteLine(driver.MostLovedCar.CarName);
                // Also the owned car list isn't updated
                foreach (Car car in driver.OwnedCars)
                {
                    Console.WriteLine(car);
                }
                // #end example
            }
        }

        private static IEmbeddedConfiguration MoreUpdateOptions()
        {
            // #example: More update options
            IEmbeddedConfiguration configuration = Db4oEmbedded.NewConfiguration();
            // Update all referenced objects for the Driver-class
            configuration.Common.ObjectClass(typeof (Driver)).CascadeOnUpdate(true);
            // #end example
            return configuration;
        }

        private static Driver QueryForDriver(IObjectContainer container)
        {
            return (from Driver d in container
                    select d).First();
        }
    }
}