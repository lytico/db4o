using System;
using System.IO;
using System.Linq;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Linq;

namespace Db4oDoc.Code.Pitfalls.Activation
{
    public class ActivationDepthPitfall
    {
        public const string DatabaseFile = "database.db4o";

        public static void Main(string[] args)
        {
            CleanUp();
            PrepareDeepObjGraph();

            try
            {
                RunIntoActivationIssue();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }

            FixItWithHigherActivationDepth();
            FixItWithExplicitlyActivating();
            Deactivate();
        }

        private static void FixItWithHigherActivationDepth()
        {
            IEmbeddedConfiguration configuration = Db4oEmbedded.NewConfiguration();
            configuration.Common.ActivationDepth = 16;
            using (IObjectContainer container = Db4oEmbedded.OpenFile(configuration, "database.db4o"))
            {
                Person jodie = QueryForJodie(container);

                Person julia = jodie.Mother.Mother.Mother.Mother.Mother;

                Console.WriteLine(julia.Name);
                string joannaName = julia.Mother.Name;
                Console.WriteLine(joannaName);
            }
        }
        private static void FixItWithExplicitlyActivating()
        {
            using (IObjectContainer container = Db4oEmbedded.OpenFile("database.db4o"))
            {
                Person jodie = QueryForJodie(container);

                // #example: Fix with explicit activation
                Person julia = jodie.Mother.Mother.Mother.Mother.Mother;
                container.Activate(julia,5);

                Console.WriteLine(julia.Name);
                string joannaName = julia.Mother.Name;
                Console.WriteLine(joannaName);
                // #end example
            }
        }
        
        private static void Deactivate() {
            using(IObjectContainer container = Db4oEmbedded.OpenFile("database.db4o"))
            {
                var jodie = QueryForJodie(container);

                container.Activate(jodie,5);

                // #example: Deactivate an object
                Console.WriteLine(jodie.Name);
                // Now all fields will be null or 0
                // The same applies for all references objects up to a depth of 5 
                container.Deactivate(jodie,5);
                Console.WriteLine(jodie.Name);
                // #end example
            } 
        }

        private static void RunIntoActivationIssue()
        {
            using (IObjectContainer container = Db4oEmbedded.OpenFile(DatabaseFile))
            {
                // #example: Run into not activated objects
                Person jodie = QueryForJodie(container);

                Person julia = jodie.Mother.Mother.Mother.Mother.Mother;

                // This will print null
                // Because julia is not activated
                // and therefore all fields are not set
                Console.WriteLine(julia.Name);
                // This will throw a NullPointerException.
                // Because julia is not activated
                // and therefore all fields are not set
                string joannaName = julia.Mother.Name;
                // #end example
            }
        }

        private static void CleanUp()
        {
            File.Delete(DatabaseFile);
        }

        private static Person QueryForJodie(IObjectContainer container)
        {
            return (from Person p in container
                    where p.Name == "Jodie"
                    select p).First();
        }

        private static void PrepareDeepObjGraph()
        {
            using (IObjectContainer container = Db4oEmbedded.OpenFile(DatabaseFile))
            {
                Person joanna = new Person("Joanna");
                Person jenny = new Person(joanna, "Jenny");
                Person julia = new Person(jenny, "Julia");
                Person jill = new Person(julia, "Jill");
                Person joel = new Person(jill, "Joel");
                Person jamie = new Person(joel, "Jamie");
                Person jodie = new Person(jamie, "Jodie");
                container.Store(jodie);
            }
        }
    }


}