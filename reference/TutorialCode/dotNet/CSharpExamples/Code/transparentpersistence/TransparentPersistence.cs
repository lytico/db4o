using System;
using System.Linq;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Linq;
using Db4objects.Db4o.TA;

namespace Db4oTutorialCode.Code.TransparentPersistence
{
    public class TransparentPersistence
    {
        private const string DatabaseFileName = "database.db4o";

        public static void Main(string[] args)
        {
            CheckEnhancement();
            StoreExampleObjects();
            ActivationJustWorks();
            UpdatesJustWork();
        }

        private static void ActivationJustWorks()
        {
            // #example: Configure transparent persistence
            IEmbeddedConfiguration configuration = Db4oEmbedded.NewConfiguration();
            configuration.Common.Add(new TransparentPersistenceSupport(new DeactivatingRollbackStrategy()));
            using (IObjectContainer container = Db4oEmbedded.OpenFile(configuration, DatabaseFileName))
            // #end example
            {
                //#example: Transparent persistence manages activation
                Driver driver = QueryForDriver(container);
                // Transparent persistence will activate objects as needed
                Console.WriteLine("Is activated? " + container.Ext().IsActive(driver));
                string nameOfDriver = driver.Name;
                Console.WriteLine("The name is " + nameOfDriver);
                Console.WriteLine("Is activated? " + container.Ext().IsActive(driver));
                //#end example
            }
        }

        private static void UpdatesJustWork()
        {
            using (IObjectContainer container = OpenDatabase())
            {
                // #example: Just update and commit. Transparent persistence manages all updates
                Driver driver = QueryForDriver(container);
                driver.MostLovedCar.CarName = "New name";
                driver.Name = "John Turbo";
                driver.AddOwnedCar(new Car("Volvo Combi"));
                // Just commit the transaction. All modified objects are stored
                container.Commit();
                // #end example
            }
        }

        private static void CheckEnhancement()
        {
            // #example: Check for enhancement
            if (!typeof (IActivatable).IsAssignableFrom(typeof (Car)))
            {
                throw new InvalidOperationException(string.Format("Expect that the {0} implements {1}", 
                    typeof (Car),typeof (IActivatable)));
            }
            // #end example
            if (!typeof (IActivatable).IsAssignableFrom(typeof (Driver)))
            {
                throw new InvalidOperationException(string.Format("Expect that the {0} implements {1}",
                    typeof (Driver),typeof (IActivatable)));
            }
        }

        private static void StoreExampleObjects()
        {
            using (IObjectContainer container = Db4oEmbedded.OpenFile(DatabaseFileName))
            {
                var beetle = new Car("VW Beetle");
                var ferrari = new Car("Ferrari");

                var driver = new Driver("John", ferrari);
                driver.AddOwnedCar(beetle);
                driver.AddOwnedCar(ferrari);

                container.Store(driver);
            }
        }

        private static IObjectContainer OpenDatabase()
        {
            IEmbeddedConfiguration configuration = Db4oEmbedded.NewConfiguration();
            configuration.Common.Add(new TransparentPersistenceSupport(new DeactivatingRollbackStrategy()));
            return Db4oEmbedded.OpenFile(configuration, DatabaseFileName);
        }

        private static Driver QueryForDriver(IObjectContainer container)
        {
            return (from Driver d in container
                    select d).First();
        }
    }
}