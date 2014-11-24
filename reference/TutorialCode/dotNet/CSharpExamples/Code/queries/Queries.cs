using System;
using System.Collections;
using System.Collections.Generic;
using Db4objects.Db4o;
using Db4objects.Db4o.Query;
// #example: Include LINQ namespaces
using System.Linq;
using Db4objects.Db4o.Linq;
// #end example

namespace Db4oTutorialCode.Code.Queries
{
    public class Queries
    {
        private const string DatabaseFile = "databaseFile.db4o";

        public static void Main(string[] args)
        {
            StoreExampleObjects();
            QueryForJoe();
            QueryPeopleWithPowerfulCar();
            QueryableInterface();
            UnoptimizableQuery();
            SodaQueryForJoe();
            SodaQueryForPowerfulCars();
        }

        private static void QueryForJoe()
        {
            using (IObjectContainer container = Db4oEmbedded.OpenFile(DatabaseFile))
            {
                // #example: Query for drivers named Joe
                var drivers = from Driver d in container
                              where d.Name == "Joe"
                              select d;
                // #end example
                Console.WriteLine("Driver named Joe");
                foreach (Driver driver in drivers)
                {
                    Console.WriteLine(driver);
                }
            }
        }

        private static void QueryPeopleWithPowerfulCar()
        {
            using (IObjectContainer container = Db4oEmbedded.OpenFile(DatabaseFile))
            {
                // #example: Query for people with powerful cars
                var drivers = from Driver d in container
                              where d.MostLovedCar.HorsePower > 150
                                    && d.Age >= 18
                              select d;
                // #end example
                Console.WriteLine("People with powerful cars:");
                foreach (Driver driver in drivers)
                {
                    Console.WriteLine(driver);
                }
            }
        }

        private static void QueryableInterface()
        {
            using (IObjectContainer container = Db4oEmbedded.OpenFile(DatabaseFile))
            {
                // #example: Get the IQueryable interface
                IQueryable<Driver> querable = container.AsQueryable<Driver>();

                var drivers = from d in querable
                              where d.Name == "Joe" 
                              select d;
                // #end example
                Console.WriteLine("Drivers named Joe");
                foreach (Driver driver in drivers)
                {
                    Console.WriteLine(driver);
                }
            }
        }

        private static void UnoptimizableQuery()
        {
            using (IObjectContainer container = Db4oEmbedded.OpenFile(DatabaseFile))
            {
                // #example: Unoptimizable query
                // This query will print a 'QueryOptimizationException' in the debugger console.
                // That means it runs very slowly and you should find an alternative.
                // This example query cannot be optimized because the hash code isn't a stored in database
                var drivers = from Driver d in container
                              where d.GetHashCode()==42
                              select d;
                // #end example
                foreach (Driver driver in drivers)
                {
                    Console.WriteLine(driver);
                }
            }
        }

        private static void SodaQueryForJoe()
        {
            using (IObjectContainer container = Db4oEmbedded.OpenFile(DatabaseFile))
            {
                // #example: Query for drivers named Joe with SODA
                IQuery query = container.Query();
                query.Constrain(typeof (Driver));
                query.Descend("name").Constrain("Joe");
                IList queryResult = query.Execute();
                IList<Driver> drivers = queryResult.Cast<Driver>().ToList();
                // #end example
                Console.WriteLine("Driver named Joe");
                foreach (Driver driver in drivers)
                {
                    Console.WriteLine(driver);
                }
            }
        }

        private static void SodaQueryForPowerfulCars()
        {
            using (IObjectContainer container = Db4oEmbedded.OpenFile(DatabaseFile))
            {
                // #example: Query for people with powerful cars with SODA
                IQuery query = container.Query();
                query.Constrain(typeof (Driver));
                query.Descend("mostLovedCar").Descend("horsePower").Constrain(150).Greater();
                query.Descend("age").Constrain(18).Greater().Equal();
                IList queryResult = query.Execute();
                IList<Driver> drivers = queryResult.Cast<Driver>().ToList();
                // #end example
                Console.WriteLine("People with powerful cars:");
                foreach (Driver driver in drivers)
                {
                    Console.WriteLine(driver);
                }
            }
        }


        private static void StoreExampleObjects()
        {
            using (IObjectContainer container = Db4oEmbedded.OpenFile(DatabaseFile))
            {
                var vwBeetle = new Car("VW Beetle", 90);
                var audi = new Car("Audi A6", 175);
                var ferrari = new Car("Ferrari", 215);

                var joe = new Driver("Joe", 42, audi);
                var joanna = new Driver("Joanna", 24, vwBeetle);
                var jenny = new Driver("Jenny", 54);
                var john = new Driver("John", 17, ferrari);
                var jim = new Driver("Jim", 18, audi);

                container.Store(joe);
                container.Store(joanna);
                container.Store(jenny);
                container.Store(john);
                container.Store(jim);
            }
        }
    }
}