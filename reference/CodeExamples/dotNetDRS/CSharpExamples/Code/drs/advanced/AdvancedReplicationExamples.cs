using System;
using System.Collections.Generic;
using System.IO;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.CS;
using Db4objects.Db4o.CS.Config;
using Db4objects.Db4o.Ext;
using Db4objects.Drs;
using Db4objects.Drs.Db4o;

namespace Db4oDoc.Drs.Advanced
{
    public class AdvancedReplicationExamples
    {
        public const string DesktopDatabaseName = "desktopDatabase.db4o";
        public const string MobileDatabaseName = "mobileDatabase.db4o";

        private const string UserName = "db4o";
        private const int Port = 4242;
        private const string Host = "localhost";


        public static void Main(string[] args)
        {
            EventExample();

            ReplicationConflicts();
            ReplicationConflictTakeLatestChange();

            ConcurrencyLimitations();

            SimpleMigration();
            MigrationOnTheFly();
        }

        private static void EventExample()
        {
            DeleteDatabases();
            StoreObjectsIn(DesktopDatabaseName);

            IObjectContainer desktopDatabase = OpenDatabase(DesktopDatabaseName);
            IObjectContainer mobileDatabase = OpenDatabase(MobileDatabaseName);

            IReplicationProvider desktopRelicationPartner
                = new Db4oEmbeddedReplicationProvider(desktopDatabase);
            IReplicationProvider mobileRelicationPartner
                = new Db4oEmbeddedReplicationProvider(mobileDatabase);

            // #example: Register a listener for information about the replication process
            IReplicationSession replicationSession 
                = Replication.Begin(desktopRelicationPartner,mobileRelicationPartner,new LogReplicationListener());
            // #end example
            ReplicateBidirectional(replicationSession);

            replicationSession.Commit();
            CloseDBs(desktopDatabase, mobileDatabase);
        }

        private static void ReplicationConflicts()
        {
            DeleteDatabases();

            IObjectContainer desktopDatabase = OpenDatabase(DesktopDatabaseName);
            desktopDatabase.Store(new Pilot("Max"));
            IObjectContainer mobileDatabase = OpenDatabase(MobileDatabaseName);

            ReplicateBidirectional(desktopDatabase, mobileDatabase);


            UpdateObject(desktopDatabase);
            UpdateObject(mobileDatabase);

            IReplicationProvider desktopRelicationPartner = new Db4oEmbeddedReplicationProvider(desktopDatabase);
            IReplicationProvider mobileRelicationPartner = new Db4oEmbeddedReplicationProvider(mobileDatabase);

            // #example: Deal with conflicts
            IReplicationSession replicationSession =
                Replication.Begin(desktopRelicationPartner, mobileRelicationPartner,
                                    new SimpleConflictResolvingListener());
            // #end example

            ReplicateBidirectional(replicationSession);
            replicationSession.Commit();

            CloseDBs(desktopDatabase, mobileDatabase);
        }

        private static void ReplicationConflictTakeLatestChange()
        {
            DeleteDatabases();

            IObjectContainer desktopDatabase = OpenDatabase(DesktopDatabaseName);
            desktopDatabase.Store(new Pilot("Max"));
            IObjectContainer mobileDatabase = OpenDatabase(MobileDatabaseName);

            ReplicateBidirectional(desktopDatabase, mobileDatabase);


            UpdateObject(desktopDatabase);
            UpdateObject(mobileDatabase);

            IReplicationProvider desktopRelicationPartner
                = new Db4oEmbeddedReplicationProvider(desktopDatabase);
            IReplicationProvider mobileRelicationPartner
                = new Db4oEmbeddedReplicationProvider(mobileDatabase);

            // #example: Take latest change
            IReplicationSession replicationSession =
                Replication.Begin(desktopRelicationPartner, mobileRelicationPartner,
                                   new TakeLatestModificationOnConflictListener());
            // #end example

            ReplicateBidirectional(replicationSession);
            replicationSession.Commit();

            CloseDBs(desktopDatabase, mobileDatabase);
        }


        private static void ConcurrencyLimitations()
        {
            DeleteDatabases();

            // #example: Lost replication
            IObjectServer serverDatabase = OpenDatabaseServer(DesktopDatabaseName);
            IObjectContainer mobileDatabase = OpenDatabase(MobileDatabaseName);

            {
                IObjectContainer serverDbConnection = 
                    Db4oClientServer.OpenClient(Host, Port, UserName, UserName);
                serverDbConnection.Store(new Pilot("Pilot 1"));
                serverDbConnection.Commit();

                // The replication starts here
                IObjectContainer connectionForReplication = 
                    Db4oClientServer.OpenClient(Host, Port, UserName, UserName);

                IReplicationProvider clientReplication
                    = new Db4oEmbeddedReplicationProvider(connectionForReplication);
                IReplicationProvider mobileRelicationPartner
                    = new Db4oEmbeddedReplicationProvider(mobileDatabase);

                IReplicationSession replicationSession =
                    Replication.Begin(clientReplication, mobileRelicationPartner);
                IObjectSet changesOnDesktop = 
                    replicationSession.ProviderA().ObjectsChangedSinceLastReplication();

                // during the replication other clients store data on the server
                serverDbConnection.Store(new Pilot("Pilot 2"));
                serverDbConnection.Commit();

                foreach (object changedObjectOnDesktop in changesOnDesktop)
                {
                    replicationSession.Replicate(changedObjectOnDesktop);
                }

                replicationSession.Commit();

                serverDbConnection.Store(new Pilot("Pilot 3"));
                serverDbConnection.Commit();
            }

            // Pilot 2 is not replicated
            PrintPilots(mobileDatabase);


            {
                IObjectContainer connectionForReplication =
                    Db4oClientServer.OpenClient(Host, Port, UserName, UserName);

                IReplicationProvider clientRelicationPartner
                    = new Db4oEmbeddedReplicationProvider(connectionForReplication);
                IReplicationProvider mobileRelicationPartner
                    = new Db4oEmbeddedReplicationProvider(mobileDatabase);

                IReplicationSession replicationSession =
                    Replication.Begin(clientRelicationPartner, mobileRelicationPartner);
                IObjectSet changesOnDesktop = 
                    replicationSession.ProviderA().ObjectsChangedSinceLastReplication();
                foreach (object changedOnDesktop in changesOnDesktop)
                {
                    replicationSession.Replicate(changedOnDesktop);
                }
                replicationSession.Commit();
            }

            // Pilot 2 is still not replicated
            PrintPilots(mobileDatabase);
            // #end example

            serverDatabase.Close();
            mobileDatabase.Close();
        }


        private static void SimpleMigration()
        {
            DeleteDatabases();

            IObjectContainer desktopDatabaseWithoutUUID = Db4oEmbedded.OpenFile(DesktopDatabaseName);
            desktopDatabaseWithoutUUID.Store(new Pilot("Max"));
            desktopDatabaseWithoutUUID.Store(new Pilot("Joe"));
            desktopDatabaseWithoutUUID.Commit();
            desktopDatabaseWithoutUUID.Close();

            IObjectContainer desktopDatabase = OpenDatabase(DesktopDatabaseName);
            IObjectContainer mobileDatabase = OpenDatabase(MobileDatabaseName);

            // #example: Updating all objects ensures that it has a UUID and timestamp
            IList<object> allObjects = desktopDatabase.Query<object>();
            foreach (object objectToUpdate in allObjects)
            {
                desktopDatabase.Store(objectToUpdate);
            }
            desktopDatabase.Commit();
            // #end example

            ReplicateBidirectional(desktopDatabase, mobileDatabase);

            PrintPilots(mobileDatabase);

            CloseDBs(desktopDatabase, mobileDatabase);
        }


        private static void MigrationOnTheFly()
        {
            DeleteDatabases();

            IObjectContainer desktopDatabaseWithoutUUID = Db4oEmbedded.OpenFile(DesktopDatabaseName);
            desktopDatabaseWithoutUUID.Store(new Car(new Pilot("Max"), "Max's Car"));
            desktopDatabaseWithoutUUID.Store(new Car(new Pilot("Joe"), "Joe's Car"));
            desktopDatabaseWithoutUUID.Commit();
            desktopDatabaseWithoutUUID.Close();

            IObjectContainer desktopDatabase = OpenDatabase(DesktopDatabaseName);
            IObjectContainer mobileDatabase = OpenDatabase(MobileDatabaseName);

            IReplicationProvider desktopRelicationPartner
                = new Db4oEmbeddedReplicationProvider(desktopDatabase);
            IReplicationProvider mobileRelicationPartner
                = new Db4oEmbeddedReplicationProvider(mobileDatabase);

            // #example: Migrate on the fly
            IReplicationSession replicationSession = Replication.Begin(desktopRelicationPartner, mobileRelicationPartner);
            IList<Car> initialReplication = desktopDatabase.Query<Car>();

            foreach (Car changedObjectOnDesktop in initialReplication)
            {
                IObjectInfo infoAboutObject = desktopDatabase.Ext().GetObjectInfo(changedObjectOnDesktop);
                if (null == infoAboutObject.GetUUID())
                {
                    desktopDatabase.Ext().Store(changedObjectOnDesktop, 2);
                }
                replicationSession.Replicate(changedObjectOnDesktop);
            }
            replicationSession.Commit();
            // #end example

            PrintCars(mobileDatabase);

            CloseDBs(desktopDatabase, mobileDatabase);
        }


        private static void UpdateObject(IObjectContainer desktopDatabase)
        {
            Pilot pilotOnDesktop = desktopDatabase.Query<Pilot>()[0];
            pilotOnDesktop.Points = 200;
            desktopDatabase.Store(pilotOnDesktop);
            desktopDatabase.Commit();
        }


        private static void ReplicateBidirectional(IObjectContainer desktopDatabase, IObjectContainer mobileDatabase)
        {
            IReplicationProvider desktopRelicationPartner
                = new Db4oEmbeddedReplicationProvider(desktopDatabase);
            IReplicationProvider mobileRelicationPartner
                = new Db4oEmbeddedReplicationProvider(mobileDatabase);

            IReplicationSession replicationSession 
                = Replication.Begin(desktopRelicationPartner,mobileRelicationPartner);
            ReplicateBidirectional(replicationSession);
            replicationSession.Commit();
        }


        private static void ReplicateBidirectional(IReplicationSession replication)
        {
            IObjectSet changesOnDesktop = replication.ProviderA().ObjectsChangedSinceLastReplication();
            IObjectSet changesOnMobile = replication.ProviderB().ObjectsChangedSinceLastReplication();

            foreach (object changedObjectOnDesktop in changesOnDesktop)
            {
                replication.Replicate(changedObjectOnDesktop);
            }

            foreach (object changedObjectOnMobile in changesOnMobile)
            {
                replication.Replicate(changedObjectOnMobile);
            }
        }

        private static IObjectContainer OpenDatabase(string fileName)
        {
            IEmbeddedConfiguration configuration = Db4oEmbedded.NewConfiguration();
            configuration.File.GenerateUUIDs = ConfigScope.Globally;
            configuration.File.GenerateCommitTimestamps = true;
            return Db4oEmbedded.OpenFile(configuration, fileName);
        }

        private static IObjectServer OpenDatabaseServer(string fileName)
        {
            IServerConfiguration configuration = Db4oClientServer.NewServerConfiguration();
            configuration.File.GenerateUUIDs = ConfigScope.Globally;
            configuration.File.GenerateCommitTimestamps = true;
            IObjectServer srv = Db4oClientServer.OpenServer(configuration, fileName, Port);
            srv.GrantAccess(UserName, UserName);
            return srv;
        }

        private static void StoreObjectsIn(string databaseFile)
        {
            using (IObjectContainer container = OpenDatabase(databaseFile)){
                Pilot john = new Pilot("John", 100);
                Car johnsCar = new Car(john, "John's Car");
                container.Store(johnsCar);
                Pilot max = new Pilot("Max", 200);
                Car maxsCar = new Car(max, "Max's Car");
                container.Store(maxsCar);
                container.Commit();
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


        private static void CloseDBs(params IObjectContainer[] databases)
        {
            foreach (IObjectContainer db in databases)
            {
                db.Dispose();
            }
        }


        private static void DeleteDatabases()
        {
            File.Delete(DesktopDatabaseName);
            File.Delete(MobileDatabaseName);
        }


        // #example: The ReplicationEventListener for informations about the replication process
        class LogReplicationListener : IReplicationEventListener
        {
            public void OnReplicate(IReplicationEvent replicationEvent)
            {
                IObjectState stateInDesktop = replicationEvent.StateInProviderA();
                if (stateInDesktop.IsNew())
                {
                    Console.WriteLine("Object '{0}' is new on desktop database",
                        stateInDesktop.GetObject());
                }
                if (stateInDesktop.WasModified())
                {
                    Console.WriteLine("Object '{0}' was modified on desktop database", 
                        stateInDesktop.GetObject());
                }
            }
        }
        // #end example


        // #example: Conflict resolving listener
        class SimpleConflictResolvingListener : IReplicationEventListener
        {
            public void OnReplicate(IReplicationEvent replicationEvent)
            {
                if (replicationEvent.IsConflict())
                {
                    IObjectState stateOfTheDesktop = replicationEvent.StateInProviderA();
                    replicationEvent.OverrideWith(stateOfTheDesktop);
                }
            }
        }
        // #end example


        // #example: Listener which takes latest changes
        class TakeLatestModificationOnConflictListener : IReplicationEventListener
        {
            public void OnReplicate(IReplicationEvent replicationEvent)
            {
                if (replicationEvent.IsConflict())
                {
                    IObjectState stateOfTheDesktop = replicationEvent.StateInProviderA();
                    IObjectState stateOfTheMobile = replicationEvent.StateInProviderB();

                    if (stateOfTheDesktop.ModificationDate() >= stateOfTheMobile.ModificationDate())
                    {
                        replicationEvent.OverrideWith(stateOfTheDesktop);
                    }
                    else
                    {
                        replicationEvent.OverrideWith(stateOfTheMobile);
                    }
                }
            }
        }
        // #end example
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