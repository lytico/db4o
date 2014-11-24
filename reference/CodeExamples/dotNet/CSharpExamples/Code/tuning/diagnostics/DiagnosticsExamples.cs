using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Diagnostic;
using Db4objects.Db4o.Linq;

namespace Db4oDoc.Code.Tuning.Diagnostics
{
    public class DiagnosticsExamples
    {
        private const string DatabaseFileName = "database.db4o";

        public static void Main(string[] args)
        {
            FilterDiagnosticMessages();
        }

        private static void FilterDiagnosticMessages()
        {
            CleanUp();
            using (IObjectContainer container = OpenDatabase())
            {
                container.Store(new SimpleClass());
                IList<SimpleClass> result = RunQuery(container);
                PrintResult(result);
            }
            CleanUp();
        }

        private static IObjectContainer OpenDatabase()
        {
            // #example: Filter for unindexed fields
            IEmbeddedConfiguration configuration = Db4oEmbedded.NewConfiguration();
            configuration.Common.Diagnostic
                .AddListener(new DiagnosticFilter(new DiagnosticToConsole(), typeof(LoadedFromClassIndex)));
            // #end example
            return Db4oEmbedded.OpenFile(configuration, DatabaseFileName);
        }

        private static IList<SimpleClass> RunQuery(IObjectContainer container)
        {
            return (from SimpleClass cwf in container
                    where cwf.Number < 100
                    select cwf).ToList();
        }

        private static void PrintResult(IEnumerable<SimpleClass> result)
        {
            foreach (SimpleClass item in result)
            {
                Console.Out.WriteLine(item);
            }
        }

        private static void CleanUp()
        {
            File.Delete(DatabaseFileName);
        }

        // #example: A simple message filter
        private class DiagnosticFilter : IDiagnosticListener
        {
            private readonly ICollection<Type> filterFor;
            private readonly IDiagnosticListener target;

            public DiagnosticFilter(IDiagnosticListener target, params Type[] filterFor)
            {
                this.target = target;
                this.filterFor = new List<Type>(filterFor);
            }

            public void OnDiagnostic(IDiagnostic diagnostic)
            {
                Type type = diagnostic.GetType();
                if (filterFor.Contains(type))
                {
                    target.OnDiagnostic(diagnostic);
                }
            }
        }
        // #end example

        private class SimpleClass
        {
            private int number = 0;

            public int Number
            {
                get { return number; }
                set { number = value; }
            }
        }
    }
}