using Db4objects.Db4o;
using Db4objects.Db4o.Config;

namespace Db4oDoc.Code.Configuration.Objectfield
{
    public class ObjectFieldConfigurations
    {
        private const string DatabaseFile = "database.db4o";

        private static void IndexField()
        {
            // #example: Index a certain field
            IEmbeddedConfiguration configuration = Db4oEmbedded.NewConfiguration();
            configuration.Common.ObjectClass(typeof (Person)).ObjectField("name").Indexed(true);
            // #end example

            IObjectContainer container = Db4oEmbedded.OpenFile(configuration, DatabaseFile);
            container.Close();
        }

        private static void CascadeOnActivate()
        {
            // #example: When activated, activate also the object referenced by this field
            IEmbeddedConfiguration configuration = Db4oEmbedded.NewConfiguration();
            configuration.Common.ObjectClass(typeof (Person)).ObjectField("father").CascadeOnActivate(true);
            // #end example

            IObjectContainer container = Db4oEmbedded.OpenFile(configuration, DatabaseFile);
            container.Close();
        }

        private static void CascadeOnUpdate()
        {
            // #example: When updated, update also the object referenced by this field
            IEmbeddedConfiguration configuration = Db4oEmbedded.NewConfiguration();
            configuration.Common.ObjectClass(typeof (Person)).ObjectField("father").CascadeOnUpdate(true);
            // #end example

            IObjectContainer container = Db4oEmbedded.OpenFile(configuration, DatabaseFile);
            container.Close();
        }

        private static void CascadeOnDelete()
        {
            // #example: When deleted, delete also the object referenced by this field
            IEmbeddedConfiguration configuration = Db4oEmbedded.NewConfiguration();
            configuration.Common.ObjectClass(typeof (Person)).ObjectField("father").CascadeOnDelete(true);
            // #end example

            IObjectContainer container = Db4oEmbedded.OpenFile(configuration, DatabaseFile);
            container.Close();
        }

        private static void RenameField()
        {
            // #example: Rename this field
            IEmbeddedConfiguration configuration = Db4oEmbedded.NewConfiguration();
            configuration.Common.ObjectClass(typeof (Person)).ObjectField("name").Rename("sirname");
            // #end example

            IObjectContainer container = Db4oEmbedded.OpenFile(configuration, DatabaseFile);
            container.Close();
        }
    }

    public class Person
    {
        private string name;
        private Person father;
    }
}