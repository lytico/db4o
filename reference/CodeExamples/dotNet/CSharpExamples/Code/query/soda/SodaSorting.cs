using System;
using System.Collections;
using System.IO;
using Db4objects.Db4o;
using Db4objects.Db4o.Query;

namespace Db4oDoc.Code.Query.Soda
{
    public class SodaSorting
    {
        private const string DatabaseFile = "database.db4o";

        public static void Main(string[] args)
        {
            CleanUp();
            using (IObjectContainer container = Db4oEmbedded.OpenFile(DatabaseFile))
            {
                StoreData(container);

                SortingOnField(container);
                SortingOnMultipleFields(container);
                CustomOrder(container);
            }
        }

        private static void SortingOnField(IObjectContainer container) {
            Console.WriteLine("Order by a field");
            // #example: Order by a field
            IQuery query = container.Query();
            query.Constrain(typeof(Pilot));
            query.Descend("name").OrderAscending();

            IObjectSet result = query.Execute();
            // #end example
            ListResult(result);
        }

        private static void SortingOnMultipleFields(IObjectContainer container)
        {
            Console.WriteLine("Order by multiple fields");
            // #example: Order by multiple fields
            IQuery query = container.Query();
            query.Constrain(typeof(Pilot));
            query.Descend("age").OrderAscending();
            query.Descend("name").OrderAscending();

            IObjectSet result = query.Execute();
            // #end example
            ListResult(result);
        }

        private static void CustomOrder(IObjectContainer container)
        {
            Console.WriteLine("Order by your comparator");
            // #example: Order by your comparator
            IQuery query = container.Query();
            query.Constrain(typeof(Pilot));
            query.SortBy(new NameLengthComperator());

            IObjectSet result = query.Execute();
            // #end example
            ListResult(result);
        }

        // #example: The string length comperator
        class NameLengthComperator :IQueryComparator
        {
            public int Compare(object first, object second)
            {
                Pilot p1 = (Pilot) first;
                Pilot p2 = (Pilot)second;
                // sort by string-length
                return Math.Sign(p1.Name.Length - p2.Name.Length);
            }
        }
        // #end example

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
            container.Store(new Pilot("John", 42));
            container.Store(new Pilot("Joanna", 45));
            container.Store(new Pilot("Brigit", 59));
            container.Store(new Pilot("Jenny", 21));
            container.Store(new Pilot("Rick", 33));
            container.Store(new Pilot("Jolanda", 33));
            container.Store(new Pilot("Chris", 22));
            container.Store(new Pilot("John", 33));
            container.Store(new Pilot("Raphael", 34));
            container.Store(new Pilot("Paul", 61));
            container.Store(new Pilot("Li", 43));
        }
    }
}