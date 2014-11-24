using System;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;

namespace Db4oDoc.Code.Configuration.ObjectConfig
{
    public class ObjectConfigurationExamples
    {
        private const string DatabaseFile = "database.db4o";


        private static void CallConstructor()
        {
            // #example: Call constructor
            IEmbeddedConfiguration configuration = Db4oEmbedded.NewConfiguration();
            configuration.Common.ObjectClass(typeof (Person)).CallConstructor(true);
            // #end example

            IObjectContainer container = Db4oEmbedded.OpenFile(configuration, DatabaseFile);
            container.Close();
        }

        private static void CascadeOnActivate()
        {
            // #example: When activated, activate also all references
            IEmbeddedConfiguration configuration = Db4oEmbedded.NewConfiguration();
            configuration.Common.ObjectClass(typeof (Person)).CascadeOnActivate(true);
            // #end example

            IObjectContainer container = Db4oEmbedded.OpenFile(configuration, DatabaseFile);
            container.Close();
        }

        private static void CascadeOnDelete()
        {
            // #example: When deleted, delete also all references
            IEmbeddedConfiguration configuration = Db4oEmbedded.NewConfiguration();
            configuration.Common.ObjectClass(typeof (Person)).CascadeOnDelete(true);
            // #end example

            IObjectContainer container = Db4oEmbedded.OpenFile(configuration, DatabaseFile);
            container.Close();
        }

        private static void CascadeOnUpdate()
        {
            // #example: When updated, update also all references
            IEmbeddedConfiguration configuration = Db4oEmbedded.NewConfiguration();
            configuration.Common.ObjectClass(typeof (Person)).CascadeOnUpdate(true);
            // #end example

            IObjectContainer container = Db4oEmbedded.OpenFile(configuration, DatabaseFile);
            container.Close();
        }

        private static void GenerateUuiDs()
        {
            // #example: Generate uuids for this type
            IEmbeddedConfiguration configuration = Db4oEmbedded.NewConfiguration();
            configuration.Common.ObjectClass(typeof (Person)).GenerateUUIDs(true);
            // #end example

            IObjectContainer container = Db4oEmbedded.OpenFile(configuration, DatabaseFile);
            container.Close();
        }

        private static void IndexObjects()
        {
            // #example: Disable class index
            IEmbeddedConfiguration configuration = Db4oEmbedded.NewConfiguration();
            configuration.Common.ObjectClass(typeof (Person)).Indexed(false);
            // #end example

            IObjectContainer container = Db4oEmbedded.OpenFile(configuration, DatabaseFile);
            container.Close();
        }

        private static void MaximumActivationDepth()
        {
            // #example: Set maximum activation depth
            IEmbeddedConfiguration configuration = Db4oEmbedded.NewConfiguration();
            configuration.Common.ObjectClass(typeof (Person)).MaximumActivationDepth(5);
            // #end example

            IObjectContainer container = Db4oEmbedded.OpenFile(configuration, DatabaseFile);
            container.Close();
        }

        private static void MinimalActivationDepth()
        {
            // #example: Set minimum activation depth
            IEmbeddedConfiguration configuration = Db4oEmbedded.NewConfiguration();
            configuration.Common.ObjectClass(typeof (Person)).MinimumActivationDepth(2);
            // #end example

            IObjectContainer container = Db4oEmbedded.OpenFile(configuration, DatabaseFile);
            container.Close();
        }

        private static void PersistStaticFieldValues()
        {
            // #example: Persist also the static fields
            IEmbeddedConfiguration configuration = Db4oEmbedded.NewConfiguration();
            configuration.Common.ObjectClass(typeof (Person)).PersistStaticFieldValues();
            // #end example

            IObjectContainer container = Db4oEmbedded.OpenFile(configuration, DatabaseFile);
            container.Close();
        }

        private static void Rename()
        {
            // #example: Rename this type
            IEmbeddedConfiguration configuration = Db4oEmbedded.NewConfiguration();
            configuration.Common.ObjectClass(typeof(Person)).Rename("Db4oDoc.NewNamespace.NewName");
            // #end example

            IObjectContainer container = Db4oEmbedded.OpenFile(configuration, DatabaseFile);
            container.Close();
        }

        private static void StoreTransientFields()
        {
            // #example: Store also transient fields
            IEmbeddedConfiguration configuration = Db4oEmbedded.NewConfiguration();
            configuration.Common.ObjectClass(typeof (Person)).StoreTransientFields(true);
            // #end example

            IObjectContainer container = Db4oEmbedded.OpenFile(configuration, DatabaseFile);
            container.Close();
        }

        private static void Translator()
        {
            // #example: Use a translator for this type
            IEmbeddedConfiguration configuration = Db4oEmbedded.NewConfiguration();
            configuration.Common.ObjectClass(typeof (Person)).Translate(new TSerializable());
            // #end example

            IObjectContainer container = Db4oEmbedded.OpenFile(configuration, DatabaseFile);
            container.Close();
        }


        private static void UpdateDepth()
        {
            // #example: Set the update depth
            IEmbeddedConfiguration configuration = Db4oEmbedded.NewConfiguration();
            configuration.Common.ObjectClass(typeof (Person)).UpdateDepth(2);
            // #end example

            IObjectContainer container = Db4oEmbedded.OpenFile(configuration, DatabaseFile);
            container.Close();
        }
    }


    public class Person
    {
    }
}