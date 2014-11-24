using System;
using System.IO;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.TA;

namespace Db4oDoc.Ta.Example
{
    public class TransparentActivationExamples
    {
        private const string DatabaseFileName = "database.db4o";

        public static void Main(string[] args)
        {
            CleanUp();

            using (IObjectContainer container = OpenDatabase())
            {
                Person person = Person.PersonWithHistory();
                container.Store(person);
            }
            using (IObjectContainer container = OpenDatabase())
            {
                Person person = QueryByName(container, "Joanna the 10");
                Person beginOfDynasty = person.Mother;

                // With transparent persistence enabled, you can navigate deeply
                // nested object graphs. db4o will ensure that the objects
                // are loaded from the database.
                while (null != beginOfDynasty.Mother)
                {
                    beginOfDynasty = beginOfDynasty.Mother;
                }
                Console.WriteLine(beginOfDynasty.Name);

                // Updating a object doesn't requires no store call.
                // Just change the objects and the call commit.
                beginOfDynasty.Name = "New Name";
                container.Commit();
            }
            using (IObjectContainer container = OpenDatabase())
            {
                Person person = QueryByName(container, "New Name");
                // The changes are stored, due to transparent persistence
                Console.WriteLine(person.Name);
            }

            CleanUp();
        }

        private static void CleanUp()
        {
            File.Delete(DatabaseFileName);
        }

        private static Person QueryByName(IObjectContainer container, string name)
        {
            return container.Query(delegate(Person p) { return p.Name.Equals(name); })[0];
        }

        private static IObjectContainer OpenDatabase()
        {
            IEmbeddedConfiguration configuration = Db4oEmbedded.NewConfiguration();
            configuration.Common.Add(new TransparentPersistenceSupport());
            IObjectContainer container = Db4oEmbedded.OpenFile(configuration, DatabaseFileName);
            return container;
        }
    }

}