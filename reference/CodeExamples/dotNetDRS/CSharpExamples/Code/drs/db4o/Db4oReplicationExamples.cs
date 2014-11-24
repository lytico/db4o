using System;
using System.Collections.Generic;
using System.IO;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Drs;
using Db4objects.Drs.Db4o;

namespace Db4oDoc.Drs.Db4o
{
    public class Db4oReplicationExamples
    {
        public const string DesktopDatabaseName = "desktopDatabase.db4o";
        public const string MobileDatabaseName = "mobileDatabase.db4o";

        public static void Main(string[] args)
        {
            OneWayReplicationExample();

            BiDirectionalReplicationExample();

            SelectiveReplicationByClass();
            SelectiveReplicationWithCondition();
            SelectiveReplicationWithQuery();

            DeletionsReplication();
        }

        private static void OneWayReplicationExample()
        {
            DeleteDatabases();
            StoreObjectsIn(DesktopDatabaseName);

            //#example: Prepare unidirectional replication
            IObjectContainer desktopDatabase = OpenDatabase(DesktopDatabaseName);
            IObjectContainer mobileDatabase = OpenDatabase(MobileDatabaseName);

            IReplicationProvider dektopReplicationProvider
                = new Db4oEmbeddedReplicationProvider(desktopDatabase);
            IReplicationProvider mobileReplicationProvider
                = new Db4oEmbeddedReplicationProvider(mobileDatabase);

            IReplicationSession replicationSession 
                = Replication.Begin(dektopReplicationProvider, mobileReplicationProvider);
            // set the replication-direction from the desktop database to the mobile database. 
            replicationSession.SetDirection(replicationSession.ProviderA(), replicationSession.ProviderB());
            //#end example

            //#example: One direction replication
            IObjectSet changes = replicationSession.ProviderA().ObjectsChangedSinceLastReplication();
            foreach (object changedObject in changes)
            {
                replicationSession.Replicate(changedObject);
            }
            replicationSession.Commit();
            //#end example

            PrintCars(mobileDatabase);

            CloseDBs(desktopDatabase, mobileDatabase);
        }

        private static void BiDirectionalReplicationExample()
        {
            DeleteDatabases();
            StoreObjectsIn(DesktopDatabaseName);
            StoreObjectsIn(MobileDatabaseName);

            // #example: Prepare bidirectional replication
            IObjectContainer desktopDatabase = OpenDatabase(DesktopDatabaseName);
            IObjectContainer mobileDatabase = OpenDatabase(MobileDatabaseName);

            IReplicationProvider dektopReplicationProvider
                = new Db4oEmbeddedReplicationProvider(desktopDatabase);
            IReplicationProvider mobileReplicationProvider
                = new Db4oEmbeddedReplicationProvider(mobileDatabase);

            IReplicationSession replicationSession
                = Replication.Begin(dektopReplicationProvider, mobileReplicationProvider);
            // #end example

            //#example: Bidirectional replication
            // First get the changes of the two replication-partners
            IObjectSet changesOnDesktop = replicationSession.ProviderA().ObjectsChangedSinceLastReplication();
            IObjectSet changesOnMobile = replicationSession.ProviderB().ObjectsChangedSinceLastReplication();

            // then iterate over both change-sets and replicate it
            foreach (object changedObjectOnDesktop in changesOnDesktop)
            {
                replicationSession.Replicate(changedObjectOnDesktop);
            }

            foreach (object changedObjectOnMobile in changesOnMobile)
            {
                replicationSession.Replicate(changedObjectOnMobile);
            }

            replicationSession.Commit();
            //#end example

            PrintCars(mobileDatabase);
            PrintCars(desktopDatabase);

            CloseDBs(desktopDatabase, mobileDatabase);
        }

        private static void SelectiveReplicationByClass()
        {
            DeleteDatabases();
            StoreObjectsIn(DesktopDatabaseName);

            IObjectContainer desktopDatabase = OpenDatabase(DesktopDatabaseName);
            IObjectContainer mobileDatabase = OpenDatabase(MobileDatabaseName);

            IReplicationProvider dektopReplicationProvider
                = new Db4oEmbeddedReplicationProvider(desktopDatabase);
            IReplicationProvider mobileReplicationProvider
                = new Db4oEmbeddedReplicationProvider(mobileDatabase);

            IReplicationSession replicationSession
                = Replication.Begin(dektopReplicationProvider, mobileReplicationProvider);

            // #example: Selective replication by class
            IObjectSet changesOnDesktop = 
                replicationSession.ProviderA().ObjectsChangedSinceLastReplication(typeof (Pilot));

            foreach (object changedObjectOnDesktop in changesOnDesktop)
            {
                replicationSession.Replicate(changedObjectOnDesktop);
            }

            replicationSession.Commit();
            // #end example

            // the car's aren't replicated, only the pilots
            PrintCars(mobileDatabase);
            PrintPilots(mobileDatabase);

            CloseDBs(desktopDatabase, mobileDatabase);
        }

        private static void SelectiveReplicationWithCondition()
        {
            DeleteDatabases();
            StoreObjectsIn(DesktopDatabaseName);

            IObjectContainer desktopDatabase = OpenDatabase(DesktopDatabaseName);
            IObjectContainer mobileDatabase = OpenDatabase(MobileDatabaseName);

            IReplicationProvider dektopReplicationProvider
                = new Db4oEmbeddedReplicationProvider(desktopDatabase);
            IReplicationProvider mobileReplicationProvider
                = new Db4oEmbeddedReplicationProvider(mobileDatabase);

            IReplicationSession replicationSession
                = Replication.Begin(dektopReplicationProvider, mobileReplicationProvider);

            // #example: Selective replication with a condition
            IObjectSet changesOnDesktop = replicationSession.ProviderA().ObjectsChangedSinceLastReplication();

            foreach (object changedObjectOnDesktop in changesOnDesktop)
            {
                if (changedObjectOnDesktop is Car)
                {
                    if (((Car) changedObjectOnDesktop).Name.StartsWith("M"))
                    {
                        replicationSession.Replicate(changedObjectOnDesktop);
                    }
                }
            }

            replicationSession.Commit();
            // #end example

            // now only the cars which names start with "M" are replicated
            PrintCars(mobileDatabase);

            CloseDBs(desktopDatabase, mobileDatabase);
        }

        private static void SelectiveReplicationWithQuery()
        {
            DeleteDatabases();
            StoreObjectsIn(DesktopDatabaseName);

            IObjectContainer desktopDatabase = OpenDatabase(DesktopDatabaseName);
            IObjectContainer mobileDatabase = OpenDatabase(MobileDatabaseName);

            IReplicationProvider dektopReplicationProvider
                = new Db4oEmbeddedReplicationProvider(desktopDatabase);
            IReplicationProvider mobileReplicationProvider
                = new Db4oEmbeddedReplicationProvider(mobileDatabase);

            IReplicationSession replicationSession
                = Replication.Begin(dektopReplicationProvider, mobileReplicationProvider);

            // #example: Selective replication with a query
            IList<Car> changesOnDesktop = 
                desktopDatabase.Query(delegate(Car car) { return car.Name.StartsWith("M"); });

            foreach (Car changedObjectOnDesktop  in changesOnDesktop)
            {
                replicationSession.Replicate(changedObjectOnDesktop);
            }

            replicationSession.Commit();
            // #end example

            // now only the cars which names start with "M" are replicated
            PrintCars(mobileDatabase);

            CloseDBs(desktopDatabase, mobileDatabase);
        }

        private static void DeletionsReplication()
        {
            DeleteDatabases();
            StoreObjectsIn(DesktopDatabaseName);
            IObjectContainer desktopDatabase = OpenDatabase(DesktopDatabaseName);
            IObjectContainer mobileDatabase = OpenDatabase(MobileDatabaseName);

            Replicate(desktopDatabase, mobileDatabase);

            Car carToDelete = desktopDatabase.Query<Car>()[0];
            desktopDatabase.Delete(carToDelete);
            desktopDatabase.Commit();

            PrintCars(mobileDatabase);

            IReplicationProvider dektopReplicationProvider
                = new Db4oEmbeddedReplicationProvider(desktopDatabase);
            IReplicationProvider mobileReplicationProvider
                = new Db4oEmbeddedReplicationProvider(mobileDatabase);

            // #example: Replicate deletions
            IReplicationSession replicationSession
                = Replication.Begin(dektopReplicationProvider, mobileReplicationProvider);

            replicationSession.ReplicateDeletions(typeof (Car));
            replicationSession.Commit();
            // #end example

            PrintCars(mobileDatabase);

            CloseDBs(desktopDatabase, mobileDatabase);
        }

        private static void Replicate(IObjectContainer desktopDatabase, IObjectContainer mobileDatabase)
        {
            IReplicationProvider dektopReplicationProvider
                = new Db4oEmbeddedReplicationProvider(desktopDatabase);
            IReplicationProvider mobileReplicationProvider
                = new Db4oEmbeddedReplicationProvider(mobileDatabase);

            IReplicationSession replicationSession
                = Replication.Begin(dektopReplicationProvider, mobileReplicationProvider);
            ReplicateChanges(replicationSession, replicationSession.ProviderA());
            ReplicateChanges(replicationSession, replicationSession.ProviderB());
            replicationSession.Commit();
        }

        private static void ReplicateChanges(IReplicationSession replication, IReplicationProvider provider)
        {
            IObjectSet changes = provider.ObjectsChangedSinceLastReplication();
            foreach (object changedObject in changes)
            {
                replication.Replicate(changedObject);
            }
        }

        private static void CloseDBs(params IObjectContainer[] databases)
        {
            foreach (IObjectContainer db in databases)
            {
                db.Dispose();
            }
        }

        private static void PrintCars(IObjectContainer database)
        {
            IList<Car> cars = database.Query<Car>();
            foreach (Car car in cars)
            {
                Console.WriteLine(car);
            }
        }

        private static void PrintPilots(IObjectContainer database)
        {
            IList<Pilot> pilots = database.Query<Pilot>();
            foreach (Pilot pilot in pilots)
            {
                Console.WriteLine(pilot);
            }
        }


        private static void StoreObjectsIn(string databaseFile)
        {
            using (IObjectContainer db = OpenDatabase(databaseFile))
            {
                Pilot john = new Pilot("John", 100);
                Car johnsCar = new Car(john, "John's Car");
                db.Store(johnsCar);
                Pilot max = new Pilot("Max", 200);
                Car maxsCar = new Car(max, "Max's Car");
                db.Store(maxsCar);
                db.Commit();
            }
        }

        private static IObjectContainer OpenDatabase(string fileName)
        {
            IEmbeddedConfiguration configuration = Db4oEmbedded.NewConfiguration();
            // #example: Configure db4o to generate UUIDs and commit timestamps
            configuration.File.GenerateUUIDs = ConfigScope.Globally;
            configuration.File.GenerateCommitTimestamps = true;
            // #end example
            return Db4oEmbedded.OpenFile(configuration, fileName);
        }

        private static void DeleteDatabases()
        {
            File.Delete(DesktopDatabaseName);
            File.Delete(MobileDatabaseName);
        }
    }

    public class Pilot
    {
        private string name;
        private int points;

        public Pilot(string name)
        {
            this.name = name;
        }

        public Pilot(string name, int points)
        {
            this.name = name;
            this.points = points;
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public int Points
        {
            get { return points; }
            set { points = value; }
        }

        public override string ToString()
        {
            return name;
        }
    }

    public class Car
    {
        private Pilot pilot;
        private string name;

        public Car(Pilot pilot, string name)
        {
            this.pilot = pilot;
            this.name = name;
        }

        public Pilot Pilot
        {
            get { return pilot; }
            set { pilot = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public override string ToString()
        {
            return name + " pilot: " + pilot;
        }
    }
}