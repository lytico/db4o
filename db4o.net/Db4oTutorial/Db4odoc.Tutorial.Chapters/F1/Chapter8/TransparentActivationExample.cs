using System;
using System.IO;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.TA;
using Db4odoc.Tutorial.F1;

namespace Db4odoc.Tutorial.F1.Chapter8
{
    public class TransparentActivationExample : Util
    {
        readonly static string YapFileName = Path.Combine(
                               Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                               "formula1.yap");  
		
        public static void Main(String[] args)
        {
            File.Delete(YapFileName);
            using(IObjectContainer db = Db4oEmbedded.OpenFile(YapFileName))
            {
                StoreCarAndSnapshots(db);
            }

            using(IObjectContainer db = Db4oEmbedded.OpenFile(YapFileName))
            {
                RetrieveSnapshotsSequentially(db);
            }

            RetrieveSnapshotsSequentiallyTA();

            DemonstrateTransparentActivation();
        }


        public static void StoreCarAndSnapshots(IObjectContainer db)
        {
            Pilot pilot = new Pilot("Kimi Raikkonen", 110);
            Car car = new Car("Ferrari");
            car.Pilot = pilot;
            for (int i = 0; i < 5; i++)
            {
                car.snapshot();
            }
            db.Store(car);
        }

        public static void RetrieveSnapshotsSequentially(IObjectContainer db)
        {
            IObjectSet result = db.QueryByExample(typeof (Car));
            Car car = (Car) result.Next();
            SensorReadout readout = car.History;
            while (readout != null)
            {
                Console.WriteLine(readout);
                readout = readout.Next;
            }
        }

        public static void RetrieveSnapshotsSequentiallyTA()
        {
            IEmbeddedConfiguration config = Db4oEmbedded.NewConfiguration();
            config.Common.Add(new TransparentActivationSupport());
            using(IObjectContainer db = Db4oEmbedded.OpenFile(config, YapFileName))
            {
                IObjectSet result = db.QueryByExample(typeof(Car));
                Car car = (Car)result.Next();
                SensorReadout readout = car.History;
                while (readout != null)
                {
                    Console.WriteLine(readout);
                    readout = readout.Next;
                }
            }
        }

        public static void DemonstrateTransparentActivation()
        {
            IEmbeddedConfiguration config = Db4oEmbedded.NewConfiguration();
            config.Common.Add(new TransparentActivationSupport());
            
            using(IObjectContainer db = Db4oEmbedded.OpenFile(config, YapFileName)){
                IObjectSet result = db.QueryByExample(typeof (Car));
                Car car = (Car) result.Next();

                Console.WriteLine("#PilotWithoutActivation before the car is activated");
                Console.WriteLine(car.PilotWithoutActivation);

                Console.WriteLine("accessing 'Pilot' property activates the car object");
                Console.WriteLine(car.Pilot);

                Console.WriteLine("Accessing PilotWithoutActivation property after the car is activated");
                Console.WriteLine(car.PilotWithoutActivation);

            }
        }
    }
}