using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Db4objects.Db4o;

namespace Db4oDoc.Code.Query.NativeQueries
{
    public class NativeQueriesSorting
    {
        private const string DatabaseFile = "database.db4o";

        public static void Main(string[] args)
        {
            CleanUp();
            using (IObjectContainer container = Db4oEmbedded.OpenFile(DatabaseFile))
            {
                StoreData(container);

                NativeQuerySorting(container);

            }
        }


        private static void NativeQuerySorting(IObjectContainer container)
        {
            // #example: Native query with sorting
            IList<Pilot> pilots = container.Query(
                delegate(Pilot p) { return p.Age > 18; },
                delegate(Pilot p1, Pilot p2) { return p1.Name.CompareTo(p2.Name); });
            // #end example

            ListResult(pilots);
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

}