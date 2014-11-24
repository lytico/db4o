using System;
using System.Collections.Generic;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;

namespace Db4oDoc.Code.Strategies.StoringStatic
{
    public class StoringStaticFields
    {
        private const string DatabaseFile = "database.db4o";

        public static void Main(string[] args)
        {
            StoreCars();
            LoadCars();
        }

        private static void LoadCars()
        {
            using (IObjectContainer container = OpenDatabase())
            {
                IList<Car> cars = container.Query<Car>();

                foreach (Car car in cars)
                {
                    // #example: Compare by reference
                    // When you enable persist static field values, you can compare by reference
                    // because db4o stores the static field
                    if (car.Color == Color.Black)
                    {
                        Console.WriteLine("Black cars are boring");
                    }
                    else if (car.Color == Color.Red)
                    {
                        Console.WriteLine("Fire engine?");
                    }
                    // #end example
                }
            }
        }

        private static void StoreCars()
        {
            using (IObjectContainer container = OpenDatabase())
            {
                container.Store(new Car(Color.Black));
                container.Store(new Car(Color.White));
                container.Store(new Car(Color.Green));
                container.Store(new Car(Color.Red));
            }
        }

        private static IObjectContainer OpenDatabase()
        {
            //#example: Enable storing static fields for our color class
            IEmbeddedConfiguration configuration = Db4oEmbedded.NewConfiguration();
            configuration.Common.ObjectClass(typeof (Color)).PersistStaticFieldValues();
            // #end example
            return Db4oEmbedded.OpenFile(configuration, DatabaseFile);
        }
    }



    public class Car
    {
        private Color color = Color.Black;

        public Car()
        {
        }

        public Car(Color color)
        {
            this.color = color;
        }

        public Color Color
        {
            get { return color; }
            set { color = value; }
        }
    }
}