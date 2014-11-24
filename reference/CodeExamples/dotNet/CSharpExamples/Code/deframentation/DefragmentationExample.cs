using System;
using System.IO;
using System.Linq;
using Db4objects.Db4o;
using Db4objects.Db4o.Defragment;
using Db4objects.Db4o.Linq;

namespace Db4oDoc.Code.Deframentation
{
    public class DefragmentationExample
    {
        public const string DatabaseFile = "database.db4o";

        public static void Main(string[] args)
        {
            SimplestPossibleDefragment();
            SimpleDefragmentationWithBackupLocation();
            DefragmentWithConfiguration();
            DefragmentationWithIdMissing();
        }

        // #end example

        private static void DefragmentWithConfiguration()
        {
            CreateAndFillDatabase();
            // #example: Defragment with configuration
            DefragmentConfig config = new DefragmentConfig("database.db4o");
            Defragment.Defrag(config);
            // #end example
        }

        private static void SimpleDefragmentationWithBackupLocation()
        {
            CreateAndFillDatabase();
            // #example: Specify backup file explicitly
            Defragment.Defrag("database.db4o", "database.db4o.bak");
            // #end example
        }


        private static void SimplestPossibleDefragment()
        {
            CreateAndFillDatabase();
            // #example: Simplest possible defragment use case
            Defragment.Defrag("database.db4o");
            // #end example
        }

        private static void DefragmentationWithIdMissing()
        {
            CreateAndFillDatabase();


            // #example: Use a defragmentation listener
            DefragmentConfig config = new DefragmentConfig("database.db4o");
            Defragment.Defrag(config, new DefragmentListener());
            // #end example
        }

        // #example: Defragmentation listener implementation
        private class DefragmentListener : IDefragmentListener
        {
            public void NotifyDefragmentInfo(DefragmentInfo defragmentInfo)
            {
                Console.WriteLine(defragmentInfo);
            }
        }
        // #end example

        private static void CreateAndFillDatabase()
        {
            CleanUp();
            using (IObjectContainer container = Db4oEmbedded.OpenFile(DatabaseFile))
            {
                container.Store(new Person("Joe"));
                container.Store(new Person("Joanna"));
                container.Store(new Person("Jenny"));
                container.Store(new Person("Julia"));
                container.Store(new Person("John"));
                container.Store(new Person("JJ"));
                Person jimmy = new Person("Jimmy");
                jimmy.BestFriend = new Person("Bunk");
                container.Store(jimmy);
            }
            LeaveInvalidId();
        }

        private static void LeaveInvalidId()
        {
            using (IObjectContainer container = Db4oEmbedded.OpenFile(DatabaseFile))
            {
                Person bunk = (from Person p in container
                               where p.Name == "Bunk"
                               select p).First();
                container.Delete(bunk);
            }
        }

        private static void CleanUp()
        {
            File.Delete(DatabaseFile);
            File.Delete(DatabaseFile + ".backup");
            File.Delete(DatabaseFile + ".bak");
        }


        private class Person
        {
            private string name;
            private Person bestFriend;

            public Person(string name)
            {
                this.name = name;
            }

            public string Name
            {
                get { return name; }
                set { name = value; }
            }

            public Person BestFriend
            {
                get { return bestFriend; }
                set { bestFriend = value; }
            }
        }
    }
}