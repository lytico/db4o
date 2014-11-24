using System;
using System.Linq;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Linq;
using Db4objects.Db4o.TA;

namespace Db4oDoc.Code.Tp.Enhancement
{
    public class TransparentPersistence
    {
        private const string DatabaseFileName = "database.db4o";

        public static void Main(string[] args)
        {
            CheckEnhancement();
            StoreExampleObjects();
            ActivationJustWorks();
            UpdatingJustWorks();
        }

        private static void CheckEnhancement()
        {
            // #example: Check for enhancement
            if (!typeof (IActivatable).IsAssignableFrom(typeof (Person)))
            {
                throw new InvalidOperationException(string.Format("Expect that the {0} implements {1}",
                                                                  typeof (Person), typeof (IActivatable)));
            }
            // #end example
        }

        private static void ActivationJustWorks()
        {
            using (IObjectContainer container = OpenDatabaseWithTA())
            {
                // #example: Activation just works
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
                // #end example
            }
        }

        private static void UpdatingJustWorks()
        {
            using (IObjectContainer container = OpenDatabaseWithTP())
            {
                // #example: Just update and commit. Transparent persistence manages all updates
                Person person = QueryByName(container, "Joanna the 10");
                Person mother = person.Mother;
                mother.Name = "New Name";
                // Just commit the transaction. All modified objects are stored
                container.Commit();
                // #end example
            }
        }

        private static IObjectContainer OpenDatabaseWithTA()
        {
            // #example: Add transparent activation
            IEmbeddedConfiguration configuration = Db4oEmbedded.NewConfiguration();
            configuration.Common.Add(new TransparentActivationSupport());
            IObjectContainer container = Db4oEmbedded.OpenFile(configuration, DatabaseFileName);
            // #end example
            return container;
        }
        private static IObjectContainer OpenDatabaseWithTP()
        {
            // #example: Add transparent persistence
            IEmbeddedConfiguration configuration = Db4oEmbedded.NewConfiguration();
            configuration.Common.Add(new TransparentPersistenceSupport(new DeactivatingRollbackStrategy()));
            IObjectContainer container = Db4oEmbedded.OpenFile(configuration, DatabaseFileName);
            // #end example
            return container;
        }

        private static Person QueryByName(IObjectContainer container, string nameOfPerson)
        {
            return (from Person p in container
                    where p.Name == nameOfPerson
                    select p).First();
        }

        private static void StoreExampleObjects()
        {
            using (IObjectContainer container = Db4oEmbedded.OpenFile(DatabaseFileName))
            {
                var person = Person.PersonWithHistory();
                container.Store(person);
            }
        }
    }
}