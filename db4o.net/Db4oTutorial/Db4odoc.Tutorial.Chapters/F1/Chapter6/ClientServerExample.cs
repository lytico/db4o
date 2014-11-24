using System;
using System.IO;
using Db4objects.Db4o;
using Db4objects.Db4o.CS;
using Db4objects.Db4o.CS.Config;

namespace Db4odoc.Tutorial.F1.Chapter6
{
    public class ClientServerExample : Util
    {
        readonly static string YapFileName = Path.Combine(
                               Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                               "formula1.yap");

        readonly static int ServerPort = 0xdb40;

        readonly static string ServerUser = "user";

        readonly static string ServerPassword = "password";

        public static void Main(string[] args)
        {
            File.Delete(YapFileName);
            AccessLocalServer();
            File.Delete(YapFileName);
            using(IObjectContainer db = Db4oEmbedded.OpenFile(YapFileName))
            {
                SetFirstCar(db);
                SetSecondCar(db);
            }

            IServerConfiguration config = Db4oClientServer.NewServerConfiguration();
            config.Common.ObjectClass(typeof(Car)).UpdateDepth(3);
            using(IObjectServer server = Db4oClientServer.OpenServer(config,
                YapFileName, 0))
            {
                QueryLocalServer(server);
                DemonstrateLocalReadCommitted(server);
                DemonstrateLocalRollback(server);
            }
            
            AccessRemoteServer();
            using(IObjectServer server  = Db4oClientServer.OpenServer(Db4oClientServer.NewServerConfiguration(),
                YapFileName, ServerPort))
            {
                server.GrantAccess(ServerUser, ServerPassword);
                QueryRemoteServer(ServerPort, ServerUser, ServerPassword);
                DemonstrateRemoteReadCommitted(ServerPort, ServerUser, ServerPassword);
                DemonstrateRemoteRollback(ServerPort, ServerUser, ServerPassword);
            }
        }
            
        public static void SetFirstCar(IObjectContainer db)
        {
            Pilot pilot = new Pilot("Rubens Barrichello", 99);
            Car car = new Car("BMW");
            car.Pilot = pilot;
            db.Store(car);
        }
    
        public static void SetSecondCar(IObjectContainer db)
        {
            Pilot pilot = new Pilot("Michael Schumacher", 100);
            Car car = new Car("Ferrari");
            car.Pilot = pilot;
            db.Store(car);
        }
    
        public static void AccessLocalServer()
        {
            using(IObjectServer server = Db4oClientServer.OpenServer(YapFileName, 0))
            {
                using(IObjectContainer client = server.OpenClient())
                {
                    // Do something with this client, or open more clients
                }
            }
        }
    
        public static void QueryLocalServer(IObjectServer server)
        {
            using(IObjectContainer client = server.OpenClient())
            {
                ListResult(client.QueryByExample(new Car(null)));
            }
        }
        
    
        public static void DemonstrateLocalReadCommitted(IObjectServer server)
        {
            using(IObjectContainer client1 =server.OpenClient(),
                client2 =server.OpenClient())
            {
                Pilot pilot = new Pilot("David Coulthard", 98);
                IObjectSet result = client1.QueryByExample(new Car("BMW"));
                Car car = (Car)result.Next();
                car.Pilot = pilot;
                client1.Store(car);
                ListResult(client1.QueryByExample(new Car(null)));
                ListResult(client2.QueryByExample(new Car(null)));
                client1.Commit();
                ListResult(client1.QueryByExample(typeof(Car)));			
                ListRefreshedResult(client2, client2.QueryByExample(typeof(Car)), 2);
            }
        }
    
        public static void DemonstrateLocalRollback(IObjectServer server)
        {
            using (IObjectContainer client1 = server.OpenClient(),
                client2 = server.OpenClient())
            {
                IObjectSet result = client1.QueryByExample(new Car("BMW"));
                Car car = (Car) result.Next();
                car.Pilot = new Pilot("Someone else", 0);
                client1.Store(car);
                ListResult(client1.QueryByExample(new Car(null)));
                ListResult(client2.QueryByExample(new Car(null)));
                client1.Rollback();
                client1.Ext().Refresh(car, 2);
                ListResult(client1.QueryByExample(new Car(null)));
                ListResult(client2.QueryByExample(new Car(null)));
            }
        }
    
        public static void AccessRemoteServer()
        {
            using(IObjectServer server = Db4oClientServer.OpenServer(YapFileName, ServerPort))
            {
                server.GrantAccess(ServerUser, ServerPassword);

                using(IObjectContainer client = Db4oClientServer.OpenClient("localhost", ServerPort, ServerUser, ServerPassword))
                {
                    // Do something with this client, or open more clients
                }
            }
        }
    
        public static void QueryRemoteServer(int port, string user, string password)
        {
            using(IObjectContainer client = Db4oClientServer.OpenClient("localhost", port, user, password))
            {
                ListResult(client.QueryByExample(new Car(null)));
            }
        }
    
        public static void DemonstrateRemoteReadCommitted(int port, string user, string password)
        {
            using(IObjectContainer client1 = Db4oClientServer.OpenClient("localhost", port, user, password),
                    client2 = Db4oClientServer.OpenClient("localhost", port, user, password))
            {
                Pilot pilot = new Pilot("Jenson Button", 97);
                IObjectSet result = client1.QueryByExample(new Car(null));
                Car car = (Car)result.Next();
                car.Pilot = pilot;
                client1.Store(car);
                ListResult(client1.QueryByExample(new Car(null)));
                ListResult(client2.QueryByExample(new Car(null)));
                client1.Commit();
                ListResult(client1.QueryByExample(new Car(null)));
                ListResult(client2.QueryByExample(new Car(null)));
            }
        }
    
        public static void DemonstrateRemoteRollback(int port, string user, string password)
        {
            using(IObjectContainer client1 =Db4oClientServer.OpenClient("localhost", port, user, password),
                client2 =Db4oClientServer.OpenClient("localhost", port, user, password))
            {
                IObjectSet result = client1.QueryByExample(new Car(null));
                Car car = (Car) result.Next();
                car.Pilot = new Pilot("Someone else", 0);
                client1.Store(car);
                ListResult(client1.QueryByExample(new Car(null)));
                ListResult(client2.QueryByExample(new Car(null)));
                client1.Rollback();
                client1.Ext().Refresh(car, 2);
                ListResult(client1.QueryByExample(new Car(null)));
                ListResult(client2.QueryByExample(new Car(null)));
            }
        }
    }
}
