using System;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Config.Encoding;
using Db4objects.Db4o.Diagnostic;
using Db4objects.Db4o.Internal.Reflect;

namespace Db4oDoc.Code.Configuration.Common
{
    public class CommonConfigurationExamples
    {
        private const string DatabaseFile = "database.db4o";

        private static void ChangeGlobalActivationDepth()
        {
            // #example: Change activation depth
            IEmbeddedConfiguration configuration = Db4oEmbedded.NewConfiguration();
            configuration.Common.ActivationDepth = 2;
            // #end example

            IObjectContainer container = Db4oEmbedded.OpenFile(configuration, DatabaseFile);
            container.Close();
        }

        private static void UpdateFileFormat()
        {
            // #example: Update the database-format
            IEmbeddedConfiguration configuration = Db4oEmbedded.NewConfiguration();
            configuration.Common.AllowVersionUpdates = true;

            // reopen and close the database to do the update
            IObjectContainer container = Db4oEmbedded.OpenFile(configuration, DatabaseFile);
            container.Close();
            // #end example
        }

        private static void DisableAutomaticShutdown()
        {
            // #example: Disable automatic shutdown
            IEmbeddedConfiguration configuration = Db4oEmbedded.NewConfiguration();
            configuration.Common.AutomaticShutDown = false;
            // #end example

            IObjectContainer container = Db4oEmbedded.OpenFile(configuration, DatabaseFile);
            container.Close();
        }

        private static void ChangeBTreeNodeSize()
        {
            // #example: Change B-tree node size
            IEmbeddedConfiguration configuration = Db4oEmbedded.NewConfiguration();
            configuration.Common.BTreeNodeSize = 256;
            // #end example

            IObjectContainer container = Db4oEmbedded.OpenFile(configuration, DatabaseFile);
            container.Close();
        }

        private static void DisableCallbacks()
        {
            // #example: Disable callbacks
            IEmbeddedConfiguration configuration = Db4oEmbedded.NewConfiguration();
            configuration.Common.Callbacks = false;
            // #end example

            IObjectContainer container = Db4oEmbedded.OpenFile(configuration, DatabaseFile);
            container.Close();
        }

        private static void CallConstructors()
        {
            // #example: Call constructors
            IEmbeddedConfiguration configuration = Db4oEmbedded.NewConfiguration();
            configuration.Common.CallConstructors = true;
            // #end example

            IObjectContainer container = Db4oEmbedded.OpenFile(configuration, DatabaseFile);
            container.Close();
        }

        private static void DisableSchemaEvolution()
        {
            // #example: Disable schema evolution
            IEmbeddedConfiguration configuration = Db4oEmbedded.NewConfiguration();
            configuration.Common.DetectSchemaChanges = false;
            // #end example

            IObjectContainer container = Db4oEmbedded.OpenFile(configuration, DatabaseFile);
            container.Close();
        }

        private static void AddDiagnosticListener()
        {
            // #example: Add a diagnostic listener
            IEmbeddedConfiguration configuration = Db4oEmbedded.NewConfiguration();
            configuration.Common.Diagnostic.AddListener(new DiagnosticToConsole());
            // #end example

            IObjectContainer container = Db4oEmbedded.OpenFile(configuration, DatabaseFile);
            container.Close();
        }

        private static void DisableExceptionsOnNotStorableObjects()
        {
            // #example: Disable exceptions on not storable objects
            IEmbeddedConfiguration configuration = Db4oEmbedded.NewConfiguration();
            configuration.Common.ExceptionsOnNotStorable = false;
            // #end example

            IObjectContainer container = Db4oEmbedded.OpenFile(configuration, DatabaseFile);

            container.Close();
        }

        private static void ChangeMessageLevel()
        {
            // #example: Change the message-level
            IEmbeddedConfiguration configuration = Db4oEmbedded.NewConfiguration();
            configuration.Common.MessageLevel = 4;
            // #end example

            IObjectContainer container = Db4oEmbedded.OpenFile(configuration, DatabaseFile);

            container.Close();
        }

        private static void ChangeOutputStream()
        {
            // #example: Change the output stream
            IEmbeddedConfiguration configuration = Db4oEmbedded.NewConfiguration();
            configuration.Common.OutStream = Console.Out;
            // #end example

            IObjectContainer container = Db4oEmbedded.OpenFile(configuration, DatabaseFile);

            container.Close();
        }

        private static void ChangeQueryMode()
        {
            // #example: Change the query mode
            IEmbeddedConfiguration configuration = Db4oEmbedded.NewConfiguration();
            configuration.Common.Queries.EvaluationMode(QueryEvaluationMode.Immediate);
            // #end example

            IObjectContainer container = Db4oEmbedded.OpenFile(configuration, DatabaseFile);

            container.Close();
        }

        private static void InternStrings()
        {
            // #example: intern strings
            IEmbeddedConfiguration configuration = Db4oEmbedded.NewConfiguration();
            configuration.Common.InternStrings = true;
            // #end example

            IObjectContainer container = Db4oEmbedded.OpenFile(configuration, DatabaseFile);

            container.Close();
        }

        private static void ChangeReflector()
        {
            // #example: Change the reflector
            IEmbeddedConfiguration configuration = Db4oEmbedded.NewConfiguration();
            configuration.Common.ReflectWith(new FastNetReflector());
            // #end example

            IObjectContainer container = Db4oEmbedded.OpenFile(configuration, DatabaseFile);
            container.Close();
        }

        private static void MaxStackSize()
        {
            // #example: Set the stack size
            IEmbeddedConfiguration configuration = Db4oEmbedded.NewConfiguration();
            configuration.Common.MaxStackDepth = 16;
            // #end example

            IObjectContainer container = Db4oEmbedded.OpenFile(configuration, DatabaseFile);

            container.Close();
        }

        private static void NameProvider()
        {
            // #example: set a name-provider
            IEmbeddedConfiguration configuration = Db4oEmbedded.NewConfiguration();
            configuration.Common.NameProvider(new SimpleNameProvider("Database"));
            // #end example

            IObjectContainer container = Db4oEmbedded.OpenFile(configuration, DatabaseFile);

            container.Close();
        }


        private static void ChangeWeakReferenceCollectionIntervall()
        {
            // #example: change weak reference collection interval
            IEmbeddedConfiguration configuration = Db4oEmbedded.NewConfiguration();
            configuration.Common.WeakReferenceCollectionInterval = (10*1000);
            // #end example

            IObjectContainer container = Db4oEmbedded.OpenFile(configuration, DatabaseFile);
            container.Close();
        }

        private static void DisableWeakReferences()
        {
            // #example: Disable weak references
            IEmbeddedConfiguration configuration = Db4oEmbedded.NewConfiguration();
            configuration.Common.WeakReferences = false;
            // #end example

            IObjectContainer container = Db4oEmbedded.OpenFile(configuration, DatabaseFile);
            container.Close();
        }

        private static void MarkTransient()
        {
            CleanUp();

            // #example: add an transient marker annotatin
            IEmbeddedConfiguration configuration = Db4oEmbedded.NewConfiguration();
            configuration.Common.MarkTransient(typeof (TransientMarkerAttribute).FullName);
            // #end example

            IObjectContainer container = Db4oEmbedded.OpenFile(configuration, DatabaseFile);
            container.Store(new WithTransient());
            container.Close();

            ReadWithTransientMarker();

            CleanUp();
        }

        private static void ChangeStringEncoding()
        {
            // #example: Use the utf8 encoding
            IEmbeddedConfiguration configuration = Db4oEmbedded.NewConfiguration();
            configuration.Common.StringEncoding = StringEncodings.Utf8();
            // #end example

            IObjectContainer container = Db4oEmbedded.OpenFile(configuration, DatabaseFile);
            container.Close();
        }

        private static void DisableRuntimeNQOptimizer()
        {
            // #example: Disable the runtime native query optimizer
            IEmbeddedConfiguration configuration = Db4oEmbedded.NewConfiguration();
            configuration.Common.OptimizeNativeQueries = false;
            // #end example

            IObjectContainer container = Db4oEmbedded.OpenFile(configuration, DatabaseFile);
            container.Close();
        }

        private static void DisableTestingConstructors()
        {
            // #example: Disable testing for callable constructors
            IEmbeddedConfiguration configuration = Db4oEmbedded.NewConfiguration();
            configuration.Common.TestConstructors = false;
            // #end example

            IObjectContainer container = Db4oEmbedded.OpenFile(configuration, DatabaseFile);
            container.Close();
        }

        private static void IncreasingUpdateDepth()
        {
            // #example: Increasing the update-depth
            IEmbeddedConfiguration configuration = Db4oEmbedded.NewConfiguration();
            configuration.Common.UpdateDepth = 2;
            // #end example

            IObjectContainer container = Db4oEmbedded.OpenFile(configuration, DatabaseFile);
            container.Close();
        }

        private static void CleanUp()
        {
            System.IO.File.Delete(DatabaseFile);
        }

        private static void ReadWithTransientMarker()
        {
            IEmbeddedConfiguration configuration = Db4oEmbedded.NewConfiguration();
            configuration.Common.MarkTransient(typeof (TransientMarkerAttribute).FullName);
            IObjectContainer container = Db4oEmbedded.OpenFile(configuration, DatabaseFile);
            WithTransient instance = container.Query<WithTransient>()[0];

            AssertTransientNotStored(instance);

            container.Close();
        }

        private static void AssertTransientNotStored(WithTransient instance)
        {
            if (null != instance.TransientString)
            {
                throw new Exception("Transient was stored!");
            }
        }
    }

    internal class WithTransient
    {
        [TransientMarker] private string transientString = "New";

        public string TransientString
        {
            get { return transientString; }
            set { transientString = value; }
        }
    }

    [AttributeUsage(AttributeTargets.Field)]
    internal class TransientMarkerAttribute : Attribute
    {
    }
}