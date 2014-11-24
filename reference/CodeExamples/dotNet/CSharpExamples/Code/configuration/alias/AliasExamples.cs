using System;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;

namespace Db4oDoc.Code.Configuration.Alias
{
    public class AliasExamples
    {
        private const string DatabaseFileName = "database.db4o";

        public static void Main(string[] args)
        {
            CleanUp();

            AliasesExample();
        }

        private static void AliasesExample()
        {
            StoreTypes();

            IEmbeddedConfiguration configuration = Db4oEmbedded.NewConfiguration();
            // #example: Adding aliases
            // add an alias for a specific type
            configuration.Common.AddAlias(
                new TypeAlias("Db4oDoc.Code.Configuration.Alias.OldTypeInDatabase, Db4oDoc",
                              "Db4oDoc.Code.Configuration.Alias.NewType, Db4oDoc"));
            // or add an alias for a whole namespace
            configuration.Common.AddAlias(
                new WildcardAlias("Db4oDoc.Code.Configuration.Alias.Old.Namespace.*, Db4oDoc",
                                  "Db4oDoc.Code.Configuration.Alias.Current.Namespace.*, Db4oDoc"));
            // #end example

            using (IEmbeddedObjectContainer container = Db4oEmbedded.OpenFile(configuration, DatabaseFileName))
            {
                int countRenamed = container.Query<NewType>().Count;
                AssertFoundEntries(countRenamed);
                int countInOtherPackage = container
                        .Query<Current.Namespace.Car>().Count;
                AssertFoundEntries(countInOtherPackage);
            }
        }

        private static void AssertFoundEntries(int countRenamed)
        {
            if (1 > countRenamed)
            {
                throw new Exception("Expected a least on entry");
            }
        }

        private static void StoreTypes()
        {
            using (IObjectContainer container = OpenDatabase())
            {
                container.Store(new OldTypeInDatabase());
                container.Store(new Old.Namespace.Car());
            }
        }

        private static void CleanUp()
        {
            System.IO.File.Delete(DatabaseFileName);
        }

        private static IObjectContainer OpenDatabase()
        {
            return Db4oEmbedded.OpenFile(DatabaseFileName);
        }
    }

    internal class OldTypeInDatabase
    {
        private string name = "default";

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
    }

    internal class NewType
    {
        private string name = "default";

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
    }

    namespace Old.Namespace
    {

        public class Pilot
        {
            private string name = "Joe";

            public string Name
            {
                get { return name; }
                set { name = value; }
            }
        }
        public class Car
        {
            private Pilot pilot = new Pilot();

            public Pilot Pilot
            {
                get { return pilot; }
                set { pilot = value; }
            }
        }
    }
    namespace Current.Namespace
    {

        public class Pilot
        {
            private string name = "Joe";

            public string Name
            {
                get { return name; }
                set { name = value; }
            }
        }
        public class Car
        {
            private Pilot pilot = new Pilot();

            public Pilot Pilot
            {
                get { return pilot; }
                set { pilot = value; }
            }
        }
    }
}