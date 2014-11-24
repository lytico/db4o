using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Db4objects.Db4o;
using Db4objects.Db4o.Query;

namespace Db4oDoc.Code.Query.Soda
{
    public class SodaQueryExamples
    {
        private const string DatabaseFile = "database.db4o";

        public static void Main(string[] args)
        {
            CleanUp();
            using(IObjectContainer container = Db4oEmbedded.OpenFile(DatabaseFile))
            {
                StoreData(container);

                SimplestPossibleQuery(container);
                EqualsConstrain(container);
                GreaterThanConstrain(container);
                GreaterThanOrEqualConstrain(container);
                NotConstrain(container);
                CombiningConstrains(container);
                StringConstrains(container);
                CompareWithStoredObject(container);
                DescentDeeper(container);

                ContainsOnCollection(container);
                DescendIntoCollectionMembers(container);
                ContainsOnMaps(container);
                FieldObject(container);

                GenericConstrains(container);
                DescendIntoNotExistingField(container);
                MixWithQueryByExample(container);
            }
        }

        private static void SimplestPossibleQuery(IObjectContainer container)
        {
            Console.WriteLine("Type constrain for the objects");
            // #example: Type constrain for the objects
            IQuery query = container.Query();
            query.Constrain(typeof(Pilot));

            IObjectSet result = query.Execute();
            // #end example
            ListResult(result);
        }

        private static void EqualsConstrain(IObjectContainer container)
        {
            Console.WriteLine("A simple constrain on a field");
            // #example: A simple constrain on a field
            IQuery query = container.Query();
            query.Constrain(typeof (Pilot));
            query.Descend("name").Constrain("John");

            IObjectSet result = query.Execute();
            // #end example
            ListResult(result);
        }

        private static void GreaterThanConstrain(IObjectContainer container)
        {
            Console.WriteLine("A greater than constrain");
            // #example: A greater than constrain
            IQuery query = container.Query();
            query.Constrain(typeof (Pilot));
            query.Descend("age").Constrain(42).Greater();

            IObjectSet result = query.Execute();
            // #end example
            ListResult(result);
        }

        private static void GreaterThanOrEqualConstrain(IObjectContainer container)
        {
            Console.WriteLine("A greater than or equals constrain");
            // #example: A greater than or equals constrain
            IQuery query = container.Query();
            query.Constrain(typeof (Pilot));
            query.Descend("age").Constrain(42).Greater().Equal();

            IObjectSet result = query.Execute();
            // #end example
            ListResult(result);
        }
        private static void NotConstrain(IObjectContainer container)
        {
            Console.WriteLine("Not constrain");
            // #example: Not constrain
            IQuery query = container.Query();
            query.Constrain(typeof (Pilot));
            query.Descend("age").Constrain(42).Not();

            IObjectSet result = query.Execute();
            // #end example
            ListResult(result);
        }

        private static void CombiningConstrains(IObjectContainer container)
        {
            Console.WriteLine("Logical combination of constrains");
            // #example: Logical combination of constrains
            IQuery query = container.Query();
            query.Constrain(typeof (Pilot));
            query.Descend("age").Constrain(42).Greater()
                .Or(query.Descend("age").Constrain(30).Smaller());

            IObjectSet result = query.Execute();
            // #end example
            ListResult(result);
        }

        private static void StringConstrains(IObjectContainer container)
        {
            Console.WriteLine("String comparison");
            // #example: String comparison
            IQuery query = container.Query();
            query.Constrain(typeof (Pilot));
            // First strings, you can use the contains operator
            query.Descend("name").Constrain("oh").Contains()
                // Or like, which is like .contains(), but case insensitive
                .Or(query.Descend("name").Constrain("AnN").Like())
                // The .endsWith and .startWith constrains are also there,
                // the true for case-sensitive, false for case-insensitive
                .Or(query.Descend("name").Constrain("NY").EndsWith(false));

            IObjectSet result = query.Execute();
            // #end example
            ListResult(result);
        }

        private static void CompareWithStoredObject(IObjectContainer container)
        {
            Console.WriteLine("Compare with existing object");
            // #example: Compare with existing object
            Pilot pilot = container.Query<Pilot>()[0];

            IQuery query = container.Query();
            query.Constrain(typeof(Car));
            // if the given object is stored, its compared by identity
            query.Descend("pilot").Constrain(pilot);

            IObjectSet carsOfPilot = query.Execute();
            // #end example
            ListResult(carsOfPilot);
        }

        private static void DescentDeeper(IObjectContainer container)
        {
            Console.WriteLine("Descend over multiple fields");

            // #example: Descend over multiple fields
            IQuery query = container.Query();
            query.Constrain(typeof (Car));
            query.Descend("pilot").Descend("name").Constrain("John");

            IObjectSet result = query.Execute();
            // #end example
            ListResult(result);
        }

        private static void ContainsOnCollection(IObjectContainer container)
        {
            Console.WriteLine("Collection contains constrain");
            // #example: Collection contains constrain
            IQuery query = container.Query();
            query.Constrain(typeof (BlogPost));
            query.Descend("tags").Constrain("db4o");

            IObjectSet result = query.Execute();
            // #end example
            ListResult(result);
        }

        private static void DescendIntoCollectionMembers(IObjectContainer container)
        {
            Console.WriteLine("Descend into collection members");
            // #example: Descend into collection members
            IQuery query = container.Query();
            query.Constrain(typeof(BlogPost));
            query.Descend("authors").Descend("name").Constrain("Jenny");

            IObjectSet result = query.Execute();
            // #end example
            ListResult(result);
        }

        private static void ContainsOnMaps(IObjectContainer container)
        {
            Console.WriteLine("Dictionary contains a key constrain");
            // #example: Dictionary contains a key constrain
            IQuery query = container.Query();
            query.Constrain(typeof (BlogPost));
            query.Descend("metaData").Constrain("source");

            IObjectSet result = query.Execute();
            // #end example
            ListResult(result);
        }

        private static void FieldObject(IObjectContainer container)
        {
            Console.WriteLine("Return the object of a field");
            // #example: Return the object of a field
            IQuery query = container.Query();
            query.Constrain(typeof(Car));
            query.Descend("name").Constrain("Mercedes");

            // returns the pilot of these cars
            IObjectSet result = query.Descend("pilot").Execute();
            // #end example
            ListResult(result);
        }

        private static void GenericConstrains(IObjectContainer container)
        {
            Console.WriteLine("Pure field constrains");
            // #example: Pure field constrains
            IQuery query = container.Query();
            // You can simple filter objects which have a certain field
            query.Descend("name").Constrain(null).Not();

            IObjectSet result = query.Execute();
            // #end example
            ListResult(result);
        }

        private static void DescendIntoNotExistingField(IObjectContainer container)
        {
            Console.WriteLine("Using not existing fields excludes objects");
            // #example: Using not existing fields excludes objects
            IQuery query = container.Query();
            query.Constrain(typeof (Pilot));
            // using not existing fields doesn't throw an exception
            // but rather exclude all object which don't use this field
            query.Descend("notExisting").Constrain(null).Not();

            IObjectSet result = query.Execute();
            // #end example
            ListResult(result);
        }

        private static void MixWithQueryByExample(IObjectContainer container)
        {
            Console.WriteLine("Mix with query by example");
            // #example: Mix with query by example
            IQuery query = container.Query();
            query.Constrain(typeof(Car));
            // if the given object is not stored,
            // it will behave like query by example for the given object
            Pilot examplePilot = new Pilot(null, 42);
            query.Descend("pilot").Constrain(examplePilot);

            IObjectSet carsOfPilot = query.Execute();
            // #end example
            ListResult(carsOfPilot);
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

            BlogPost firstPost = new BlogPost("db4o", "Content about db4o");
            firstPost.AddTags("db4o", ".net", "java", "database");
            firstPost.AddMetaData("comment-feed-link", "localhost/rss");
            firstPost.AddAuthors(new Author("John"), new Author("Jenny"), new Author("Joanna"));

            container.Store(firstPost);

            BlogPost secondPost = new BlogPost("cars", "Speedy cars");
            secondPost.AddTags("cars", "fast");
            secondPost.AddMetaData("comment-feed-link", "localhost/rss");
            secondPost.AddMetaData("source", "www.wikipedia.org");
            secondPost.AddAuthors(new Author("Joanna"), new Author("Jenny"));

            container.Store(secondPost);
        }
    }
}