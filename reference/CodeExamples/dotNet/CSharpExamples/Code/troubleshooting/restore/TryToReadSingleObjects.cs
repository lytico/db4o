using System;
using Db4objects.Db4o;

namespace Db4oDoc.Code.Troubleshooting.Restore
{
    public class TryToReadSingleObjects
    {
        private const string DatabaseFileName = "database.db4o";

        public static void Main(string[] args)
        {
            StoreExampleObjects();
            using (IObjectContainer container = Db4oEmbedded.OpenFile(DatabaseFileName))
            {
                // #example: Try to read the intact objects
                long[] idsOfPersons = container.Ext().StoredClass(typeof (Person)).GetIDs();
                foreach (long id in idsOfPersons)
                {
                    try
                    {
                        var person = (Person) container.Ext().GetByID(id);
                        container.Ext().Activate(person, 1);
                        // store the person to another database
                        Console.Out.WriteLine("This object is ok {0}", person);
                    }
                    catch (Exception e)
                    {
                        Console.Out.WriteLine("We couldn't read the object with the id {0} anymore." +
                                              " It is lost", id);
                        Console.Out.WriteLine(e);
                    }
                }
                // #end example
            }
        }

        private static void StoreExampleObjects()
        {
            using (IObjectContainer container = Db4oEmbedded.OpenFile(DatabaseFileName))
            {
                for (int i = 0; i < 100; i++)
                {
                    container.Store(new Person("Fun" + i));
                }
            }
        }
    }


    internal class Person
    {
        private readonly string name;

        public Person(string name)
        {
            this.name = name;
        }

        public string Name
        {
            get { return name; }
        }
    }
}