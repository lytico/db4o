using System;
using System.IO;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Diagnostic;
using Db4objects.Db4o.Linq;

namespace Db4oDoc.Code.Indexing.Where
{
    public class WhereToIndexExample
    {
        private const string DatabaseFile = "database.db4o";

        public static void Main(string[] args)
        {
            StoreObjects();
            RunQuery();
            AddIndex();
            RunQuery();
        }

        private static void AddIndex()
        {
            IEmbeddedConfiguration configuration = Db4oEmbedded.NewConfiguration();
            configuration.Common.ObjectClass(typeof (IndexedClass)).ObjectField("id").Indexed(true);
            using (IObjectContainer container = Db4oEmbedded.OpenFile(configuration, DatabaseFile))
            {
                container.Query<IndexedClass>();
            }
        }

        private static void RunQuery()
        {
            IEmbeddedConfiguration configuration = Db4oEmbedded.NewConfiguration();
            // #example: Find queries which cannot use index
            configuration.Common.Diagnostic.AddListener(new IndexDiagnostics());
            // #end example
            using (IObjectContainer container = Db4oEmbedded.OpenFile(configuration, DatabaseFile))
            {
                var result = from IndexedClass i in container select i;
                result.Count();
            }
        }


        private static void StoreObjects()
        {
            CleanUp();
            using (IObjectContainer container = Db4oEmbedded.OpenFile(DatabaseFile))
            {
                StoreObjects(container);
            }
        }

        private static void CleanUp()
        {
            File.Delete(DatabaseFile);
        }

        private static void StoreObjects(IObjectContainer container)
        {
            Random rnd = new Random();
            for (int i = 0; i < 10000; i++)
            {
                container.Store(IndexedClass.Create(rnd));
            }
        }
    }

    // #example: Index diagnostics
    internal class IndexDiagnostics : IDiagnosticListener
    {
        public void OnDiagnostic(IDiagnostic diagnostic)
        {
            if (diagnostic is LoadedFromClassIndex)
            {
                Console.WriteLine("This query couldn't use field indexes " +
                                  ((LoadedFromClassIndex) diagnostic).Reason());
                Console.WriteLine(diagnostic);
            }
        }
    }
    // #end example

    internal class IndexedClass
    {
        private int id;
        private string otherData;

        public IndexedClass(int id)
        {
            this.id = id;
            otherData = "This is more data =)";
        }

        public static IndexedClass Create(Random rnd)
        {
            int intIndex = NewInt(rnd);
            return new IndexedClass(intIndex);
        }

        private static int NewInt(Random rnd)
        {
            return rnd.Next();
        }

        public int ID
        {
            get { return id; }
        }
    }
}