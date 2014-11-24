using System;
using System.IO;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.TA;
using Db4odoc.Tutorial.F1;

namespace Db4odoc.Tutorial.F1.Chapter9
{
    public class TransparentPersistenceExample : Util
    {
        readonly static string YapFileName = Path.Combine(
                               Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                               "formula1.yap");  
		
        public static void Main(String[] args)
        {
            File.Delete(YapFileName);
            StoreCarAndSnapshots();
            ModifySnapshotHistory();
            ReadSnapshotHistory();

        }

        public static void StoreCarAndSnapshots()
        {
            IEmbeddedConfiguration config = Db4oEmbedded.NewConfiguration();
            config.Common.Add(new TransparentPersistenceSupport());
            using(IObjectContainer db = Db4oEmbedded.OpenFile(config, YapFileName))
            {
                Car car = new Car("Ferrari");
                for (int i = 0; i < 3; i++)
                {
                    car.snapshot();
                }
                db.Store(car);
            }
        }

        public static void ModifySnapshotHistory()
        {
            IEmbeddedConfiguration config = Db4oEmbedded.NewConfiguration();
            config.Common.Add(new TransparentPersistenceSupport());
            using(IObjectContainer db = Db4oEmbedded.OpenFile(config, YapFileName))
            {
                System.Console.WriteLine("Read all sensors and modify the description:");
                IObjectSet result = db.QueryByExample(typeof(Car));
                Car car = (Car)result.Next();
                SensorReadout readout = car.History;
                while (readout != null)
                {
                    System.Console.WriteLine(readout);
                    readout.Description = "Modified: " + readout.Description;
                    readout = readout.Next;
                }
                db.Commit();
            }
        }

        public static void ReadSnapshotHistory()
        {
            IEmbeddedConfiguration config = Db4oEmbedded.NewConfiguration();
            config.Common.Add(new TransparentPersistenceSupport());
            using(IObjectContainer db = Db4oEmbedded.OpenFile(config, YapFileName))
            {
                System.Console.WriteLine("Read all modified sensors:");
                IObjectSet result = db.QueryByExample(typeof(Car));
                Car car = (Car)result.Next();
                SensorReadout readout = car.History;
                while (readout != null)
                {
                    System.Console.WriteLine(readout);
                    readout = readout.Next;
                }
            }
        }
    }
}