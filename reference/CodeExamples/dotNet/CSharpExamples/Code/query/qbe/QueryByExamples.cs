using System;
using System.Collections;
using System.IO;
using Db4objects.Db4o;

namespace Db4oDoc.Code.Query.QueryByExample
{
    public class QueryByExamples
    {
        private const string DatabaseFile = "database.db4o";

        public static void Main(string[] args)
        {
            CleanUp();
            using (IObjectContainer container = Db4oEmbedded.OpenFile(DatabaseFile))
            {
                StoreData(container);

                QueryForName(container);
                QueryForAge(container);
                QueryForNameAndAge(container);

                NestedObjects(container);
                AllObjects(container);
                AllObjectsOfAType(container);
                AllObjectsOfATypeWithEmptyObject(container);

                ContainsQuery(container);
                StructuredContains(container);
            }
        }

        private static void QueryForName(IObjectContainer container)
        {
            // #example: Query for John by example
            Pilot theExample = new Pilot();
            theExample.Name = "John";
            IList result = container.QueryByExample(theExample);
            // #end example

            ListResult(result);
        }

        private static void QueryForAge(IObjectContainer container)
        {
            // #example: Query for 33 year old pilots
            Pilot theExample = new Pilot();
            theExample.Age = 33;
            IList result = container.QueryByExample(theExample);
            // #end example

            ListResult(result);
        }

        private static void QueryForNameAndAge(IObjectContainer container)
        {
            // #example: Query a 29 years old Jo
            Pilot theExample = new Pilot();
            theExample.Name = "Jo";
            theExample.Age = 29;
            IList result = container.QueryByExample(theExample);
            // #end example

            ListResult(result);
        }

        private static void AllObjects(IObjectContainer container)
        {
            // #example: All objects
            IList result = container.QueryByExample(null);
            // #end example

            ListResult(result);
        }

        private static void AllObjectsOfAType(IObjectContainer container)
        {
            // #example: All objects of a type by passing the type
            IList result = container.QueryByExample(typeof (Pilot));
            // #end example

            ListResult(result);
        }

        private static void AllObjectsOfATypeWithEmptyObject(IObjectContainer container)
        {
            // #example: All objects of a type by passing a empty example
            Pilot example = new Pilot();
            IList result = container.QueryByExample(example);
            // #end example

            ListResult(result);
        }

        private static void NestedObjects(IObjectContainer container)
        {
            // #example: Nested objects example
            Pilot pilotExample = new Pilot();
            pilotExample.Name = "Jenny";

            Car carExample = new Car();
            carExample.Pilot = pilotExample;
            IList result = container.QueryByExample(carExample);
            // #end example

            ListResult(result);
        }

        private static void ContainsQuery(IObjectContainer container)
        {
            // #example: Contains in collections
            BlogPost pilotExample = new BlogPost();
            pilotExample.AddTags("db4o");
            IList result = container.QueryByExample(pilotExample);
            // #end example

            ListResult(result);
        }

        private static void StructuredContains(IObjectContainer container)
        {
            // #example: Structured contains
            BlogPost pilotExample = new BlogPost();
            pilotExample.AddAuthors(new Author("John"));
            IList result = container.QueryByExample(pilotExample);
            // #end example

            ListResult(result);
        }


        private static void ListResult(IEnumerable result)
        {
            foreach (object obj in result)
            {
                Console.WriteLine(obj);
            }
        }

        private static void CleanUp()
        {
            File.Delete(DatabaseFile);
        }

        private static void StoreData(IObjectContainer container)
        {
            Pilot john = new Pilot("John", 42);
            Pilot joanna = new Pilot("Joanna", 45);
            Pilot jenny = new Pilot("Jenny", 21);
            Pilot rick = new Pilot("Rick", 33);
            Pilot juliette = new Pilot("Juliette", 33);
            container.Store(new Pilot("Jo", 34));
            container.Store(new Pilot("Jo", 29));
            container.Store(new Pilot("Jimmy", 33));


            container.Store(new Car(john, "Ferrari"));
            container.Store(new Car(joanna, "Mercedes"));
            container.Store(new Car(jenny, "Volvo"));
            container.Store(new Car(rick, "Fiat"));
            container.Store(new Car(juliette, "Suzuki"));


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