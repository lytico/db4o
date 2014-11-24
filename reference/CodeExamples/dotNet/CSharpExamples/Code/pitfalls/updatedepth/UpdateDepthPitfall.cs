using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Linq;

namespace Db4oDoc.Code.Pitfalls.UpdateDepth
{
    public class UpdateDepthPitfall
    {
        public const string DatabaseFile = "database.db4o";

        public static void Main(String[] args)
        {
            CleanUpAndPrepare();

            ToLowUpdateDepthOnObject();
            ToLowUpdateDepthOnCollection();

            ExplicitlyUpdateObjects();
            ExplicitlyStateUpdateDepth();
            UpdateDepthForCollection();
        }

        private static void ToLowUpdateDepthOnObject()
        {
            // #example: Update depth limits what is store when updating objects
            using (IObjectContainer container = Db4oEmbedded.OpenFile(DatabaseFile))
            {
                Car car = QueryForCar(container);
                car.CarName = "New Mercedes";
                car.Driver.Name = "New Driver Name";

                // With the default-update depth of one, only the changes
                // on the car-object are stored, but not the changes on
                // the person
                container.Store(car);
            }
            using (IObjectContainer container = Db4oEmbedded.OpenFile(DatabaseFile))
            {
                Car car = QueryForCar(container);
                Console.WriteLine("Car-Name:" + car.CarName);
                Console.WriteLine("Driver-Name:" + car.Driver.Name);
            }
            // #end example
        }

        private static void ToLowUpdateDepthOnCollection()
        {
            // #example: Update doesn't work on collection
            using (IObjectContainer container = Db4oEmbedded.OpenFile(DatabaseFile))
            {
                Person jodie = QueryForJodie(container);
                jodie.Add(new Person("Jamie"));
                // Remember that a collection is also a regular object
                // so with the default-update depth of one, only the changes
                // on the person-object are stored, but not the changes on
                // the friend-list.
                container.Store(jodie);
            }
            using (IObjectContainer container = Db4oEmbedded.OpenFile(DatabaseFile))
            {
                Person jodie = QueryForJodie(container);
                foreach (Person person in jodie.Friends)
                {
                    // the added friend is gone, because the update-depth is to low
                    Console.WriteLine("Friend=" + person.Name);
                }
            }
            // #end example
        }

        private static void ExplicitlyUpdateObjects()
        {
            CleanUp();
            using (IObjectContainer container = Db4oEmbedded.OpenFile(DatabaseFile))
            {
                // #example: Explicitly store changes on the driver
                Car car = QueryForCar(container);
                car.CarName = "New Mercedes";
                car.Driver.Name = "New Driver Name";

                // Explicitly store the driver to ensure that those changes are also in the database
                container.Store(car);
                container.Store(car.Driver);
                // #end example
            }
            PrintCar();
        }

        private static void ExplicitlyStateUpdateDepth()
        {
            CleanUp();
            using (IObjectContainer container = Db4oEmbedded.OpenFile(DatabaseFile))
            {
                // #example: Explicitly use the update depth
                Car car = QueryForCar(container);
                car.CarName = "New Mercedes";
                car.Driver.Name = "New Driver Name";

                // Explicitly state the update depth
                container.Ext().Store(car, 2);
                // #end example
            }
            PrintCar();
        }

        private static void UpdateDepthForCollection()
        {
            // #example: A higher update depth fixes the issue
            IEmbeddedConfiguration config = Db4oEmbedded.NewConfiguration();
            config.Common.UpdateDepth = 2;
            using (IObjectContainer container = Db4oEmbedded.OpenFile(DatabaseFile))
            {
                // #end example
                Person jodie = QueryForJodie(container);
                jodie.Add(new Person("Jamie"));
                container.Store(jodie);
            }
            config = Db4oEmbedded.NewConfiguration();
            config.Common.UpdateDepth = 2;
            using (IObjectContainer container = Db4oEmbedded.OpenFile(DatabaseFile))
            {
                Person jodie = QueryForJodie(container);
                foreach (Person person in jodie.Friends)
                {
                    // the added friend is gone, because the update-depth is to low
                    Console.WriteLine("Friend=" + person.Name);
                }
            }
        }

        private static void PrintCar()
        {
            using (IObjectContainer container = Db4oEmbedded.OpenFile(DatabaseFile))
            {
                Car car = QueryForCar(container);
                Console.WriteLine("Car-Name:" + car.CarName);
                Console.WriteLine("Driver-Name:" + car.Driver.Name);
            }
        }


        private static Car QueryForCar(IObjectContainer container)
        {
            return container.Query<Car>()[0];
        }

        private static void CleanUpAndPrepare()
        {
            CleanUp();
            PrepareDeepObjGraph();
        }


        private static void CleanUp()
        {
            File.Delete(DatabaseFile);
        }

        private static Person QueryForJodie(IObjectContainer container)
        {
            return (from Person p in container
                    where p.Name == "Jodie"
                    select p).First();
        }

        private static void PrepareDeepObjGraph()
        {
            using (IObjectContainer container = Db4oEmbedded.OpenFile(DatabaseFile))
            {
                Person jodie = new Person("Jodie");

                jodie.Add(new Person("Joanna"));
                jodie.Add(new Person("Julia"));

                container.Store(jodie);

                container.Store(new Car(new Person("Janette"), "Mercedes"));
            }
        }
    }


    internal class Car
    {
        private Person driver;
        private string carName;

        internal Car(Person driver, string carName)
        {
            this.driver = driver;
            this.carName = carName;
        }

        public Person Driver
        {
            get { return driver; }
            set { driver = value; }
        }

        public string CarName
        {
            get { return carName; }
            set { carName = value; }
        }
    }

    internal class Person
    {
        private IList<Person> friends = new List<Person>();

        private string name;

        internal Person(string name)
        {
            this.name = name;
        }


        public IList<Person> Friends
        {
            get { return friends; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public void Add(Person item)
        {
            friends.Add(item);
        }
    }
}