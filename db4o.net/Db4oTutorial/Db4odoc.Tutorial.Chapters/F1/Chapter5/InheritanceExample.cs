using System;
using System.IO;
using Db4objects.Db4o;

using Db4objects.Db4o.Query;

namespace Db4odoc.Tutorial.F1.Chapter5
{   
    public class InheritanceExample : Util
    {
        readonly static string YapFileName = Path.Combine(
                               Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                               "formula1.yap");  
		
        public static void Main(string[] args)
        {
            File.Delete(YapFileName);
            using(IObjectContainer db = Db4oEmbedded.OpenFile(YapFileName))
            {
                StoreFirstCar(db);
                StoreSecondCar(db);
                RetrieveTemperatureReadoutsQBE(db);
                RetrieveAllSensorReadoutsQBE(db);
                RetrieveAllSensorReadoutsQBEAlternative(db);
                RetrieveAllSensorReadoutsQuery(db);
                RetrieveAllObjects(db);
            }
        }
        
        public static void StoreFirstCar(IObjectContainer db)
        {
            Car car1 = new Car("Ferrari");
            Pilot pilot1 = new Pilot("Michael Schumacher", 100);
            car1.Pilot = pilot1;
            db.Store(car1);
        }
        
        public static void StoreSecondCar(IObjectContainer db)
        {
            Pilot pilot2 = new Pilot("Rubens Barrichello", 99);
            Car car2 = new Car("BMW");
            car2.Pilot = pilot2;
            car2.Snapshot();
            car2.Snapshot();
            db.Store(car2);
        }
        
        public static void RetrieveAllSensorReadoutsQBE(IObjectContainer db)
        {
            SensorReadout proto = new SensorReadout(DateTime.MinValue, null, null);
            IObjectSet result = db.QueryByExample(proto);
            ListResult(result);
        }
        
        public static void RetrieveTemperatureReadoutsQBE(IObjectContainer db)
        {
            SensorReadout proto = new TemperatureSensorReadout(DateTime.MinValue, null, null, 0.0);
            IObjectSet result = db.QueryByExample(proto);
            ListResult(result);
        }
        
        public static void RetrieveAllSensorReadoutsQBEAlternative(IObjectContainer db)
        {
            IObjectSet result = db.QueryByExample(typeof(SensorReadout));
            ListResult(result);
        }
        
        public static void RetrieveAllSensorReadoutsQuery(IObjectContainer db)
        {
            IQuery query = db.Query();
            query.Constrain(typeof(SensorReadout));
            IObjectSet result = query.Execute();
            ListResult(result);
        }
        
        public static void RetrieveAllObjects(IObjectContainer db)
        {
            IObjectSet result = db.QueryByExample(new object());
            ListResult(result);
        }
    }
}
