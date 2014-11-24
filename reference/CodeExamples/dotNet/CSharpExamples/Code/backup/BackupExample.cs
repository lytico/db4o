using System.IO;
using Db4objects.Db4o;
using Db4objects.Db4o.IO;

namespace Db4oDoc.Code.Backup
{
    public class BackupExample
    {
        private const string DatabaseFile = "database.db4o";

        public static void Main(string[] args)
        {
            CleanDb();
            SimpleBackup();
            BackupWithStorage();
        }

        private static void CleanDb()
        {
            File.Delete(DatabaseFile);
        }

        private static void BackupWithStorage()
        {
            using (IObjectContainer container = Db4oEmbedded.OpenFile(DatabaseFile))
            {
                StoreObjects(container);
                // #example: Store a backup with storage
                container.Ext().Backup(new FileStorage(), "advanced-backup.db4o");
                // #end example
            }
        }

        private static void SimpleBackup()
        {
            using (IObjectContainer container = Db4oEmbedded.OpenFile(DatabaseFile))
            {
                StoreObjects(container);
                // #example: Store a backup while using the database
                container.Ext().Backup("backup.db4o");
                // #end example
            }
        }

        private static void StoreObjects(IObjectContainer container)
        {
            container.Store(new Person("John", "Walker"));
            container.Store(new Person("Joanna", "Waterman"));
            container.Commit();
        }

        private class Person
        {
            private string sirname;
            private string firstname;

            public Person(string name, string firstname)
            {
                this.firstname = firstname;
                this.sirname = name;
            }

            public string Sirname
            {
                get { return sirname; }
            }

            public string Firstname
            {
                get { return firstname; }
            }
        }
    }
}