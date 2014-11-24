using System;
using System.Collections.Generic;
using System.IO;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;

#region LINQ-Imports
//#example: Use the LINQ namespace
using System.Linq;
using Db4objects.Db4o.Linq;
// #end example
#endregion

namespace Db4oDoc.Code.Query.Linq
{
    public class LinqExamples
    {
        private const string DatabaseFileName = "database.db4o";

        public static void Main(string[] args)
        {
            CleanUp();
            PrepareData();
            Console.Out.WriteLine("SimpleQuery:---");
            SimpleQuery();
            Console.Out.WriteLine("QueryForName:---");
            QueryForName();
            Console.Out.WriteLine("QueryWithConstrain:---");
            QueryWithConstrain();
            Console.Out.WriteLine("QueryWithSortingConstrain:---");
            QueryWithSortingConstrain();
            Console.Out.WriteLine("AsQueriable:---");
            AsQueryable();
            Console.Out.WriteLine("OptimizableQuery:---");
            OptimizableQuery();
            Console.Out.WriteLine("UnOptimizableQuery:---");
            UnOptimizableQuery();
            Console.Out.WriteLine("MixingOptimizing:---");
            MixingOptimizing();

            Console.Read();
        }

        private static void SimpleQuery()
        {
            using (var container = OpenDB())
            {
                // #example: Simple query
                var allPersons = from Person p in container
                                 select p;
                // #end example
                ListAll(allPersons);
            }
        }

        private static void QueryForName()
        {
            using (var container = OpenDB())
            {
                // #example: Query for name
                var allPersons = from Person p in container
                                 where p.Name.Equals("Joe")
                                 select p;
                // #end example
                ListAll(allPersons);
            }
        }

        private static void QueryWithConstrain()
        {
            using (var container = OpenDB())
            {
                // #example: Query with a constraint
                var allPersons = from Person p in container
                                 where p.Age > 21
                                 select p;
                // #end example
                ListAll(allPersons);
            }
        }

        private static void QueryWithSortingConstrain()
        {
            using (var container = OpenDB())
            {
                // #example: Use sorting on the query
                var allPersons = from Person p in container
                                 where p.Age > 21
                                 orderby p.Name
                                 select p;
                // #end example
                ListAll(allPersons);
            }
        }

        private static void AsQueryable()
        {
            using (var container = OpenDB())
            {
                // #example: Get a IQueryable-instance
                IQueryable<Person> personQuerable = container.AsQueryable<Person>();
                var adults = from p in personQuerable
                             where p.Age > 18
                             orderby p.Name
                             select p;
                // #end example
                ListAll(adults);
            }
        }

        private static void OptimizableQuery()
        {
            using (var container = OpenDB())
            {
                // #example: A query which is optimizable
                var adults = from Person p in container
                             where p.Age > 18 && p.Age < 70
                             orderby p.Name
                             select p;
                // #end example
                ListAll(adults);
            }
        }

        private static void UnOptimizableQuery()
        {
            using (var container = OpenDB())
            {
                // #example: Unoptimizable query, because of the 'operations' withing the query
                var adults = from Person p in container
                             where p.Name.ToLowerInvariant().Equals("joe")
                             select p;
                // #end example
                ListAll(adults);
            }
        }

        private static void MixingOptimizing()
        {
            using (var container = OpenDB())
            {
                // #example: Unoptimizable query
                var adults = from Person p in container
                             where p.Age > 18 && p.Age < 70
                                    && p.Name.Substring(2).Contains("n")
                             select p;
                // #end example
                ListAll(adults);

                // #example: Splitting into two parts
                var optimizedPart = from Person p in container
                                    where p.Age > 18 && p.Age < 70
                                    select p;
                var endResult = from p in optimizedPart.AsEnumerable()
                              where p.Name.Substring(2).Contains("n")
                              select p;
                // #end example
                ListAll(endResult);
            }
        }

        private static void PrepareData()
        {
            using (var container = OpenDB())
            {
                container.Store(new Person("Joanna", 34));
                container.Store(new Person("Joe", 42));
                container.Store(new Person("Julia", 62));
                container.Store(new Person("John", 23));
                container.Store(new Person("Jonathan", 19));
                container.Store(new Person("Amelia", 38));
                container.Store(new Person("Amanda", 17));
            }
        }

        private static void ListAll<T>(IEnumerable<T> printOut)
        {
            foreach (var item in printOut)
            {
                Console.Out.WriteLine(item);
            }
        }


        private static IObjectContainer OpenDB()
        {
            IEmbeddedConfiguration configuration = Db4oEmbedded.NewConfiguration();
            return Db4oEmbedded.OpenFile(configuration, DatabaseFileName);
        }

        private static void CleanUp()
        {
            File.Delete(DatabaseFileName);
        }
    }


    internal class Person
    {
        private string name;
        private int age;

        public Person()
        {
            name = "";
            age = 42;
        }

        public Person(string name, int age)
        {
            this.name = name;
            this.age = age;
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public int Age
        {
            get { return age; }
            set { age = value; }
        }

        public override string ToString()
        {
            return string.Format("Name: {0}, Age: {1}", name, age);
        }
    }

}