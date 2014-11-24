using System;
using System.IO;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Events;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.TA;

namespace ObjectActivationSample
{
    class Program
    {
        public const string DATABASE_FILE = "test.odb";

        static void Main(string[] args)
        {
            DeleteDatabaseFile();
            InsertData();

            IConfiguration configuration = Db4oFactory.NewConfiguration();
            if (args.Length == 1 && args[0].ToUpper() == "-TA")
            {
                configuration.Add(new TransparentActivationSupport());    
            }
            
            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine("\r\nActivation Depth: {0}", i);
                ShowResultsAtActivationLevel(i, configuration);
                Console.WriteLine("{0}\r\n", new String('-', 80));
            }
        }

        private static void ShowResultsAtActivationLevel(int level, IConfiguration config)
        {
            config.ActivationDepth(level);
            using(IObjectContainer db = Db4oFactory.OpenFile(config, DATABASE_FILE))
            {
                RegisterObjectIdUpdaterEvent(db, activationTraker);

                IQuery query = QueryLeaf(db);
                object obj = query.Execute().Next();
                Console.WriteLine("Activation Count: {0}\r\n\r\n{1}", activationCount, obj);
            }
        }

        private static void activationTraker(object sender, ObjectEventArgs args)
        {
            activationCount++;
        }

        private static void RegisterObjectIdUpdaterEvent(IObjectContainer db, ObjectEventHandler activationTracker)
        {
            IEventRegistry eventRegistry = EventRegistryFactory.ForObjectContainer(db);
            
            eventRegistry.Activated += activationTracker;
            activationCount = 0;

            eventRegistry.Instantiated += delegate(object sender, ObjectEventArgs args)
                                          {
                                              SupportObjectId obj = args.Object as SupportObjectId;
                                              if (obj != null)
                                              {
                                                  obj.ObjectId = db.Ext().GetID(obj);
                                              }
                                          };
        }

        private static IQuery QueryLeaf(IObjectContainer db)
        {
            IQuery query = db.Query();

            query.Constrain(typeof (Person));
            query.Descend("name").Constrain("Caroline").Equal();
            
            return query;
        }

        private static void DeleteDatabaseFile()
        {
            if (File.Exists(DATABASE_FILE))
            {
                File.Delete(DATABASE_FILE);
            }
        }

        private static void InsertData()
        {
            using (IObjectContainer db = Db4oFactory.OpenFile(DATABASE_FILE))
            {
                Person grandParent = new Person("Joseph", 65, new Address("Descalvado", "JV street", 160), null);
                Person father = new Person("Adrian", 36, new Address("São Paulo", "C. L. E. M. street", 250), grandParent);
                Person child = new Person("Caroline", 4, new Address("São Paulo", "C. L. E. M. street", 250), father);

                db.Set(child);
            }
        }

        private static int activationCount = 0;
    }
}
