using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;

namespace Db4oDoc.Code.Query.NativeQueries
{
    public class NativeQueryExamples
    {
        private const string DatabaseFile = "database.db4o";

        public static void Main(string[] args)
        {
            CleanUp();
            IEmbeddedConfiguration cfg = Db4oEmbedded.NewConfiguration();
            using (IObjectContainer container = Db4oEmbedded.OpenFile(cfg, DatabaseFile))
            {
                StoreData(container);

                Equality(container);
                Comparison(container);
                RageOfValues(container);
                CombineComparisons(container);
                FollowReferences(container);
                QueryInSeparateClass(container);
                AnyCode(container);
            }
        }

        private static void Equality(IObjectContainer container)
        {
            // #example: Check for equality of the name
            IList<Pilot> result = container.Query(
                delegate(Pilot pilot) { return pilot.Name == "John"; });
            // #end example

            ListResult(result);
        }

        private static void Comparison(IObjectContainer container)
        {
            // #example: Compare values to each other
            IList<Pilot> result = container.Query(
                delegate(Pilot pilot) { return pilot.Age < 18; });
            // #end example

            ListResult(result);
        }

        private static void RageOfValues(IObjectContainer container)
        {
            // #example: Query for a particular rage of values
            IList<Pilot> result = container.Query(
                delegate(Pilot pilot) { return pilot.Age > 18 && pilot.Age < 30; });
            // #end example

            ListResult(result);
        }

        private static void CombineComparisons(IObjectContainer container)
        {
            // #example: Combine different comparisons with the logical operators
            IList<Pilot> result = container.Query(
                delegate(Pilot pilot)
                    {
                        return (pilot.Age > 18 && pilot.Age < 30)
                               || pilot.Name == "John";
                    });
            // #end example

            ListResult(result);
        }

        private static void FollowReferences(IObjectContainer container)
        {
            // #example: You can follow references
            IList<Car> result = container.Query(
                delegate(Car car)
                {
                    return car.Pilot.Name=="John";
                });
            // #end example

            ListResult(result);
        }

        private static void QueryInSeparateClass(IObjectContainer container)
        {
            // #example: Use the predefined query
            IList<Pilot> result = container.Query(new Predicate<Pilot>(AllJohns));
            // #end example

            ListResult(result);
        }

        // #example: Query as method
        private static bool AllJohns(Pilot pilot)
        {
            return pilot.Name == "John";
        }

        // #end example


        private static void AnyCode(IObjectContainer container)
        {
            // #example: Arbitrary code
            IList<int> allowedAges = Array.AsReadOnly(new int[] {18, 20, 35});
            IList<Pilot> result = container.Query(
                delegate(Pilot pilot)
                    {
                        return allowedAges.Contains(pilot.Age) ||
                               pilot.Name.ToLowerInvariant() == "John";
                    });
            // #end example

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
            Pilot juliette = new Pilot("Juliette", 33);

            container.Store(new Car(john, "Ferrari"));
            container.Store(new Car(joanna, "Mercedes"));
            container.Store(new Car(jenny, "Volvo"));
            container.Store(new Car(rick, "Fiat"));
            container.Store(new Car(juliette, "Suzuki"));
        }
    }
}