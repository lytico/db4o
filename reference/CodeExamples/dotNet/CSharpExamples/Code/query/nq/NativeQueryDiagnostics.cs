using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Diagnostic;

namespace Db4oDoc.Code.Query.NativeQueries
{
    public class NativeQueryDiagnostics
    {

        private const string DatabaseFile = "database.db4o";

        public static void Main(string[] args)
        {
            CleanUp();
            IEmbeddedConfiguration configuration = Db4oEmbedded.NewConfiguration();
            // #example: Use diagnostics to find unoptimized queries
            configuration.Common.Diagnostic.AddListener(new NativeQueryListener());
            // #end example
            using (IObjectContainer container = Db4oEmbedded.OpenFile(configuration,DatabaseFile))
            {
                StoreData(container);

                OptimizedNativeQuery(container);
                NotOptimizedNativeQuery(container);

            }
        }


        private static void OptimizedNativeQuery(IObjectContainer container)
        {
            IList<Pilot> result = container.Query(
                delegate(Pilot pilot) { return pilot.Name.StartsWith("J"); });

            ListResult(result);
        }

        private static void NotOptimizedNativeQuery(IObjectContainer container)
        {
            IList<Pilot> result = container.Query(
                delegate(Pilot pilot)
                    {
                        return pilot.Name.ToUpper(CultureInfo.CurrentUICulture).StartsWith("J");
                    });

            ListResult(result);
        }

        private static void CleanUp()
        {
            File.Delete(DatabaseFile);
        }


        private static void ListResult(IEnumerable result)
        {
            foreach (object obj in result)
            {
                Console.WriteLine(obj);
            }
        }

        private static void StoreData(IObjectContainer container)
        {
            Pilot john = new Pilot("John", 42);
            Pilot joanna = new Pilot("Joanna", 45);
            Pilot jenny = new Pilot("Jenny", 21);
            Pilot rick = new Pilot("Rick", 33);

            container.Store(new Car(john, "Ferrari"));
            container.Store(new Car(joanna, "Mercedes"));
            container.Store(new Car(jenny, "Volvo"));
            container.Store(new Car(rick, "Fiat"));
        }

    }

    // #example: The native query listener
    class NativeQueryListener :IDiagnosticListener
    {
        public void OnDiagnostic(IDiagnostic diagnostic)
        {
            if(diagnostic is NativeQueryNotOptimized){
                Console.WriteLine("Query not optimized"+diagnostic);
            } else if(diagnostic is NativeQueryOptimizerNotLoaded){
                Console.WriteLine("Missing native query optimisation assemblies" + diagnostic);
            }
        }
    }
    // #end example
}