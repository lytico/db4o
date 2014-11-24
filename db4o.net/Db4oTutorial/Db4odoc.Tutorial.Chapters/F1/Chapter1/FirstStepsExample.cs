using System;
using System.IO;
using Db4objects.Db4o;
using Db4objects.Db4o.Query;
using Db4odoc.Tutorial;

namespace Db4odoc.Tutorial.F1.Chapter1
{
    public class FirstStepsExample : Util
    {
        readonly static string YapFileName = Path.Combine(
                               Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                               "formula1.yap");

        public static void Main(string[] args)
        {
            File.Delete(YapFileName);
            AccessDb4o();
            File.Delete(YapFileName);
            using(IObjectContainer db = Db4oEmbedded.OpenFile(YapFileName))
            {
                StoreFirstPilot(db);
                StoreSecondPilot(db);
                RetrieveAllPilots(db);
                RetrievePilotByName(db);
                RetrievePilotByExactPoints(db);
                UpdatePilot(db);
                DeleteFirstPilotByName(db);
                DeleteSecondPilotByName(db);
            }
        }

        public static void AccessDb4o()
        {
            using(IObjectContainer db = Db4oEmbedded.OpenFile(YapFileName))
            {
                // do something with db4o
            }
        }

        public static void StoreFirstPilot(IObjectContainer db)
        {
            Pilot pilot1 = new Pilot("Michael Schumacher", 100);
            db.Store(pilot1);
            Console.WriteLine("Stored {0}", pilot1);
        }

        public static void StoreSecondPilot(IObjectContainer db)
        {
            Pilot pilot2 = new Pilot("Rubens Barrichello", 99);
            db.Store(pilot2);
            Console.WriteLine("Stored {0}", pilot2);
        }

        public static void RetrieveAllPilotQBE(IObjectContainer db)
        {
            Pilot proto = new Pilot(null, 0);
            IObjectSet result = db.QueryByExample(proto);
            ListResult(result);
        }

        public static void RetrieveAllPilots(IObjectContainer db)
        {
            IObjectSet result = db.QueryByExample(typeof(Pilot));
            ListResult(result);
        }

        public static void RetrievePilotByName(IObjectContainer db)
        {
            Pilot proto = new Pilot("Michael Schumacher", 0);
            IObjectSet result = db.QueryByExample(proto);
            ListResult(result);
        }

        public static void RetrievePilotByExactPoints(IObjectContainer db)
        {
            Pilot proto = new Pilot(null, 100);
            IObjectSet result = db.QueryByExample(proto);
            ListResult(result);
        }

        public static void UpdatePilot(IObjectContainer db)
        {
            IObjectSet result = db.QueryByExample(new Pilot("Michael Schumacher", 0));
            Pilot found = (Pilot)result.Next();
            found.AddPoints(11);
            db.Store(found);
            Console.WriteLine("Added 11 points for {0}", found);
            RetrieveAllPilots(db);
        }

        public static void DeleteFirstPilotByName(IObjectContainer db)
        {
            IObjectSet result = db.QueryByExample(new Pilot("Michael Schumacher", 0));
            Pilot found = (Pilot)result.Next();
            db.Delete(found);
            Console.WriteLine("Deleted {0}", found);
            RetrieveAllPilots(db);
        }

        public static void DeleteSecondPilotByName(IObjectContainer db)
        {
            IObjectSet result = db.QueryByExample(new Pilot("Rubens Barrichello", 0));
            Pilot found = (Pilot)result.Next();
            db.Delete(found);
            Console.WriteLine("Deleted {0}", found);
            RetrieveAllPilots(db);
        }
    }
}
