using System;
using System.IO;
using System.Linq;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Linq;
using Db4objects.Db4o.TA;

namespace Db4oDoc.Ta.Example
{
    public class TransparentActivationExamples
    {
        private const string DatabaseFileName = "database.db4o";

        public static void Main(string[] args)
        {
            TransparentActivationExample();
            TransparentPersistenceExample();
        }

        private static void TransparentActivationExample()
        {
            CleanUp();

            // #example: Transparent activation in action
            using (IObjectContainer container = OpenDatabaseTA())
            {
                Person person = Person.PersonWithHistory();
                container.Store(person);
            }
            using (IObjectContainer container = OpenDatabaseTA())
            {
                Person person = QueryByName(container, "Joanna the 10");
                Person beginOfDynasty = person.Mother;

                // With transparent activation enabled, you can navigate deeply
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
            // #end example

            CleanUp();
        }


        private static void TransparentPersistenceExample()
        {
            CleanUp();

            // #example: Transparent persistence in action
            using (IObjectContainer container = OpenDatabaseTP())
            {
                Person person = Person.PersonWithHistory();
                container.Store(person);
            }
            using (IObjectContainer container = OpenDatabaseTP())
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
            using (IObjectContainer container = OpenDatabaseTP())
            {
                Person person = QueryByName(container, "New Name");
                // The changes are stored, due to transparent persistence
                Console.WriteLine(person.Name);
            }
            // #end example

            CleanUp();
        }

        private static void CleanUp()
        {
            File.Delete(DatabaseFileName);
        }

        private static Person QueryByName(IObjectContainer container, string name)
        {
            return (from Person p in container
                   where p.Name.Equals(name)
                   select p).First();
        }

        private static IObjectContainer OpenDatabaseTP()
        {
            // #example: Activate transparent persistence
            IEmbeddedConfiguration configuration = Db4oEmbedded.NewConfiguration();
            configuration.Common.Add(new TransparentPersistenceSupport());
            IObjectContainer container = Db4oEmbedded.OpenFile(configuration, DatabaseFileName);
            // #end example
            return container;
        }
        private static IObjectContainer OpenDatabaseTA()
        {
            // #example: Activate transparent activation
            IEmbeddedConfiguration configuration = Db4oEmbedded.NewConfiguration();
            configuration.Common.Add(new TransparentActivationSupport());
            IObjectContainer container = Db4oEmbedded.OpenFile(configuration, DatabaseFileName);
            // #end example
            return container;
        }
    }

}