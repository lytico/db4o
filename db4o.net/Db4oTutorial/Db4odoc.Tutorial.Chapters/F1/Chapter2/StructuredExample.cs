using System;
using System.IO;

using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Query;

namespace Db4odoc.Tutorial.F1.Chapter2
{
    public class StructuredExample : Util
    {
        readonly static string YapFileName = Path.Combine(
                               Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                               "formula1.yap");  
		
        public static void Main(String[] args)
        {
            File.Delete(YapFileName);

            using(IObjectContainer db = Db4oEmbedded.OpenFile(YapFileName))
            {
                StoreFirstCar(db);
                StoreSecondCar(db);
                RetrieveAllCarsQBE(db);
                RetrieveAllPilotsQBE(db);
                RetrieveCarByPilotQBE(db);
                RetrieveCarByPilotNameQuery(db);
                RetrieveCarByPilotProtoQuery(db);
                RetrievePilotByCarModelQuery(db);
                UpdateCar(db);
                UpdatePilotSingleSession(db);
                UpdatePilotSeparateSessionsPart1(db);
            }
            using(IObjectContainer db = Db4oEmbedded.OpenFile(YapFileName))
            {
                UpdatePilotSeparateSessionsPart2(db);
            }
            UpdatePilotSeparateSessionsImprovedPart1();
            using(IObjectContainer db = Db4oEmbedded.OpenFile(YapFileName))
            {
                UpdatePilotSeparateSessionsImprovedPart2(db);
                DeleteFlat(db);
            }
            DeleteDeep();
            DeleteDeepRevisited();
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
            db.Store(pilot2);
            Car car2 = new Car("BMW");
            car2.Pilot = pilot2;
            db.Store(car2);
        }

        public static void RetrieveAllCarsQBE(IObjectContainer db)
        {
            Car proto = new Car(null);
            IObjectSet result = db.QueryByExample(proto);
            ListResult(result);
        }

        public static void RetrieveAllPilotsQBE(IObjectContainer db)
        {
            Pilot proto = new Pilot(null, 0);
            IObjectSet result = db.QueryByExample(proto);
            ListResult(result);
        }

        public static void RetrieveCarByPilotQBE(IObjectContainer db)
        {
            Pilot pilotproto = new Pilot("Rubens Barrichello", 0);
            Car carproto = new Car(null);
            carproto.Pilot = pilotproto;
            IObjectSet result = db.QueryByExample(carproto);
            ListResult(result);
        }

        public static void RetrieveCarByPilotNameQuery(IObjectContainer db)
        {
            IQuery query = db.Query();
            query.Constrain(typeof(Car));
            query.Descend("_pilot").Descend("_name")
                .Constrain("Rubens Barrichello");
            IObjectSet result = query.Execute();
            ListResult(result);
        }

        public static void RetrieveCarByPilotProtoQuery(IObjectContainer db)
        {
            IQuery query = db.Query();
            query.Constrain(typeof(Car));
            Pilot proto = new Pilot("Rubens Barrichello", 0);
            query.Descend("_pilot").Constrain(proto);
            IObjectSet result = query.Execute();
            ListResult(result);
        }

        public static void RetrievePilotByCarModelQuery(IObjectContainer db)
        {
            IQuery carQuery = db.Query();
            carQuery.Constrain(typeof(Car));
            carQuery.Descend("_model").Constrain("Ferrari");
            IQuery pilotQuery = carQuery.Descend("_pilot");
            IObjectSet result = pilotQuery.Execute();
            ListResult(result);
        }

        public static void RetrieveAllPilots(IObjectContainer db)
        {
            IObjectSet results = db.QueryByExample(typeof(Pilot));
            ListResult(results);
        }

        public static void RetrieveAllCars(IObjectContainer db)
        {
            IObjectSet results = db.QueryByExample(typeof(Car));
            ListResult(results);
        }

        public class RetrieveCarsByPilotNamePredicate : Predicate
        {
            readonly string _pilotName;

            public RetrieveCarsByPilotNamePredicate(string pilotName)
            {
                _pilotName = pilotName;
            }

            public bool Match(Car candidate)
            {
                return candidate.Pilot.Name == _pilotName;
            }
        }

        public static void RetrieveCarsByPilotNameNative(IObjectContainer db)
        {
            string pilotName = "Rubens Barrichello";
            IObjectSet results = db.Query(new RetrieveCarsByPilotNamePredicate(pilotName));
            ListResult(results);
        }

        public static void UpdateCar(IObjectContainer db)
        {
            IObjectSet result = db.QueryByExample(new Car("Ferrari"));
            Car found = (Car)result.Next();
            found.Pilot = new Pilot("Somebody else", 0);
            db.Store(found);
            result = db.QueryByExample(new Car("Ferrari"));
            ListResult(result);
        }

        public static void UpdatePilotSingleSession(IObjectContainer db)
        {
            IObjectSet result = db.QueryByExample(new Car("Ferrari"));
            Car found = (Car)result.Next();
            found.Pilot.AddPoints(1);
            db.Store(found);
            result = db.QueryByExample(new Car("Ferrari"));
            ListResult(result);
        }

        public static void UpdatePilotSeparateSessionsPart1(IObjectContainer db)
        {
            IObjectSet result = db.QueryByExample(new Car("Ferrari"));
            Car found = (Car)result.Next();
            found.Pilot.AddPoints(1);
            db.Store(found);
        }

        public static void UpdatePilotSeparateSessionsPart2(IObjectContainer db)
        {
            IObjectSet result = db.QueryByExample(new Car("Ferrari"));
            ListResult(result);
        }

        public static void UpdatePilotSeparateSessionsImprovedPart1()
        {
            IEmbeddedConfiguration config = Db4oEmbedded.NewConfiguration();
            config.Common.ObjectClass(typeof(Car)).CascadeOnUpdate(true);
            using(IObjectContainer db = Db4oEmbedded.OpenFile(config, YapFileName))
            {
                IObjectSet result = db.QueryByExample(new Car("Ferrari"));
                Car found = (Car)result.Next();
                found.Pilot.AddPoints(1);
                db.Store(found);
            }
        }

        public static void UpdatePilotSeparateSessionsImprovedPart2(IObjectContainer db)
        {
            IObjectSet result = db.QueryByExample(new Car("Ferrari"));
            ListResult(result);
        }

        public static void DeleteFlat(IObjectContainer db)
        {
            IObjectSet result = db.QueryByExample(new Car("Ferrari"));
            Car found = (Car)result.Next();
            db.Delete(found);
            result = db.QueryByExample(new Car(null));
            ListResult(result);
        }

        public static void DeleteDeep()
        {
            IEmbeddedConfiguration config = Db4oEmbedded.NewConfiguration();
            config.Common.ObjectClass(typeof(Car)).CascadeOnDelete(true);
            using(IObjectContainer db = Db4oEmbedded.OpenFile(config, YapFileName)){
                IObjectSet result = db.QueryByExample(new Car("BMW"));
                Car found = (Car)result.Next();
                db.Delete(found);
                result = db.QueryByExample(new Car(null));
                ListResult(result);
            }
        }

        public static void DeleteDeepRevisited()
        {
            IEmbeddedConfiguration config = Db4oEmbedded.NewConfiguration();
            config.Common.ObjectClass(typeof(Car)).CascadeOnDelete(true);
            using(IObjectContainer db = Db4oEmbedded.OpenFile(config, YapFileName))
            {
                IObjectSet result = db.QueryByExample(new Pilot("Michael Schumacher", 0));
                Pilot pilot = (Pilot)result.Next();
                Car car1 = new Car("Ferrari");
                Car car2 = new Car("BMW");
                car1.Pilot = pilot;
                car2.Pilot = pilot;
                db.Store(car1);
                db.Store(car2);
                db.Delete(car2);
                result = db.QueryByExample(new Car(null));
                ListResult(result);
            }
        }
    }
}
